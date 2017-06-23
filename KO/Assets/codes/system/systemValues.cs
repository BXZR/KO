using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameType : int {PVP,PVC,CVC};
//PVP people vs people
//pvc people vs computer
//cvc computer vs computer
public class systemValues : MonoBehaviour {
	//这个类保有全局静态公有参数以及方法
	//作为程序设定的基本设置参数
	//这个文件的部分内容最重要写到配置文件中
	//这是对配置文件唯一进行接触的类

	//全局配置文件======================================非常重要=======================================================

	//游戏人物的轴与按键值的转换关系
	//因为只有两个轴才可以这么用
	public static string forwardAxisName1 = "forward1";
	public static string forwardAxisName2 = "forward2";
	public static string upAxisName1 = "up1";
	public static string upAxisName2 = "up2";

	//各种按键的转换公式（仅仅是一个权宜之计，不是什么好方法）
	//连招检测固定为WASDJK几个值，用户使用不同的值时候需要有一个转换
	//玩家操作的字符列表
	public static string  [] theInputToMatchForPlayer1 ={"W", "S","A","D","G","H" };
	public static string  [] theInputToMatchForPlayer2 ={"UpArrow", "DownArrow","LeftArrow","RightArrow","K","L" };
	//最终的连招检查检查的就是WASDJK字符串
	public static string  [] theInputToReturnForPlayer ={"W", "S","A","D","J","K" };
	//这个数组就是在选择界面的显示的连招提示
	public static string  [] theInputToReturnForPlayerChinese ={"上", "下","左","右","拳","腿" };

	public static GameObject[] players;//获取游戏人物模型

	//增加新人物的时候增加这里面的项目就可以了
	//这些数组的内容是纵向对应的并且与Resources文件夹中资源内容相对应
	public static string[] thePlayerNamesForPlay = {"theFighterAshe","theFighterIce"};
	private static string [] fighterInResourceNames = {"theFighterWukong","theFighterIce","theFighterdemon","theFighterknight","theFighterknife","theFighterAshe"};
	public static string[] fighterPicInResourceNames = {"wukong","iceMonster","DEMON","knightHead","knifeHead","asheHead"};//获取头像名称用于显示
	public static string[] fighterCharacterNames = {"孙悟空","冰原魔人","地狱恶魔","恶灵骑士","归海一刀","寒冰射手"};//获取头像名称用于显示

	public static string [] theFighterStories =
	{
		"七龙珠的孙悟空，绝世的武道家，拥有极强的爆发能力",
		"来自严寒地带的战士，拥有极强的生存能力和丰富多彩的战斗技术",
		"来自地狱的恶魔，拥有高额的伤害和强韧的身体",
		"战场骑士灵魂幻化而成，坚守着骑士道，并渴望着能够展示骑士的力量",
		"天下第一刀归海一刀灵魂穿越而来，为寻找同样穿越过来的上官海棠而战",
		"为了弗雷尔卓德，艾希参加了战斗"
	};
	public static string[,] fighterWinTalk = 
	{
		{"你的攻击很精彩，或许我们应该再打一场","我在这一场战斗中学到了很多","龟派气功看来仍有可开发的空间啊"},
		{"没有人能够击败来自极寒的力量","我们经历过的风雨不同啊","冰霜的力量或许超乎你我的想象"},
		{"又一个阻碍我称霸天下的人倒下了","我，就是这个这个世界的主宰","颤抖吧，卑微的人类"},
		{"这是一场精彩的战斗","我要讴歌你的骑士道","战斗，还将继续下去"},
		{"人即是刀，人生也即是刀","没有刀，我将无法守护，有了刀，我却无法拥抱","刀，并不是一个可以简单驾驭的东西"},
		{"和平，需要不懈的努力","世间万物皆系一箭之上","为了弗雷尔卓德！"}
	};
	private static int indexForFighterName = 0;//轮转到的name下标

