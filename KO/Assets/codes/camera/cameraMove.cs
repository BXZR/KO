﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cameraMoveMode   {normal,rotate,damp}

public class cameraMove : MonoBehaviour {

	//摄像机最基本的移动脚本
	//摄像机随着游戏人物的移动而进行简单的移动
	//因为是真-横版游戏，所以可以就是左右移动了

	//public float cameraMoveSpeed = 0.12f;//摄像头的移动速度
	//public float theCameraZMin = 0;//摄像机坐标限制(左侧)
	//public float theCameraZMax = 20;//摄像机坐标限制(左侧)

	public cameraMoveMode theMode = cameraMoveMode.rotate;//摄像机的动作模式
	//一般模式：就是正常的移动不带旋转
	//旋转模式，摄像机会不断绕着视点旋转而不要固定坐标
	private float timeWaitForCamera = 0;//对于摄像机来讲，调用invokeRepeating时间间隔
	//保留值作为界限，真的没有必要进行重复计算，尤其还是除法
	//private float theXmin = Screen .width *5/11;//屏幕左侧界限
	//private float theXmax = Screen .width *7/11;//屏幕左侧界限


	//下面的一些变量是为了保留引用，减少new出来的新的vector的数量使用的一些变量
	//private Vector2 thePosition2D;//二维坐标临时保存
	private Vector3 thePosition3D;//二维坐标临时保存
	//private Vector3 thePosition3DNew;//二维坐标临时保存
	//private Vector3 theMoveForward;//摄像机的移动方向
	//private Vector3 theMotion;//具体移动的方向大小信息的保存

	private Vector3 thePosition2D1;//摄像机始终观察的三维坐标
	private Vector3 thePosition2D2;//摄像机始终观察的三维坐标
	//者人格三维坐标的计算公式为 两个player的坐标中心值
	private Transform thePlayer1;//游戏人物1
	private Transform thePlayer2;//游戏人物2
	//这两个游戏人物没有先后顺序
	private float minus ;//世界坐标转屏幕坐标之后的坐标差值（X轴）
	private float value ;//摄像机的焦距变量变化中间记录

	private bool isStarted =false;//是否已经开启
	//为了保证流畅性，摄像机的位置更新每一秒钟要更新2*系统默认运行次数\

	//计算中间的参数
	Camera theCamera;
	Vector3 thePosition;
	private Transform thisTransform;//自己的trans的引用保留


	void Start () 
	{
		//makeStart ();
		thisTransform = this .GetComponent <Transform>();//似乎用这种方式更加标准一点
		thePosition = this.transform.position;
	}


	public void makeStart()
	{
		//摄像机是最重要的现实，因此为了保证其流畅性，多更新一点
		timeWaitForCamera = systemValues.updateTimeWait / 12;
		//InvokeRepeating ("updateCameraPosition",0.02f,timeWaitForCamera);
		value = this.GetComponent <Camera> ().orthographicSize;
		theCamera = this.GetComponent <Camera> ();
		isStarted = true;
	}

	void OnDestroy()
	{
		CancelInvoke ();//在销毁的时候取消掉所有的重复调用更新方法
	}


	void playerGet()
	{
		if (!thePlayer1)
			thePlayer1 = systemValues.players [0].transform;
		if(!thePlayer2)
			thePlayer2 = systemValues.players [1].transform;
	}


	/***********************摄像机重要的底层方法**************************/
	private void getPositionForCamera()//获取坐标为摄像机更新做准备
	{
		playerGet ();
		thePosition3D = (thePlayer1.position + thePlayer2.position) / 2;//用这个值更新摄像机的坐标位置
		thePosition2D1 = Camera.main.WorldToScreenPoint (thePlayer1.position);//世界坐标转屏幕坐标
		thePosition2D2 = Camera.main.WorldToScreenPoint (thePlayer2.position);//世界坐标转屏幕坐标
	}

	private  void operateOrthographicSize ()//修改摄像机的视野大小
	{
		//修改视野大小
		minus = Mathf.Abs (thePosition2D1.x - thePosition2D2.x);
		if (minus >= Screen.width * 4 / 5)
			value += 0.01f;
		if (minus <= Screen.width *3 / 5)
			value -= 0.01f;
		//这儿离其实有点问题，有可能会有摄像机视野跳变的问题存在
		value = Mathf.Clamp (value, 2.0f, 3.3f);
		theCamera.orthographicSize = value;
	}


