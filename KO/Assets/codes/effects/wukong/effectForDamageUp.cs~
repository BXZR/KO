using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectForDamageUp : effectBasic {
	//攻击的吸血效果
	public float damageUp  =12;//增加的攻击力
	public float shiledMis  =0.15f;//增加的护甲百分比穿透
	public float lastingTime = 9f;//下过持续时间
	private GameObject theEffectP;//效果引用保存
	private GameObject theEffect;//效果引用保存

	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "斗气爆发";
		theEffectInformation ="消耗8%生命立刻获得25%最大斗气值\n增加"+damageUp+"攻击力,持续时间内无视15%护甲\n并且斗气消耗减少10%,持续"+lastingTime+"秒\n再次激活增加2秒时间且消耗减半";
		makeStart ();
		this.thePlayer.ActerWuliDamage += damageUp;
		this.thePlayer.CActerWuliDamage += damageUp;
		this.thePlayer.ActerWuliInerPercent += shiledMis;//加10%护甲穿透
		this.thePlayer.CActerWuliInerPercent += shiledMis;//加10%护甲穿透
		theEffectP = (GameObject)Resources.Load ("effects/wukongSaiYa");
		theEffect = GameObject.Instantiate (theEffectP);
		theEffect.transform.parent = thePlayer.transform;
		theEffect.transform.position = thePlayer.transform.position;
		theEffect .transform .localScale*= thePlayer.transform.localScale.y;
		theEffect.transform.Rotate (new Vector3 (-90,0,0));
		thePlayer.ActerSp += thePlayer.ActerSpMax * 0.25f;
		thePlayer.ActerHp *= 0.92f;
	}
 
	public override void OnUseSP (float spUse = 0)
	{
		thePlayer.ActerSp += spUse*0.1f;
	}

	public override void updateEffect ()
	{
		//没有写在介绍里面的小花絮
		//更新这个技能的时候会消耗5%当前生命值但会使得持续时间增加2秒
		thePlayer.ActerHp *= 0.96f;
		lastingTime += 2f;
	}

	public override void effectDestory ()
	{
		this.thePlayer.ActerWuliDamage -= damageUp;
		this.thePlayer.CActerWuliDamage -= damageUp;
		this.thePlayer.ActerWuliInerPercent -= shiledMis;//加10%护甲穿透
		this.thePlayer.CActerWuliInerPercent -= shiledMis;//加10%护甲穿透
		if( theEffect)
			Destroy (theEffect .gameObject);
	}

	void OnDestroy()
	{
		effectDestory ();
	}

 
	public override void effectOnUpdate ()
	{
		lastingTime --;

		if (lastingTime <= 0  || thePlayer .isAlive ==false)
		{
			Destroy (theEffect .gameObject);
			Destroy (this.gameObject .GetComponent <effectForDamageUp>());//为了保证灵活性，也还是应该使用人工计时的方法
		}
	}
 
	void Update ()
	{
		//effectOnUpdate ();
	}
}
