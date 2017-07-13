using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class state_basicAttack  : stateBasic  
{

	//状态：基本攻击
	//条件：两者距离达到一定程度
	//跳转条件1：时间到或者生命值较低转换到逃跑模式
	//跳转条件2：玩家法力值充足的时候进入使用技能的模式
	//时间限制：有
	//说明：这个包含所有的基本攻击方式，也就是随机的“J”和“K”输入

	private float distance = 99f;
	private float timer =0.0f;//每隔一小段时间进行一次攻击，模拟的是键位动作
	//（时间越短灵活性越好，所以可以考虑调用游戏人物playerBasic的属性做计算参数）
 
	private float stateTimer = 5f;//每隔一段时间强制进行一次决策
	private Vector3 enemyPosition;//目标位置
	//来自父类的三大方法
	//初始化、行动和决策
	public override void stateIint ()
	{
		if (thePlayer) 
			this.theEnemy = systemValues.getEMY (this.thePlayer.transform).GetComponent <PlayerBasic>();

	}
		
	public override void stateAction ()
	{

		theMoveController.moveAction (0,0);//虽然不移动，但是应该调用计算确保位置的正确

		timer -= Time.deltaTime;
		if (timer < 0    &&  theActionController.canChangeToNextAttack())//一个动作完成了才可以开始下一个动作
			//&&  theActionController .isAttacking()==false()这个判定条件统一放置到attackLinkController里面
		{
			//Debug.Log ("trueToattack!");
			timer = systemValues .theAIActionThinkTime;
			float theRandomValue = Random.value;
			if(theRandomValue >0.5f)
			theActionController.makeAttackLink("K",false);
			else
			theActionController.makeAttackLink("J",false);
		}
	}

	public override stateBasic thinkNextState ()
	{
		stateTimer -= Time.deltaTime;
		enemyPosition = theEnemy.GetComponent <Transform> ().position;
		distance = Vector3.Distance (this.thePlayer.transform.position, this.enemyPosition);
		//难点：跳转法力值的界定，如果法力充足就应该用技能进行攻击
		//同时因为判断法力是否足够和选择招式的决策方法不同，所以可能会出现因为使用技能透支生命的可能性
		//股anyUI进呢过试用的决策，也是参见父类stateBasic
		if (this.isSPLow(this.theActionController,this .thePlayer) == false )
		{
			state_SkillAttack theSkillAttack = new 	state_SkillAttack ();
			theSkillAttack.makeState (this.theMoveController ,this.theActionController,this.thePlayer);
			theSkillAttack.stateIint ();
			//Debug.Log ("toAttack!");
			return theSkillAttack;
		}
		if (distance > thePlayer .theAttackAreaLength || stateTimer <0)
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

	void Start () //重要的初始化
	{


	}

}
