using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class selectOverPanel : MonoBehaviour {

	public static string[] thePlayerCharacterName = {"孙悟空","冰原魔人"};//获取头像名称用于显示
	public static string[] thePlayerPicNAme = {"wukong","iceMonster"};//获取头像名称用于显示
	public static string[] thePlayerName = {"孙悟空","冰原魔人"};//获取头像名称用于显示
	public Image theImageForPlayer1;
	public Image theImageForPlayer2;
	public Text theTextForPlayer1;
	public Text theTextForPlayer2;

	public void makeThePanel()
	{
		theImageForPlayer1.overrideSprite = (Sprite)Resources.Load ("pictures/"+thePlayerPicNAme[0],typeof(Sprite)) as Sprite;
		theImageForPlayer2.overrideSprite = (Sprite)Resources.Load ("pictures/"+thePlayerPicNAme[1],typeof(Sprite)) as Sprite;
		theTextForPlayer1.text = thePlayerCharacterName [0];
		theTextForPlayer2.text = thePlayerCharacterName [1];
		PrepareMakeNextScene ();
	}

	public void PrepareMakeNextScene()
	{
		selectSceenController.step = 0;//更新回0已被下一次选用
		systemValues.canAttack = false;
		CancelInvoke ();
	}

	// Use this for initialization
	void Start () {
		makeThePanel ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
