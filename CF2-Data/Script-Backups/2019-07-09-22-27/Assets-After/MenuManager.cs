// Code auto-converted by Control Freak 2 on Tuesday, July 09, 2019!
using UnityEngine;
using System.Collections;
public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 
	}

	public void playCarScene(){
		Application.LoadLevel("Race_Track_01");

	}

	public void playBikeScene(){
		Application.LoadLevel("Race_Track_02-2");

	}
}
