using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startBuilder : MonoBehaviour {
	//有关感悟的小模块整体控制器
	public Scrollbar theShowSlider;//用于控制进度显示的slider
	public TextAsset theText;//用于显示的所有文字都存放在这个文本文件里面，便于修改
	public Text theShowText;//用于显示的text
	private bool isStart = false;//开启之后自动滚动，只有内部能够控制
	public float theSpeed=0.01f;//文字滚动速度
	private  float valueNow = 1f;//记录的中间值

	//用系统回调自动控制
	void OnEnable()
	{
		startShow ();
	}

	void OnDisable()
	{
		endShow ();
	}


	private void startShow()
	{
		theShowText.text = theText.text;
		valueNow = 1f;
		theShowSlider.value = 1f;
		isStart = true;	
	}
	private void endShow()
	{
		isStart = false;	
	}

		
	void Update ()
	{
		if (valueNow > 0 && isStart)
		{
			valueNow -= theSpeed * Time.deltaTime;
			theShowSlider.value = valueNow;
		}
	}
}
