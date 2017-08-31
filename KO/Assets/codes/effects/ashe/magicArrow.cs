using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class magicArrow :  effectBasic {
  
	//魔法水晶箭
	float lastingTime = 2.5f;//眩晕时间
	//这个眩晕的时间是jiyun脚本里面设定的，现在还不知道有什么好办法联系这二者
	GameObject Arrow;//弹矢引用保存
	Vector3 forward;
	float arrowLife = 7f;// 弹矢生存时间
	float arrowLifeupdate = 0.3f;// 弹矢生存时间
	float coolingTime = 13f;//冷却时间
	GameObject theArrow ;//真正的弹矢
 
	void Start () 
	{
		Init ();//进行初始化
	}



	public override void Init ()
	{
		theEffectName = "魔法水晶箭";
		theEffectInformation ="发射可击晕目标"+lastingTime+"秒的魔法水晶箭\n使任何伤害追加1%最大生命值真实伤害\n这个技能每"+coolingTime+"秒仅可以使用一次\n冷却中使用将转化为普通射击";
		makeStart ();
		forward = this.thePlayer.transform.forward.normalized;
		Arrow = (GameObject)  Resources.Load ("effects/aShe_Arrow_Magic");
		/////////
		theArrow = (GameObject)GameObject .Instantiate( Arrow);
		theArrow.GetComponent <extraWeapon> ().setPlayer (this.thePlayer);

		Vector3 positionNew = thePlayer.transform.position +  new Vector3 (0,0.7f*thePlayer .transform .localScale .y , forward .normalized.z*0.1f) ;
		theArrow.transform .position = positionNew;
		theArrow.transform.localScale *= thePlayer.transform.localScale.y;

		 
		GameObject theEMY = systemValues.getEMY (this.thePlayer.transform);
		if(theEMY !=null)
		{
		Vector3 emyPosition = theEMY .transform.position;
		Vector3 theMoveForward = (emyPosition - this.thePlayer.transform.position).normalized;   
		theArrow.transform.LookAt (emyPosition+new Vector3 (0,1,0) );
		//艾希的箭矢有一个扇形的射击范围，超过这个范围箭矢就不会射中的
		if (theMoveForward.z * thePlayer.transform.forward.z < 0)
			theMoveForward = thePlayer.transform.forward;
		else
			theMoveForward = new Vector3 (theMoveForward.x, Mathf.Clamp (theMoveForward.y, -0.25f, 0.25f),theMoveForward .z);
		
		theArrow.transform .forward   = theMoveForward;
		}
		else
		{
			theArrow.transform .forward   = thePlayer .transform .forward;
		}
		//添加额外的击晕特效(算是比较动态的方法了)
		//-------------------------------------------
		theArrow .gameObject .AddComponent<extraEffectMaker>();
		extraEffectMaker theMaker = theArrow.gameObject.GetComponent <extraEffectMaker> ();
		theMaker.thePlayer = this.thePlayer;
		theMaker.theEffectName = "jiyun";
		theMaker.theEffectTimer = this.lastingTime;
		//------------------------------------------

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

		forward = this.thePlayer.transform.forward;
		Arrow = (GameObject)  Resources.Load ("effects/aShe_Arrow");
		/////////
		theArrow = (GameObject)GameObject .Instantiate( Arrow);
		theArrow.GetComponent <extraWeapon> ().setPlayer (this.thePlayer);

		Vector3 positionNew = thePlayer.transform.position + new Vector3 (0,0.8f*thePlayer .transform .localScale .y , forward .normalized.z*0.2f) ;
		theArrow.name = "magicArrow_2";
		theArrow.transform.localScale *= thePlayer.transform.localScale.y;
		theArrow.transform.position = positionNew  ;

		 
		GameObject theEMY = systemValues.getEMY (this.thePlayer.transform);
		if(theEMY !=null)
		{
		Vector3 emyPosition = theEMY .transform.position;
		Vector3 theMoveForward = (emyPosition - this.thePlayer.transform.position).normalized;   
		theArrow.transform.LookAt (emyPosition+new Vector3 (0,1,0) );
		//艾希的箭矢有一个扇形的射击范围，超过这个范围箭矢就不会射中的
		if (theMoveForward.z * thePlayer.transform.forward.z < 0)
			theMoveForward = thePlayer.transform.forward;
		else
			theMoveForward = new Vector3 (theMoveForward.x, Mathf.Clamp (theMoveForward.y, -0.25f, 0.25f),theMoveForward .z);
		
		theArrow.transform .forward   = theMoveForward;
		}
		else
		{
			theArrow.transform .forward   = thePlayer .transform .forward;
		}
		Destroy (theArrow,arrowLifeupdate);
	}

    public override void OnAttack (PlayerBasic aim)
	{
		if (aim.GetComponent <jiyun> ())
			aim.ActerHp -= aim.ActerHpMax * 0.01f;
	}
}
