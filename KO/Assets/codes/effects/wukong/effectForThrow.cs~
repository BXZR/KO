using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectForThrow : effectBasic {

	float allTimer = 25f;//总持续时间
	float timer = 0.5f;//时间控制
	bool canEffect =true;
	Vector3 theCircleCenterPosition ;//圆心坐标
	float theLength = 1.6f;//半径
	Vector3 rotateAxis = new Vector3 (1,0,0);//旋转的轴

	Vector3 thePositionNow ;//当前的位置
	float theRotateSpeed ;//每一秒钟旋转的角度
	float theSpeed =0;
	move theMoveController;//用于控制移动
	private Animator theEMYAnimator;//对手的动画控制器
	attackLinkController theAttackLinkController;
	float throwMaxAngle = 150;//投掷弧线的最大角度

	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "逆流投掷";
		theEffectInformation ="攻击并尝试投掷目标,使之失控"+timer+"秒\n落地造成(最大生命3-7%+45)真实伤害\n技能对同一个目标有"+allTimer+"秒的冷却时间\n如目标当前无法中招则变为回旋踢";
		makeStart ();

		theMoveController = this.GetComponent <move> ();
		theAttackLinkController = this.GetComponent <attackLinkController>();
		theMoveController.canMove = false;//强制移动阶段无法自主移动
		theAttackLinkController.canControll = false;//也没有办法进行任何动作
		this.thePlayer.GetComponent <move>().canLook = false;
		makePoint();
		theRotateSpeed = throwMaxAngle / timer;
		theSpeed = theRotateSpeed * Time.deltaTime;
		if (this.transform.forward.z < 0)
			theSpeed = -theSpeed;
	}

	void OnCollisionEnter(Collision collisioner)
	{
		if (collisioner.collider.name == "wall") 
		{
			//如果撞到墙就立刻停止
			//否则就可能会出现
			timer = -99;
			this.transform.position = new Vector3 (this.transform .position .x ,0.3f,this.transform .position .z);
		}
	}

 
	//现在的思路是用一个半圆来模拟坐标(上半圆)
	//圆心为当前位置前方的一点点位置坐标
	//半径为一个常量
	//计算坐标进行更新

	//这个方法计算得到圆心
	void makePoint()
	{
		theCircleCenterPosition = this.transform .position + this.transform .forward .normalized *theLength;
	}


	void circle()
	{
		if(canEffect && systemValues .canAttack)
		{
		 this.transform.RotateAround (theCircleCenterPosition,rotateAxis,theSpeed);
		}
	}

	public override void updateEffect ()
	{
		if (!this.theEMYAnimator)
			theEMYAnimator = systemValues.getEMY (this.thePlayer.transform).GetComponentInChildren<Animator> ();
		theEMYAnimator.CrossFade ("extraAttack",0.02f);
		//this.thePlayer.ActerHp *= 0.99f;
	}

 


	void Update () 
	{
		circle ();
		timer -= Time.deltaTime;
		if (canEffect && timer < 0) 
		{
			canEffect = false;
			effectOver ();
			makeDamage ();
		}

		allTimer -= Time.deltaTime;
		if (allTimer < 0  || thePlayer .isAlive ==false)
		{
			Destroy (this.gameObject .GetComponent (this.GetType()));//为了保证灵活性，也还是应该使用人工计时的方法
		}
	}
 

	void makeDamage()
	{
		//this.thePlayer.ActerSp *=0.6f;
		float ran = Random.Range (0.03f,0.07f);
		this.thePlayer.ActerHp -= (thePlayer.ActerHpMax * ran+45);
	}

	void effectOver()
	{
		this.GetComponent <attackLinkController> ().playDrop ();
		//this.transform.position = new Vector3 (this.transform.position.x, 0.7f, this.transform.position.z);
		this.transform.rotation = new Quaternion (0,0,0,0);
		//this.GetComponent <move> ().enabled = true;
		theMoveController.canMove = true;//强制移动阶段无法自主移动
		theAttackLinkController.canControll = true;//也没有办法进行任何动作
		this.thePlayer.GetComponent <move>().canLook = true;
		this.transform.position = new Vector3 (this.transform .position .x ,0.15f,this.transform .position .z);
		theMoveController.moveAction (0.01f,0.1f); 
	}
	void OnDestroy()
	{
		theAttackLinkController.canControll = true;//也没有办法进行任何动作
		this.thePlayer.GetComponent <move>().canLook = true;
		//this.transform.position = new Vector3 (this.transform .position .x ,0.2f,this.transform .position .z);
	}

 
}
