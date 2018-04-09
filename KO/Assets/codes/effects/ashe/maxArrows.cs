﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class maxArrows :  effectBasic {

	//因为艾希普通攻击的升级版，万箭齐发

	GameObject Arrow;//弹矢引用保存
	Vector3 forward;
	float arrowLife = 0.3f;// 弹矢生存时间
	float lastingTime =10f;//根据规则产生的脚本覆盖时间间隔，这个时间越短，但是发射频率越高，也就是攻速越快
	float maxCount =5;//最多弹矢数量
	float damagePercent = 0.9f;//每一支箭矢的攻击力百分比
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
			catch
			{
				//print ("箭矢销毁失败");
			}
		}
	}
 
	public override void OnAttack (PlayerBasic aim, float TrueDamage)
	{
		
		if (makeDamageCount < maxCount)
		{
			makeDamageCount++;
			aim.ActerHp += TrueDamage * (damageTruePercent + (1-damagePercent));
			aim.ActerHp -= this.thePlayer.ActerWuliDamage * damageTruePercent;
		}
	}


	public override void Init ()
	{
		theEffectName = "万箭齐发";
		theEffectInformation ="连续发射"+maxCount+"支具有"+damagePercent*100+"%攻击力的箭矢\n其攻击力的"+damageTruePercent*100+"%转化为真实伤害\n此技能"+lastingTime+"秒内仅可以使用一次\n冷却中使用将转化为普通射击";
		makeStart ();
		forward = this.thePlayer.transform.forward;
		Arrow = (GameObject)  Resources.Load ("effects/aShe_Arrow");
		StartCoroutine (arrows ());

	} 

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

	public override void updateEffect ()
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
		theArrow.name = "maxArrow_2";
		Destroy (theArrow,arrowLife);
	}

	IEnumerator arrows()
	{
		for (int i = 0; i < maxCount; i++) 
		{
			theArrow = (GameObject)GameObject.Instantiate (Arrow);
			theArrow.GetComponent <extraWeapon> ().setPlayer (this.thePlayer);
			//theArrow.GetComponentInChildren<MeshRenderer> ().material.color = Color.magenta;
			Vector3 positionNew = thePlayer.transform.position + new Vector3 (0,0.8f*thePlayer .transform .localScale .y , forward .normalized.z*0.1f) ;
			positionNew += getPositionExtra (i);

			theArrow.transform.localScale *= thePlayer.transform.localScale.y;
			theArrow.transform.position = positionNew;
			 
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
			theArrow.name = "maxArrow";
			Destroy (theArrow, arrowLife);
			Destroy (this.GetComponent (this.GetType ()), lastingTime);
			yield return new WaitForSeconds (0.05f);
		}
		StopAllCoroutines ();
	}

	private int abs = -1;
	Vector3 getPositionExtra(int i)
	{
		if (i == 0)
			return Vector3.zero;
		
		abs = -abs;
		if ((i) % 2 == 1)
			abs *= 2;
		
		return new Vector3 (0, 0.07f, 0) * abs;
	}

}
