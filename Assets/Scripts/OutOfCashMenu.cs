using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class OutOfCashMenu : MonoBehaviour {

	public Text totalCoins;
	public Text requiredCoins;
	PlayerDataSerializeable playerData;
	public static bool isCarBuy;

    public GameObject LevelSelection;
	void OnEnable(){
		playerData = PlayerDataController.Instance.playerData;
		if (isCarBuy) {
			requiredCoins.text = (playerData.gunsList[playerData.SelectedVehicle_temp].UnlockPrice-playerData.PlayerGold).ToString();

		} else {
			requiredCoins.text = (playerData.envioronmentList[playerData.CurrentEnvironment].UnlockPrice-playerData.PlayerGold).ToString();
		}
		totalCoins.text = playerData.PlayerGold.ToString ();
		
		var localPos = transform.localPosition;

		if (MainMenuManager.Instance.currentMenuName == MenuNames.MAIN_MENU)
			localPos.z = 0;
		else
			localPos.z = -1500f;
        
		transform.localPosition = localPos;
	}

    private void OnDisable()
    {
        if (!LevelSelection && LevelSelection.activeSelf)
        {
            MainMenuManager.Instance.showSubMenu(SubMenuNames.SECODORY_GUN_SELECTION);
            Debug.Log("Called");
        }
    }
}
