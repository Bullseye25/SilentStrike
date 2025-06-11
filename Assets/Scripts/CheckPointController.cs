using UnityEngine;
using System.Collections;

public class CheckPointController : MonoBehaviour {
	public static CheckPointController instance;
	[HideInInspector]
	public GameObject targetCheckPoint;

	public AudioSource pickAudioSource;
	[HideInInspector]
	public GameObject resetPoint;

	// Use this for initialization
	void Awake () {
		instance = this;
	}

	public void PlayPickSound(){
		pickAudioSource.Play ();
	}

	public void setResetPoint(GameObject go){
		resetPoint = go;
	}

}
