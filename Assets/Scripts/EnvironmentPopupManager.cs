using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EnvironmentPopupManager : MonoBehaviour {
	//public Text totalCoins;
	public Text requiredCoins;
	PlayerDataSerializeable playerData;

	void OnEnable(){
		playerData = PlayerDataController.Instance.playerData;	
		requiredCoins.text = playerData.envioronmentList[playerData.CurrentEnvironment-1].UnlockPrice.ToString();

	}
}