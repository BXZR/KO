using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class theShowButtons : MonoBehaviour {
	//这个脚本用于选择人物界面中的显示内容
	public Text theShowHead;//显示题头信息、
	public Text theComplexInformation;//显示详细信息

	private string theLinkeString;//连招信息，这个信息与makeActions作为一个“绑定”

	public void makeExtraInit(string theLinkeStringIn)//为了完成这个功能做出的额外初始化
	{
		theLinkeString = theLinkeStringIn;
	}

	//这两个方法之所以拆开是因为有一些按钮没有必要使用这样的额外方法
	public void makeInformation(string head,string complex)
	{
		theShowHead.text = head;
		theComplexInformation.text = complex;
	}
    //玩家点击按钮之后就左边的人物就应该作出相应的动作
	public void makeActions()
	{
		if (selectSceenController .theAttackLinkController != null && 　!string.IsNullOrEmpty (theLinkeString)) 
		{
			selectSceenController .theAttackLinkController.makeAttackLink (theLinkeString,false);//直接做连招，不需要翻译
		}
	}
}
