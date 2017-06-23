using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startSceneButtons : MonoBehaviour  {

	//开始界面比较简单，一共就几个按钮
	//这个脚本将会包含所有的startScene里面的按钮的所有处理方法

	public void PTP()//玩家对战
	{
		systemValues.theGameType = GameType.PVP;
		try
		{
			SceneManager.LoadScene ("selectForStart");
		}
		catch
		{
			//页面无法跳转，说实话那就没有必要做什么动作了
		}
	}
	public void PTC()//人机对战
	{
		systemValues.theGameType = GameType.PVC;
		try
		{
			SceneManager.LoadScene ("selectForStart");
		}
		catch
		{
			//页面无法跳转，说实话那就没有必要做什么动作了
		}
	}

	public void CTC()//AI对战
	{
		systemValues.theGameType = GameType.CVC;
		try
		{
			SceneManager.LoadScene ("selectForStart");
		}
		catch
		{
			//页面无法跳转，说实话那就没有必要做什么动作了
		}
	}

	public void over()
	{
		systemValues.canAttack = false;//开启这个标记能够使得游戏人物在被观看的时候特可以做出动作
		Application.Quit ();//直接结束游戏
	}


	void Start () 
	{
		systemValues.canAttack  = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
