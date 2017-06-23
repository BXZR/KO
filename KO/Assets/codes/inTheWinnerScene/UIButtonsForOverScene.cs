using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonsForOverScene : MonoBehaviour {

	public GameObject theShowPanel;
	private bool opened = false;

	public void setShow()
	{
		opened = !opened;
		theShowPanel.SetActive (opened);
	}

 

}
