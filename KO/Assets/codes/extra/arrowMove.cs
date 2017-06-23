using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class arrowMove : MonoBehaviour {

	private  Transform Arrow;
	public float speed = 13f;//速度
	public Vector3 forward = new Vector3 (0,0,1);//默认向前
	void Start () {
		Arrow = this.GetComponent <Transform> ();
	//	if (SceneManager.GetSceneByName != "KO")
	//		Destroy (this.gameObject);//非战斗场景不需要显示弹矢
	}
	
	// Update is called once per frame
	void Update () {
		Arrow.transform.Translate (  forward *speed*Time .deltaTime);
	}
}
