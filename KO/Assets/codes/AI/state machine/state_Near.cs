﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class state_Near : stateBasic  
{

	//状态：靠近
	//条件，两者距离太长，自身生命值还比较高
	//跳转条件，距离达到一定程度
	//时间限制：没有
	//值得注意的是距是在全局配置文件SystemValues里面进行设定的

	//private GameObject theEnemy;//敌人物体，这个可以通过systemValues获取得到
	private float distance = 9999f;
	private Vector3 enemyPosition;//目标位置
	private float theHeightToJump = 0.4f;//这个值在初始化的时候会变成目标跳跃最大高度的一半
	//来自父类的三大方法
	//初始化、行动和决策
	public override void stateIint ()
	{
		if(thePlayer)
			this.theEnemy = systemValues.getEMY (this.thePlayer .transform ).GetComponent <PlayerBasic>();
		theHeightToJump = this.theEnemy.GetComponent <move> ().jumpMaxHeight / 3;
	}

	public override void stateAction ()
	{
		enemyPosition = this.theEnemy.GetComponent <Transform> ().position;
		distance = Vector3.Distance (this.thePlayer.transform.position, this.enemyPosition );
		if(distance > thePlayer .theAttackAreaLength)
		 theMoveController .moveAction (this.thePlayer .transform .forward .z*1.35f  ,0f);//游戏人物移动的输入接口
		//else
		// theMoveController .moveAction (0.75f ,0f);//游戏人物移动的输入接口	
	}

	public override stateBasic thinkNextState ()
	{
		if (distance <= thePlayer .theAttackAreaLength ) 
		{
			state_basicAttack theattack = new state_basicAttack ();
			theattack.makeState (this.theMoveController ,this.theActionController,this.thePlayer);
			theattack.stateIint ();
			//Debug.Log ("toAttack!");
			return theattack;
		}

		float theHeightMinus = this.theEnemy.transform.position.y - this.thePlayer.transform.position.y;//注意各个减法绝对是由方向性的，只有在高度差为证才有可能跳起来

		if ( theHeightMinus > theHeightToJump) 
		{
			stage_Jump theJump = new stage_Jump ();
			theJump .makeState (this.theMoveController ,this.theActionController,this.thePlayer);
			theJump.stateIint ();
			//Debug.Log ("toAttack!");
			return theJump ;
		}


		return this;
	}

	//void Start () //重要的初始化
	//{
		 

	//}
 
}
