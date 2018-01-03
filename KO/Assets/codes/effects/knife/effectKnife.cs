using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectKnife :  effectBasic {

	//刀哥的被动

	public int count =0;//计数器
	int countMax = 5;//计数器的上限

	float  baojiPercent = 0.12f;//暴击几率增加
	float baojiDamageAdd = 0.12f;//暴击伤害增加

	//下面的内容事实上过于耦合了，在这里有一些追求效果了，后期可以考虑想办法优化
	public GameObject theKnife;//用于控制刀的颜色
	private Color normalColor = Color .white;//没有效果的时候的颜色
	private Color effectColor = Color.blue;//有效果的时候的颜色

	int missCount = 1;//衰减系数
	int missCountAll = 2;//等待的秒数
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
		theEffectInformation ="享有额外"+baojiPercent*100+"%的暴击率和"+baojiDamageAdd*100+"%暴击伤害\n任何攻击命中可叠加刀气，最多"+(countMax-1)+"层\n满层后"+missCountAll +"秒内的下一击造成双倍伤害\n(每一次独立计算暴击和附加效果)";
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


	public override void effectOnUpdate ()
	{
		//只有在收集到一定层数之后才会有衰减
		if (count == countMax - 1) 
		{
			//等待时间，三秒之后衰减
			missCount++;
			if (missCount % missCountAll == 1)
			{
				missCount = 1;
				count =0;
				if (theKnife)
					theKnife.GetComponentInChildren<MeshRenderer> ().material.color = normalColor;
			}
		}

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
			//用count做防止递归的gate
			count = 0;
			this.thePlayer.OnAttack (aim);
			this.thePlayer.OnAttack (aim);

			if(theKnife)
			theKnife.GetComponentInChildren<MeshRenderer> ().material.color = normalColor;
			missCount = 1;
		}

	}


}
