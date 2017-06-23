using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotateInPoss : MonoBehaviour {

	//这只是一个小效果
	//在任务开打之前摆Poss的时候摄像机简单的动作

	public float speedMode = 1;//只可能是1和-1两种值，分别表示两种不同的旋转方向
	Vector3 Axis= new Vector3 (0,1,0);//保留引用

	public void setMode(int I)//这个一定会有数学公式的方法计算的，想必也是一个优化点
	{
		if (I == 0)
			speedMode = 1;
		else
			speedMode = -1;
	}

	void Update () 
	{
		this.transform.RotateAround (this.transform .root .position ,Axis,20*Time .deltaTime * speedMode);
	}
}
