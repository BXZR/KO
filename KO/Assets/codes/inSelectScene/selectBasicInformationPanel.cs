using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selectBasicInformationPanel : MonoBehaviour {

	//选人界面中用于显示最基本的信息的面板
	public Text theBasicInformationText;//用于显示最基本信息的面板（故事）
	public Text theInformationForPlay;//用于显示战斗信息的内容
	public Text theHeadTitle;//用于显示题头（名字XXXX）

	public void makeInformation(string title ,string story, string information)
	{ 
		theBasicInformationText.text = story;
		theInformationForPlay.text = information;
		theHeadTitle.text = title;
	}
}
