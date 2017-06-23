using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectHanbingAcher : effectBasic {

	private float lastingTime = 2.5f;//下过持续时间
	private float extraPercentDamage = 0.10f;//额外伤害百分比
	private float movePercentMinus = 0.15f;//额外减速
	private bool attacked= false;//是否已经增加了额外伤害
	void Start () 
	{

		Init ();//进行初始化
	}

	public override bool isBE ()
	{
		return true;
	}
	public override void Init ()
	{
		theEffectName = "冰霜射击";
		theEffectInformation ="施加冰霜效果，减少目标"+ (movePercentMinus*100).ToString("f0") +"%移动速度\n对有冰霜效果的目标首次伤害提升"+extraPercentDamage*100+"%\n" +
			"冰霜效果持续"+lastingTime+"秒\n";
		makeStart ();
	}
	public override void OnAttack (PlayerBasic aim)
	{
		if (!aim.GetComponent <EffectHanBing> ())
			aim.gameObject.AddComponent <EffectHanBing>();
	}
}
