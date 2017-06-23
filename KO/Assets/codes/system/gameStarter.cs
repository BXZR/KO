using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameStarter : MonoBehaviour {

	//游戏初始化选人物的时候等等初始化信息
	//游戏中所有资源的统一初始化脚本

	public systemValues theSystem;//游戏控制器进行初始化
	//跨场景的任务名称选取(只可能是两个人)
	//即使是游戏预备人物增多，这里也不应该有所修改

	public GameObject [] thePlayerGameObjects;//游戏人物进行初始化
	public PlayerBasic [] thePlayers;//游戏人物进行初始化
	//几乎所有的游戏初始化都是根据游戏人物来进行的
	//当选择人物的时候，顺带初始化这两个人物，就可以初始化全部

	public cameraMove theCameraMove;//游戏摄像机移动脚本
	public basicUIUpdate [] theUIs;//基本UI的初始化
	public Transform [] startPositions;//游戏人物的起始位置，因为程序已经设定好了，在初始化设定旋转角没什么意义。
	//这个起始位置只会有两个

	//全游戏真正的初始化
	//在这里的初始化是存在初始化顺序的
	//在这里要千万注意
	private bool isStarted = false;//全局初始化只可以被使用一次
	playerWeapon[] weapons;//临时中间变量

	public roundController roundController;//显示多少轮的控制器，一般都挂在控制器上
	public pictureMaking thePictueController;//截图使用控制器
	//private bool isShowingPoss = false;//是否在展示打架的人的POSS如果是，就可以通过按下任意键取消掉



	//初始化方法(一共有两个)
	//初始化方法一共有两个，其一是makeStart，这个是最初初始化的内容
	//第二阶段的初始化为makeAllStart这个是在摆完POSS之后的初始化设定

	public void makeStart()
	{

		if (isStarted == false) {
			//加载游戏场景
			thePlayerGameObjects = new GameObject[2];
			thePlayers = new PlayerBasic[2];
			//数值参数的设定

			for (int i = 0; i < systemValues . thePlayerNamesForPlay.Length; i++) 
			{
				thePlayerGameObjects [i] = (GameObject)Resources.Load ("fighters/" + systemValues . thePlayerNamesForPlay [i]);
				thePlayerGameObjects [i] = (GameObject)GameObject.Instantiate (thePlayerGameObjects [i]);
				thePlayers [i] = thePlayerGameObjects [i].GetComponent<PlayerBasic> ();
				thePlayers [i].transform.position = startPositions [i].position;
				thePlayers[i].headPictureName = systemValues.getHeadPictureNameWithPlayerName (systemValues . thePlayerNamesForPlay [i]);//从此，人物头像名称统一由systemValues控制
				thePlayers [i].makeStart ();
				theUIs [i].thePlayerToUpdate = thePlayers [i];
				theUIs [i].makeStart ();
				thePlayers [i].GetComponent<attackLinkController> ().makeStart ();


			}

			//这只是一个简单的着色方法，但是这个方法效率实在是很低下而且效果一般
			//这个方法暂时先放在这里表示一种思路，如果有缘，或许会有更好的方法修改
			/*
			string acterName = thePlayers [0].ActerName;
			for (int i = 1; i < thePlayerNames.Length; i++) 
			{
				if (thePlayers [i].ActerName == acterName) 
				{
					MeshRenderer [] render = thePlayers [i].transform .root .GetComponentsInChildren<MeshRenderer> ();
					for (int j = 0; j < render.Length; j++) 
					{ 
						for (int k = 0; k < render [j].materials.Length; k++) 
						{
							render [j].materials [k].color = Color.magenta;
						}
					}
				}
			}
			*/
			//千万注意的就是游戏人物的playerIndex是一个非常重要的选项
			//0代表由游戏控制手柄组1进行控制
			//1代表由游戏控制手柄2或者游戏AI进行控制
			theSystem.makeStartForSystem ();
			for (int i = 0; i < systemValues.thePlayerNamesForPlay.Length; i++) 
			{
				thePlayers [i].GetComponent<move> ().lookAtEmyImmediate();
			}
			this.GetComponent <possController>(). showPossStart();

		}
 
	}

	//两段式初始化
	//其余的全部数值的初始化会在这里完成
		public void  makeAllStart()
	{

		for (int i = 0; i < systemValues . thePlayerNamesForPlay.Length; i++) 
		{
			weapons = thePlayers [i].GetComponentsInChildren<playerWeapon> ();
			for (int k = 0; k < weapons.Length; k++) 
			{
				weapons [k].makeStart ();//特殊武器
			}
		
			thePlayers [i].playerIndex = i;//设定这个游戏人物的操控轴
			thePlayers [i].GetComponent<move> ().makeStart ();

			if (systemValues.theGameType  == GameType.PVC && i!=0 ) //如果有AI角色依旧是人机对战，需要加上一些额外的东西
			{
				//thePlayers [i].gameObject.AddComponent<theAI> ();//这个地方有一点写死了，可以考虑使用字符串动态赋值
				thePlayers [i]. gameObject.AddComponent (System.Type.GetType ( systemValues . getAIScript() ));
			}

			if (systemValues.theGameType  == GameType.CVC ) //如果有AI角色依旧是人机对战，需要加上一些额外的东西
			{
				//thePlayers [i].gameObject.AddComponent<theAI> ();//这个地方有一点写死了，可以考虑使用字符串动态赋值
				thePlayers [i]. gameObject.AddComponent (System.Type.GetType ( systemValues . getAIScript() ));
				//Camera.main.GetComponent <cameraMove> ().theMode = cameraMoveMode.rotate;
			}
		}

		//打架之前的动作显示需要在这三个动作之前进行操作
		theCameraMove.makeStart ();
		roundController.makeStart ();
		thePictueController.makeStart ();
		roundController.makeRoundFlash();//第一次也算是
		StopAllCoroutines();
	}



	void Start () 
	{
		makeStart ();
	}
	
	// Update is called once per frame
	//void Update ()
	//{
		//if (Input.GetKeyDown (KeyCode.Space))
			//makeStart ();
	//}
}
