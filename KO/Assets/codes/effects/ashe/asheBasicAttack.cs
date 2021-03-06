﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class asheBasicAttack : effectBasic {

	//因为艾希的攻击只有远程攻击，所以需要一个基本的脚本用于发射远程攻击
	//这个类有可能成为一个基类,这样就可以重载每一种弹矢的效果
	//当然，主要的攻击方式与冲击波、龟派气功一样，都是远程攻击脚本

	    GameObject Arrow;//弹矢引用保存
		Vector3 forward;
	   float arrowLife = 0.3f;// 弹矢生存时间
		float lastingTime =0.05f;//根据规则产生的脚本覆盖时间间隔，这个时间越短，但是发射频率越高，也就是攻速越快
	   GameObject theArrow ;//真正的弹矢

		void Start () 
		{
			Init ();//进行初始化
		}

    //手动调用的额外销毁方法
	public override void effectDestoryExtra ()
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
		public override void Init ()
		{
			theEffectName = "射击";
		    theEffectInformation ="通过发射远程箭矢造成伤害\n每一支箭矢持续"+arrowLife+"秒";
			makeStart ();
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
		    //print ("move forward for arrow " + theMoveForward);
		    
		    theArrow.transform .forward   = theMoveForward;
			}
			else
			{
				theArrow.transform .forward   = thePlayer .transform .forward;
			}
			Destroy (theArrow, arrowLife);
			Destroy (this.GetComponent (this.GetType ()), lastingTime);

		} 

	}
