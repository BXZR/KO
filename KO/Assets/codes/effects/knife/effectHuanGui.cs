using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//幻鬼三叠杀
public class effectHuanGui :   effectBasic {

	//刀哥的被动

	int step =0;//不同阶段的不同效果
	float  timerForLive = 2f;//这个脚本的生存时间

	//第一次
	float aimDamageMinus = 0.15f;//削减15伤害
	float damageMinus = 0;//寄存被削减的攻击力
	PlayerBasic theAim ;//保留目标引用

	//第二次
	float acterhpUp = 20;//直接回复的生命值
	float actershieldHp = 15;//获得的护盾值
	//第三次
	float damagePercent = 0.04f;//追加的斩杀效果伤害百分比

	void Start () 
	{

		Init ();//进行初始化

	}

	void OnDestroy()
	{
		//第一次
		if(theAim)
		theAim .ActerWuliDamage += damageMinus;
	}

	public override void Init ()
	{
		theEffectName = "阿鼻道三刀";
		theEffectInformation ="在"+timerForLive+"秒内的三次攻击命中触发不同特效：\n";
		theEffectInformation += "第一次，削减目标"+aimDamageMinus*100+"%攻击力，最多"+timerForLive+"秒\n";
		theEffectInformation += "第二次，恢复"+acterhpUp+"生命值,获得"+actershieldHp+"护盾\n";
		theEffectInformation += "第三次，造成目标"+damagePercent*100+"%已损生命真实伤害";
		makeStart ();
		Destroy (this.GetComponent (this.GetType()),timerForLive);

	} 

	public override void OnAttack (PlayerBasic aim)
	{
		if (step == 0)
		{ //第一次削减攻击力
			theAim = aim;
			damageMinus = aim.ActerWuliDamage * aimDamageMinus;
			theAim.ActerWuliDamage -= damageMinus;
			step++;
		} 
		else if (step == 1) 
		{//第二次，偷取生命值
			hpup();
			step++;
		}
		else if(step==2)
		{
			float damage = (aim.ActerHpMax - aim.ActerHp) * damagePercent;
			aim.ActerHp -= damage;
		}

	}

	void hpup()
	{
		if (this.thePlayer) 
		{
			this.thePlayer.ActerHp += acterhpUp;
			this.thePlayer.ActerShieldHp += actershieldHp;
		}
	}

}
