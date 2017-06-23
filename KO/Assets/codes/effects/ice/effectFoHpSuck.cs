using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectFoHpSuck : effectBasic {
	//攻击的吸血效果
	public int hpSuck =3;//每一次发起攻击吸收的生命值
	public float hpSuckPercent = 0.07f;//攻击伤害的10%转化为生命

	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "血之渴望";
		theEffectInformation ="吸血效果增加("+hpSuck+"直接吸血+"+ hpSuckPercent*100+"%伤害吸血),"+"\n斗气消耗提升8%,额外伤害不计入吸血\n攻击随机削减1-2目标最大生命值";
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
		aim.ActerHpMax -= Random.value * 2f;
	}

	public override void OnUseSP (float spUse = 0)
	{
		thePlayer.ActerSp -= spUse * 0.08f;
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
