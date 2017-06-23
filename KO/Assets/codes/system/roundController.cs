using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roundController : MonoBehaviour {

	//用来控制Round1或者Round2，这种的
	//为了保证封装性，在这里使用另一个脚本来统一控制这个问题
	[HideInInspector]
	public  int roundNum =0;//最开始的时候就是第一回合
	public overPanel theOverPanel;//打完了之后显示胜利者的界面，默认关闭
	public basicUIUpdate[] basicPanels;//这里进行统一调度直接调用而不经过controller
	public Text theRoundTimerShowTexy ;//用于显示当前时间的Text

	//某一个Round结束，需要判定输赢
	//这个方法被某一个场上任务死亡的时候调用
	private float roundTimer = 90f;//默认每一局的战斗时间
	//如果超过时间，判定当前生命值百分比比较高的胜利
	//如果百分比也相同，就认为两者平局
	private float roundTimerMax = 90f;//默认每一局的战斗时间

	public void roundEnd ()
	{
		systemValues.canAttack = false;
		PlayerBasic thePlayerGet = null;
		bool getWinner = false;//整个游戏是否已经结束
		for (int i = 0; i < systemValues . players.Length; i++) 
		{
			thePlayerGet =  systemValues.players [i].GetComponentInChildren<PlayerBasic> ();
			if (thePlayerGet.isAlive == true)
				thePlayerGet.winCountNow++;
			if (thePlayerGet.winCountNow >= systemValues.winCount)
			{
				//已经胜利达到一定次数
				Win(thePlayerGet);//进入胜利之后的展示
				getWinner = true;
			}
			theAI AI = thePlayerGet.GetComponent <theAI> ();
			if (AI )//如果是AI人物这个AI计算脚本需要重新启动
			{
				AI.stopAI ();
			}
		}
		if (!getWinner) 
		{
			Invoke ("makeRoundFlash", 3.5f);
			if (thePlayerGet)
				messageController .showMessage ("击倒！");
		}
	}

	public void roundEndWithTime ()//时间到了自动结束的
	{
		systemValues.canAttack = false;
		bool getWinner = false;//整个游戏是否已经结束

		//因为只有两个角色，我决定排除掉循环带来的查询顺序不公平的问题，直接用立即数
	 
		PlayerBasic thePlayerGets = null;
		PlayerBasic thePlayerGet1 = systemValues .players [0].GetComponent <PlayerBasic>();
		PlayerBasic thePlayerGet2 = systemValues .players [1].GetComponent <PlayerBasic>();

		float percent1 = thePlayerGet1.ActerHp / thePlayerGet1.ActerHpMax;
		float percent2 = thePlayerGet2.ActerHp / thePlayerGet2.ActerHpMax;

		if (percent1 >= percent2) //预防和局
		{
			thePlayerGet1.winCountNow++;
			thePlayerGets = thePlayerGet1; 
			thePlayerGet2.theAnimationController.playDead(false);//让对手“假死”
		} 
		else if (percent1 < percent2) 
		{
			thePlayerGet2.winCountNow++;
			thePlayerGets = thePlayerGet2; 
			thePlayerGet1.theAnimationController.playDead(false);//让对手“假死”
		}
		else//和局，这是一个非常罕见的现象如果不计数有可能会有问题
		{
			thePlayerGet1.winCountNow++;
			thePlayerGet2.winCountNow++;
		}

		if (thePlayerGets && thePlayerGets.winCountNow >= systemValues.winCount)
		{//已经胜利达到一定次数
			Win (thePlayerGets);
			getWinner = true;
		}
		theAI AI = thePlayerGet1.GetComponent <theAI> ();
		if (AI )//如果是AI人物这个AI计算脚本需要重新启动
			AI.stopAI ();
		AI = thePlayerGet2.GetComponent <theAI> ();
		if (AI )//如果是AI人物这个AI计算脚本需要重新启动
			AI.stopAI ();

		if (!getWinner) {
			Invoke ("makeRoundFlash", 3.3f);
		
			if (thePlayerGets)
				messageController .showMessage ("时间到!");
		}
	}

	public void makeStart()
	{
		theOverPanel.gameObject.SetActive (false);
	}

	  public   void  makeRoundFlash()//重新建立所有场上人物的状态值
	 {
		this.roundTimerMax = systemValues.roundTimerMax;
		this.roundTimer = this.roundTimerMax;
		for (int i = 0; i < basicPanels.Length; i++)
			basicPanels [i].makeWinCountFlash ();
		
		for (int i = 0; i < systemValues . players.Length; i++) 
		{
			if (systemValues.players [i].GetComponent <PlayerBasic> ().isAlive == false)
				systemValues.players [i].transform.position = new Vector3 (systemValues.players [i].transform.position.x,0.4f,systemValues.players [i].transform.position.z);
			systemValues .players [i].GetComponentInChildren<attackLinkController> ().reStart ();
			systemValues.players [i].GetComponentInChildren<attackLinkController> ().canControll = true;
			systemValues.players [i].GetComponentInChildren<move> ().canMove = true;
		}
		roundNum++;
		messageController.showMessage ("Round" + roundNum,false);//信息显示的时间已经被统一托管到信息显示控制器，也因此这里的显示机制需要调整
		Invoke ("shutRoundPanel",2.8f);
		 
	}

	private void Win(PlayerBasic player)
	{
		theOverPanel.gameObject.SetActive (true);
		theOverPanel.theGameOver(player);
		this.GetComponent <possController> ().makePoss (player .gameObject,2,1);
	}

	private void shutRoundPanel()//这才是这一轮正式开始的战斗
	{

		messageController.shut ();
		systemValues.canAttack = true;
		for (int i = 0; i < systemValues . players.Length; i++) 
		{
			theAI AI = systemValues .players [i].GetComponent <theAI> ();
			if (AI )//如果是AI人物这个AI计算脚本需要重新启动
			{
				AI.startAI ();
			}
		}
	}

	//每一帧计算得到一次这个显示内容实在是浪费，每一秒计算得到一次就可以了
	//所以用Invoke可以将计算的次数减少到1/50还是用这个吧 
	//担忧：会不会因为Invoke过多导致死锁
	void roundTimeShowUpdate()
	{
		if (systemValues.canAttack) 
		{
			roundTimer --;
			theRoundTimerShowTexy.text = roundTimer.ToString("f0");
			if (roundTimer < 0) 
			{
				roundTimer = roundTimerMax;
				roundEndWithTime ();
			}
		}
		else
			theRoundTimerShowTexy.text = systemValues .roundTimerMax.ToString("f0");
	}

	void Start () 
	{
		InvokeRepeating ("roundTimeShowUpdate",0f,1f);
	}
 
}
