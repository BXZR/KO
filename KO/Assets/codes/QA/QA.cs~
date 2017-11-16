using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QA : MonoBehaviour {

	//一些性能优化用的方法在这里集成

	//这个脚本在初始化的时候调用一次
	//并非强制
	//主要功能就是判断平台并且取消掉一些特殊的效果
	public Light theLight;//光源控制单元
	void Start ()
	{
		Application.targetFrameRate = 50;//更新游戏FPS
		if (Application.platform == RuntimePlatform.Android) 
		{
			theLight.shadows = LightShadows.None;//安卓平台取消掉阴影计算（虽然这并不是一个好方法）
			Application.targetFrameRate = 50;//更新游戏FPS
		}
	}
}
