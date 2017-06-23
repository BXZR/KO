using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theSoundController : MonoBehaviour {
	//这个类专用用于管理较长的音效
	//拟使用AssetBundle这种方式加载，因为音乐这种东西还是想做成随便换的
	//模型、预设物这些我不想给出这些改变，暂时使用的方式是Resource
	 
	public AudioClip theClip;//默认的Clip
	private AudioClip theClipNow;//当前正在播放的Clip
	private AudioSource theSource;//挂在在自身上面的声源组件

	//事实上我也在考虑每一个人物都有不同的BGM，这个可以用这种动态加载的方法而不是用数组
	//因为是用数组的方法就意味着所有的BGM都会加载这不是我们想看到的
	//此外的可扩展：变化的BGM（可控还是不可控）


     //加载音乐的方法（注意，加载音效的方法不是在这里处理的）
	void loadMusicForGame()
	{  
		try
		{
           //最开始的时候尝试从资源中加载
		   //theBGMName存放在systemValues里面，全局共享
		   theClipNow = Resources .Load<AudioClip> ("audios/" +systemValues .theBGMName);
		}
		catch
		{
			//如果加载失败就直接用赋值过来的默认组件
			theClipNow = theClip;
		}
		finally 
		{
			if (theClipNow) 
			{
				theSource.clip = theClipNow;
				theSource.Play ();
				theSource.loop = true;
			}
		}
	}
	 
	void Start () 
	{
		this.theSource = this.GetComponent <AudioSource> ();
		loadMusicForGame ();
	}
	

}
