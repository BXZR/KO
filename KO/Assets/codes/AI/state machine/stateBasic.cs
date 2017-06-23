using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateBasic 
{

	//有限状态机的通用父类给出所有状态的被调用的方式
	//这个类并不会实际参与到计算中

	protected  move theMoveController;//游戏人物移动控制单元
	protected attackLinkController theActionController;//游戏人物动作控制单元
	protected PlayerBasic thePlayer;//游戏人物，通通过状态等等进行决策
	protected PlayerBasic theEnemy;//游戏人物的对手

	//此方法被初始化的时候调用
	//转换状态的时候，返回的是一个已经makeState的状态
	public void makeState( move theMoveController,attackLinkController theActionController,PlayerBasic thePlayer)
	{
		this.theMoveController = theMoveController;
		this.theActionController = theActionController;
		this.thePlayer = thePlayer;
		this.theEnemy = systemValues.getEMY (thePlayer .transform ).GetComponent <PlayerBasic>();
		 
	}
 
	public virtual void stateIint(){}//状态的额外初始化（一般的初始化在makeState里面完成）
	public virtual void stateAction(){}//这个状态下思考得到的行为
	public virtual stateBasic thinkNextState(){return null;}//状态转换思考方式，返回一个state如果返回为空，就认为是不转换状态



	public void lookAtEMY()//控制AI看向他的目标
	{
		this.theMoveController.lookAtEMY (false);
	}


	//判定游戏人物法力是否不足
	//这个方法没有必要重写或者覆盖，所以直接就在父类里面实现就好
	public  bool isSPLow(attackLinkController theAttackLinkController,PlayerBasic thePlayer)
	{
		float min = 9999;
		for (int i = 0; i < theAttackLinkController.attackLinks.Length; i++) 
		{
			if (theAttackLinkController.attackLinks [i].spUse>5 &&  theAttackLinkController.attackLinks [i].spUse < min) 
			{
				min = theAttackLinkController.attackLinks [i].spUse;
			}
		}
		if (min > thePlayer.ActerSp)
			return true;
		return false;
	}


	/*****************在设计中觉得有意思的地方，放在这里用于修改和扩展吧*****************/
	//决策：使用哪一个连招进行攻击的方法
	//因为判定法力值否不足和判定使用连招的机制不同，所依可能会出现透支生命的可能性
	//这里是有很多智能的算法可扩展的地方，所以可能会有很多的备用方法用于决策
	/************************************************************************************/

	//方法一，基于贪心策略返回攻击力/法力消耗的最大值
	//连招效果脚本在这里算作额外的攻击力
	/*
	 * 这个算法的缺点，
	 * 1 有些脚本挂载是存在持续时间的，
	 * 在这个持续时间之内使用相同的连招只是浪法费力
	 * 但是这个方法没有考虑到这些
	 * 
	 * 2 有些性价比很低的招式很可能一直不被使用
	 * （简易解决方法，法力消耗也算作一个负数加权值做分子）
	 * 
	*/
	public string getAttackLinkString(attackLinkController theAttackLinkController,PlayerBasic thePlayer)
	{
		string stringToReturn = "";
		float value = -9998;
		attackLink theLinkNow;
		float theValueToCheckUp = 0;//分子
		float theValueToCheckDown = 2;//分母
		float theValueToCheck = 0;//分子分母计算结果
		for (int i = 0; i < theAttackLinkController.attackLinks.Length; i++) 
		{
			if (theAttackLinkController.attackLinks [i].spUse > 5) //不耗蓝或者耗蓝超级低的都不算战斗技能，只是普攻与加权
			{
				theValueToCheckUp = 0;//分子
				theValueToCheckDown = 0;//分母
				theValueToCheck = 0;//分子分母计算结果

				theLinkNow = theAttackLinkController.attackLinks [i];

				if (string.IsNullOrEmpty (theLinkNow.conNameToEMY) == false)
					theValueToCheckUp += 25;//脚本级别的加权值有待界定
				if (string.IsNullOrEmpty (theLinkNow.conNameToSELF) == false)
				{
					theValueToCheckUp += 60;//能给自己补充状态的优先
				}
				theValueToCheckUp += theLinkNow.extraDamage;//攻击力比较高的优先
				theValueToCheckUp += (thePlayer .ActerSp - theLinkNow.spUse) ;//用完剩余法力比较高的优先

				theValueToCheckUp += Random.Range (0, 60);//引入随机加成，让游戏人物的动作变得不可控

				//theValueToCheckDown =  20;//因为有些招式是不需要法力的
                //theValueToCheck = theValueToCheckUp / theValueToCheckDown;
				//这里其实应该有一页除法运算做加权，但是现在还没应用到这一块
				theValueToCheck = theValueToCheckUp;
				if (value < theValueToCheck) 
				{
					value = theValueToCheck;
					stringToReturn = theLinkNow.attackLinkString;
				}
			}

		}
		//Debug.Log (thePlayer .ActerName +"--"+stringToReturn);
		return stringToReturn;
	}
}
