using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectFotPublicJiFei :  effectBasic {
	//所有的人都会的战斗技能：击飞
	float timer = 0.75f;
	bool gate = false;//标记量
	float gateTimer = 0.2f;
	move theMoveController;//移动控制单元
	float upSpeed = 5f;
	float timerAdd = 0.5f;
	float gateTimerAdd = 0.15f;

	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "击飞";
		theEffectInformation ="击飞目标，使之上冲"+timer+"秒并失重"+gateTimer+"秒\n目标击飞过程中可再次击飞叠加效果\n增加"+timerAdd+"秒上冲时间和"+gateTimerAdd+"秒失重时间";
		makeStart ();
		theMoveController = thePlayer.GetComponent <move> ();
		makeStart ();
		theMoveController .canMove = false;
		theMoveController.canGravity = false;	
	}

	public override bool isPublic ()
	{
		return true;//这个技能只要是个人都会
	}

	void makeOver ()//一段时间之后可以移动
	{
		theMoveController .canMove = true;
		theMoveController.canGravity = true;
	}

	void makeUpdate()//但是向上的冲击力还会持续一段时间
	{
		theMoveController.moveSimple (0,upSpeed);
		if (this.thePlayer.transform.position.y > theMoveController.jumpMaxHeight)
			this.thePlayer.transform.position = new Vector3 (this.thePlayer .transform .position .x ,theMoveController.jumpMaxHeight,this.thePlayer .transform .position .z);
	}

	override public  void updateEffect ()
	{
        //所有标记量的更新
		timer += timerAdd;
		gate = false;//标记量
		gateTimer = gateTimerAdd;
		upSpeed *=1.5f;//向上的速度越来越快，对抗越来越强的重力
		theMoveController.canMove = false;
	}

	void OnDestroy()
	{
		makeOver ();
	}

	void Update ()
	{
		if (!gate)
		{
			theMoveController.moveSimple (0,1f);
			gateTimer -= Time.deltaTime;
			if (gateTimer < 0) 
			{
				gate = true;
				makeOver ();
			}
		}
		timer -= Time.deltaTime;
		if (timer >= 0) 
		{
			makeUpdate ();
		} 
		else
		{
			Destroy (this.GetComponent (this.GetType()));
		}
	}
}
