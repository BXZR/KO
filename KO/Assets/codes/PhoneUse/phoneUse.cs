using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class phoneUse : MonoBehaviour {

	//这个脚本用于手机端控制的时候与游戏人物和EasyTouch交互的时候使用

	private attackLinkController theAttackLinkController;
	private move theMoveController;

	public void makeKick()
	{
		theAttackLinkController .makeAttack ("K",false);
	}

	public void makePunch()
	{
		theAttackLinkController .makeAttack ("J",false);
	}

	public void makeLinkS()
	{

		theAttackLinkController .makeAttack ("S",false);	
	}
	public void makeLinkW()
	{

		theAttackLinkController .makeAttack ("W",false);	
	}
	public void makeLinkD()
	{

			theAttackLinkController .makeAttack ("D");	
	}

	public void makeLinkA()
	{

		theAttackLinkController .makeAttack ("A");	
	}


	Vector2 move = new Vector2 (0,0);

	float time = 0;
	public void makeMove(Vector2 xx)
	{
		//这个方法的效用时间间隔大概是0.02秒
		//print (Time.time - time);
		//time = Time.time;

		//print ("XX = " + xx);
		move = xx * 2f;
	}
 


	public void makeMoveEnd()
	{
		move = new Vector2 (0,0);
	}


	private void makePhone()
	{
		theAttackLinkController = systemValues.players [0].GetComponent <attackLinkController> ();
		theMoveController = systemValues.players [0].GetComponent <move> ();
		theMoveController .enabled = false;
		theAttackLinkController.enabled = false;
	}

	// Use this for initialization
	void Start () 
	{

		if (Application.platform != RuntimePlatform.Android)
			Destroy (this.gameObject);
		else 
		{
			Invoke ("makePhone" , 1f);//等待初始化完成
		}
	}
	
	// Update is called once per frame
	void Update () {
		//没有控制的时候是需要有重力控制的
		if (theMoveController != null) 
		{
			//控制移动并且取消掉后退减速
			theMoveController.moveAction (move.x, move.y,false);
			theMoveController.makeLook ();
		}
	}
}
