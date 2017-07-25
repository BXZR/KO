using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectForIceKick :  effectBasic {
	//攻击的吸血效果
	public float lastingTime = 9f;//下过持续时间
	float damagePercent = 0.10f;//削减的攻击力百分比
	float damageMinus; 
	private GameObject theEffectP;//效果引用保存
	private GameObject theEffect;//效果引用保存

	float kickTimer = 0.5f;//被踢击退的时间
	float kickSpeed = 1.9f;//被击退的速度


	float damageSave = 0;//存储的伤害
	float damageSavePercentMake = 0.25f;//结束的时候的第二段伤害百分比

	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "碎裂之腿";
		theEffectInformation ="击退目标"+kickTimer+"秒，并削减其10%攻击力\n持续时间内储存目标受到的伤害\n效果结束时造成总储存量"+damageSavePercentMake*100+"%真实伤害\n最多持续"+this.lastingTime+"秒，攻击命中减少1秒时间";
		makeStart ();
		damageMinus =thePlayer.ActerWuliDamage * damagePercent;
		thePlayer.ActerWuliDamage -= damageMinus;

		theEffectP = (GameObject)Resources.Load ("effects/roundKick");
		theEffect = GameObject.Instantiate (theEffectP);
		theEffect.transform.parent = thePlayer.transform;
		theEffect.transform.position =new Vector3( thePlayer.transform.position.x,thePlayer .transform .position .y +0.1f,thePlayer.transform .position .z) ;
		thePlayer.GetComponent<move> ().canMove = false;
	}

	 


	public override void OnBeAttack (float damage)
	{
		damageSave += damage;
		this.lastingTime -= 1f;
	}

	public override void effectDestory ()
	{
		thePlayer.ActerWuliDamage += damageMinus;
		thePlayer.CActerWuliDamage += damageMinus;
		if( theEffect)
			Destroy (theEffect .gameObject);
		CancelInvoke ();
		//thePlayer.GetComponent<move> ().canMove = true;
	}

	void OnDestroy()
	{
		
		float damage =  damageSave * damageSavePercentMake;
		print ("kick -- "+ damageSave * damageSavePercentMake);
		thePlayer.ActerHp -= damage;

		effectDestory ();
		thePlayer.GetComponent<move> ().canMove = true;
	}


 
	public override void updateEffect ()
	{
		//这个技能的击退效果可以连续触发，但是伤害提升的效果不能
		kickTimer = 0.5f;
		thePlayer.GetComponent<move> ().canMove = false;
	}

 
	//这个方法会由挂上的人物的PlayerBasi统一调用，调用时间间隔为1秒
	public override void effectOnUpdate ()
	{
		
		lastingTime --;
		if (lastingTime <= 0  || thePlayer .isAlive ==false)
		{
			Destroy (this.gameObject .GetComponent (this.GetType()));//为了保证灵活性，也还是应该使用人工计时的方法
		}
	}

	void Update ()
	{
		 
		if (kickTimer > 0 )
		{
			kickTimer -= Time.deltaTime;

			thePlayer.GetComponent<move> ().beMakeBack (kickSpeed);
			if (kickTimer <= 0)
			{
				thePlayer.GetComponent<move> ().canMove = true;
			}
		} 
 
	}
}
