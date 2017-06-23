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

 
//	public override void effectDestory ()
//	{
//		if (theArrow)
//		{
//			try
//			{
//				Destroy (theArrow );
//			}
//			catch(Exception d)
//			{
//				//print (d.ToString());
//			}
//		}
//	}
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

			Vector3 positionNew = thePlayer.transform.position + new Vector3 (0,1f, forward .normalized.z*0.1f) ;
			theArrow.transform.position = positionNew  ;
			theArrow.transform.forward = thePlayer.transform.forward;

			Destroy (theArrow,arrowLife);
		    Destroy (this.GetComponent(this.GetType()),lastingTime);
		} 

	}
