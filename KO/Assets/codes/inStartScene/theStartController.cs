using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class theStartController : MonoBehaviour {
	//功能：按下任意键取消视频并且显示UI

	public GameObject theMoviePlane;//用于显示影片的plane
	public GameObject theUIRoot;//整个UI的Root随便用一个空物体就行
	public GameObject theBuildersPanel;//制作群面板
	public lightAndDarkMove lights;//阴阳太极的阳  
	public lightAndDarkMove darks;//阴阳太极的阴 
	private MovieTexture theMovieTexture;//播放动画的控件
	public static bool isPlayed = false;//因为有可能会不断跳转回来，这个时候在start里面的调用没有必要播放 
	private bool isBuildInformationShow = false;//是否显示build信息
	void Start () 
	{
 
		makeStart ();
	}

	void makeStart()
	{
		closeOthers ();
 
		if (theBuildersPanel && theBuildersPanel.gameObject.active)
			theBuildersPanel.gameObject.SetActive (false);
 
		if (isPlayed == false) 
		{
			//这个标记只有在下一次进入游戏的时候才会更新
			isPlayed = true;
			playMovie ();
		}
		else
			startWithoutPlayMovie ();
	}

	public void playMovie()//播放前尘忆梦的方法
	{
		closeOthers ();
		if (!theMoviePlane.active)
			theMoviePlane.gameObject.SetActive (true);
		theMovieTexture = theMoviePlane.GetComponent <MV>().theMovie;
		theMoviePlane.GetComponent <MV> ().makePlay ();
	}


	void closeOthers()
	{
		if (theBuildersPanel.gameObject.active)
			theBuildersPanel.gameObject.SetActive (false);
		if (theUIRoot.gameObject.active)
			theUIRoot.gameObject.SetActive (false);
	}

	public void startWithoutPlayMovie()
	{
		if (theMovieTexture) 
		{
			theMovieTexture.Stop ();
			theMoviePlane.gameObject.SetActive (false);
		}
		theUIRoot.SetActive (true);//淡入淡出的特效什么的可以在这里加方法
	}


	//制作群方法
	//显示制作群，感言等等
	public void makeInformation()
	{
 
		//存在先后顺序
		//打开的时候是先动太极图然后出界面
		//关闭的时候是先关闭界面，然后动太极图

		//还是额外建立UI显示比较好
		//还需要考虑具有“开关两个特性”

		if (isBuildInformationShow == false)
		{
			isBuildInformationShow = true;
			taiji ();
			Invoke ("build", 0.4f);//这个时间是阴阳太极图移动的时间和本身延迟时间的和
		} 
		else 
		{
			isBuildInformationShow = false;
			build ();
			taiji ();
		}
	}


	//需要有一个既短暂的延迟时间来等待太极图片闭合或者打开
	void build()
	{
		if (theBuildersPanel.gameObject.active) 
		{
			theBuildersPanel.gameObject.SetActive (false);
		} 
		else 
		{
			theBuildersPanel.gameObject.SetActive (true);
		}
	}

	//阴阳太极图的移动
	void taiji()
	{
		lights.moveOperate ();
		darks.moveOperate ();

	}

	void Update () 
	{
		if ( Input.anyKeyDown  && theMovieTexture &&theMovieTexture.isPlaying && theMoviePlane) 
		{
		   theMovieTexture.Stop ();
		   theMoviePlane.gameObject.SetActive (false);
		   theUIRoot.SetActive (true);//淡入淡出的特效什么的可以在这里加方法
		}
	}
}
