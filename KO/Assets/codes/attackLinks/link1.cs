using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class link1: attackLink
{

	// Use this for initialization
	void Start () {
		makeStart ();//初始化ARRAY，这个是为了方便输入，并且在Unity中编辑字符串要比char数组方便
	}
	public override  void attackLinkEffect()//连招的效果在这里写
	{
		//print (skillName+" 发动！");
		this.theAnimatorOfPlayer.CrossFade (animationName,0.1f);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
