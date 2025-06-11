﻿using UnityEngine;
using System.Collections;

public class RateUIMenuManager : MonoBehaviour {
	public GameObject UI1;
	public GameObject UI2;

	void OnEnable(){
		UI1.SetActive (true);
		UI2.SetActive (false);
	}

	public void enjoyingGame(){
		UI1.SetActive (false);
		UI2.SetActive (true);

	}

	public void closeRateUs(){
		gameObject.SetActive (false);
	}

	public void RateUSURL(){
		gameObject.SetActive (false);
		Application.OpenURL(MConstants.RATE_US);
		PlayerDataController.Instance.playerData.isRateUSDone = true;

	}

	public void NeverShoRateUS(){
		gameObject.SetActive (false);
		PlayerDataController.Instance.playerData.isRateUSDone = true;
	}
}
