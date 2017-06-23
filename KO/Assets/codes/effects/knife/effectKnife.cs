using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectKnife :  effectBasic {

	//刀哥的被动

	int count =0;//计数器
	int countMax = 7;//计数器的上限
	float spup =20;//恢复的斗气

	float  baojiPercent = 0.15f;//暴击几率增加
	float baojiDamageAdd = 0.2f;//暴击伤害增加

	//下面的内容事实上过于耦合了，在这里有一些追求效果了，后期可以考虑想办法优化
	public GameObject theKnife;//用于控制刀的颜色
	private Color normalColor = Color .white;//没有效果的时候的颜色
	private Color effectColor = Color.blue;//有效果的时候的颜色

	public override bool isBE ()
	{
		return true;//这个是被动技能
	}

	void Start () 
	{

		Init ();//进行初始化

	}

	public override void Init ()
	{
		theEffectName = "霸刀";
		theEffectInformation ="每第"+countMax+"次攻击造成两次伤害并回复"+spup+"斗气值\n此外，此人享有额外"+baojiPercent*100+"%的暴力几率和"+baojiDamageAdd*100+"%的暴击伤害";
		makeStart ();
		thePlayer.ActerSuperBaldePercent += baojiPercent;
		thePlayer.CActerSuperBaldePercent += baojiPercent;
		thePlayer.ActerSuperBaldeAdder += baojiDamageAdd;
		thePlayer.CActerSuperBaldeAdder += baojiDamageAdd;

		//耦合部分 
		effectColor = new Color (1f, 0.5f, 0f);//比较深的橙红色

	} 
	void OnDestroy()
	{
		thePlayer.ActerSuperBaldePercent -= baojiPercent;
		thePlayer.CActerSuperBaldePercent -= baojiPercent;
		thePlayer.ActerSuperBaldeAdder -= baojiDamageAdd;
		thePlayer.CActerSuperBaldeAdder -= baojiDamageAdd;

	}
	public override void OnAttack (PlayerBasic aim)
	{
		count++;
		if (count == countMax - 1) 
		{
			if(theKnife)
			theKnife.GetComponentInChildren<MeshRenderer> ().material.color = effectColor;
		}
		if (count >= countMax) 
		{
			count = 0;
			this.thePlayer.OnAttack (aim);
			this.thePlayer.ActerSp += spup;//恢复斗气
			if(theKnife)
			theKnife.GetComponentInChildren<MeshRenderer> ().material.color = normalColor;
		}

	}


}
