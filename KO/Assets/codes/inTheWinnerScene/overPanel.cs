using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class overPanel : MonoBehaviour {

	public Image theWinnerImmage;
	public void theGameOver(PlayerBasic thePlayer,float timerWait = 2f)
	{
		theWinnerImmage.overrideSprite = (Sprite)Resources.Load ("pictures/"+thePlayer.headPictureName,typeof(Sprite)) as Sprite;
		theWinnerScene.setTheWinnerName (systemValues .getResourceNameWithPictrueName(thePlayer.headPictureName));
		Invoke ("makeNextScene",timerWait);
	}

	//玩家按下esc键位结束战斗的时候用，用于跳转
	public void justOver()
	{
		Destroy (theWinnerImmage.gameObject);//没有必要显示头像，仅仅需要一个跳转就可以
		Destroy(this.GetComponentInChildren<Text>().gameObject);
		Invoke("makeNextSceneSimple" , 1.5f);
	}

	void makeNextSceneSimple()
	{
		SceneManager.LoadScene ("Start");
	}

	void makeNextScene()
	{
		try
		{
			SceneManager.LoadScene ("fightOverScene");
		}
		catch
		{
			//页面无法跳转，说实话那就没有必要做什么动作了
		}
		CancelInvoke ();
	}
}
