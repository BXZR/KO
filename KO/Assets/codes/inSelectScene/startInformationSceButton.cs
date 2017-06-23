using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class startInformationSceButton : MonoBehaviour {

	//选择人物的界面所有的按钮任务都写在这里

    //用来获取信息的对象
	public static PlayerBasic thePlayer;
	public selectSceenController theController;//只在确定选择的时候有用
	private  GameObject theShowButtons;//用于显示的预设物
	private static  List<GameObject> theButtons;//用于显示的Button
	public Transform theContent;//用于显示Button的容器

	public void basicInformation()//界面0基本信息
	{
		flashClear ();
		makeBasicInformation ();
	}

	public void getAttackLinInformation()
	{
		flashClear ();
		//getAttackLinkInformationOld ();
		getAttackLinkInformationNew ();
		theController.openPanel (1);
	}

	public void skillInformation()
	{
		flashClear ();
		//getSkillInformationOld ();
		getSkillInformationNew();
		theController.openPanel (2);
	}



	/***************************************顶层与底层的分割线************************************************/
	//真正被顶层调用的底层方法


	private void  getSkillInformationNew()//新的显示方法
	{
		string theName = "";
		string theInformation = "";
		string theAttackLink = "";//连击字符串
		GameObject theButton;

		for (int i = 0; i < selectSceenController.theSkillInformationOfPlayerList.Count; ) 
		{
			theName = selectSceenController.theSkillInformationOfPlayerList [i];
			i++;
			theInformation = selectSceenController.theSkillInformationOfPlayerList [i];
			i++;
			theAttackLink = selectSceenController.theSkillInformationOfPlayerList [i];
			i++;
			theButton = GameObject.Instantiate <GameObject> (	theShowButtons);
			theButton.transform.SetParent (theContent); 
			theButton.transform.localScale = new Vector3 (1,1,1);
			theButton.GetComponent <theShowButtons> ().makeInformation ( theName ,theInformation);
			theButton.GetComponent <theShowButtons> ().makeExtraInit (theAttackLink);//获取连招战斗字符串
			theButtons.Add (theButton);

		}
	}
		
	//注意的是显示用的Content并不是同一个对象
	//这是两种不同的按钮进行区分的一个重要的依据
	private void getAttackLinkInformationNew()
	{
//		for (int i = 0; i < selectSceenController.theAttackLinkOfPlayerList.Count; i++) 
//		{
//			print (selectSceenController .theAttackLinkOfPlayerList[i]);
//		}

		//这里有有下标超界////////////////////////////////////////////////////////////////////////////
		GameObject theButton;
		string theName = "";//名字
		string theInformation = "";//连击信息
		string theAttackLink = "";//连击字符串
		for (int i = 0; i < selectSceenController.theAttackLinkOfPlayerList.Count;  ) 
		{
			theName = selectSceenController.theAttackLinkOfPlayerList [i];
			i++;
			theInformation =  selectSceenController.theAttackLinkOfPlayerList [i];
			i++;
			theAttackLink = selectSceenController.theAttackLinkOfPlayerList [i];
			i++;
			theButton = GameObject.Instantiate <GameObject> (	theShowButtons);
			theButton.transform.SetParent (theContent); 
			theButton.transform.localScale = new Vector3 (1,1,1);
			theButton.GetComponent <theShowButtons> ().makeInformation ( theName ,theInformation);
			theButton.GetComponent <theShowButtons> ().makeExtraInit (theAttackLink);//获取连招战斗字符串
			theButtons.Add (theButton);
		}
	}

	public 	void flashClear()//删除掉生成的那些Button
	{
		for (int i = 0; i < theButtons.Count; i++) 
			Destroy (theButtons [i].gameObject );
		theButtons.Clear ();
		 
	}


	public   void makeBasicInformation()
	{
		theController.openPanel (0);
		string theStrory = systemValues .getStory();//人物生平
		theController.thePanels [0].GetComponent <selectBasicInformationPanel> ().makeInformation (thePlayer .ActerName,theStrory,thePlayer .getPlayerInformation()+"\n"+thePlayer .getPlayerInformationExtra());
	}
	public void makeSelect()//进行选择
	{

		systemValues . thePlayerNamesForPlay [selectSceenController.step] = systemValues.fighterNowGet ();
		selectOverPanel.thePlayerPicNAme [selectSceenController.step] = systemValues.getPicName();
		selectOverPanel.thePlayerCharacterName [selectSceenController.step] = systemValues.getCharacterName ();
		selectSceenController.step++;
		theController.makeNextStart ();
		if (selectSceenController.step >= selectSceenController.stepMax) 
		{
			theController.selectOverPanel.SetActive (true);
			goToBNextScene ();
		}
	}

	private void goToBNextScene()
	{
		try
		{
			SceneManager.LoadScene ("loading");
		}
		catch
		{
			//页面无法跳转，说实话那就没有必要做什么动作了
		}
		//theController.selectOverPanel.SetActive (true);
		//theController.selectOverPanel.GetComponent<selectOverPanel> ().makeThePanel ();
	}
	void Start () 
	{
		theShowButtons   = Resources.Load <GameObject> ("uis/theShowButtons");
		theButtons = new List<GameObject> ();
	}

}
