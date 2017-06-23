using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class keyUseToSelect : MonoBehaviour {
	//在选择人物的界面使用按键进行操作的控制器
	//特征在于使用systemValues类所定义的值来进行操作
	//所以实际上也是使用键位按下判断建位值的方法执行的
	//当然我也在考虑在这个简单的界面是否需要这样的功能呢？

	//事实上这个脚本就是挂在选择人物控制器上面的
	private selectSceenController theController;//选择人物控制器
	public startInformationSceButton basicButton;//显示基本信息的按钮
	public startInformationSceButton attackLinkButton;//显示连招信息的按钮
	public startInformationSceButton skillButton;//显示技能信息的按钮
	public startInformationSceButton theMakeSureButton;//用于确认选择

	//事实上这些按钮可以用更加封装的方式进行处理，但是考虑可扩展性或许这种方法更好
	//因为显示信息的按钮做掌控的Panel都并不相同，所以我在这里使用的方法就只是模拟按耨按下而不是更加高深的方式（此处或许可以优化）
	//（其实如果实际良好可以考虑设为私有或者直接取消这个引用）
	private int index = 0;//显示的模式，因为有可能显示多种信息
	private int indexMax= 3;//显示的模式的种类数量，如3表示3种类显示信息


	void Start () 
	{
		this.theController = this.GetComponent <selectSceenController> ();
	}

	void makeIndexAdd()
	{
		index++;
		if (index >= indexMax)
			index = 0;
	}

	void makeIndexMinus()
	{
		index--;
		if (index < 0)
			index = indexMax -1;
	}

	void makeInformation()
	{
		switch (index) {
		case 0:
			basicButton.basicInformation ();
			break;
		case 1:
			attackLinkButton.getAttackLinInformation ();
			break;
		case 2:
			skillButton.skillInformation ();
			break;
		}
	}

	//简单地使用Input类进行的案件检测，说实话灵活性一般，我也在考虑使用systemValues类的方法用配置文件处理
	//在这里就当是留下两套方案吧
	private void keyCheck()
	{
		if (Input.GetKeyDown (KeyCode.LeftArrow))
			theController.getBackFighter ();
		else if (Input.GetKeyDown (KeyCode.RightArrow))
			theController.getForwardFighter ();
		else if (Input.GetKeyDown (KeyCode.Return))
			theMakeSureButton.makeSelect ();
		//额外操作
		else if (Input.GetKeyDown (KeyCode.UpArrow)) 
		{
			makeIndexAdd ();
			makeInformation ();
		} 
		else if (Input.GetKeyDown (KeyCode.DownArrow))
		{
			makeIndexMinus ();
			makeInformation ();
		} 
		else if (Input.GetKeyDown (KeyCode.R))
		{
			theController.makeRandomFighter ();
		}
	
	}
	void Update ()
	{
		keyCheck ();
	}
}
