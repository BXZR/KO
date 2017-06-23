using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWeapon : MonoBehaviour 
{
 
    //这个是武器碰撞攻击脚本
	//用的方法就是碰撞检测
	//这种方法繁的缺点是可能会造成额外的伤害

	private PlayerBasic thePlayer;//引用保存，各种信息调用
	private AudioSource theSource;//声音音源引用保存
    
	public static bool isStarted =false;//是否开启


	public void extraEffectSELF()
	{
		if (string.IsNullOrEmpty (thePlayer . conNameToSELF) == false ) //效果不可叠加
		{
			if (!thePlayer.gameObject.GetComponent (System.Type.GetType (thePlayer . conNameToSELF))) {
				try
				{
					thePlayer.gameObject.AddComponent (System.Type.GetType (thePlayer . conNameToSELF));
				}
				catch 
				{
					//无法添加这个效果
					//那么就转换成恢复效果，恢复2生命
					thePlayer.ActerHp += 2f;
					//print ("canNotAddSELF");
				}
			} 
			else
			{
				effectBasic theEffect = thePlayer.gameObject.GetComponent (System.Type.GetType (thePlayer . conNameToSELF)) as effectBasic;
				theEffect.updateEffect ();
			}
			thePlayer . conNameToSELF = "";
		}
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
		if (thePlayer && thePlayer . canHit && isStarted) 
		{
			
			PlayerBasic playerAim = collisioner.gameObject.GetComponent <PlayerBasic> ();
			if (playerAim == null)
				playerAim = collisioner.gameObject.GetComponentInParent<PlayerBasic> ();
			if (playerAim == null)
				playerAim = collisioner.gameObject.GetComponentInChildren<PlayerBasic> ();
			if (thePlayer &&　playerAim && playerAim != thePlayer) 
			{
				
				thePlayer . canHit = false;
				//print (playerAim.ActerHpSuck );
				//print (thePlayer .ActerWuliIner);
				thePlayer.OnAttack (playerAim,0,false);//造成直接的伤害
				extraDamageEffect (playerAim);//添加额外的计算脚本，每个脚本的效果由脚本自己决定
				if (theSource)
					theSource.PlayOneShot (theSource .clip);//击中目标播放音效
			}
		}
	} 
	void OnCollisionEnter(Collision collisioner)
	{
		
	}

	void Start () 
	{
		thePlayer = this.GetComponentInParent<PlayerBasic> ();
		//print ("thgPlayer's name = " + thePlayer .name);
		InvokeRepeating("extraEffectSELF",0,systemValues .updateTimeWait);//每0.1秒检查自身添加特效
		theSource = this.GetComponent<AudioSource>();
	}

	public void makeStart()//初始化方法，由总控单元统一进行初始化
	{
		isStarted = true;
	}

	public void setPlayer(PlayerBasic thePlayerIn)
	{
		thePlayer = thePlayerIn;
	}

 
}
