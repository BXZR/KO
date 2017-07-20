using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectHanBing :effectBasic
{

	private float lastingTime = 2f;//下过持续时间
	private float extraPercentDamage = 0.20f;//额外伤害百分比
	private float movePercentMinus = 0.10f;//额外减速
	private bool attacked= false;//是否已经增加了额外伤害
	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "冰霜射击";
		theEffectInformation ="施加冰霜效果，减少目标"+ (movePercentMinus*100).ToString("f0") +"%移动速度\n对有冰霜效果的目标首次伤害提升"+extraPercentDamage*100+"%\n" +
			"冰霜效果持续"+lastingTime+"秒\n";
		makeStart ();
		this.thePlayer.ActerMoveSpeedPercent -= movePercentMinus;
		Destroy (this.gameObject .GetComponent (this.GetType()),lastingTime);
	}
 

	public override void OnBeAttack (float damage = 0)
	{
		if (thePlayer && attacked == false) 
		{
			thePlayer.ActerHp -= damage * extraPercentDamage;
			attacked = true;
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
 
}
