using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class theWinnerScene : MonoBehaviour{

	//这个脚本用于展示胜利者的界面全部内容，因为说实话这没什么内容，知识应该有这个功能而已
	//仿照的是“撕裂重罪打完之后的一个出现任务对话的界面”

	//private static string [] fighterInResourceNames = {"theFighterWukong","theFighterIce","theFighterdemon","theFighterknight"};
	private static string theWinnerName = "theFighterWukong";//胜利者的名字0,这个名字需要与systemValues中fighterInResourceNames数组的值相同
	public Transform  theModePosition;//cube位置，这个就是用于显示mode的Transform的位置
	private GameObject thePlayerMode;//用于显示的人物模型，放在这里方便扩展
	public textChanger theTextShow;//用于显示文本的控件控制脚本
	float timer = 0;//防止误按的计时器，一定时间之后才可以取消

	public static void setTheWinnerName(string theWinnerNAmeGet)
	{
		theWinnerName = theWinnerNAmeGet;

	}

 

	//展示胜利者的模型
	private void makeTheWinnerMode()
	{
		try
		{
			
			thePlayerMode = (GameObject)GameObject.Instantiate ((GameObject)Resources.Load ("fighters/" + theWinnerName));  
			thePlayerMode .transform .position = theModePosition .position;	
			thePlayerMode .transform.rotation = new Quaternion (0,0,0,0);
			thePlayerMode.transform .SetParent(theModePosition);
			string theTalk = systemValues .getWinSpeak(theWinnerName);//shiy9ongsystemValues新的方法获取说话的内容
			theTextShow .setTheString(theTalk);//托管到下一个脚本中去
		}
		catch (Exception e)
		{
			//makeNextScene();
			//找不到游戏人物的模型，出岔子了就直接跳转到开始界面吧，反正也只是一个显示
			print("ERROR in show");
			print ("ERROR information:"+e.Message);
			print ("ERROR information:"+e.ToString());
		}
	}

	void OnDestroy()//非常必要
	{
		CancelInvoke ();
	}

	void Start ()
	{
		Invoke( "makeTheWinnerMode" ,0.2f);//由外部方法来控制整体流程
	}
	
 
	void Update () 
	{
 
	}
}
