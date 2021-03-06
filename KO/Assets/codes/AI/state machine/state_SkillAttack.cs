﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class state_SkillAttack  : stateBasic  
{

	//状态：技能攻击
	//条件：两者距离达到一定程度，法力值充足
	//跳转条件1：时间到或者生命值较低转换到逃跑模式
	//跳转条件2：玩家法力值不足的时候进入使用普通攻击的模式

	//时间限制：有
	//说明：这个包含所有的技能攻击方式，获取到所有的连招字符串并进行决策
	//大致思路是根据伤害、脚本和法力消耗做一个排序

	private float distance = 99f;
	private float timer =0f;//每隔一小段时间进行一次攻击，模拟的是键位动作
	//（时间越短灵活性越好，所以可以考虑调用游戏人物playerBasic的属性做计算参数）
 

	private float stateTimer = 5f;//每隔一段时间强制进行一次决策

	//来自父类的三大方法
	//初始化、行动和决策
	public override void stateIint ()
	{
		if (thePlayer) 
			this.theEnemy = systemValues.getEMY (this.thePlayer.transform).GetComponent <PlayerBasic>();

	}


	/*
	 * 存在一个逻辑上的BUG
	 * 当游戏人物斗气值比较低的时候，
	 * 这个时候他使用一个技能，立刻就会被判定为缺蓝，
	 * 从而立刻取消掉当前进行的动作，这无形中白白浪费了导致缺蓝的动作的斗气值
	 * */
	public override void stateAction ()
	{
		theMoveController.moveAction (0,0);//虽然不移动，但是应该调用计算确保位置的正确

		timer -= Time.deltaTime;
		if (timer < 0  &&  theActionController .canChangeToNextAttack()) 
			//&&  theActionController .isAttacking()==false 这个判定条件统一放置到attackLinkController里面
		{
			//Debug.Log ("trueToattack!");
			timer = systemValues .theAIActionThinkTimeForSkill;
			//string theLink = this.getAttackLinkString (this.theActionController,this .thePlayer);
			string theLink = this.getAttackLinkString2 (this.theActionController,this .thePlayer);//伪，基于增强学习概念的方法
			theActionController.makeAttackLink(theLink,false);//这里是需要进行决策的地方
		}
	}

	public override stateBasic thinkNextState ()
	{
		stateTimer -= Time.deltaTime;

		distance = Vector3.Distance (this.thePlayer.transform.position, this.theEnemy.transform.position);
		if (this.isSPLow(this.theActionController,this.thePlayer) )
		{
			state_basicAttack theBAsicAttack = new state_basicAttack ();
			theBAsicAttack.makeState (this.theMoveController ,this.theActionController,this.thePlayer);
			theBAsicAttack.stateIint ();
			return theBAsicAttack;
		}
		if ((distance > thePlayer .theAttackAreaLength  || stateTimer <0))
		{
			state_Near theNear = new state_Near ();
			theNear.makeState (this.theMoveController ,this.theActionController,this.thePlayer);
			theNear.stateIint ();
			//Debug.Log ("toAttack!");
			return theNear;
		}

		float hpMinus = (theEnemy.ActerHp - thePlayer.ActerHp);
		if (hpMinus > 100  && hpMinus < 200) 
		{
			stage_Back theBack = new stage_Back ();
			theBack.makeState (this.theMoveController ,this.theActionController,this.thePlayer);
			theBack.stateIint ();
			return theBack;
		}
		return this;
	}

	//void Start () //重要的初始化
	//{


	//}

}
