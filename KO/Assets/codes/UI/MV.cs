using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MV : MonoBehaviour {

	public MovieTexture theMovie;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void makePlay()
	{
		this.GetComponent<Renderer>().material.mainTexture = theMovie;
		theMovie.Play();//跳转场景就进行播放
	}
}
