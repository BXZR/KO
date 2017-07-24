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

	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "碎裂之腿";
		theEffectInformation ="击退目标0.5秒(目标无法自主移动)\n削减目标10%当前攻击力\n持续时间内目标受到的伤害提升5%\n最多持续"+this.lastingTime+"秒，攻击命中减少1秒时间";
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
		float theDamege = damage * 0.05f * (1 - this.thePlayer.ActerWuliShield / 1500);
		this.thePlayer.ActerHp -= theDamege;
		this.thePlayer.addDamageRead (theDamege);
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
		effectDestory ();
	}


 
	public override void updateEffect ()
	{
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
