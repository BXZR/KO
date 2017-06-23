using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textChanger : MonoBehaviour {
	//效果，通过某个方法传入一个字符串，然后通过用“蹦字”的方式显示出一串文字
	//推荐用协程和Invoke但是需要注意不可以重复开启，需要加上标记
	//此外在销毁的时候需要取消调用

	public Text theShowInformationText;//用于显示信息的那个Text
	private bool isStarted = false;
	private string theInformation = "";//用于展示的完整信息文本（完整版）
	private int indexNow = 0;//完整文本的下标获取值
	private string theShowString = "";//正式用于显示的字符串
	private float theFlashWaitTime = 0.2f;//Invoke的更新速率，这个数值越小更新越快

	//对外调用接口
	//外面的方法只会知道这一个方法并加以调用
	//此外这个方法仅仅会被调用一次
	public void setTheString(string theInformationGet)
	{
		if (isStarted == false)
		{
			isStarted = true;
			//下面是具体的工作细节，上面是标记量
			this.theInformation = theInformationGet;
			InvokeRepeating ("makeShowUpdate",0,theFlashWaitTime);
		}
	}

	public void showAll()//直接显示所有的内容
	{
		theShowInformationText.text = theInformation;
		indexNow = 999;
	}
	public bool isShowOver()//所有需要显示的信息是否都显示完
	{
		if (indexNow < theInformation.Length)
			return false;
		return true;
	}

	private	void makeShowUpdate()
	{
		if (indexNow < theInformation.Length) 
		{
			theShowString += theInformation [indexNow];
		    //更新显示的文本就可以了
			theShowInformationText .text = theShowString;//显示文本，必有改动吧，纯文本可能会有各种小问题吧
			indexNow ++;
		}
	}

	void OnDestroy()//非常必要
	{
		StopAllCoroutines ();
	}
	void Start () 
	{
		 
	}
	void Update () 
	{
		
	}
}
