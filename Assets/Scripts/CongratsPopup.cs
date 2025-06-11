using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CongratsPopup : MonoBehaviour {
	public int Reward= 50;
	//public Text totalCoins;
	public Text requiredCoins;
	PlayerDataSerializeable playerData;

	void OnEnable(){
		playerData = PlayerDataController.Instance.playerData;	
		requiredCoins.text =Reward.ToString();
		
		var localPos = transform.localPosition;

		if (MainMenuManager.Instance.currentMenuName == MenuNames.MAIN_MENU)
			localPos.z = 0;
		else
			localPos.z = -1500f;
        
		transform.localPosition = localPos;
	}

	public void giveReward(){
		playerData.PlayerGold += Reward;
		PlayerDataController.Instance.Save ();
	}
}
