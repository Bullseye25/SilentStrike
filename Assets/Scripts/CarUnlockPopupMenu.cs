using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CarUnlockPopupMenu : MonoBehaviour {
	//public Text totalCoins;
	public Text requiredCoins;
	PlayerDataSerializeable playerData;
	public bool isUpgrade;
	void OnEnable(){
		playerData = PlayerDataController.Instance.playerData;
		if (isUpgrade) {
			requiredCoins.text = playerData.gunsList [playerData.SelectedVehicle_temp].UpgradePrice.ToString ();				
		} else {
			requiredCoins.text = playerData.gunsList[playerData.SelectedVehicle_temp].UnlockPrice.ToString();
		}
		//totalCoins.text = playerData.PlayerGold.ToString ();
	}
}
