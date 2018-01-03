﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class selectSceenController : MonoBehaviour
{

	private GameObject theGameObject ;//显示的模型信息，其实是为了方便删除
	public Transform showPosition;//显示的位置
	//(这是使用3D世界坐标制作的标记方法，现在先用下面的2D转世界坐标的方法替换,但是这个方法仍然保留)
	public Canvas theCanvas;//用showPosition2D计算反推三维世界坐标的时候需要有这个作为依据）
	public Transform showPosition2D;//显示的位置
	public Text theNameText;//显示人物的名字（感觉其实可有可无）
	public GameObject selectOverPanel;//选择完成显示的界面


	public static string theBasicInformationOfPlayer;//为了减少计算次数而使用这个字符串记录一次
	public static string theSkillInformationOfPlayer;//为了减少计算次数而使用这个字符串记录一次
	public static string theAttackLinkInformationOfPlayer;//为了减少计算次数而使用这个字符串记录一次

	public static List<string>  theSkillInformationOfPlayerList;//为了减少计算次数而使用这个字符串记录一次(用于保存技能信息)
	public static List<string>  theAttackLinkOfPlayerList;//为了减少计算次数而使用这个字符串记录一次（用于保存连招信息）
	public static attackLinkController theAttackLinkController;//用于控制人物动作的控制器（这个地方设计得有信息冗余，优化的时候还是应该看看怎么重构）
	//当前选择的阶段，因为一共就两个人，所以只会有0,1两种阶段
	//分别对应的是战斗场景中用于初始化的thePlayerNames数组0和1
	public static int step =0;//为了方便做成静态的，算是选择界面的systemValues
	public static int stepMax =2;//因为人人对战的时候是需要选择两次

	//因为是屏幕坐标转的世界坐标，其基础的缩放值会很小，因此需要使用比较大的缩放
	private float minFov =3f;//最小缩放值
	private float maxFov = 4.6f;//最大缩放值
	private float sensitivity = 1f;//增大减小的参数
	private float fov = 3.7f;//获取当前缩放值

	public GameObject[] thePanels;//被控制的面板用这种方式控制，所以下标顺序非常的重要
	 
	public startInformationSceButton useButtonToControll;//这是为了获取信息，同时这个是随机按钮，在随机状态之下应该变色
	//用这种方式实现的重用，这个按钮上面事实上有并未使用仅仅用于调用的脚本
	private Image theRandomImage;//在随机状态下应该变色
	private bool isRandoming = false;//是否正在开启随机

	void Update () 
	{
		fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;//根据鼠标滚轮充值这个参数
		fov = Mathf.Clamp(fov, minFov, maxFov);//限制参数的值
		if (theGameObject)
			theGameObject.transform.localScale = new Vector3 (fov,fov,fov);
 
	}



	void OnDestroy()
	{
		stepMax = 2;//因为有step=1的情况，但是人工维护太麻烦了，不如使用这种方法了
		CancelInvoke ();
	}


	public void openPanel(int index)
	{
		for (int i = 0; i < thePanels.Length; i++) 
		{
			thePanels [i].SetActive (false);
		}
		thePanels [index].SetActive(true);
	}

	//用UGUI坐标转换游戏世界的坐标
	//用的是屏幕的UI坐标转的世界坐标
	Vector3 getPosition(Transform theshowPositionOnScreen , Canvas theCanvas)
	{
		Vector3 scr= RectTransformUtility.WorldToScreenPoint(theCanvas.worldCamera, theshowPositionOnScreen.position);
	   scr.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
	   return Camera.main.ScreenToWorldPoint(scr);
    }

	public void makeMode(string nameGet)
	{
		if(theGameObject)
		Destroy (theGameObject .gameObject);
		theGameObject = (GameObject)GameObject.Instantiate ((GameObject )Resources.Load("fighters/"+nameGet));
		//theGameObject.transform.position = showPosition.position;
		//theGameObject.transform.SetParent (showPosition);
		//theGameObject.transform.localPosition = new Vector3 (0,0,0);
		theGameObject .transform .position =  getPosition(showPosition2D.GetComponent<Transform>(), theCanvas) ;

		theGameObject.AddComponent (System.Type.GetType ("extraRotate"));
		PlayerBasic thePlayer = theGameObject.GetComponent<PlayerBasic> ();
		theAttackLinkController = thePlayer.GetComponent <attackLinkController> ();//获取静态的动作控制器

		theBasicInformationOfPlayer =  thePlayer.getPlayerInformation () +"\n"+ thePlayer .getPlayerInformationExtra();
		//theSkillInformationOfPlayer = thePlayer.getSkillInformation ();
		 getSkillInformation(thePlayer);//新方法获取技能信息
		//theAttackLinkInformationOfPlayer = thePlayer.getAttackLinkInformation ();
		getAttackLinkInformation(thePlayer);//新方法获取连招信息
		theNameText.text = systemValues .importantColor + "玩家"+(step +1)+"正在选择"+systemValues .colorEnd +"\n\n"+ thePlayer.ActerName;

		thePlayer.GetComponent <attackLinkController> ().makeStart ();//在这个界面中玩家可以按下连击键位查看连击的动作
		startInformationSceButton .thePlayer = thePlayer;
	}

	//原本的getSkillInformation方法为thePlayer.getSkillInformation ();
	//这个方法的脚本代码仍然存在于这个playerBasic中未被删除以备后用
	//但是因为需要显示的内容的数据结构有所改变，新的方法决定使用此类内部方法
	private void getSkillInformation(PlayerBasic thePlayer)
	{
		try
		{
		theSkillInformationOfPlayerList = new List<string> ();
		attackLink [] theWeapons = thePlayer.GetComponentsInChildren<attackLink> ();

			List<string > linkStrings = new List<string> ();
			List<string> skillNames = new List<string> ();

		for (int i = 0; i < theWeapons.Length; i++) 
		{
			if (string.IsNullOrEmpty (theWeapons [i].conNameToEMY) == false) 
			{
					string  skillName = theWeapons [i].conNameToEMY;
					if(!thePlayer.gameObject .GetComponent(Type.GetType (skillName )))
					{
					thePlayer.gameObject.AddComponent (Type.GetType (skillName ));

					if(skillNames.Contains(skillName) == false)
					{
						linkStrings .Add(theWeapons [i].attackLinkString);
						skillNames.Add(skillName);
					}
					}
			}
			if (string.IsNullOrEmpty (theWeapons [i].conNameToSELF) == false) 
			{
					string  skillName = theWeapons [i].conNameToSELF;
					if(!thePlayer.gameObject .GetComponent(Type.GetType (skillName )))
					{
					thePlayer.gameObject.AddComponent (Type.GetType (skillName ));

					if(skillNames.Contains(skillName) == false)
					{
						linkStrings .Add(theWeapons [i].attackLinkString);
						skillNames.Add(skillName);
					}
					}
			}
		}

			//for(int i=0;i<linkStrings .Count;i++)
			//{
				//print(linkStrings[i]);
				//print(skillNames[i]);

			//}

			effectBasic [] effectsPE  = thePlayer.GetComponentsInChildren <effectBasic> ();
			int indexFotAttackLink= 0;
			for (int i = 0; i <  effectsPE .Length; i++)
		{
				//有些特殊的effect脚本是为了系统内部调用便利才继承这个类，并不算是真实的技能
				if(effectsPE [i].isExtraUse ())
					continue;
				effectsPE [i].Init ();
				string head = "";
			/*************************进行染色************************************************/
			if (effectsPE [i].isBE () )
					head =  systemValues .BESkillColor + effectsPE[i].getEffectName() +systemValues .colorEnd;
			else
					head =  systemValues .NBESkillColor + effectsPE[i].getEffectName() +systemValues .colorEnd;
			if(effectsPE[i].isPublic())
					head =  systemValues .publicSkillColor + effectsPE[i].getEffectName() +systemValues .colorEnd;
			/********************************************************************************/
				if(theSkillInformationOfPlayerList.Contains(head)==false)
				{
				 theSkillInformationOfPlayerList.Add(head);	
				 theSkillInformationOfPlayerList .Add(effectsPE [i].theEffectInformation);
				   if(effectsPE[i].isBE()==false)
				   {
						
					theSkillInformationOfPlayerList.Add (linkStrings[indexFotAttackLink]);
					indexFotAttackLink++;
				   }
				   else
				   {
					theSkillInformationOfPlayerList.Add ("-");	
				   }
				}
			effectsPE [i].effectDestoryExtra ();
			effectsPE [i].effectDestory ();
			Destroy (thePlayer.GetComponent (effectsPE [i].GetType()),0.004f);
		}
			//这里有问题尚未解决，获取到的信息是空值
//			print ("技能表：");
//			for (int i = 0; i < theSkillInformationOfPlayerList.Count; i++)
//				print (theSkillInformationOfPlayerList[i]);
		}
		catch(Exception e) 
		{
			//print (e.ToString());
		}
	}


	 //用于获取连招信息的新方法，连招信息存到List里面（分成两个部分）
	public void  getAttackLinkInformation(PlayerBasic thePlayer)
	{
		 
		attackLink  [] theLinks  = thePlayer.GetComponentsInChildren<attackLink> ();
		theAttackLinkOfPlayerList = new List<string> ();//获取连击的信息（这两个是不一样的）
		for (int i = 0; i < theLinks.Length; i++)
		{
			string theName = theLinks [i].getAttackLinkName ();
			//if (!theAttackLinkOfPlayerList.Contains (theName)) 
			//{
				theAttackLinkOfPlayerList.Add (theName);
				theAttackLinkOfPlayerList.Add (theLinks [i].getInformation ());
			    theAttackLinkOfPlayerList.Add (theLinks [i].attackLinkString);
			//}
		}
	}

	//随机选人的外部方法
	public void makeRandomFighter()
	{
		if (isRandoming == false)
		{
			InvokeRepeating ("getRandomFighter",0,0.5f);
			theRandomImage.color = Color.yellow;
			isRandoming = true;
		}
		else 
		{
			CancelInvoke ();
			theRandomImage.color = Color.white;
			isRandoming = false;
		}
	}

 
		
 

	 
	//以下是一些用于操作的内容
	public void getBackFighter()
	{
		//startInformationSceButton.flashClear ();
		string nameGet = systemValues.fighterBackNameGet ();
		makeMode (nameGet);
		useButtonToControll.basicInformation ();
	}

	public void getForwardFighter()
	{
		//startInformationSceButton.flashClear ();
		string nameGet = systemValues.fighterForwardNameGet();
		makeMode (nameGet);
		useButtonToControll.basicInformation ();
	}

	//获取随机英雄
	private  void getRandomFighter()
	{
		string nameGet = systemValues.fighterRandomNameGet ();
		makeMode (nameGet);
		useButtonToControll.basicInformation ();
	}
	public void flashInStart()
	{
		//startInformationSceButton.flashClear ();
		string nameGet = systemValues.fighterZeroGet();
		makeMode (nameGet); 
		useButtonToControll.basicInformation ();
		theRandomImage = useButtonToControll.GetComponent <Image> ();
	}

	public void makeNextStart()
	{
		systemValues.flashForSelect ();
		flashInStart ();
	}
	void Start () 
	{
		flashInStart ();

	}
	 
}
