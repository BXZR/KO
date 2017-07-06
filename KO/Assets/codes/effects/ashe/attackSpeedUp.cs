using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class attackSpeedUp :  effectBasic {

	float coolingTime = 17f;//冷却时间，这个技能太强大了，如果没有冷却时间会出事
	float lastingTime = 3f;//眩晕时间
	GameObject theEffect;//效果
	Transform theArm;
	private  bool isOpened = false;//效果开启
	private int coutHpUp =10;//可吸血上限次数
	private float theSpeedAdd =0.3f;//增加XXXX%速度
	/*********************************************/
	GameObject Arrow;//弹矢引用保存
	GameObject theArrow;
	Vector3 forward;
	float arrowLife = 0.3f;// 弹矢生存时间
	float hpup = 20f;//攻击命中的恢复的生命值

	bool doubled = false;

	void Start () 
	{
		Init ();//进行初始化
	}
	public override void Init ()
	{
		theEffectName = "射手的专注";
		theEffectInformation = "速度增加"+theSpeedAdd *100+"%，持续"+lastingTime+"秒,冷却"+coolingTime+"秒\n下一击造成双倍伤害（不附加特效）\n攻击命中恢复"+hpup+"生命值,最多触发"+coutHpUp+"次\n冷却中使用将转化为慢速普通射击";
		makeStart ();
		makeAdd ();

		theEffect = GameObject.Instantiate<GameObject> (Resources.Load<GameObject> ("effects/asheSpeedUp"));
		theArm =   this.GetComponentInChildren<playerWeapon>().gameObject .transform  ;
		theEffect.transform.SetParent (theArm);
		theEffect.transform.localPosition = Vector3.zero;

		isOpened = true;
		Destroy (this.GetComponent(this.GetType()),coolingTime);
	}

	public override void updateEffect ()
	{
		//这个加速技能在冷却的时候却比较慢，所以需要一个延迟的时间来匹配动作
		Invoke ("forUpdate",0.3f);
	}


	private  void forUpdate()
	{
		forward = this.thePlayer.transform.forward;
		Arrow = (GameObject)  Resources.Load ("effects/aShe_Arrow");
		/////////
		theArrow = (GameObject)GameObject .Instantiate( Arrow);
		theArrow.GetComponent <extraWeapon> ().setPlayer (this.thePlayer);

		Vector3 positionNew = thePlayer.transform.position + new Vector3 (0,1f,forward .normalized.z*0.1f) ;
		theArrow.transform.position = positionNew  ;
		theArrow.transform.forward = thePlayer.transform.forward;

		Destroy (theArrow,arrowLife);
	}

	//这个方法会由挂上的人物的PlayerBasi统一调用，调用时间间隔为1秒
	public override void effectOnUpdate ()
	{
		if (isOpened)
		{
			lastingTime --;
			if (lastingTime < 0) 
			{
				makeOver ();
				isOpened = false;
				if(theEffect)
				Destroy (theEffect .gameObject );
			}
		}
	}

	public override void OnAttack ()
	{
		if (isOpened && coutHpUp > 0) 
		{
			coutHpUp--;
			this.thePlayer.ActerHp += hpup;
		}
	}

	public override void OnAttack (PlayerBasic aim)
	{
		if(	doubled == false)
		{
			doubled = true;
		this.thePlayer.OnAttackWithoutEffect (aim);//不附加特效
		}
	}

	void  makeAdd()
	{
		//Animator theAction =	this.thePlayer.GetComponentInChildren<Animator> ();
		//theAction  .speed += attackSpeedAdd;

		//只修改某一层的某一个动画的播放速度的代码
		//很有用，千万注意编译出去不能够使用，因为是Editor
		Animator theAction =	this.thePlayer.GetComponentInChildren<Animator> ();
		theAction .speed += theSpeedAdd ;
		this.thePlayer.ActerMoveSpeedPercent += theSpeedAdd;
		/*UnityEditor.Animations .AnimatorController ac = theAction.runtimeAnimatorController as UnityEditor.Animations .AnimatorController  ;
		int idForLayer = systemValues.theAttackLayerIndex;
		for (int i = 0; i < ac.layers [idForLayer].stateMachine.states.Length; i++) 
		{    
			if (ac.layers [idForLayer].stateMachine.states [i].state.name == "punch1")
			{    
				ac.layers [idForLayer].stateMachine.states [i].state.speed += attackSpeedAdd;    
			}    
		} */
	}

	void makeOver()
	{
		//Animator theAction =	this.thePlayer.GacetComponentInChildren<Animator> ();
		//theAction.speed -= attackSpeedAdd;

		Animator theAction =	this.thePlayer.GetComponentInChildren<Animator> ();
		theAction .speed -= theSpeedAdd;
		this.thePlayer.ActerMoveSpeedPercent -= theSpeedAdd;
		/*
		UnityEditor.Animations .AnimatorController ac = theAction.runtimeAnimatorController as UnityEditor.Animations .AnimatorController  ;
		int idForLayer = systemValues.theAttackLayerIndex;
		for (int i = 0; i < ac.layers [idForLayer].stateMachine.states.Length; i++) 
		{    
			if (ac.layers [idForLayer].stateMachine.states [i].state.name == "punch1") 
			{    
				ac.layers [idForLayer].stateMachine.states [i].state.speed -= attackSpeedAdd;    
			}    
		} 
		*/
	    
	}


	void OnDestroy()
	{
		effectDestory ();
	}


	public override void effectDestory ()
	{
		if(theEffect)
			Destroy (theEffect .gameObject );
		if (isOpened) 
			makeOver ();
	}
}
