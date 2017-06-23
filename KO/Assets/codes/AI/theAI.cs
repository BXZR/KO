using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class theAI : MonoBehaviour {

	//想写一个牛逼的AI来进行强大的战斗
	//AI有分化，总标记
    //对任务进行操控的接口有这个人物所对应的轴以及相对应的输入组
	//所以大体的思路就是对这些输入的值进行模拟并且输入到对应的方法中
	//AI的模式有很多，这个类是游戏的AI的基类

	//此外，这个脚本应该被放置在游戏人物的root上方便整体调配

	//为了学习，初步计划是使用是使用两种方法进行实现
	//1 有限状态机（应该差不多）
	//2 深度学习（看算法不难，但是现在没有基础，还需要学习一下再议）

	//输入 玩家和AI角色的状态 玩家的输入 
	//输出 诸如SDWAJK数组，约定为这个数组里面只会有WASDJK六种的字母（已经实现）

	public  move theMoveController;//游戏人物移动控制单元
	public attackLinkController theActionController;//游戏人物动作控制单元
	public PlayerBasic thePlayer;//游戏人物，通通过状态等等进行决策

	private bool isAIing =false;//AI是否正在执行

	//协程用不了所以只能用脑残大法了
	private float timerLate = 0.3f;//记录时间
	private float timerLateMAx = 0.3f;//执行
	private int indexNow = 0;//当前的数组下标 


	private string stringIn = "JKWJWKDDJDDKSDJSDKSDSDJSDSDK";
 

	public virtual void startAI()
	{
		if (isAIing == false) 
		{
			isAIing = true;
			StartCoroutine ("attackCheck");
			//下面这两个方法暂时废弃
			StartCoroutine ("moveCheck");
			//StartCoroutine ("onlyCheck");
		}
	}

	public virtual void stopAI()
	{
		StopAllCoroutines ();
		isAIing = false;

	}


	public void makeStart()//初始化获取脚本等等所有的子类直接调用
	{
		thePlayer = this.GetComponent <PlayerBasic> ();
		theMoveController = this.GetComponent<move> ();
		theActionController = this.GetComponent<attackLinkController>();
		//禁止玩家输入
		theMoveController.enabled  = false;
		theActionController.enabled = false;
	}

  /*************************************************原有方法********************************************************************/
	//这是一种AI调用模式，使用协程的原因是想通过return来控制游戏人物“思考”的时间间隔
	//这里存在一个非常诡异的错误，就是这个方法其实不能产生一个字符串序列，而是产生多个重合的字符串序列，甚至可以说是乱序的
	//这个应该是跟协程的机制有关，对于顺序性这么强的我还是不推荐这个方法
	//上述错误的原因是不小心打开了2个协程结果互相干扰

	IEnumerator attackCheck()//随便写的一个字符串
	{
		int i = 0; 
		while(i<= stringIn.Length )
		{
			//0.2秒一算一次下一个动作
			yield return new WaitForSeconds (0.5f);
			//print ("read: "+stringIn[i].ToString());
			theActionController.makeAttack (stringIn[i].ToString(),false);
			//千万注意，这里的移动参数只能是0-1否则会有不可预料的移动方向
			i++;
			if (i == stringIn.Length)
				i = 0;
		}
	}

/*************************************************原有方法********************************************************************/


	//这个方法每一帧都会调用的
	//与上面的协程方法不同的是，这个
	void makeAttackAction()
	{
		if (systemValues.canAttack)
		{
			timerLate -= Time.deltaTime;
			if (timerLate < 0) {
				timerLate = timerLateMAx;
				//print ("--"+stringIn [indexNow]);
				theActionController.makeAttack (stringIn [indexNow].ToString (), false);
				indexNow++;
				if (indexNow >= stringIn.Length)
					indexNow = 0;
			}
		}
	}

	//这是一种AI调用模式，使用协程的原因是想通过return来控制游戏人物“思考”的时间间隔
	IEnumerator moveCheck()//随便写的一个字符串
	{
		while(systemValues .canAttack )
		{
			yield return new WaitForSeconds (0.02f);
			theMoveController .moveAction (this.transform .forward .z ,0f);//游戏人物移动的输入接口
		}
	}



	void Start () 
	{
		makeStart ();
		startAI ();
	}

	void Update () 
	{
		//makeAttackAction ();
	}
}
