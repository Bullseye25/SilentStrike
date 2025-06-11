using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckPoint : MonoBehaviour {


	public GameObject nextPoint;
	public bool isFinishPoint = false;
	public bool isStartPoint = false;
	public GameObject target;
	public bool isResetPoint;
	public MeshRenderer chilfMesh;

	void Start(){
		if(nextPoint && !isFinishPoint ){
			if(nextPoint.GetComponent<CapsuleCollider> ()){
				nextPoint.GetComponent<CapsuleCollider> ().enabled = false;
			}else if(nextPoint.GetComponent<BoxCollider> ()){
				nextPoint.GetComponent<BoxCollider> ().enabled = false;
			}
		}
		if(isStartPoint){
			CheckPointController.instance.targetCheckPoint = target;
			CheckPointController.instance.setResetPoint (target);
			gameObject.GetComponent<Renderer> ().material = LevelsManager.instance.greenCheckPointMaterial;
			if(chilfMesh){
				chilfMesh.material = LevelsManager.instance.greenCheckPointMaterial;

			}
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("PlayerCollider")){
			if (isFinishPoint) {
				nextPoint.SetActive (true);
				HudMenuManager.instance.GameComplete ();
//				if(GameObject.FindObjectOfType<RCC_Camera>()){
//					GameObject.FindObjectOfType<RCC_Camera>().cam.enabled = false;
//				}
			} else {
				if(nextPoint){
					if(nextPoint.GetComponent<CapsuleCollider> ()){
						nextPoint.GetComponent<CapsuleCollider> ().enabled = true;
					}else if(nextPoint.GetComponent<BoxCollider> ()){
						nextPoint.GetComponent<BoxCollider> ().enabled = true;
					}
					CheckPointController.instance.targetCheckPoint = nextPoint.GetComponent<CheckPoint> ().target;
					if(nextPoint.GetComponent<Renderer> ()){
						nextPoint.GetComponent<Renderer> ().material = LevelsManager.instance.greenCheckPointMaterial;
						if(nextPoint.GetComponent<CheckPoint> ().chilfMesh){
							nextPoint.GetComponent<CheckPoint> ().chilfMesh.material = LevelsManager.instance.greenCheckPointMaterial;
						}
					}

				}
				CheckPointController.instance.PlayPickSound ();
			}
			if(isResetPoint){
				CheckPointController.instance.setResetPoint (target);
			}
			gameObject.SetActive (false);
		}
	}




}
