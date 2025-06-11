using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SenstivityController : MonoBehaviour {
	
	public float Value =1f;
	public Slider slr;

	void OnEnable () {
		if(PlayerDataController.Instance != null){
			Value = PlayerDataController.Instance.playerData.SensivityValue;
			if(Value < 0.3f){
				Value = 0.3f;
			}
		}

		slr.value = Value;
		HudMenuManager.sensitivity = Value;
	}

	public void OnValueChanged (float Value) {
		if(PlayerDataController.Instance != null){
			PlayerDataController.Instance.playerData.SensivityValue =Value ;
            PlayerDataController.Instance.Save();
		}
		HudMenuManager.sensitivity = Value;
	}
   
}
