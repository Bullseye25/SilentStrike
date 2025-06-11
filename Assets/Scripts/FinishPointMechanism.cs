using UnityEngine;
using System.Collections;

public class FinishPointMechanism : MonoBehaviour {

	public bool isfinishLineCorectChecker;
	public GameObject hurdle;


	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("PlayerCollider")){
//			if(isfinishLineCorectChecker){
//				FinishLineController.instance.isValidFinishLineCrossing = true;
//				hurdle.SetActive (false);
//			}else if(FinishLineController.instance.isValidFinishLineCrossing){
//				FinishLineController.instance.isValidFinishLineCrossing = false;
//				FinishLineController.instance.IncreamentRoundLaps ();
//				LevelsManager.instance.PlayerLaps++;
//				if(LevelsManager.instance.PlayerLaps>LevelsManager.instance.currentLevelsNumbersOfLaps){
//					LevelsManager.instance.PlayerPosition = LevelsManager.instance.totalVehicleReachedBeforePlayer + 1;
//					LevelsManager.instance.SetEndTime();
//					HudMenuManager.instance.GameOver ();
//					//Race Over Logic
//				}
//			}
		}

		if(other.gameObject.CompareTag("AICarCollider") && !isfinishLineCorectChecker){
			//Debug.Log ("AICarCollider");

//			SAICSmartAICar carAi = other.gameObject.GetComponentInParent<SAICSmartAICar> ();
//			if(carAi != null){
//				carAi.currentLap++;
//				if(carAi.currentLap>LevelsManager.instance.currentLevelsNumbersOfLaps){
//					LevelsManager.instance.totalVehicleReachedBeforePlayer++;
//					carAi.inBrakeZone = true;
//					carAi.endTime = Time.time;
//				}
//			}
		}
	}

}
