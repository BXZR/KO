using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class overPictures : MonoBehaviour {

	//结束界面显示光荣时刻截图
	public pictureMaking thePictureMaking;//用于读取的类
	private List< Sprite>  theSprites;//用于接纳返回值的数组
	private GameObject ButtonLoad;//用于记录加载显示的按钮
	private List<GameObject> theButtons;//保存引用用于删除
	public Transform theContent;//容器，用于容纳生成的按钮
	GameObject theButton;//中间存储的变量
	//调用机制就是OnEnable方法，这样可以减少很多不必要的控制脚本
	void OnEnable()
	{
		//预防空引用
		if (thePictureMaking == null)
			thePictureMaking = this.GetComponent <pictureMaking> ();
		if(ButtonLoad ==null)
			ButtonLoad = (GameObject)Resources.Load ("uis/showButtonBlank");
		if (theButtons == null)
			theButtons = new List<GameObject> ();
		/////////////////////////////////////////////
		for (int i = 0; i < theButtons.Count; i++)
			Destroy (theButtons [i].gameObject);

		theSprites = thePictureMaking.loadTextures ();
		//for (int i = 0; i < theSprites.Count; i++)
		 
		for (int i = 0; i < systemValues .pictureLoadCount; i++)
		{
			if (i >= theSprites.Count)
				break;
			theButton = GameObject.Instantiate<GameObject> (ButtonLoad);
			theButton.transform.SetParent (theContent .transform );
			theButton.GetComponent <Image> ().sprite = theSprites [i];
			theButtons.Add (theButton);
		}
	}


	void Start ()
	{
		
	}

}
