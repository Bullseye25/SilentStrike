using UnityEngine;
using System.Collections;

public class OnBack : MonoBehaviour {
	public GameObject back;
	void OnEnable(){
		back.SetActive (false);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}
}
