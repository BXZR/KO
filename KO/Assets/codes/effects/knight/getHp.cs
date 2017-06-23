using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getHp: effectBasic {

	float lastingTime = 5f;
 

	void Start () 
	{

		Init ();//进行初始化
	}

	public override void Init ()
	{
		theEffectName = "嗜血横斩";
		theEffectInformation ="攻击并在攻击结束后回复已损生命8%\n"+lastingTime+"秒内此效果只会触发一次\n只有生命在80%之下才会生效";

		makeStart ();
		float theValue = this.thePlayer.ActerHp / this.thePlayer.ActerHpMax;
		if (this.thePlayer &&  theValue<= 0.8f)
		{
			thePlayer.ActerHp += (1-theValue) * thePlayer.ActerHpMax *0.08f;
		}
		else
			lastingTime = -999;
	} 

 
	//因为这个方法在update里面统一调配有一定的统一调用间隔时间，所以其实这个脚本不会立即消失，存在一定的冷却机制，虽然很短
	public override void effectOnUpdate ()
	{
		lastingTime --;
		if (lastingTime <= 0  || thePlayer .isAlive ==false)
		{
		   Destroy (this.gameObject .GetComponent(this.GetType()));//为了保证灵活性，也还是应该使用人工计时的方法
		}
	}

	public override void OnAttack (PlayerBasic aim)
	{
		 

	}
 
}
