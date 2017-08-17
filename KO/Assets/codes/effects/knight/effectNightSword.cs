using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectNightSword  : effectBasic {
 
	float lastingTime = 8f;
	int count =0;
	int countMax =3;
	float damageMinus = 0.35f;//目标减少的输出百分比
	float hpDamgePercent = 0.02f;//最大生命值2%真实伤害


	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "裁决之刃";
		theEffectInformation ="目标攻击基本输出效果减少"+damageMinus*100 +"%\n目标攻击命中消耗"+hpDamgePercent*100+"%最大生命值\n这个效果在持续时间内最多触发"+countMax+"次\n效果持续时间为"+lastingTime+"秒";
		makeStart ();
	} 



	public override void effectOnUpdate ()
	{
		lastingTime --;

		if (lastingTime <= 0  || thePlayer .isAlive ==false)
		{
			Destroy (this.gameObject .GetComponent(this.GetType()));//为了保证灵活性，也还是应该使用人工计时的方法
		}
	}

	public override void OnAttack (PlayerBasic aim)
	{
		aim.ActerHp += this.thePlayer .ActerWuliDamage * damageMinus;
		count++;
		if (count <= countMax && this.thePlayer)
		{
			float theDamage = this.thePlayer.ActerHpMax * hpDamgePercent;
			this.thePlayer.addDamageRead (theDamage);
			this.thePlayer.ActerHp -= theDamage;
		}
		else
			Destroy (this.gameObject .GetComponent(this.GetType()));

	}

 
}
