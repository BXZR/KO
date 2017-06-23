using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class newMethodAttack : MonoBehaviour {

	//动画时间点伤害脚本
	//这个是非常严格的攻击脚本
	//这个是在动画中插入关键帧的方法，没有使用碰撞检测的方法制作的
	//但同时也有限制，就是只能用于这种唯一目标的游戏而没有办法很好地模拟武器AOE

	private PlayerBasic theEMY;//目标
	private PlayerBasic thePlayer;//自身
	float theDistance = 0;//距离中间变量

	private void extraEffectSELF()
	{
		if (string.IsNullOrEmpty (thePlayer . conNameToSELF) == false ) //效果不可叠加
		{
			if (!thePlayer.gameObject.GetComponent (System.Type.GetType (thePlayer . conNameToSELF))) {
				try
				{
					thePlayer.gameObject.AddComponent (System.Type.GetType (thePlayer . conNameToSELF));
				}
				catch (Exception E)
				{
					print (E.ToString());
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
				catch(Exception E)
				{
					print (E.ToString());
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

	//makeDamage 有些动画是不造成伤害的
	//吐槽，这个方法的调用限制有点多，实时上传如多个类型的参数就会报错......（到手的动画资源大多数只读，所以关键帧方法还是要用这种的）
	//这个是这个机制不够灵活的地方
	/*
	 *makeDamage参数的多种用途
	 *1如果非正则可以造成伤害
	 *2 如果为负数则要根据这个值减小攻击距离
	 *3 如果是整数就无法造成伤害
	 */
	public  void attackForAnimation( float makeDamage)//攻击方法（带伤害）
	{
		if (systemValues.canAttack) 
		{
			//防止空引用
			try
			{
			if (!thePlayer)
				thePlayer = this.GetComponentInParent <PlayerBasic> ();
			if (!theEMY)
				theEMY = systemValues.getEMY (this.thePlayer.transform).GetComponent <PlayerBasic> ();
			}
			catch
			{
				//因为这个方法与动画播放的绑定比较紧密，因此在查看的界面中有可能会出问题
				//如果没有获取到引用就说明是某些特殊的调用方式
			}
			if(thePlayer)
			{
			    extraEffectSELF ();//添加自身特效
			}
			if (thePlayer && theEMY　) 
			{
				
				theDistance = Vector3.Distance (thePlayer.transform.position, theEMY.transform.position);
				float theDistanceCheck = thePlayer.theAttackAreaLength;
				if (makeDamage >= 0) {
					theDistanceCheck += makeDamage;
				}
				//print (theDistanceCheck);
				if (theDistance <= theDistanceCheck) {
					if (thePlayer && thePlayer.canHit && theEMY) {
						if (makeDamage >= 0) {//有些时候仅仅是增加脚本，例如“斗气爆发”不具备攻击效果
							thePlayer.OnAttack (theEMY, 0, false);//造成直接的伤害
							extraDamageEffect (theEMY);//添加额外的计算脚本，每个脚本的效果由脚本自己决定
						}
					}

				}
			}
		}
	}

}
