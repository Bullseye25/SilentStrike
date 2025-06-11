using UnityEngine;
using System.Collections;

public class ReviveMenuManager : MonoBehaviour {

	void OnEnable () {
		Time.timeScale = 0.001f;
		//AudioListener.volume =0;
	}

	public void NoRevive(){
		gameObject.SetActive (false);
		Time.timeScale = 1;
		HudMenuManager.instance.GameOverByNoRevive ();

	}

	public void YesRevive(){
		gameObject.SetActive (false);
		Time.timeScale = 1;

		/*AdsManager.isRevive = true;
		AdsManager.instance.ShowRewardedAdd ();*/
	}
}
