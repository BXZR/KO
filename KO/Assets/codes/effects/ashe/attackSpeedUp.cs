using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class attackSpeedUp :  effectBasic {

	float coolingTime = 17f;//冷却时间，这个技能太强大了，如果没有冷却时间会出事
	float lastingTime = 3f;//眩晕时间
	GameObject theEffect;//效果
	Transform theArm;
	private  bool isOpened = false;//效果开启
	private int coutHpUp =5;//可吸血上限次数
	private float theSpeedAdd =0.3f;//增加XXXX%速度
	/*********************************************/
	GameObject Arrow;//弹矢引用保存
	GameObject theArrow;
	Vector3 forward;
	float arrowLife = 0.3f;// 弹矢生存时间
	float hpup = 12.5f;//攻击命中的恢复的生命值

	int doubled = 2;
	float adderPercent = 0.30f;//增加的百分比
	float adderAbslute = 9f;//增加的真实伤害

	void Start () 
	{
		Init ();//进行初始化
	}
	public override void Init ()
	{
		theEffectName = "射手的专注";
		theEffectInformation = "速度增加"+theSpeedAdd *100+"%，持续"+lastingTime+"秒,冷却"+coolingTime+"秒\n前"+doubled+"次攻击附加("+adderPercent*100+"%+"+adderAbslute+")真实伤害\n攻击命中恢复"+hpup+"生命值,最多触发"+coutHpUp+"次\n冷却中使用将转化为慢速普通射击";
		makeStart ();
		makeAdd ();

		theEffect = GameObject.Instantiate<GameObject> (Resources.Load<GameObject> ("effects/asheSpeedUp"));
		theArm =   this.GetComponentInChildren<playerWeapon>().gameObject .transform  ;
		theEffect.transform.SetParent (theArm);
		theEffect.transform.localPosition = Vector3.zero;
		theEffect .transform .localScale*= thePlayer.transform.localScale.y;
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

		Vector3 positionNew = thePlayer.transform.position + new Vector3 (0,0.8f*thePlayer .transform .localScale .y , forward .normalized.z*0.1f) ;
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
		if(	doubled >0)
		{
		     doubled  --;
			aim.ActerHp -= (this.thePlayer.ActerWuliDamage * adderPercent +adderAbslute);
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
