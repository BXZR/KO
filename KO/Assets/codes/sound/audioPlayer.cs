using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof (AudioSource))]//需要有audioSource组件

public class audioPlayer : MonoBehaviour 
{
	//游戏人物战斗播放器
	public AudioClip theBasicAudio;//默认播放音效攻击命中
	private AudioSource theSource;//声源组件
	[HideInInspector]//不需要设定
	public AudioClip audioNow;//当前需要播放的音效

	public AudioClip attackActSource;//默认攻击动作音效

	public void playAttackActSound(AudioClip theClip = null)//播放攻击动作音效
	{
		if(theClip != null)
			theSource.PlayOneShot (theClip);
		else if (attackActSource)
			theSource.PlayOneShot (attackActSource);
	}

	public void playAttackSound()//播放攻击命中的音效
	{
		if (theSource == null)
			return;
		else if (audioNow)
			theSource.PlayOneShot (audioNow);
		else if (theBasicAudio)
			theSource.PlayOneShot (theBasicAudio);
		audioNow = null;
	}

	public void playClip(AudioClip theClip = null)//默认可以为空，播放指定的音效
	{
		if (theSource == null)
			return;
		if (theClip)
			theSource.PlayOneShot (theClip);
	}
		
	// 初始化回调方法
	void Start ()
	{
		theSource = this.GetComponent <AudioSource> ();
	}

}
