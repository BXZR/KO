using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectForKnight: effectBasic {
 
	float timer = 0.5f;//计时器（最开始的时候有一层充能）
	float timerMAx =7f;//时间备份
	bool canAttackAdd =false;//是否可以格外攻击

	public GameObject theSword;//武器，可以不赋值
	private Material theSwordMaterial ;//游戏人物剑上面的材质
	private Color normalColor = Color .white;//没有效果的时候的颜色
	private Color effectColor = Color.red;//有效果的时候的颜色
	private ParticleSystem theEffect;//剑上面的效果
	//使用不同的材质颜色来决定是否现在拥有这个效果


	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		effectColor = new Color (255, 155, 0);
		theEffectName = "逆天之剑";
		theEffectInformation ="每隔" + timerMAx +"秒的下一次攻击能够造成目标最大生命值2%的额外物理伤害\n若自身生命值低于50%，伤害提升为5%\n攻击命中可以减少这个冷却时间0.25秒";
		makeStart ();
		theSwordMaterial = theSword.GetComponent<MeshRenderer> ().material;
		theSwordMaterial.color = normalColor ;
		theEffect = theSword.GetComponentInChildren<ParticleSystem> ();
		theEffect.Stop ();
	}
		

	public override bool isBE ()
	{
		return true;
	}

	public override void OnAttack (PlayerBasic aim)
	{
		if (canAttackAdd) 
		{
			float theDamage = 0;
			if (this.thePlayer.ActerHp / thePlayer.ActerHpMax >= 0.5f) 
			{
				theDamage = aim.ActerHpMax * 0.02f * (1 - aim.ActerWuliShield / 1500);
				aim.ActerHp -= theDamage;
			} 
			else 
			{
				theDamage = aim.ActerHpMax * 0.05f * (1 - aim.ActerWuliShield / 1500);
				aim.ActerHp -= theDamage;
			}
			aim.addDamageRead (theDamage);
			canAttackAdd = false;
			if(theSwordMaterial)
				theSwordMaterial.color = normalColor ;
			theEffect .Stop();
		}
		timer -= 0.25f;
	}
 
	void Update ()
	{
		if (!canAttackAdd) 
		{
			timer -= Time.deltaTime;
			if (timer < 0) 
			{
				canAttackAdd = true;
				timer = timerMAx;
				if(theSwordMaterial)
					theSwordMaterial.color = effectColor;
				theEffect .Play ();
			}
		}
	}
}
