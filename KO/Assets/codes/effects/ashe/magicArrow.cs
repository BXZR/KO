using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


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
	float damageLater = 60;

	void Start () 
	{
		Init ();//进行初始化
	}



	public override void Init ()
	{
		theEffectName = "魔法水晶箭";
		theEffectInformation ="发射水晶箭冰冻目标"+lastingTime+"秒\n冰冻期间再次使用可造成"+damageLater+"后续伤害\n这个技能每"+coolingTime+"秒仅可以使用一次\n冷却中使用将转化为普通射击";
		makeStart ();
		forward = this.thePlayer.transform.forward.normalized;
		Arrow = (GameObject)  Resources.Load ("effects/aShe_Arrow_Magic");
		/////////
		theArrow = (GameObject)GameObject .Instantiate( Arrow);
		theArrow.GetComponent <extraWeapon> ().setPlayer (this.thePlayer);

		Vector3 positionNew = thePlayer.transform.position +  new Vector3 (0,0.7f*thePlayer .transform .localScale .y , forward .normalized.z*0.1f) ;
		theArrow.transform .position = positionNew;
		theArrow.transform.localScale *= thePlayer.transform.localScale.y;
		theArrow.transform.forward = thePlayer.transform.forward;
		if (SceneManager.GetActiveScene ().name == systemValues .theFightSceneName) 
		{
			Destroy (theArrow, arrowLife);
			Destroy (this.GetComponent (this.GetType ()), coolingTime);
		}
		else 
		{
			Destroy (theArrow, 0.2f);
			Destroy (this.GetComponent (this.GetType ()), 0.2f);
		}

	}

	//手动调用的额外销毁方法
	public override void effectDestoryExtra ()
	{
		Destroy (theArrow);
	}

	 
	public override void effectDestory ()
	{
		if (theArrow)
		{
			try
			{
				Destroy (theArrow );
			}
			catch
			{
				print ("箭矢销毁失败");
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

		Vector3 positionNew = thePlayer.transform.position + new Vector3 (0,0.8f*thePlayer .transform .localScale .y , forward .normalized.z*0.2f) ;
		theArrow.name = "magicArrow_2";
		theArrow.transform.localScale *= thePlayer.transform.localScale.y;
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
