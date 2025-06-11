using UnityEngine;
using System.Collections;

public class IndicatorController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (PlayerDataController.Instance != null && PlayerDataController.Instance.playerData.CurrentMode == 1) {
			gameObject.SetActive (false);
		}
	}

}
