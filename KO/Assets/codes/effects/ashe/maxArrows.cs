using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class maxArrows :  effectBasic {

	//因为艾希普通攻击的升级版，万箭齐发

	GameObject Arrow;//弹矢引用保存
	Vector3 forward;
	float arrowLife = 0.3f;// 弹矢生存时间
	float lastingTime =10f;//根据规则产生的脚本覆盖时间间隔，这个时间越短，但是发射频率越高，也就是攻速越快
	float maxCount =3;//最多弹矢数量
	GameObject theArrow;//真正的弹矢

	float damageTruePercent = 0.3f;//转化的真实伤害百分比

	int makeDamageCount = 0;//因为有冷却时间，这个伤害需要计数器进行控制

	void Start () 
	{
		Init ();//进行初始化
	}


	public override void effectDestory ()
	{
		if (theArrow)
		{
			try
			{
				Destroy (theArrow );
			}
			catch(Exception d)
			{
				//print (d.ToString());
			}
		}
	}
 
	public override void OnAttack (PlayerBasic aim, float TrueDamage)
	{
		
		if (makeDamageCount < maxCount)
		{
			makeDamageCount++;
			aim.ActerHp += TrueDamage * damageTruePercent;
			aim.ActerHp -= this.thePlayer.ActerWuliDamage * damageTruePercent;
		}
	}


	public override void Init ()
	{
		theEffectName = "万箭齐发";
		theEffectInformation ="连续发射"+maxCount+"支特殊的箭矢\n其攻击力的"+damageTruePercent*100+"%转化为真实伤害\n此技能"+lastingTime+"秒内仅可以使用一次\n冷却中使用将转化为普通射击";
		makeStart ();
		forward = this.thePlayer.transform.forward;
		Arrow = (GameObject)  Resources.Load ("effects/aShe_Arrow");


		StartCoroutine(arrows());
	} 

	public override void updateEffect ()
	{
		forward = this.thePlayer.transform.forward;
		Arrow = (GameObject)  Resources.Load ("effects/aShe_Arrow");
		/////////
		theArrow = (GameObject)GameObject .Instantiate( Arrow);
		theArrow.GetComponent <extraWeapon> ().setPlayer (this.thePlayer);

		Vector3 positionNew = thePlayer.transform.position + new Vector3 (0,0.9f,forward .normalized.z*0.1f) ;
		theArrow.transform.position = positionNew  ;
		theArrow.transform.forward = thePlayer.transform.forward;

		Destroy (theArrow,arrowLife);
	}

	IEnumerator arrows()
	{
		for (int i = 0; i < maxCount; i++) 
		{
			theArrow = (GameObject)GameObject.Instantiate (Arrow);
			theArrow.GetComponent <extraWeapon> ().setPlayer (this.thePlayer);

			Vector3 positionNew = thePlayer.transform.position + new Vector3 (0, 1f, forward.normalized.z * 0.1f);
			theArrow.transform.position = positionNew;
			theArrow.transform.forward = thePlayer.transform.forward;

			Destroy (theArrow, arrowLife);
			Destroy (this.GetComponent (this.GetType ()), lastingTime);
			yield return new WaitForSeconds (0.1f);
		}
		StopAllCoroutines ();
	}

}
