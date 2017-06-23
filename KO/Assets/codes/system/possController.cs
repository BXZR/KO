using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class possController : MonoBehaviour {

 
	private bool thePossIsEnded = false;//标记量，因为多次点击有可能多次调用，这就尴尬了
	public void showPossStart()
	{
		StartCoroutine ("showPoss");
	}

	private void endPoss()
	{
		if(thePossIsEnded == false)
		{
		thePossIsEnded = true;
		StopAllCoroutines ( );
		this.GetComponent <gameStarter>(). makeAllStart ();
		showPossInStartSlash ();
		this.GetComponent <possController> ().enabled = false;//不再进行update检测
			//Destroy (this.GetComponent (this.GetType()));
		}
	}

	//开战之前的嘚瑟摆poss嘚瑟完事之后才会开启下一个流程
	//时间上面的控制是一个问题，如何做到动态控制？
	public void showPossInStartSlash()//立即执行方式(仅仅作为样例没有使用)
	{
		for (int i = 0; i < systemValues.players.Length; i++) 
		{
			makePoss (systemValues.players[i],0,0);
		}
	}



	IEnumerator    showPoss()
	{
		for (int i = 0; i < systemValues.players.Length; i++)
		{
			makePoss (systemValues.players[i],3f,i);
			yield return new WaitForSeconds (3);
		}
		endPoss ();
		//Destroy (this.GetComponent (this.GetType()));
		//作为一个优化方法可以消除掉这段脚本，这样可以至少减少一个引用
		//但是因为还需要在胜利的时候继续POSS所以还是应该保留的
	}

	//打架之前的动作显示需要摄像机进行查看
	//因为给出默认参数，不传入参数也行
	//值得注意的是index，用index分辨顺时针转还是逆时针转
	//需要注意到的是，在战斗结束之后的POSS调用也是使用这个方法
	public void makePoss(GameObject thePlayer = null , float timerForDestory = 3f, int index = 0)
	{
		if (thePlayer)
		{	 
			Camera theCamera = thePlayer.GetComponentInChildren<Camera> ();
			if (theCamera) 
			{
				theCamera.gameObject.AddComponent<cameraRotateInPoss> ();
				theCamera.gameObject.GetComponent <cameraRotateInPoss> ().setMode (index %2);
				theCamera.depth = 99;
				Destroy (theCamera.gameObject ,timerForDestory);
			}
			//这个名字也可以在全局配置文件systemValues里面进行配置
			thePlayer.GetComponentInChildren<Animator> ().Play (systemValues .theShowPossStateNAme);
		}
	}



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	float timePass = 0;//一个极其简单的计时器
	void Update () 
	{
		timePass += Time.deltaTime;
		if (Input.anyKeyDown &&  timePass>0.5f ) 
		{
			endPoss ();
		}
	}
}
