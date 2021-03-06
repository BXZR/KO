﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stage_Jump :stateBasic  
{

	//状态：跳跃
	//条件，处于near状态并且目标
	//跳转条件，距离达到一定程度
	//时间限制：0。5秒
	//值得注意的是距是在全局配置文件SystemValues里面进行设定的

	//private GameObject theEnemy;//敌人物体，这个可以通过systemValues获取得到
	private float distance = 9999f;
	private Vector3 enemyPosition;//目标位置
	private float theTimerForJumpStage = 0.7f;//有跳跃的时间，否则跳跃高度会太低
	private float theHeightToJump = 0.7f;//这个值在初始化的时候会变成目标跳跃最大高度的一半
	float upValue = 1.25f;//临时变量
	//来自父类的三大方法
	//初始化、行动和决策
	public override void stateIint ()
	{
		if(thePlayer)
			this.theEnemy = systemValues.getEMY (this.thePlayer .transform ).GetComponent <PlayerBasic>();
		theHeightToJump = this.theEnemy.GetComponent <move> ().jumpMaxHeight / 2;
		//	Debug .Log(this.theEnemy .name);
	}
	//(事实上这是一个变相的near)
	public override void stateAction ()//因为这个方法是每一帧都要调用的
	{
		enemyPosition = this.theEnemy.GetComponent <Transform> ().position;
		distance = Vector3.Distance (this.thePlayer.transform.position, this.enemyPosition );

		theTimerForJumpStage -= Time.deltaTime;
		if (theTimerForJumpStage < 0)
			upValue = 0;

		if(distance > thePlayer .theAttackAreaLength )
			theMoveController .moveAction (this.thePlayer .transform .forward .z*1.35f  ,upValue);//游戏人物移动的输入接口
		else
			theMoveController .moveAction (0f ,upValue);//游戏人物移动的输入接口	
	}

	public override stateBasic thinkNextState ()
	{
		if (distance <= thePlayer .theAttackAreaLength  ) 
		{
			state_basicAttack theattack = new state_basicAttack ();
			theattack.makeState (this.theMoveController ,this.theActionController,this.thePlayer);
			theattack.stateIint ();
			//Debug.Log ("toAttack!");
			return theattack;
		}
		float theHeightMinus = this.theEnemy.transform.position.y - this.thePlayer.transform.position.y;//注意各个减法绝对是由方向性的，只有在高度差为证才有可能跳起来
		if (theHeightMinus  < theHeightToJump && theTimerForJumpStage < 0  || theHeightMinus<-theHeightToJump) //必须要跳起来了才可以转换 
		{
			state_Near theNear = new state_Near ();
			theNear.makeState (this.theMoveController ,this.theActionController,this.thePlayer);
			theNear.stateIint ();
			return theNear;
		}
		return this;
	}

	//void Start () //重要的初始化
	//{


	//}

}
