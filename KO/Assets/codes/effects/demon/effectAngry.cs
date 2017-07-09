using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectAngry: effectBasic {
	//攻击的吸血效果
	public float hpup =8;//每秒额外恢复的生命值
	public float lastingTime = 10f;//下过持续时间
	private GameObject theEffectP;//效果引用保存
	private GameObject theEffect;//效果引用保存
	private float superBladePercentAdd = 0.25f;//增加的暴击率
	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "恶魔之怒";
		theEffectInformation = "获得25%暴击率，持续" + lastingTime + "秒\n每秒额外恢复(已损生命2.5%+" + hpup + ")生命\n持续时间内斗气恢复效率减少15%";
		makeStart ();

		theEffectP = (GameObject)Resources.Load ("effects/evilAngry");
		theEffect = GameObject.Instantiate (theEffectP);
		theEffect.transform.parent = thePlayer.transform;
		theEffect.transform.localScale *= thePlayer.transform.localScale.y;
		theEffect.GetComponentInChildren<ParticleSystem> ().startSize *= thePlayer.transform.localScale.y;

		theEffect.transform.position = thePlayer.transform.position;
		thePlayer.ActerSuperBaldePercent += superBladePercentAdd;
	}

	public override void effectDestory ()
	{
		thePlayer.ActerSuperBaldePercent -= superBladePercentAdd;
		if( theEffect)
			Destroy (theEffect .gameObject);
	}

	void OnDestroy()
	{
		effectDestory ();
	}
		
	public override void OnSpUp ()
	{
		thePlayer.ActerSp-= thePlayer .ActerSpUp * systemValues.updateTimeWait * 0.15f;
	}

	//这个方法会由挂上的人物的PlayerBasi统一调用，调用时间间隔为1秒
	public override void effectOnUpdate ()
	{
		lastingTime --;
		if (lastingTime <= 0  || thePlayer .isAlive ==false)
		{
			Destroy (theEffect .gameObject);
			Destroy (this.gameObject .GetComponent(this.GetType()));//为了保证灵活性，也还是应该使用人工计时的方法
		}
		/////
		this.thePlayer .ActerHp += ((this.thePlayer .ActerHpMax - this.thePlayer .ActerHp )*0.025f+hpup);
	}
 
 

	void Update ()
	{
 
	}
}
