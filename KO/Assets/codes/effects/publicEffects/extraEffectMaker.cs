using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extraEffectMaker : MonoBehaviour {


	//一些带击晕的远程攻击并不需要在effect的Onattack里面调用
	//这种效果是弹矢本身的效果，如果没有命中，这个效果无法触发
	//此外因为冷却时间的设定，可能会造成超时的额外效果触发
	//这当然也是一个BUG

	//所以将一些效果额外提出来


	//动态添加额外的脚本
	public string theEffectName;//需要动态添加的效果
	public PlayerBasic thePlayer ; //使用这个效果的人
	public float theEffectTimer ;//持续时间

	private void extraEffect(PlayerBasic playerAim)//额外添加挂在的计算脚本
	{

		if(!playerAim.gameObject.GetComponent (System.Type.GetType (theEffectName)))
			{
				try
				{
				  playerAim.gameObject.AddComponent (System.Type.GetType (theEffectName) );
				  effectBasic theEffect = playerAim.gameObject.GetComponent (System.Type.GetType (theEffectName)) as effectBasic;
				  theEffect  .extraTimer = this.theEffectTimer;
				}
				catch
				{
				}
			}
			else
			{
			    effectBasic theEffect = playerAim.gameObject.GetComponent (System.Type.GetType (theEffectName)) as effectBasic;
				theEffect.updateEffect ();
			}
	}


	void OnTriggerEnter(Collider collisioner)
	{ 

		PlayerBasic playerAim = collisioner.gameObject.GetComponent <PlayerBasic> ();
		if (playerAim == null)
			playerAim = collisioner.gameObject.GetComponentInParent<PlayerBasic> ();
		if (playerAim == null)
			playerAim = collisioner.gameObject.GetComponentInChildren<PlayerBasic> ();

		if (thePlayer && 　playerAim && playerAim != thePlayer)
		{
			//添加额外的计算脚本，每个脚本的效果由脚本自己决定
			extraEffect (playerAim);
		}

    } 
}
