using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectFoHpSuck : effectBasic {
	//攻击的吸血效果
	public int hpSuck =3;//每一次发起攻击吸收的生命值
	public float hpSuckPercent = 0.07f;//攻击伤害的10%转化为生命
	public float percentUpdate = 3f;//额外增加的百分比
	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "血之渴望";
		//注意的是，最大生命值每回合都会更新的，这个最大生命值的削弱仅仅限制于本回合(如果削减最大斗气值就太变态了)
		theEffectInformation ="吸血效果增加("+hpSuck+"直接吸血+"+ hpSuckPercent*100+"%伤害吸血),"+"\n斗气消耗的20%会转化为护盾生命值\n且攻击能随机偷取1-3目标最大生命值\n每损失1%生命直接吸血效率提升"+percentUpdate +"%";
		makeStart ();
		this.thePlayer.ActerHpSuck += this.hpSuck;
		this.thePlayer.CActerHpSuck += this.hpSuck;
		this.thePlayer.ActerHpSuckPercent += hpSuckPercent;
		this.thePlayer.CActerHpSuckPercent += hpSuckPercent;
		//Debug .Log(this.thePlayer .name +"的额外效果："+ this.getInformation());
	}
 
	public override bool isBE ()
	{
		return true;//这个是被动技能
	}

	public override void OnAttack (PlayerBasic aim)
	{
		float adder = Random.value * 3f;
		aim.ActerHpMax -= adder;
		aim.CActerHpMax -= adder;
		this.thePlayer.ActerHpMax += adder;
		this.thePlayer.CActerHpMax += adder;

		float thePercent = 1 - thePlayer.ActerHp / thePlayer.ActerHpMax;//损失生命百分比
		float hpuper = this.hpSuck * (thePercent *percentUpdate);
		thePlayer.ActerHp += hpuper;
	}

	public override void OnUseSP (float spUse = 0)
	{
		thePlayer.ActerShieldHp += spUse * 0.2f;
	}


	public override void effectDestory ()
	{
		this.thePlayer.ActerHpSuck -= this.hpSuck;
		this.thePlayer.CActerHpSuck -= this.hpSuck;
		this.thePlayer.ActerHpSuckPercent -= hpSuckPercent;
		this.thePlayer.CActerHpSuckPercent -= hpSuckPercent;
	}

	void OnDestroy()
	{
		effectDestory ();
	}
 
}
