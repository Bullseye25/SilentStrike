using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnviornmentSelectionMenu : MonoBehaviour {
	public MainMenuManager menuManger;

	public void SelectEnviornMent(int id){
		
		PlayerDataController.Instance.playerData.CurrentEnvironment = id;
		PlayerDataSerializeable pDta = PlayerDataController.Instance.playerData;

		if (pDta.envioronmentList [id-1].ID == id && pDta.envioronmentList [id-1].isLocked) {
			if (PlayerDataController.Instance.playerData.PlayerGold >= PlayerDataController.Instance.playerData.envioronmentList [id - 1].UnlockPrice) {
				menuManger.showSubMenu (SubMenuNames.ENV_UNLOCK_POPUP);
			} else {
				OutOfCashMenu.isCarBuy = false;
				menuManger.showSubMenu (SubMenuNames.OUT_OF_CASH);
			}

			return;
		}

	
		switch(id){
		case 1:
			//MConstants.CurrentLevelNumber = 1;
			Application.LoadLevel(MConstants.Env_1);

			break;

		case 2:
			//MConstants.CurrentLevelNumber = 2;

			Application.LoadLevel(MConstants.Env_2);

			break;
		case 3:
			//MConstants.CurrentLevelNumber = 3;

			Application.LoadLevel(MConstants.Env_3);

			break;
		}

		menuManger.showSubMenu (SubMenuNames.LOADING);

		//UnityAnalyticsScript.instance.AddUnityEvent ("SelectEnviroment", new Dictionary<string, object>{
		//	{ "Enviroment", ""+id}
		//});

		//MainMenuManager.Instance.showMenu (MenuNames.ENVIORNMENT_SELECTION);
	}

	public void UnlockEnviornment(){
	
		PlayerDataController.Instance.playerData.PlayerGold -= PlayerDataController.Instance.playerData.envioronmentList [PlayerDataController.Instance.playerData.CurrentEnvironment- 1].UnlockPrice;
		PlayerDataController.Instance.playerData.envioronmentList [PlayerDataController.Instance.playerData.CurrentEnvironment - 1].isLocked = false;
		PlayerDataController.Instance.Save ();

		menuManger.RefreshData ();
		SelectEnviornMent(PlayerDataController.Instance.playerData.CurrentEnvironment);

		//UnityAnalyticsScript.instance.AddUnityEvent ("UnlockEnviroment", new Dictionary<string, object>{
		//	{ "Enviroment", ""+PlayerDataController.Instance.playerData.CurrentEnvironment}
		//});
	}
}
