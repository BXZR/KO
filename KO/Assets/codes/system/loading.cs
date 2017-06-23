using UnityEngine;
using System.Collections;
using UnityEngine .UI;
using UnityEngine.SceneManagement;


public class loading : MonoBehaviour {

	public float timeDely = 1f;//延迟时间
	public Slider theProcessBar;//进度条slider
	public string nextSceneName = "KO";//下一个场景的名字（可以在属性界面再一次编辑）
	bool opener = false;//因为是协程还不能用Invoke大法，所以就老老实实用计时器

	//float timers = 0;//测试用计时器 

	IEnumerator startLoading()//异步加载新的场景
	{
		//SceneManager貌似是新版的场景管理
		//异步加载场景
		AsyncOperation acop = SceneManager.LoadSceneAsync(nextSceneName);
		//因为太快了所以需要加一点缓冲
		acop.allowSceneActivation = false;
		theProcessBar.value = 0;
		float value = 0f;

		//等待40帧大约0.8秒
		for (int i = 0; i < 40; i++)
		{
			value++;
			//theProcessBar.value =  value /120;
			theProcessBar.value = value/40;
			yield return new WaitForEndOfFrame ();
		}
	
		theProcessBar.value = value+0.8f*acop.progress;

		acop.allowSceneActivation = true;
		yield return acop;

	}

	void Start () 
	{
		
	}
	void OnDestroy()
	{
		StopAllCoroutines ();//取消协程
		CancelInvoke ();//取消调用

		//print ("总时长: "+ timers);//测试用计时器 
	}

	void Update ()
	{
		if (!opener) 
		{
			timeDely -= Time.deltaTime;
			if (timeDely < 0) 
			{
				opener= true;
				StartCoroutine (startLoading());
			}
		}

		//timers += Time.deltaTime;//测试用计时器 

	}


}
