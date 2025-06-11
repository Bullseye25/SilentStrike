// Code auto-converted by Control Freak 2 on Tuesday, July 09, 2019!
using UnityEngine;
using System.Collections;

public class OnBack : MonoBehaviour {
	public GameObject back;
	void OnEnable(){
		back.SetActive (false);
	}
	// Update is called once per frame
	void Update () {
		if (ControlFreak2.CF2Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}
}
