using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectSteps :  effectBasic {

	//刀哥的被动

	float timer =0.15f;//突进时间长度
	float speed =12f;//突进速度
	float hpupAdder = 0.06f;//最大恢复生命百分比
	float hpupPerSecond ;//每秒钟恢复的生命
	float lastingTimer =6f;//冷却时间
	move theMoveController ;//移动控制器
	GameObject theEffect;//显示效果

	void Start () 
	{

		Init ();//进行初始化
		theMoveController = thePlayer .GetComponent <move>();
		theMoveController.canMove = false;
		Destroy (this.gameObject.GetComponent (this.GetType ()),lastingTimer);
	}

	public override void Init ()
	{
		theEffectName = "退进步";
		theEffectInformation ="向前高速突进，持续"+timer+"秒\n突进时受到攻击就会中断\n突进期间持续恢复共"+hpupAdder*100+"%最大生命\n此技能每"+lastingTimer+"秒仅可使用一次";
		makeStart ();
		hpupPerSecond = thePlayer.ActerHpMax * hpupAdder / timer;
		theEffect = (GameObject)Resources.Load ("effects/slash");
		theEffect = GameObject.Instantiate (theEffect);
		theEffect.transform.SetParent (this.thePlayer .transform );
		theEffect.transform.localPosition = new Vector3 (0,0.2f,0);
		theEffect.transform.localRotation = Quaternion.identity;
	} 

	public override void OnBeAttack (float damage = 0)
	{
		timer = -1;//通过计时器中断突进
		theMoveController.canMove = true;
		Destroy (this.gameObject.GetComponent (this.GetType ()));
	}

	void OnDestroy()
	{
		theMoveController.canMove = true;
		if (theEffect)
			Destroy (theEffect .gameObject);
	}
	void Update ()
	{
		if (timer > 0) 
		{
			timer -= Time.deltaTime;
			theMoveController.beMakeForward (speed);//向前突进
			thePlayer.ActerHp += hpupPerSecond*Time .deltaTime;
			if (timer < 0)
			{
				theMoveController.canMove = true;
				if (theEffect)
					Destroy (theEffect .gameObject);

			}
		}
	}


}
