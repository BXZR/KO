using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectForLastingDamage : effectBasic {
	//攻击的吸血效果
	private float damage  =12;//每秒造成的伤害
	private float lastingTime = 3f;//下过持续时间
	private float allTimer = 3;//总共时长
	private float allTimerMax = 6;//总共时长
	private float timerAddPerUpdate = 0.5f;//每一次触发增加的持续时间
	private float movePercentMinus = 0.3f;//减少的移动速度百分比



	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "冰封之拳";
		theEffectInformation ="减少目标"+ (movePercentMinus*100).ToString("f0") +"%移动速度\n每秒钟造成最少"+damage+"真实伤害\n最少持续"+allTimer+"秒，最多持续"+allTimerMax+"秒\n连续触发增加"+timerAddPerUpdate+"秒持续时间和15%伤害";
		makeStart ();
		this.thePlayer.ActerMoveSpeedPercent -= movePercentMinus;
	}

	//这个方法会由挂上的人物的PlayerBasi统一调用，调用时间间隔为1秒
	public override void effectOnUpdate ()
	{
		float theDamage = damage ;
		this.thePlayer.ActerHp -=theDamage ;
		this.thePlayer.addDamageRead (theDamage);

		lastingTime --;
		if (lastingTime <= 0  || thePlayer .isAlive ==false)
		{
			Destroy (this.gameObject .GetComponent <EffectForLastingDamage>());//为了保证灵活性，也还是应该使用人工计时的方法
		}
	}


	public override void effectDestory ()
	{
		
		this.thePlayer.ActerMoveSpeedPercent += movePercentMinus;
		if (this.thePlayer.ActerMoveSpeedPercent > 1)
			this.thePlayer.ActerMoveSpeedPercent = 1;
	}

	void OnDestroy()
	{
		effectDestory ();
	}

 

	override public  void updateEffect ()
	{
		damage *= 0.15f;//这一次连续触发还会增加伤害
		allTimer += timerAddPerUpdate;
		if(allTimer <= allTimerMax)
			lastingTime += timerAddPerUpdate;
	}

 
	void Update ()
	{
		//effectOnUpdate ();
	}
}
