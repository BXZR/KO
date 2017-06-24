using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class magicArrow :  effectBasic {
  
	//魔法水晶箭
	float lastingTime = 2.5f;//眩晕时间
	GameObject Arrow;//弹矢引用保存
	Vector3 forward;
	float arrowLife = 7f;// 弹矢生存时间
	float arrowLifeupdate = 0.3f;// 弹矢生存时间
	float coolingTime = 13f;//冷却时间
	GameObject theArrow ;//真正的弹矢
	private bool  yun =false;//是否已经使用了击晕

	public static jiyun jiyunSave;//保留引用
	float damageLater = 50;

	void Start () 
	{
		Init ();//进行初始化
	}
	public override void Init ()
	{
		theEffectName = "魔法水晶箭";
		theEffectInformation ="发射水晶箭冰冻目标"+lastingTime+"秒\n冰冻期间再次使用可造成"+damageLater+"后续伤害\n这个技能每"+coolingTime+"秒仅可以使用一次\n冷却中使用将转化为普通射击";
		makeStart ();
		forward = this.thePlayer.transform.forward;
		Arrow = (GameObject)  Resources.Load ("effects/aShe_Arrow_Magic");
		/////////
		theArrow = (GameObject)GameObject .Instantiate( Arrow);
		theArrow.GetComponent <extraWeapon> ().setPlayer (this.thePlayer);

		Vector3 positionNew = thePlayer.transform.position + new Vector3 (0,0.9f,forward .normalized.z*0.1f) ;
		theArrow.transform.position = positionNew ;
		theArrow.transform.forward = thePlayer.transform.forward;
		Destroy (theArrow,arrowLife);
		Destroy (this.GetComponent(this.GetType()),coolingTime);
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
		

	public override void updateEffect ()
	{

		if (jiyunSave != null) 
		{
			jiyunSave.GetComponent <PlayerBasic> ().ActerHp -= damageLater;
			Destroy (jiyunSave);
		}

		forward = this.thePlayer.transform.forward;
		Arrow = (GameObject)  Resources.Load ("effects/aShe_Arrow");
		/////////
		theArrow = (GameObject)GameObject .Instantiate( Arrow);
		theArrow.GetComponent <extraWeapon> ().setPlayer (this.thePlayer);

		Vector3 positionNew = thePlayer.transform.position + new Vector3 (0,0.9f,forward .normalized.z*0.1f) ;
		theArrow.transform.position = positionNew  ;
		theArrow.transform.forward = thePlayer.transform.forward;

		Destroy (theArrow,arrowLifeupdate);
	}

    public override void OnAttack (PlayerBasic aim)
	{
		if (yun== false)
		{
			if (!aim.GetComponent <jiyun> ())
				aim.gameObject.AddComponent <jiyun> ();
			aim.GetComponent <jiyun> ().lastingTime = this.lastingTime;
			if (jiyunSave == null)
				jiyunSave = aim.GetComponent <jiyun> ();
			yun = true;
		}
	}
}
