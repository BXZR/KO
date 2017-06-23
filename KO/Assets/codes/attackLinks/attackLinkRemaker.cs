using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class attackLinkRemaker : MonoBehaviour {
	//因为各种原因替换游戏中的键位的方法都应该被封装到这里
	//这个脚本放在游戏人物身上（控制是否生效）或者临时加载在游戏人物身上
	public attackLink [] theLinksOfThePlayer;//这个人物拥有动作技能
	public int indexToMake = 2;
	private Event events;//当前的获取的事件
	private string newLinkString = "";//重新设定的联机字符串
	private bool isChanging =false;//是否正在改变连击字符串

	string oldChar = "";


	private void getLinks()
	{
		theLinksOfThePlayer = this.GetComponent <attackLinkController> ().attackLinks;//因为有可能会有所改变，所以每一次换键位的时候都应该重新获取这个数组
		indexToMake = 0;//可以换的按键的下标
		//此前已经设定过了只可以更改单字符的动作按钮，这个是可以深入编写的地方
	}
	void Start ()
	{

		getLinks ();

//		string d = "ddgv";
//		if (d.Contains ("g"))
//			print ("A method");

	}
	void OnGUI()
	{
		if (isChanging)
		{
			GUI.Button (new Rect (Screen .width *3/7,Screen .height*2/5,Screen .width /7,Screen .height /5),"isChanging");
		}
		//在OnGUI中检测按键的方法，原因：GUI方法更新比较快
		if (Input.anyKeyDown) 
		{
			events = Event.current;//获取到按键事件的引用
			if (events !=null && events .isKey) 
			{
				if (events.keyCode != KeyCode.None)
				{
					if (events.keyCode == KeyCode.Return )//第一次按回车开启换键位，第二次关闭
					{
						if (isChanging == false) 
						{
							isChanging = true;
						}
						else 
						{
							getLinks ();//获取动作数组进行级联更新
							//在这里有一个规则：所有的高级复杂动作都是用两个轴和单独按键动作组合而成的
							if (theLinksOfThePlayer [indexToMake].IsChangeAble()) 
							{
								oldChar = theLinksOfThePlayer [indexToMake].attackLinkString;//获取旧的字符串7
								 print("old = "+ oldChar +" new = "+newLinkString);//控制台显示更新成功
								for (int i = 0; i < theLinksOfThePlayer.Length; i++) 
								{
									string newLink = theLinksOfThePlayer [i].attackLinkString.Replace (oldChar, newLinkString);//返回字符串替换结果
									theLinksOfThePlayer [i].attackLinkString = newLink; //重新赋值
									theLinksOfThePlayer [i].makeAttackArray ();//重新获取检测数组
								}
								newLinkString = "";
								isChanging = false;
							}
						}
							

					}
					if (events.keyCode != KeyCode.Return && isChanging == true) 
					{
						newLinkString = events.keyCode.ToString ().ToCharArray () [0].ToString ();
					}
				}
			}
		}
	}

}
