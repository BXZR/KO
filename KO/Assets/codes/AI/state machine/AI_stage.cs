using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_stage : theAI {
	//AI 实现方法之有限状态机，说白了就是一堆状态以及相关的状态的方法
    //我在考虑用一个文件夹的方式实现这个有限状态机
	//这个脚本类似于是有限状态机的控制脚本（是挂在游戏人物上面的脚本）

	private stateBasic theStateNow;
	private bool isRunning = false;//标记，当前状态机是否运作

	void Start () 
	{
		makeStart ();//来自父类的统一调用方法
	}
	public override void startAI ()
	{
		if (isRunning == false)
		{
			theStateNow = new state_Near ();
			theStateNow.makeState (this.theMoveController, this.theActionController, this.thePlayer);
			theStateNow.stateIint ();
			isRunning = true;
			InvokeRepeating ("think",0,systemValues .theAIActionThinkTime);//AI思考的速度也是非常重要的
		}

	}
	public override void stopAI ()
	{
		theStateNow = null;
		isRunning = false;
	}
 
	void think()
	{
		if(theStateNow!=null )
		theStateNow = theStateNow.thinkNextState ();
	}

	void Update () 
	{
		if (theStateNow != null &&  isRunning )
		{
			theStateNow.stateAction ();
			theStateNow.lookAtEMY ();
		}
	}
}



