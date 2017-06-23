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
