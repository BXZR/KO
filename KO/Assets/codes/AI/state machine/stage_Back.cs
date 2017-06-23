using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stage_Back : stateBasic  
{

	//状态：远离
	//条件，敌人生命值大大高于自身
	//跳转条件，时间到
	//时间限制：3秒
	//值得注意的是距是在全局配置文件SystemValues里面进行设定的


	private float stateTimer = 2f;//每隔一段时间强制进行一次决策

	//来自父类的三大方法
	//初始化、行动和决策
	public override void stateIint ()
	{
		if(thePlayer)
			this.theEnemy = systemValues.getEMY (this.thePlayer .transform ).GetComponent <PlayerBasic>();
		//	Debug .Log(this.theEnemy .name);
	}

	public override void stateAction ()
	{
		theMoveController .moveAction (-this.thePlayer .transform .forward .z*1.35f  ,0f);//游戏人物移动的输入接口
	}

	public override stateBasic thinkNextState ()
	{
		stateTimer -= systemValues .theAIActionThinkTime;
		//Debug.Log (stateTimer +"--");
		if (stateTimer <= 0 ) 
		{
			state_basicAttack theattack = new state_basicAttack ();
			theattack.makeState (this.theMoveController ,this.theActionController,this.thePlayer);
			theattack.stateIint ();

			//Debug.Log ("BacktoAttack!");
			return theattack;
		}
		return this;
	}

	void Start () //重要的初始化
	{


	}

}
