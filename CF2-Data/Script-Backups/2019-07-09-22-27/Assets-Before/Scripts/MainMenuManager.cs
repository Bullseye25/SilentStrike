using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour {
	
	public Text PlayerGoldText;
	public Text PlayerCashText;
	public Text PlayerRankText;

	public MenuNames currentMenuName;
	public MenuNames previousMenuName;

	public SubMenuNames currentSubMenu;

	public List<GameObject> menusList;
	public List<GameObject> subMenusList;
	public List<MenuNames> menusStack;
	public static MainMenuManager Instance; 
	public static bool isRankUp = false;
	public static bool isGoToGrage = false;
	public GameObject promoIcon;
	public List<Sprite> promoSpriteList;
	public Image promoImage;
	public bool isSubMenuVisible;
	public GameObject backButton;

	void Awake(){
		Instance = this;
	}
	// Use this for initialization
	void OnEnable() {
		Time.timeScale = 1;
		RefreshData ();
		menusStack = new List<MenuNames> ();
		showMenu (MenuNames.MAIN_MENU);
		ToggleSound (PlayerDataController.Instance.playerData.isSoundOn);
		ChangeController (PlayerDataController.Instance.playerData.SelectedControl);
		if(isGoToGrage){
			isGoToGrage = false;
			showMenu (MenuNames.VEHICLE_MENU);
		}

		if(isRankUp){
			isRankUp = false;
			showSubMenu (SubMenuNames.LEVEL_UP);
		}

		//refreshPromoData ();
	}

	public void RefreshData(){
		PlayerGoldText.text = PlayerDataController.Instance.playerData.PlayerGold.ToString();
		PlayerCashText.text = PlayerDataController.Instance.playerData.PlayerCash.ToString();
		PlayerRankText.text = PlayerDataController.Instance.playerData.Rank.ToString();
		if(PlayerDataController.Instance.playerData.isPromoClicked){
			promoIcon.SetActive (false);
		}

	}

	void refreshPromoData(){

	

	}



	public void ToggleSound(bool isOn){
		if(isOn){
			AudioListener.volume = 1;
		}else{
			AudioListener.volume =0;

		}
	}

	public void ChangeController(int index){

//		switch(index){
//
//		case 0://Buttons
//			RCC_Settings.Instance.useAccelerometerForSteering = false;
//			RCC_Settings.Instance.useSteeringWheelForSteering = false;
//		
//
//			break;
//		case 1://Tilt
//			RCC_Settings.Instance.useAccelerometerForSteering = true;
//			RCC_Settings.Instance.useSteeringWheelForSteering = false;
//		
//			break;
//		case 2://Steering
//
//			RCC_Settings.Instance.useAccelerometerForSteering = false;
//			RCC_Settings.Instance.useSteeringWheelForSteering = true;
//			break;
//
//		}

	}

	public void showMenu(MenuNames menuName){
		previousMenuName = currentMenuName;
		menusList [currentMenuName.GetHashCode ()].SetActive (false);
		menusList [menuName.GetHashCode ()].SetActive (true);
		menusStack.Add (menuName);

		currentMenuName = menuName;
		if (currentMenuName == MenuNames.MAIN_MENU) {
			backButton.SetActive (false);
		} else {
			backButton.SetActive (true);
		}
	}


	public void showSubMenu(SubMenuNames menuName){

		for (int i = 0; i < subMenusList.Count; i++) {
			subMenusList [i].SetActive (false);
		}
		isSubMenuVisible = true;
		currentSubMenu = menuName;
		subMenusList [menuName.GetHashCode ()].SetActive (true);

	}

	public void handleBackMenu (){
		if(isSubMenuVisible){
			CloseSubMenu ();
			return;
		}
		if (menusStack.Count >= 2) {
			MenuNames toshow = menusStack [menusStack.Count - 2]; 
			MenuNames toRemove = menusStack [menusStack.Count - 1]; 
			menusStack.Remove (toRemove);
			currentMenuName = toshow;
			previousMenuName = currentMenuName;

			menusList [toRemove.GetHashCode ()].SetActive (false);
			menusList [toshow.GetHashCode ()].SetActive (true);
			if (currentMenuName == MenuNames.MAIN_MENU) {
				backButton.SetActive (false);
			} else {
				backButton.SetActive (true);
			}
		} else {
			Application.Quit();
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			handleBackMenu ();
		}
	}


	public void CloseSubMenu(){
		isSubMenuVisible = false;

		subMenusList [currentSubMenu.GetHashCode ()].SetActive (false);

	}

	public void StartMenuDrive(){
		showMenu (MenuNames.VEHICLE_MENU);
		//StartMenu.SetActive (false);
		//GarageMenu.SetActive (true);
		//GarageCars[currentCar-1].SetActive (true);
		//GarageCars[currentCar-1].GetComponent<Animator>().enabled = true;

		UnityAnalyticsScript.instance.AddUnityEvent ("MainMenuPlay", new Dictionary<string, object>{
		});
	}

	public void openGameModesMenu(){
		 PlayerDataSerializeable playerData;

		playerData = PlayerDataController.Instance.playerData;

		if (playerData.CarsList [playerData.CurrentSelectedVehicle - 1].isLocked) {
			openCarUnlockPopup ();
		} else {
			showMenu (MenuNames.MODE_SELCT_MENU);
			UnityAnalyticsScript.instance.AddUnityEvent ("VehicleMenuPlay", new Dictionary<string, object>{
			});
		}

	}
	public void BackToStartMenu(){
		showMenu (MenuNames.MAIN_MENU);

		//GarageCars[currentCar-1].transform.position = new Vector3(-24.36f, 1.51f, 13.45f);
		//GarageCars[currentCar-1].SetActive(false);
	}

	public void openSettings(){
		showSubMenu (SubMenuNames.SETTING_MENU);
	}



	public void openCarUnlockPopup(){
		if (PlayerDataController.Instance.playerData.PlayerGold >= PlayerDataController.Instance.playerData.CarsList [PlayerDataController.Instance.playerData.CurrentSelectedVehicle - 1].UnlockPrice) {
			showSubMenu (SubMenuNames.CAR_UNLOCK_POPUP);
		} else {
			OutOfCashMenu.isCarBuy = true;
			showSubMenu (SubMenuNames.OUT_OF_CASH);
		}
	}


	public void openCarUpGradePopup(){
		if (PlayerDataController.Instance.playerData.PlayerCash >= PlayerDataController.Instance.playerData.CarsList [PlayerDataController.Instance.playerData.CurrentSelectedVehicle - 1].UpgradePrice) {
			showSubMenu (SubMenuNames.CAR_UPGRADE_POPUP);
		} else {
			showSubMenu (SubMenuNames.OUT_OF_CASH);
		}
	}

	public void RateUs(){
		Application.OpenURL(MConstants.RATE_US);

		UnityAnalyticsScript.instance.AddUnityEvent ("Rate Us", new Dictionary<string, object>{
		});
	}

	public void MoreGames(){
		Application.OpenURL("https://play.google.com/store/apps/dev?id=4653793284141226364");

		UnityAnalyticsScript.instance.AddUnityEvent ("More Games", new Dictionary<string, object>{
		});
	}

	public void FB(){
		Application.OpenURL("https://web.facebook.com/top3Dgamessimstudio/");

		UnityAnalyticsScript.instance.AddUnityEvent ("FB", new Dictionary<string, object>{
		});
	}

	public void ShowLeaderBoard(){
	//	SocialManager.instance.OnShowLeaderBoard ();

		UnityAnalyticsScript.instance.AddUnityEvent ("Leaderboard", new Dictionary<string, object>{
		});
	}

	public void AdGold(){
		showSubMenu (SubMenuNames.FREE_GOLD_POPUP);

		UnityAnalyticsScript.instance.AddUnityEvent ("FreeGold", new Dictionary<string, object>{
			{ "position", "MainMenu"}
		});

	}

	public void PromoteGame(){
//		for (int i = 0; i <PromoDataController.Instance.playerData.PromoListList.Count; i++) {
//			if(PromoDataController.Instance.playerData.PromoListList[i].Id == promo.Id){
//				PromoDataController.Instance.playerData.PromoListList [i].isPromoClicked = true;
//			}
//		}
//		PromoDataController.Instance.Save ();
//		Application.OpenURL(promo.gameLink);
//
//		refreshPromoData ();
//
//		UnityAnalyticsScript.instance.AddUnityEvent ("PromoClicked", new Dictionary<string, object>{
//		});
	}

}
