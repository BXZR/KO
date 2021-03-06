﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectDemonMind : effectBasic {
 
	int  theBeAttackCount =0;//受到攻击的次数
	float hpupPercent = 0.05f;//最大生命值的百分比
	float percentAdd = 0.075f;
	float theBeAttckCountMax = 10;//收到XX次数的攻击就会触发某一些效果

	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "恶魔之力";
		theEffectInformation ="所有攻击伤害效果提升7.5%\n攻击命中额外消耗自身1%当前生命值\n每受到10次攻击，恢复5%最大生命值";
		makeStart ();
		this.thePlayer.ActerDamageAdderPercent += percentAdd ;
	}


	public override bool isBE ()
	{
		return true;//这个是被动技能
	}

	public override void OnAttack (PlayerBasic aim)
	{
		if(this.thePlayer .ActerHp >25f)
			this.thePlayer.ActerHp *= 0.99f;
		 
	}

	public override void OnBeAttack (float damage = 0)
	{
		theBeAttackCount++;
		if (theBeAttackCount >= theBeAttckCountMax) 
		{
			theBeAttackCount = 0;
			thePlayer.ActerHp += thePlayer.ActerHpMax * hpupPercent;//恢复一定的百分比最大生命值
		}
	}
 
	public override void effectDestory ()
	{
		this.thePlayer.ActerDamageAdderPercent -= percentAdd ;
	}

	void OnDestroy()
	{
		effectDestory ();
	}

 
}
