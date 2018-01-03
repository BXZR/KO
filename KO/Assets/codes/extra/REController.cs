using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class REController : MonoBehaviour {

	//这个脚本强制就改分辨率
	void Start () 
	{
//		if (Application.platform != RuntimePlatform.Android)
//		{
//			Screen.fullScreen = false;
//		}
//
		Screen.SetResolution (1024, 600, true, 60);
		this.GetComponent <Camera > ().aspect = 1024f / 600f;

		if (Application.platform != RuntimePlatform.Android)
		{
			Screen.fullScreen = false; //这个游戏强制窗口模式（因为强制了分辨率）
		}
//
		//Screen.fullScreen = false;需要做两遍，否则就会出现不正确的情况
		//但是如果将GUI的影片播放延迟一小点时间就不会出现问题
		//这可能是因为最开始的时候的GUImovieTexture与screen之间的值的矛盾
		Destroy (this.gameObject.GetComponent (this.GetType ()));
	}
	
 
}
