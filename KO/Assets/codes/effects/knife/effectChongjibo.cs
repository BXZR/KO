using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class effectChongjibo: effectBasic {


	GameObject chongjibo;
	Vector3 forward;
	float lastingTime =0.4f;
	float speed = 5f;

	float trueDamageAdd = 70;//额外的真实伤害最少
	float coolingTime = 8f;//这个技能太强，需要有冷却时间
	bool cuted =false;//这个斩击持续时间内只可以有一次
	Animator theAnimationController ;//人物动画控制器

	void Start () 
	{
      Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "雄霸天下";
		theEffectInformation ="大力斩击发出可以击退目标的刀气\n刀气额外造成"+trueDamageAdd+ "真实伤害\n这个技能每"+coolingTime+"秒内只能使用一次\n冷却中使用会转化为一次上挑斩击";

		makeStart ();
		forward = this.thePlayer.transform.forward;
		chongjibo = (GameObject)GameObject .Instantiate(  (GameObject)Resources.Load ("effects/chongjibo"));
		Destroy (chongjibo,lastingTime);
		chongjibo.GetComponent <extraWeapon> ().setPlayer (this.thePlayer);
		forward = thePlayer.transform.forward;//记录向前的方向

		Vector3 positionNew = thePlayer.transform.position + new Vector3 (0,0.5f,forward .normalized.z*0.1f) ;
		chongjibo.transform.position = positionNew  ;

		chongjibo.transform.forward = forward;
		theAnimationController = this.thePlayer.GetComponentInChildren<Animator> ();
		//print (SceneManager.GetActiveScene().name );
		if( SceneManager.GetActiveScene().name == systemValues .theFightSceneName)
		Destroy (this.gameObject .GetComponent(this.GetType()),coolingTime);
		else//非战斗场景把波放出去就直接冷却
		Destroy (this.gameObject .GetComponent(this.GetType()),lastingTime);	
	} 

	public override void updateEffect ()
	{
 
		theAnimationController  .Play("extraAttack" );
		//this.thePlayer.ActerHp *= 0.99f;
	}



	public override void effectDestory ()
	{
		if (chongjibo)
		{
			try
			{
				Destroy (chongjibo );
			}
			catch(Exception d)
			{
				//print (d.ToString());
			}
		}
	}


	public override void OnAttack (PlayerBasic aim)
	{
		if (cuted == false)
		{
			cuted = true;
			aim.ActerHp -= trueDamageAdd ;
		}
	}
	void OnDestroy()
	{
		effectDestory ();
	}

	void Update ()
	{
 
		if (chongjibo)
			chongjibo.transform.Translate ( new Vector3(0,0 ,1) *speed*Time .deltaTime);
	}

}
