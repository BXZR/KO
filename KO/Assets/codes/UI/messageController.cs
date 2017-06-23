using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class messageController : MonoBehaviour {

	//通用消息控制单元，这个脚本也应该挂在游戏控制器（总控单元上）
	//目前希望是做成静态全局可调用的
	//显示一下"击倒！"，“时间到！”这种基本的信息

	//这个也是round提示的整合
	//(所有的提示信息都在这个脚本表示，取缔原先的roundController的做法)

	public GameObject theMessagePanel;//用于显示的Text(用于面板赋值)
	private static GameObject theMessagePanelStatic;//引用的静态保存 
	private static Text showStaticText;//用于全局静态方法调用的Text
	private static float showTimer = 1.5f;//每一次显示信息的持续时间，新的消息到来会刷新这个时间 
	private static float showTimerMax = 1.5f;//每一次显示信息的持续时间，新的消息到来会刷新这个时间 
	private static bool canCool =false;//开启冷却时间

	void Start () //这种东西在界面里面进行初始化就足够了，没必要由controller进行统一调配
	{
		showStaticText = theMessagePanel .GetComponentInChildren<Text>();
		theMessagePanelStatic = theMessagePanel;
	}

	//静态信息提示方法（果然这种小组间应该用静态的方式调用）
	//传入的bool是否开启自动的时间控制的方法
	//传入的float为定制的显示时间
	public static void showMessage(string theString = "暂无消息", bool canPrivatlyControl = true, float ShowTimerIn = 1.5f)
	{
		theMessagePanelStatic .SetActive (true);
		showStaticText.text = theString;
		showTimer =  ShowTimerIn ;
		canCool = canPrivatlyControl;
	}
   
	//关闭信息显示面板（静态方法）
	public static void shut()
	{
		showTimer = -1;
		theMessagePanelStatic .SetActive (false);
	}

	// Update is called once per frame
	void Update ()
	{
		//作为一个实时检查的方法，这个方法其实有一点消耗资源了
		if (canCool)
		{
			showTimer -= Time.deltaTime;
			if (showTimer < 0)
			{
				showTimer = showTimerMax;
				theMessagePanelStatic .SetActive (false);
				canCool = false;
			}
		}
	}
}
