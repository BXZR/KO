using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class theInformationGrid : MonoBehaviour {

	//信息显示容器

	//此脚本完全控制显示信息的额外面板的内部控制
	//此效果仿照的是仙剑奇侠传五前传的技能信息显示列表
	//20170512这一天是UI内容与表现的重大变革的开端

	private string theHeadString = "诸神黄昏";//题头信息（例如技能名称）
	private string theComplexInformation = "宗旨是非常厉害的招式";//详细信息（例如技能效果信息）
	public  Text theButtonText;//Button上面显示的信息
	public Text  theInformationText;//详细信息的界面Text
	public GameObject theComplexInformationPanel;//用于显示详细信息的Panel(其实是scrow view)
	//(反正是要做成预设物的，不如直接拖拽减少程序初始化的运算量)
	private bool isOpened = false;

	//用于初始化的方法
	public void getInformation(string headInformation,string theInformationIn)
	{
		this.theHeadString = headInformation;
		this.theComplexInformation = theInformationIn;
		//我在考虑是否应该保留字符串的引用
		theButtonText.text = theHeadString;
		theInformationText.text = theComplexInformation;
		theComplexInformationPanel.SetActive (false);
	}

	//按钮点击调用的方法
	public void makeTheShow()
	{
		if (isOpened == false) 
		{
			theComplexInformationPanel.SetActive (true);
			isOpened = true;
		}
		else 
		{
			theComplexInformationPanel.SetActive (false);
			isOpened = false;
		}
	}


	void Start () 
	{
		//测试用
		getInformation(theHeadString,theComplexInformation);
	}
	
 
	void Update () {
		
	}
}
