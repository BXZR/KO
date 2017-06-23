using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class attackLinkController : MonoBehaviour {

	private int index = 0;//当前全局搜索位置
	public attackLink[] attackLinks;//保存连招公式的数组
	private List<attackLink> attackLinkMayUsing;//当前可能使用的操作公式
	private List<attackLink> attackBeDelete;//删除中转站，记录需要删除的信息
	private Event events;//当前的获取的事件
	private KeyCode keyUse;// 检测到的键值
	public float timerForLinkAtack=0.5f;//必须在一定时间内按出来这个串，否则无法使用技能
	public float timerAddForCheck= 0.2f;//按对了一次按键得到的时间奖励
	private float timerForLinkMax = 0.5f;//tim的erForLinkAtack的默认最大时间
	bool startTimer = false;//是否开启计时器
	public float timerDifficulty = 0.75f;//时间难度，越大难度越大
    //上面这些参数计算关乎于连招

	//下面的参数关乎于游戏整体的联系和结合
	private PlayerBasic thePlayer;//这个游戏人物的对象
	[HideInInspector]
	public Animator theAnimator;//动画控制器
	[HideInInspector]//为了保证设定面板的简洁，暂时隐藏之
	public string deadAnimatorName ="dead";//死亡动画状态名称(这个名字做成默认名字因为没有必要分化)
	[HideInInspector]//为了保证设定面板的简洁，暂时隐藏之
	public string dropAnimatorName ="drop";//跌倒动画状态名称(这个名字做成默认名字因为没有必要分化)
	[HideInInspector]//为了保证设定面板的简洁，暂时隐藏之
	public string beHitAnimatorName ="beHit";//跌倒动画状态名称(这个名字做成默认名字因为没有必要分化)


	//public string basicPunchKey = "J";//基础的拳头按键
	//public string basicKickKey = "K";//基础的踢腿按键

	private bool isStarted = true;//是否开启
	public bool canControll = true ;//是否可以通过玩家/AI进行操作

	//用于存储的内容容易存储在systemValue类
	public bool canChangeToNextAttack()
	{
		 //在这里加上trycatch是为了保证一定会转换
		try
		{
			return systemValues .canChangeToNextAttack(this.theAnimator);
		}
		catch (Exception R)
		{
			print (R.Message);
			return true;
		}
	}

	//有些键位是具有方向性的
	//当游戏人物的朝向不同的时候会有不同的招式触发方式
	//例如： 一个招式的连击触发字符串是 SDJ，这其实是有两个含义
	//当游戏人物面向正面的时候SDJ触发，而当游戏人物面向背面的时候，SAJ触发
	//D始终被翻译成游戏人物的正面朝向（AD均有可能）
	//A始终被翻译成游戏人物的背面朝向（DA均有可能）
	char directForKey(char keyChar)
	{
		if (this.transform.forward.z < 0)
		{
			//其实正宗一点的做法应该是
			//if(keyChar == 轴的正方向键位)
				//keyChar = 轴的反方向键位
			//AD有方向性
			if (keyChar == 'D') keyChar = 'A';
			else if (keyChar == 'A') keyChar = 'D';
		}
		return keyChar;
	}
	//检测连招的方法，每一次按键都要求检测
	//这个是根据玩家的输入进行控制的方法，在制作玩家AI时需要继承并重写方法
	// useInterpret标记着是否使用对应的翻译机制
	//对于人的输入必须要用翻译，对于AI则不需要
	public virtual void makeAttack(string keyString,bool useInterpret = true) 
	{
		
		if (!canControll)
			return;//如果当前是不可操控的，那么直接返回
	 
	 
		attackBeDelete.Clear ();//每一次检测都会刷新这个集合
		char keyChar=' ';

//
//		print ("=============");
//		print (keyString);
//		print ("Player1 " + systemValues .getFromA( 0 ,keyString));
//		print ("Player2 " + systemValues .getFromA(1 ,keyString));


		if (string .IsNullOrEmpty( keyString) ==false)
		{ //此处有待商榷，用这种方式只能支持A-Z的按键输入（需要加入转换的方法）

			//keyChar = keyString.ToCharArray () [0];//取出这个字符
			if (useInterpret)//人输入也就是默认状态下是需要“翻译”的而AI输入或者用于展示的输入则不需要这一步
			{
			   keyChar = systemValues.getFromA (thePlayer.playerIndex, keyString);
			   keyChar = directForKey (keyChar);//按键的方向调整
			}
			else
               keyChar = keyString.ToCharArray() [0];
			
			//print("--keyChar--"+keyChar);

			foreach (attackLink AL in attackLinkMayUsing) //输入符合要求，可以进行下一步的检测了
			{
				char getChar = AL.getCharWithIndex (index); //获取char
				//正是因为获取的是char，这个方法有很大的限制
			   //print (getChar);
				if (   getChar != keyChar ) 
				{
					attackBeDelete .Add (AL);//将不符合规范的输入去除掉
				}
			}
			foreach (attackLink  AL in attackBeDelete)
			{
				//因为List数据结构正在被使用（foreach），因此需要将删除与遍历分开进行
				attackLinkMayUsing.Remove (AL);//记录当前需要删除的技能
			}
			if (attackLinkMayUsing.Count == 0) 
			{//如果当前已经没有用于检测的串，说明输入不对，重头开始
				flashLink ();//更新列表
				reMake();//完全重头开始
			}
			else 
		    {
				startTimer = true;//检测到了符合的公式，则开始计时
				timerForLinkAtack += (timerAddForCheck* 1/timerDifficulty);
				//有一点加成等待下一个键位的输入（手残党的福利）
            	check ();//进行检查
			}
		}
		else
		{
			flashLink ();//更新列表
			reMake();//完全重头开始
		}	
	}

	//一次性检查某一个连招是否已经成立
	//例如某个连招的按键为SDK那么如果参数为“SDK”就认为这个招式可以使用了
	//如果输入过长的字符串，以检测到的最后一个招式为准
	public virtual void makeAttackLink(string keyString,bool useInterpret = true) 
	{
		for (int i = 0; i < keyString.Length; i++)
			makeAttack (keyString [i].ToString(),useInterpret);
	 
	}


	void check()//真正进行检测的方法
	{
		bool isOver = false;//标记量
		foreach(attackLink AL in attackLinkMayUsing)
		{
			if (AL.attackLinkString.Length == index + 1 )//因为长度和index的关系，就比本的要求就是输入正确（当然还有SP等等的需求）
			{
				if(canChangeToNextAttack())
				{
				thePlayer.onPlayattackActions ();
				AL.attackLinkEffect ();//发生效果
				flashLink ();//更新列表
				reMake();//完全重头开始
				isOver = true;//标记量，是否已经使用了一个技能
				break;//每一次生效的连招只能够有一个
				}
			}
		}
		if(!isOver )
			index ++;//向下一个目录前进
	}

	void reMake()//其他用于恢复初始状态的参数设置
	{
		timerForLinkAtack = timerForLinkMax*1/timerDifficulty; //更新时间到初始上线
		startTimer = false;//关闭计时器
		index = 0;//完全重头开始
	}


	//这个搜索方式是减法机制的，因此需要在初始化个更新的时候获得所有的公式
	//减法机制：每按一个键就会从公式集合中删除掉不符合规范的额公式
	//如果公式集合空，则重建公式集合，之前的探索完全作废
	void flashLink()
	{
		//清空并重新获取所有的公式
		attackLinkMayUsing.Clear ();
		for (int i = 0; i < attackLinks.Length; i++) 
		{
			attackLinkMayUsing.Add (attackLinks[i]);
		}
	}


	public void makeStart()//这个方法是用于连招本身的初始化方法+
	{
		attackLinks = this.GetComponentsInChildren<attackLink> ();
		attackLinkMayUsing = new List<attackLink> (); //重建这个对象
		attackBeDelete = new List<attackLink> ();//重建对象
		timerForLinkAtack *= 1/timerDifficulty;//根据难度调整时间
		timerForLinkMax = timerForLinkAtack; //唯一的一次对时间上限的写操作
		flashLink ();//更新列表
		index = 0;//完全重头开始
		flashLink ();//列表更新
		reMake();//参数更新
		makStartExtra ();//其他的初始化
		isStarted = true;
	}

	public void reStart()//击倒之后进入下一个回合
	{
		thePlayer.ActerHpMax = thePlayer.CActerHpMax;
		thePlayer.ActerHp = thePlayer.ActerHpMax;
		thePlayer.ActerSpMax = thePlayer.CActerSpMax;
		thePlayer.ActerSp = thePlayer.ActerSpMax;
		if (thePlayer.isAlive == false) 
		{
			theAnimator.CrossFade ("dropReStand", 0.25f);
		}
		thePlayer.isAlive = true;
		this.GetComponent<CharacterController> ().enabled = true;//关闭物理效果使之能够下降
	}

	private void makStartExtra()//这个是用于与工程中其他的脚本进行联系的初始化
	{
		thePlayer = this.GetComponentInChildren<PlayerBasic> ();//获取游戏玩家对象
		theAnimator = this.GetComponentInChildren<Animator>();
	}

	public void playDead(bool extra = true)//死亡的动画状态在这里就可以了，这个没有必要由分动画元素控制，就放在总控制器中
	{
		thePlayer.isAlive = false;
		theAnimator.CrossFade (deadAnimatorName ,0.25f);
		this.GetComponent<CharacterController> ().enabled = false;//关闭物理效果使之能够下降
		this.transform .position = new Vector3 (this.transform .position .x ,-0.15f,this.transform.position .z);
		if(extra)//有些时候没有必要强制roundEnd而只是需要一个效果，那么传入false作为参数即可
		GameObject.Find ("/gameController").GetComponent<roundController> ().roundEnd ();

		//Destroy (this.gameObject ,0.75f);
	}

	public void playDrop()//跌倒的动画状态在这里就可以了，这个没有必要由分动画元素控制，就放在总控制器中
	{
		theAnimator.CrossFade (dropAnimatorName ,0.25f);
	}

	public void playFaint()//被击中
	{
		theAnimator.CrossFade (beHitAnimatorName ,0.25f);
	}
    ///////////////////////////////////////////////////////////////////////////////////////////////以下是真正的调用方法入口

	//初始化操作
	void Start ()
	{
		 
		//makeStart ();//连击初始化
	}
	//这个应该是因为OnGUI比起Update刷新快造成的
	//这个方法在Update里面是无法使用的，events会被放空
	void OnGUI()
	{ 
		if (isStarted)
		{
			if (Input.anyKeyDown &&thePlayer&& thePlayer.isAlive && systemValues.canAttack) 
			{
				//为了保证更强大的兼容性暂定使用Event的方法，虽然这种方法有额外的开销
				//InputString是一个非常强势的方法，但是比较有缺陷的就是只是针对有输出的键位有效果
				//例如QWE等等，但是对于ctrl等等貌似无效果（作为一种替换的键位监测方案放在这里）
				//此外值得注意的就是InputString检测到的键位输出是小写的
				//print (Input.inputString+" isUsed");
				 
				events = Event.current;
				if (events != null && events.isKey) 
				{
					if (events.keyCode != KeyCode.None) 
					{
						keyUse = events.keyCode;
						makeAttack (keyUse.ToString ());//进入连招检查
					}
				}
			}
		}
	}
	//这里只对计时器有更新
	void Update () 
	{
		if (isStarted&& startTimer && thePlayer.isAlive) 
		{
			timerForLinkAtack -= Time.deltaTime;
			if (timerForLinkAtack <= 0) 
			{
				flashLink ();//列表更新
				reMake();//参数更新
			}
		}
	    
		//if (thePlayer.isAlive == false)
		//{
				/*
				 * if (Input.GetKeyDown (KeyCode.Space))
				{
			
					reStart ();
				}
				*/
		//}
	}
}
