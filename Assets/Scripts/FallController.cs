using UnityEngine;
using System.Collections;

public class FallController : MonoBehaviour {

	void OnTriggerEnter ( Collider other ){

		if(other.gameObject.CompareTag("PlayerCollider")){
			// MConstants.isPlayerWin = false;
			// HudMenuManager.instance.GameOverByNoRevive ();
		}
	}
}
