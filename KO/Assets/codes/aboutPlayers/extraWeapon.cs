﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extraWeapon : MonoBehaviour {

	private PlayerBasic thePlayer;//引用保存，各种信息调用
	private bool canMakeDamage = true;


	public void setPlayer(PlayerBasic thePlayerIn)
	{
		thePlayer = thePlayerIn;
	}
		
	private void extraDamageEffect(PlayerBasic playerAim)//额外添加挂在的计算脚本
	{
		
		if (string.IsNullOrEmpty (thePlayer . conNameToEMY) == false)//效果不可叠加
		{
			if(!playerAim.gameObject.GetComponent (System.Type.GetType (thePlayer . conNameToEMY)))
			{
				try
				{
					playerAim.gameObject.AddComponent (System.Type.GetType (thePlayer . conNameToEMY) );
					//print("makeEffect3");
				}
				catch
				{
					//无法添加这个效果
					//那么就转换成伤害，造成2点真实伤害
					thePlayer.OnAttack (playerAim,2,true);
					//print ("canNotAddEMY");
				}
			}
			else
			{
				effectBasic theEffect = playerAim.gameObject.GetComponent (System.Type.GetType (thePlayer . conNameToEMY)) as effectBasic;
				theEffect.updateEffect ();
				//print ("update");
			}
			thePlayer .conNameToEMY= "";
		}


	}

	void OnTriggerEnter(Collider collisioner)
	{ 

		if (thePlayer && canMakeDamage == true) 
		{

			PlayerBasic playerAim = collisioner.gameObject.GetComponent <PlayerBasic> ();
			if (playerAim == null)
				playerAim = collisioner.gameObject.GetComponentInParent<PlayerBasic> ();
			if (playerAim == null)
				playerAim = collisioner.gameObject.GetComponentInChildren<PlayerBasic> ();

			if (thePlayer && 　playerAim && playerAim != thePlayer)
			{
				canMakeDamage = false;
				//print (playerAim.ActerHpSuck );
				//print (thePlayer .ActerWuliIner);

				thePlayer.OnAttackExtra(playerAim);//造成直接的伤害
				extraDamageEffect (playerAim);//添加额外的计算脚本，每个脚本的效果由脚本自己决定
			}
		} 

	} 
}