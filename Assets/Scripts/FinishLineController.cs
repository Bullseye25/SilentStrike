using UnityEngine;
using System.Collections;

public class FinishLineController : MonoBehaviour {

	public static FinishLineController instance;
	public bool isValidFinishLineCrossing;
	public int currentRountLaps=0;
	public GameObject cylinder;
	float enterTime ;

	void Awake(){
		instance = this;
	}
	public void IncreamentRoundLaps(){

	}



		void OnTriggerEnter(Collider other){

		}

	void gameOver(){
		// HudMenuManager.instance.GameOver ();

	}
	void OnTriggerStay(Collider other){
		if(other.gameObject.CompareTag("PlayerCollider") && Time.time > enterTime+3 &&  isValidFinishLineCrossing){
			if(cylinder != null){
				cylinder.SetActive (false);
			}
		
		}

	

	}

	}