	//非战斗状态机状态记录（取消掉非战斗状态之下的偶然碰撞其碰撞）
	public static string[] isNotAttackStateName = { "drop","dropReStand","dead","moveMent","poss","beHit"};
	public static string theShowPossStateNAme = "poss";//在开战之前的嘚瑟动作
	public static string canAttackStateInBasicLayer = "moveMent";//动画层1只有在这个状态之下才可以运行战斗动画
	public static string[] isNotMoveStateName = {"drop", "dropReStand","dead","poss"};//在这些状态下不能移动
	public static string[] isNottoBeAttackStateName = { "dropReStand","dead","drop" };//在这些状态下不会受伤
	public static int theNotAttackLayerIndex = 0;//不攻击的状态全放在这一个动画层
	public static int theAttackLayerIndex  = 1;//攻击状态都放在这个层里面
	/****************************************************************************************************/
	//全局设置参数
	public static  bool isGUIShowHP = false;//是否使用GUI显示人物的生命值
	//两个角色最大的距离，在战斗场景中两个人物之间距离超过这个不可以再远离了
	public static float maxDistance =10f;
	//每隔多长是时间更新一次一些核心的内容
	//大量使用involkeRepeating方法，这个参数用于设定时间间隔（第三参数）
	//所有非update托管的更新都需要依靠这个参数
	//这样可以在降低实时性的条件下大幅减少计算
	//以0.2为例，每秒钟只计算5次，这在较高的FPS游戏中减少的计算量是很大的
	public static float updateTimeWait = 0.2f;
	//胜利制度
	public static int winCount= 2;//三局两胜制度
	//这个胜利次数有范围，只可能是1（一局胜利）、2（三局两胜）、3（五局三胜）
	public static bool canAttack = false;//在最开始的时候有一小段时间是不能够做出攻击动作的
	//这个参数由roundController控制，attackLinkController进行判断
	public static float roundTimerMax = 120f;//每一局战斗最多时间长度
	//是否可以在战斗中取消后摇，也就是直接切换动作
	//推荐是false各种取消后摇与修改动作真的是各种动作不完全，需要考虑的东西也比较多
	public static bool canInteruptAction = false ;

	//全局状态参数
	//用枚举的方式而不是int标记是因为代码读者更容易看懂
	//但说实话，如果只是用int 标记其效率当然会高一点
	public static GameType theGameType = GameType.PVP ;
	//public static int theGameMode = 0;
	//PVP people vs people
	//pvc people vs computer
	//cvc computer vs computer

	//这个标记用于检查是否是人机对战，这在开始界面中就已经进行赋值了
	//如果isPVC是true需要在初始化玩家模型2的时候额外添加AI脚本并且在AI脚本初始化的时候进行一些额外的初始化操作

	/************************************************AI相关**********************************************/
	//AI 脚本字符串，使用这个字符串为AIn进行赋值，这是一个数组，表名使用哪一个AI，这使得AI也可以有很多可变的情况了
	public static string [] theAISCripts = {"theAI","AI_stage"};
	public static int indexOfAI = 1;//当前选择的AI脚本名称下标、、这个可以在开始界面进行选择，是为难度的选择 
	public static float AIThinkTimer = 0.2f;//AI思考状态转换的时候的时间间隔
	//public static float attackDistance = 0.7f;//AI的攻击范围，关系到状态转换
	//这个参数暂时废弃，由每一个playerBasic控制
	public static float theAIActionThinkTime = 0.6f;//AIn角色发起的操作之间的时间间隔，这个值越小AI越凶狠（但是数值太小就会发生颤动，这个还需要调整一下）

	/********************************************BGM***************************************************/
	public static string theBGMName = "fightBGM";//播放的BGM的名称，这个可以根据任务不同、条件不同有所改变

	/********************************************精彩瞬间截图***************************************************/
	public static int pictureLoadCount = 5;//在精彩时刻界面中最多加载的图片数量（如果加载图片太多，会很卡）
	public static string thePicEndType = ".jpeg";//截图默认保存格式，之所以选这个格式是因为这个格式最小，否则I/O时间太长会卡
	public static bool canMAkePicture = true;//是否允许截图（在比较低配置的机器上面截图会卡）

	/******************************************有关颜色***************************************************/
	//所有的颜色标签都在这里设置
	public static string normalColor = "<color=#000000>";//什么都不加成的颜色 黑色
	public static string BESkillColor = "<color=#FF2400>";//被动技能的颜色   橙红色
	public static string NBESkillColor = "<color=#6B238E>";//主动技能的颜色   深石板蓝
	public static string publicSkillColor = "<color=#7F00FF>";//公有技能的颜色
	public static string importantColor = "<color=#FFFF00>";//重点颜色
	public static string normalAttackLinkColor = "<color=#855E42>";//一般招式的颜色
	public static string hasSkillColor = "<color=#8E1717>";//有技能的连招的颜色
	public static string colorEnd = "</color>";

