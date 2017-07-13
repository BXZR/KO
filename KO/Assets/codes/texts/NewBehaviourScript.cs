using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			//this.GetComponentInChildren <Animator> ().StopPlayback ();
			this.GetComponentInChildren <Animator> ().CrossFadeInFixedTime("punch1",0f);
		}
		
	}
}
