using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class basicUIUpdate : MonoBehaviour {
	//总是在显示和更新的面板1号在这里被控制

	public PlayerBasic thePlayerToUpdate;//更新指定的游戏人物的信息，可以随意赋值

	public Slider theHpSlider;//显示UI——生命值
	public Slider theSpSlider;//显示UI——能量值
	public Image theHeadImage;//头像

	private Image  theHpFullImage; //显示颜色保留的图片引用

	public Image[] theWinCountShowImages;//用于显示胜利次数的图片，每一次round更新
	//值得注意的是因为对称性，不推荐使用代码获取的方式，还是大后宫拖动吧，反正也没有多少

	Color theHpColor;//当前显示的slider的颜色
	float hpValue;//当前的生命值百分比，保留这个引用可以少一次计算

	public string  thePlayerNamesResource = "";//获取到开始的ID，也就是预设物的名字，用这个查询头像名称
	//虽然这样会花掉额外的开销，但是设置上会更统一简单

	//使用invokeRepeating的方式不断调用UI更新方法，而不是每一帧都更新
	//尽可能减少每一帧需要计算的次数
	//暂定： 每0.2秒更新一次（每秒更新5次而不是每一秒钟更新fps数）
	//更新时间间隔在systemValues脚本中统一进行记录和更改

	private void valueUpdate()
	{
		if (thePlayerToUpdate != null) //非空就时常更新了
		{

			//在这里更新中不得已大量使用了除法
			hpValue  =  thePlayerToUpdate.ActerHp / thePlayerToUpdate.ActerHpMax;
			theSpSlider.value = thePlayerToUpdate.ActerSp / thePlayerToUpdate.ActerSpMax;
			/*
			 *跟俊绑定的任务的当前生命值显示slider的图片颜色
			 *参数给的是立即数，有空会继续调整
			 *值得注意的是RGB的颜色必须使0-1的，而不是1-255
			 *这个作为额外的功能可以注销掉
			*/
			theHpSlider.value = hpValue;
			theHpColor.r = 1 - hpValue * 0.5f;
			theHpColor.g = hpValue* 0.2f +0.8f ;
			theHpFullImage.color = theHpColor;

		}
	}

	public void makeWinCountFlash()
	{
		for (int i = 0; i < thePlayerToUpdate.winCountNow; i++) 
		{
			theWinCountShowImages [i].gameObject.SetActive (true);
		}
	}

	public void makeStart()
	{
		for (int i = 0; i < theWinCountShowImages.Length ; i++) 
			theWinCountShowImages [i].gameObject.SetActive (false);//默认全部关闭
		
		InvokeRepeating ("valueUpdate",0f,systemValues .updateTimeWait);
		theHpColor = new Color (0.5f,1f,0f);//初始化颜色，这一步其实没什么用的作为初始化B存在
		theHpFullImage = theHpSlider.GetComponentInChildren<Image> ();//获取图片引用、
		string thePicName = thePlayerToUpdate.headPictureName;
		theHeadImage.overrideSprite = (Sprite)Resources.Load ("pictures/"+thePicName,typeof(Sprite)) as Sprite;
	}
	void Start () 
	{
		//makeStart ();
	}

	void OnDestroy()
	{
		CancelInvoke ();//在销毁的时候取消掉所有的重复调用更新方法
	}

}