	/******************************************有关场景***************************************************/
	public static string theFightSceneName = "KO";//战斗用的场景，有一些效果在非战斗场景没有冷却时间






	/********************************************方法集********************************************/
	//获取相关的战斗胜利感慨语言
	public static string getWinSpeak(string theResourceName)
	{
		int theIndex = -9;
		for (int i = 0; i < theResourceName.Length; i++) 
		{
			if (fighterInResourceNames [i] == theResourceName) 
			{
				theIndex = i;
				break;
			}
		}
		if (theIndex >= 0)
		{
			int length = fighterWinTalk.GetLength (1);
			//这个写法实际上不是什么好的方法
			//int row = Arr.GetLength(1);  //第一维的长度（即行数）
			//int col  = Arr.GetLength(0); //第二维的长度（即列数）
			//这就意味着所有的人生里感言数组被固定长度了，有一点不太可扩展
			int theIndexGet = Random.Range (0, length-1);
			return  fighterWinTalk [theIndex,theIndexGet];
		}
		return "";
	}


	public static bool canBeAttack(Animator theAnimator)
	{
		for (int i = 0; i < isNottoBeAttackStateName.Length; i++) 
		{
			if (theAnimator.GetCurrentAnimatorStateInfo (theNotAttackLayerIndex).IsName (systemValues .isNotAttackStateName[i]))
				return false;
		}
		return true;
	}


	public static bool canMoveState(Animator theAnimatorOfPlayer)
	{
		for (int i = 0; i < systemValues.isNotMoveStateName.Length; i++) 
		{
			if (theAnimatorOfPlayer.GetCurrentAnimatorStateInfo (theNotAttackLayerIndex).IsName (systemValues.isNotMoveStateName[i]))
				return false;
		}
		return true;
	}
	public static bool canChangeToNextAttack(Animator theAnimator)
	{
		//非攻击状态下的才可以转换，否则不行
		//默认只有在攻击层的状态转换才能判断这个
		//因为分层，这里有一些内容已经需要有改变了，人物能够进入攻击动作的条件：
		//层1处于moveMent状态，也就是没有异常状态例如beHit
		//层2处于moveMent状态，也就是上一个攻击动作已经完成
		if (theAnimator.GetCurrentAnimatorStateInfo (theNotAttackLayerIndex).IsName (canAttackStateInBasicLayer) && 
			theAnimator.GetCurrentAnimatorStateInfo (theAttackLayerIndex).IsName (canAttackStateInBasicLayer)) //在层1中只有在移动状态下才可以进行着各种战斗动作
		return  true;
		
		return  false;
	}
	public static bool canInteruptActionInAttack(attackLinkController theAttackLinkController)
	{
		//如果传入的是空值，那么有可能不是在战斗状态，在非战斗场景怎么乱动都可以
		if (!theAttackLinkController) 
		{
			//这是一个很特殊的机制，用于在选人界面里面进行动作控制
			//print ("nullController");
			return true;
		}
		//如果允许中断，啥也不说了直接返回true
		if (canInteruptAction )
			return true;
		//如果不允许中断就需要看状态
		//在攻击状态之下不允许中断，一般状态随意
		return (canChangeToNextAttack(theAttackLinkController .theAnimator));

	}

	public static string getAIScript()
	{
		return theAISCripts [indexOfAI];
	}


	//现在有图片名，我需要获取到预设物名，所以在这里写一个转换
	//这个方法其实很烂的
	//这个方法的应用前提：所有的相关信息数组都是纵向对应的，例如第一项都是关于孙悟空的
	public static string getResourceNameWithPictrueName(string thePictureName)
	{
		for (int i = 0; i < fighterPicInResourceNames.Length; i++) 
		{
			if (fighterPicInResourceNames [i] == thePictureName)
				return fighterInResourceNames [i];
		}
		return "";
	}

	public static string  getHeadPictureNameWithPlayerName(string nameIn)
	{
		string headPictureName = "";
		for (int i = 0; i <fighterInResourceNames.Length; i++) 
		{
			if (fighterInResourceNames [i] == nameIn)
				return fighterPicInResourceNames  [i];
		}
		return "";
	}

