using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectHit  :  effectBasic {
 
	Transform theArm;
	GameObject theEffect;
	float theTimer = 4f;
	void Start () 
	{

		Init ();//进行初始化
		int  length = this.transform.GetComponentsInChildren<playerWeapon>().Length;
		theArm = this.transform.GetComponentsInChildren<playerWeapon>()[length-1].transform ;
		theEffect = GameObject.Instantiate<GameObject> (Resources.Load<GameObject> ("effects/demonHit"));
		theEffect.transform.SetParent (theArm);
		theEffect.GetComponentInChildren<ParticleSystem> ().startSize *= thePlayer.transform.localScale.y;
		theEffect.transform.localPosition = new Vector3 (-0.08f,0,0);
	}

	public override void Init ()
	{
		theEffectName = "破灭重击";
		theEffectInformation ="为下一次攻击蓄力并尝试进行重击\n下一击额外造成当前生命值6%真实伤害\n造成百分比伤害的22%转化为斗气值\n蓄力效果持续"+theTimer+"秒";
		makeStart ();
		Destroy (this.gameObject .GetComponent (this.GetType()),theTimer);//为了保证灵活性，也还是应该使用人工计时的方法
	} 
	 


	public override void OnAttack (PlayerBasic aim)
	{
		float damage = aim.ActerHp * 0.06f;
		aim.ActerHp -= damage;
		aim.addDamageRead (damage);
		this.thePlayer.ActerSp += damage * 0.22f;
		aim.theAnimationController.playFaint ();
		aim.ActerWuliShield -= 10;
		if (aim.ActerWuliShield < 0)
			aim.ActerWuliShield = 0;
		Destroy (this.gameObject .GetComponent (this.GetType()));//为了保证灵活性，也还是应该使用人工计时的方法
	}


	void OnDestroy()
	{
		if(theEffect)
		Destroy (theEffect .gameObject );
	}

}
