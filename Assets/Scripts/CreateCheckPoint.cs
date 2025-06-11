using UnityEngine;
using System.Collections;

public class CreateCheckPoint : MonoBehaviour {
	public GameObject parent;
	public GameObject clone;
	public Transform transformPos;

	public void createObject(){
		GameObject go = GameObject.Instantiate (clone)  as GameObject;
		go.transform.position = transformPos.position;
		go.transform.parent = parent.transform;
	}
}