	//将SDSDJ这种格式翻译成为下前下前拳
	//WASDJK是默认定好的，系统自定义，应该不会改了
	//如果修改，可以修改systemValue类和attackLinks的各自的设置
	public static  string getattackLinkChinese(string attackString)
	{
		string information = "";
		char[] array = attackString.ToCharArray ();
		for(int i=0; i < array.Length;i++)
		{
			for (int j = 0; j < theInputToReturnForPlayer.Length; j++) 
			{
				if (array [i].ToString() == theInputToReturnForPlayer [j]) 
				{
					information += theInputToReturnForPlayerChinese [j];
					break;
				}
			}
		}
		return information;
	}


	public static string fighterForwardNameGet()//获取下一个fighterName
	{
		indexForFighterName++;
		if (indexForFighterName >= fighterInResourceNames.Length)
			indexForFighterName = 0;
		return fighterInResourceNames[indexForFighterName];
	}
	public static string fighterBackNameGet()//获取上一个fighterName
	{
		indexForFighterName--;
		if (indexForFighterName < 0)
			indexForFighterName = fighterInResourceNames.Length - 1;
		return fighterInResourceNames[indexForFighterName];
	}
	public static string fighterRandomNameGet()
	{
		int index = Random.Range (0,fighterInResourceNames.Length);
		indexForFighterName = index;
		return fighterInResourceNames[index ];
	}
	public static string fighterZeroGet()//获取上一个fighterName
	{
		return fighterInResourceNames[0];
	}

	public static string fighterNowGet()//获取上一个fighterName
	{
		return fighterInResourceNames[indexForFighterName];
	}

	public static void flashForSelect()//一个玩家选择完成之后的重新初始化
	{
		indexForFighterName = 0;
	}

	public static string getPicName()
	{
		return fighterPicInResourceNames[indexForFighterName];
	}
	public static string getStory()
	{
		return theFighterStories[indexForFighterName];
	}

	public static string getCharacterName()
	{
		return fighterCharacterNames[indexForFighterName];
	}
	//这个距离用于限制两个人物之间的最大距离
	//原则上超出这个最大距离就会进制横向移动（一些效果的推动力例外）
	//但这个算法仅仅是针对两个游戏人物进行格斗的效果
	//因为如果是横板冒险游戏没有必要限制这个距离
	public static float nowDistance(Transform T)//返还两个角色之间的距离
	{
		try
		{
		if (players [0] .transform== T)
			return players [1].transform.position.z;
		else
			return players [0].transform .position.z;
		}
		catch
		{
			return 3;
		}
		
	}

	//获取目标单位
	public static  GameObject   getEMY(Transform T)//返还两个角色之间的距离
	{
		
		if (players [0] .transform== T)
			return players [1];
		else
			return players [0];
	}


	//用户输入与机器中内容之间的翻译
	//在连击效果中默认写法 上下左右拳脚分别为 WSADJK
	//但是用户的键位设置未必就是这些值，所以需要一个转换
	//用于匹配查找的输入数组就是当前玩家1和玩家2的键位，这个按理说应该根据配置文档进行初始化
	public static char getFromA(int index,string  input)
	{
		char theReturnChar = ' ';
		if (index == 0) { //玩家1
			for (int i = 0; i < theInputToMatchForPlayer1.Length; i++) {
				if (input == theInputToMatchForPlayer1 [i]) {
					theReturnChar = theInputToReturnForPlayer [i].ToCharArray () [0];//所以说返回的仍然是首字母
					return theReturnChar;
				}
			}

		} 
		else 
		{
			for (int i = 0; i < theInputToMatchForPlayer2.Length; i++) {
				if (input == theInputToMatchForPlayer2 [i]) {
					theReturnChar = theInputToReturnForPlayer[i].ToCharArray () [0];
					return theReturnChar;
				}
			}
		}
		return '?';//返回必然出错信息
	}






	//初始化方法，由控制器统一调用
	public void makeStartForSystem()
	{
		//因为这个获取的过程，本脚本应该在场景中有一个实例
		players = GameObject.FindGameObjectsWithTag ("Player");
	}

	//void Start () 
	//{
		//makeStart ();
	//}
 
}
