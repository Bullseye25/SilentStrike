using UnityEngine;
using System.Collections;

public class VehicleSwitcher : MonoBehaviour {
	public GameObject currentCarController;

	void OnTriggerEnter ( Collider other ){

		if(other.gameObject.CompareTag("PlayerCollider")){
//			RCC_Demo.instance.switchVehicle (currentCarController);
//			currentCarController.transform.parent = null;
//			HudMenuManager.instance.BlackCurtain.SetActive (false);
//			HudMenuManager.instance.BlackCurtain.SetActive (true);
//			currentCarController.SetActive (true);
//			other.gameObject.gameObject.transform.root.gameObject.SetActive (false);
//			gameObject.SetActive (false);
		}

	}
}
