using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneButton : MonoBehaviour {

	//专门用于跳转场景的Button一起其他控制方法
	//带有跳转场景功能的按钮的基类，本身应该很难在场景中出现了

	public string theNextSceneName="";//可以跳转的目标场景名称，有些时候也可以不设置，但一定要在调用的时候给出参数
	public bool useEscToSkip = true;//是否使用ESC快捷键快速跳转
	//在一个场景中如果有多个sceneButton这个选项一定要关闭
	//可以传入场景名
	public void goToNextScene(string theName = "")//跳转到下一个场景的方法
	{
		try
		{
			SceneManager.LoadScene (theName);
		}
		catch
		{
			//页面无法跳转，说实话那就没有必要做什么动作了
		}
	}
	//也可以不传值，但是要在Inspector面板设置
	public void goToNextSceneWithName()
	{
		try
		{
			SceneManager.LoadScene (theNextSceneName);
			selectSceenController.step = 0;
		}
		catch
		{
			//页面无法跳转，说实话那就没有必要做什么动作了
		}
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape) && useEscToSkip) 
		{
			goToNextSceneWithName ();
		}
	}

}
