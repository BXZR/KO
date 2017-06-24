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
	//生存时间做成静态是因为外部很多时间需要根据这个时间做简单的计算，此外也是为了两个阴阳图时间的统一
	public static  float moveTime = 0.30f;
	//下面这两个值会在开始的时候初始化
	private   float moveTimer = 0.30f;//生存时间
	private   float moveTimeMax = 0.30f;//生存时间
	private float moveUseSpeed = 0;//真正用来移动的速度
	private bool isOut =false;//是否已经向外面移动了，这个是一个标记

	void Start ()
	{
		moveTimer = moveTime;
		moveTimeMax = moveTime;
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
			moveTimer = moveTimeMax;
			moveUseSpeed = speed;
			isStarted = true;
			isOut = true;
		}

	}
	private void moveIn ()//阴阳相分别向中间移动
	{
		if (isStarted == false) 
		{
			moveTimer = moveTimeMax;
			moveUseSpeed = -speed;
			isStarted = true;
			isOut = false;
		}

	}

	//在这个显示效果中不允许有跳帧的情况出现，所以在FixedUpdate里面调用
	//减少的时间固定，是在time面板里面设置的
	void FixedUpdate ()
	{
		if (isStarted) 
		{
			moveTimer -= 0.01f;//因为是静态的，一共有两个这个时间在减，所以每一个仅仅减少一半减少量就可以了
			//这个移动的大小是根据屏幕尺寸动态移动的，所以在移动的过程中变化尺寸应该不会太糟糕
			this.transform.Translate (theMoveDircetion * moveUseSpeed *Screen.width * Time.deltaTime);
			if (moveTimer <= 0) 
			{
				isStarted = false;
			}
		}

	}
}
