using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jiyun  :  effectBasic {
	//攻击的吸血效果
	public float lastingTime = 9f;//下过持续时间
	private GameObject theEffect;//因为没有重复使用。所以只是用一个引用不保留预设物的引用了
	private Animator thePlayerOfBeHit;//动画控制器引用保留，因为要调用多次
	float animatorSpeedMinus = 0.99f;//动画速度减少
	void Start () 
	{

		Init ();//进行初始化
	}

	private void Init ()
	{
		theEffectName = "击晕";
		theEffectInformation ="短时间内无法做出任何操作";
		makeStart ();
		thePlayer.GetComponent<move> ().canMove = false;
		thePlayer.GetComponent <attackLinkController> ().canControll = false;
		Destroy (this.gameObject .GetComponent (this .GetType()),lastingTime);
		//下面是一个小的效果
		theEffect = GameObject.Instantiate ((GameObject)Resources.Load ("effects/jiyun"));
		theEffect.transform.parent = thePlayer.transform;
		theEffect.transform.position = thePlayer.transform.position;

		//减少动画播放速度，但是不会减为负数
		thePlayerOfBeHit = this.thePlayer.GetComponentInChildren<Animator> ();
		if (thePlayerOfBeHit.speed < animatorSpeedMinus)
			animatorSpeedMinus = thePlayerOfBeHit.speed;
		thePlayerOfBeHit.speed -= animatorSpeedMinus;
		thePlayerOfBeHit.Play ("beHit");//给一个播放就好
		thePlayer .GetComponent <move>().canLook = false;
	}

	 
	public override void effectDestory ()
	{
		thePlayer.GetComponent<move> ().canMove = true;
		thePlayer.GetComponent <attackLinkController> ().canControll = true;
		thePlayerOfBeHit.speed += animatorSpeedMinus;
		thePlayer .GetComponent <move>().canLook = true;
		if (theEffect)
			Destroy (theEffect .gameObject );
	}

	void OnDestroy()
	{
		effectDestory ();
	}
		
	 
}