	/*********************************************************************************************************/
	//摄像机的经典移动方式，还是很有用的
	void cameraActMode1()
	{
		getPositionForCamera ();//获取坐标为摄像机更新做准备
		operateOrthographicSize ();//修改摄像机的视野大小

		thePosition.z = thePosition3D.z ;
		thePosition.y = 3.5f + (thePlayer1.position.y + thePlayer2.position.y)/2;
		//在游戏人物挑起的时候摄像机也会跟着向上移动（思路就是给一个加权）

		//Vector3 theLook = new Vector3 (thePosition3D .x ,1f+ (thePlayer1.position.y + thePlayer2.position.y)/2,thePosition3D .z );

		//this.transform.RotateAround (theLookPosition,new Vector3 (0,1,0),Random .Range(20,50)*Time .deltaTime);

		Vector3 theLookPosition = new Vector3 (thePosition3D .x ,1.3f+ (thePlayer1.position.y + thePlayer2.position.y)/1.75f,thePosition3D .z );


		/*
			 *这是额外的一个摄像机的效果，通过检测第一玩家输入轴的情况对摄像机做一点调整 
			 * 看上群就是一个旋转的效果
			 * 事实上因为摄像机需要实时显示，其内容不允许有跳变，所以只可以使用连续的量，在这里使用的是输入轴
			 * 此外使用绕着旋转的做法效果不太好，因为最后会lookat
			 * 
			 * 还需要注意输入轴的变量值跳变的问题
			 * 因为这个与连招按键是连在一起的，需要阻止在连招的过程中摄像机跳变
			 */
		//对于一个横板来说这只是一个非常小的效果
		if(Input.GetAxis (systemValues.forwardAxisName1)>0.6f )
			thePosition.z -= (Input.GetAxis (systemValues.forwardAxisName1) - 0.6f)*0.3f;
		else if(Input.GetAxis (systemValues.forwardAxisName1)<-0.6f )
			thePosition.z -= (Input.GetAxis (systemValues.forwardAxisName1) + 0.6f)*0.3f;


		thisTransform.position = thePosition;
		thisTransform.LookAt (theLookPosition);
	}

	/*********************************************************************************************************/
	//摄像机的移动模式2
	//根本就移动，只是不断旋转
	bool rotated = true;
	float rotateTimer = 0.5f;

	void cameraActMode2()
	{
		getPositionForCamera ();//获取坐标为摄像机更新做准备
		operateOrthographicSize ();//修改摄像机的视野大小


		thePosition.z = thePosition3D.z ;
		thePosition.y = 3.5f + (thePlayer1.position.y + thePlayer2.position.y)/2;
		//在游戏人物挑起的时候摄像机也会跟着向上移动（思路就是给一个加权）
 
		Vector3 theLookPosition = new Vector3 (thePosition3D .x ,1.3f+ (thePlayer1.position.y + thePlayer2.position.y)/1.75f,thePosition3D .z );
 
		//这是一个额外的旋转效果，但是跟时间挂钩了，如果不是长时间使用这个模式，推荐暂时关闭
		//extraRotated (theLookPosition);

		thisTransform.LookAt (theLookPosition);
	}
		
	void extraRotated (Vector3 theLookPosition)
	{

			if (thePosition2D1.x < Screen.width / 2) 
			{
				if (rotated)
				{
					rotateTimer -= Time.deltaTime;
					this.transform.RotateAround (theLookPosition, new Vector3 (0, 1, 0), 30 * Time.deltaTime);
					if (rotateTimer < 0)
					{
						rotateTimer = 1f;
						rotated = false;
					}
				}
			} 
			else 
			{ //thePosition2D2.x > Screen.width*2 / 3 &&
			    if (rotated == false)
			    {
				rotateTimer -= Time.deltaTime;
				this.transform.RotateAround (theLookPosition, new Vector3 (0, 1, 0), -40 * Time.deltaTime);
					if (rotateTimer < 0)
					{
						rotateTimer = 1f;
						rotated = true;
					}
			    }
			}

	}
	/*********************************************************************************************************/
	public  float distanceFromTarget=1.5f;//与目标的距离
	public  float moveDamp = 10f;//移动阻尼

	void cameraActMode3()
	{
		getPositionForCamera ();//获取坐标为摄像机更新做准备
		operateOrthographicSize ();//修改摄像机的视野大小

		thisTransform = this.GetComponent <Transform> ();
		//修改摄像机的位置
		thePosition.z = thePosition3D.z ;
		thePosition.y = 3.5f + (thePlayer1.position.y + thePlayer2.position.y)/2;

		Vector3 theLookPosition = new Vector3 (thePosition3D .x ,1.3f+ (thePlayer1.position.y + thePlayer2.position.y)/1.75f,thePosition3D .z );
	
		Vector3 destination  = Vector3.Slerp (this.transform .position , thePosition ,moveDamp *Time .deltaTime);

		thisTransform.position = destination - thisTransform.forward *distanceFromTarget;

		thisTransform.LookAt (theLookPosition);

	}




	void LateUpdate ()
	{
			updateCameraPosition ();
	}
	void updateCameraPosition () 
	{
		if (isStarted)
		{
			if (this.theMode == cameraMoveMode.normal)
				cameraActMode1 ();
			else if (this.theMode == cameraMoveMode.rotate)
				cameraActMode2 ();
			else if (this.theMode == cameraMoveMode.damp)
				cameraActMode3 ();
		}

 
//	     * 这是最原始的思路，放在这里仅仅作为一个参考
//	     *if(playerToLook)
//		thePosition2D = Camera.main.WorldToScreenPoint (playerToLook.position);//世界坐标转屏幕坐标
//		theMoveForward  = Vector3 .zero;
//		if (thePosition2D.x < theXmin)
//			theMoveForward = new Vector3 (-1, 0, 0);
//		else if(thePosition2D.x > theXmax)
//			theMoveForward = new Vector3 (1, 0, 0);
//		else
//			theMoveForward = Vector3 .zero;//在这里可以考虑如何使用惯性
//		theMotion = theMoveForward*cameraMoveSpeed*timeWaitForCamera;
//		this.transform.Translate (theMotion);
//		//因为对游戏人物的移动有物体碰撞的墙体所以摄像机的跟踪坐标限制没必要做
//		//但是需要考虑摄像机移动的惯性
//	     * 
//	     * 
//	     */
	 
	}
}
