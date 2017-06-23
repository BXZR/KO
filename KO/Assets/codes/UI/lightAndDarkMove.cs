using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightAndDarkMove : MonoBehaviour {

	//专用脚本，此效果并非通用
	//在初始场景开始的时候阴阳标记分别消失的效果
	//这个效果是为了向仙剑奇侠传三游戏致敬！

	public Vector3 theMoveDircetion ;//移动的方向
	public float speed =10;//移动速度的绝对值用于移动

	private float waitTime = 0.1f;//为了能让人看到，有一点点时间的延迟
	private bool isStarted = false;
	private float moveTime = 0.3f;//生存时间
	private float moveTimeMax = 0.32f;//生存时间
	private float moveUseSpeed = 0;//真正用来移动的速度
	private bool isOut =false;//是否已经向外面移动了，这个是一个标记

	void Start ()
	{
		Invoke ("moveOperate", waitTime);
	}

	//统一外部调用方法，这里面还有条件判断需要完善
	//这个移动不允许直接人为控制，只能是到
	public void moveOperate()
	{
		if (isOut == false)
			moveOut ();
		else
			moveIn ();
	}


	private void moveOut ()//阴阳相分别向两边移动
	{
		if (isStarted == false) 
		{
			moveTime = moveTimeMax;
			moveUseSpeed = speed;
			isStarted = true;
			isOut = true;
		}

	}
	private void moveIn ()//阴阳相分别向中间移动
	{
		if (isStarted == false) 
		{
			moveTime = moveTimeMax;
			moveUseSpeed = -speed;
			isStarted = true;
			isOut = false;
		}

	}

	void Update ()
	{
		if (isStarted) 
		{
			moveTime -= Time.deltaTime;
			this.transform.Translate (theMoveDircetion * moveUseSpeed * Time.deltaTime);
			if (moveTime <= 0) 
			{
				isStarted = false;
			}
		}

	}
}
