using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using CompleteProject;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
	
	public Text PlayerGoldText;
    public Text PlayerGoldTextInGuns;
    public Text PlayerCashText;
	public Text PlayerRankText;
	public GameObject adsManagerObject;
	/*public GameObject dataManager;*/

	public GunSelectionMenu gunSelectionScript;
	public LevelSelectionMenuManager levelSelectionScript;

	public MenuNames currentMenuName;
	public MenuNames previousMenuName;

	public SubMenuNames currentSubMenu;

	public List<GameObject> menusList;
	public List<GameObject> subMenusList;
	public List<MenuNames> menusStack;
	public static MainMenuManager Instance; 
	public static bool isRankUp = false;
	public static bool isGoToGrage = false, isModeScreen = false, isGoToWeaponScreen;
	public GameObject promoIcon;
	public List<Sprite> promoSpriteList;
	public Image promoImage;
	public bool isSubMenuVisible;
	public GameObject backButton;
    public CursorLockMode wantedMode;
    public static bool FromKidsMode = false, FromProMode = false, FromSquidMode, FromBattleFieldMode;
    public GameObject TestAdsText;
    public GameObject expertLockOverlay, expertModeInfo;
    public GameObject zombieLockOverlay, zombieModeInfo;
    public GameObject squidLockOverlay, squidModeInfo;
    public GameObject battlefieldLockOverlay, battlefieldModeInfo;
    public GameObject removeAdsButton;
	public GameObject leaderBoard;
	//public GameObject SquidMode;
    
    [Header("Tutorial"), Space(5)]
    public GameObject mainMenuTutorialBg;
    public GameObject mainMenuClickImage;
	public AudioSource CoinSound;


	public float coinsAnimationTime = 1;
    private float _initialNumber;
    private float _currentNumber;
    private float _desiredNumber;
    private bool _startAddingCoins;
    
    void Awake(){
        if (Instance == null)
			Instance = this;


		removeAdsButton.SetActive(!PlayerDataController.Instance.playerData.isRemoveAds);
		//promoAd.SetActive(!PlayerDataController.Instance.playerData.isRemoveAds);

		if (PlayerDataController.Instance.playerData.isRemoveAds && PlayerDataController.Instance.playerData.unlockedAllGuns && PlayerDataController.Instance.playerData.unlockedAllLevels)
		{
			PlayerDataController.Instance.playerData.unlockEverything = true;
			PlayerDataController.Instance.Save();
		}
		if (!PlayerDataController.Instance.playerData.unlockEverything)
		{
			PlayerDataController.Instance.playerData.megaSaleCount++;
			PlayerDataController.Instance.Save();
			
			if (PlayerDataController.Instance.playerData.megaSaleCount % 4 == 0)
			{
				//showSubMenu(SubMenuNames.MEGA_OFFER_MENU);
			}
		}
		
	}
	
	public void ShowSurvivalLockInfo()
    {
	    expertModeInfo.SetActive(true);
    }
    
    public void ShowZombieLockInfo()
    {
	    zombieModeInfo.SetActive(true);
    }
    
    public void ShowBattlefieldLockInfo()
    {
	    battlefieldModeInfo.SetActive(true);
    }
    
    public void ShowSquidLockInfo()
    {
	    squidModeInfo.SetActive(true);
    }
    
    public void ExtraCheckForGunsSecondreymenu()
    {
        if (menusList[2].activeSelf)
        {
            showSubMenu(SubMenuNames.SECODORY_GUN_SELECTION);
        }
    }
   
        public void YoutubeOpen()
    {
        Application.OpenURL("https://www.youtube.com/channel/UCGdM7_wDDteLs1F_02Zc8tQ?sub_confirmation=1");

        //UnityAnalyticsScript.instance.AddUnityEvent("Youtube", new Dictionary<string, object>
        //{
        //});
    }
    public void ApplicationQuit()
    {
        Application.Quit();
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
            if (FromKidsMode)
            {
                MConstants.CurrentGameMode = MConstants.GAME_MODES.MODE3_Zombie;
            }
            else if (FromProMode)
            {
                MConstants.CurrentGameMode = MConstants.GAME_MODES.MODE2_Expert;
            }
            else if (FromSquidMode)
            {
	            MConstants.CurrentGameMode = MConstants.GAME_MODES.MODE4_Squid;
            }
            else if (FromBattleFieldMode)
            {
	            MConstants.CurrentGameMode = MConstants.GAME_MODES.MODE5_BATTLEFIELD;
            }
            
            FromKidsMode = false;
            FromProMode = false;
            FromSquidMode = false;
            FromBattleFieldMode = false;
            showMenu(MenuNames.LEVEL_SELECTION);
        }
		else if (isGoToWeaponScreen)
		{
			isGoToWeaponScreen = false;
			showMenu(MenuNames.GUN_MENU);
		}
		
		if (PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode1 > 5)
		{
			//expertLockOverlay.SetActive(false);
			battlefieldLockOverlay.SetActive(false);
		}
		
		if (PlayerDataController.Instance.playerData.LastUnlockedLevel_BattleMode > 5)
		{
			zombieLockOverlay.SetActive(false);
		}
		
		if (PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode3 > 5)
		{
			expertLockOverlay.SetActive(false);
		}
		if (PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode2 > 5)
		{
			squidLockOverlay.SetActive(false);
		}
		if (isModeScreen)
        {
            isModeScreen = false;
            showMenu(MenuNames.MODE_SELCT_MENU);
        }

		if(isRankUp){
			isRankUp = false;
			showSubMenu (SubMenuNames.LEVEL_UP);
		}

		if (!PlayerDataController.Instance.playerData.firstTimeTutorial)
		{
			mainMenuTutorialBg.SetActive(true);
			mainMenuClickImage.SetActive(true);
		}
		//refreshPromoData ();
	}

	public void RefreshData(){
		PlayerGoldText.text = PlayerDataController.Instance.playerData.PlayerGold.ToString();
        PlayerGoldTextInGuns.text = PlayerDataController.Instance.playerData.PlayerGold.ToString();

        PlayerCashText.text = PlayerDataController.Instance.playerData.PlayerCash.ToString();
		PlayerRankText.text = PlayerDataController.Instance.playerData.Rank.ToString();
		if(PlayerDataController.Instance.playerData.isPromoClicked){
			promoIcon.SetActive (false);
		}

	}

	public void BuyUnlockEverything()
	{
		Purchaser.instance.BuyNonConsumableUnlockEverything();
	}


	public void ToggleSound(bool isOn){
		if(isOn)
		{
			AudioListener.volume = 1;
		}
		else
		{
			AudioListener.volume =0;

		}
		
		if (PlayerDataController.Instance.playerData.isHighQuality)
		{
			QualitySettings.SetQualityLevel(5, true);
		}
		else
		{
			QualitySettings.SetQualityLevel(1, true);
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
    public void ShowGunsPopUp()
    {
        showSubMenu(SubMenuNames.SECODORY_GUN_SELECTION);
    }
    public void ShowMaps()
    {
        showMenu(MenuNames.MODE_SELCT_MENU);
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
            //showSubMenu(SubMenuNames.EXIT);
		}
	}

	void Update () {
        ControlFreak2.CFCursor.visible = true;
        ControlFreak2.CFCursor.lockState = wantedMode;
        if (ControlFreak2.CF2Input.GetKeyDown (KeyCode.Escape)) {
			handleBackMenu ();
		}
        
        if (_startAddingCoins)
        {
	        if (_currentNumber != _desiredNumber)
	        {
		        if (_initialNumber < _desiredNumber)
		        {
					CoinSound.enabled = true;
					_currentNumber += (coinsAnimationTime * Time.deltaTime) * (_desiredNumber - _initialNumber);
			        if (_currentNumber > _desiredNumber)
			        {
						CoinSound.enabled = false;
						_currentNumber = _desiredNumber;
				        _startAddingCoins = false;
			        }
		        }
	        }

	        PlayerGoldText.text = ((int) _currentNumber).ToString();
        }
	}

    string subject = "Frenzy Games Studio";
    string body = "Free Download City Sniper Shooter Mission: Sniper games offline from Google Play Store. " + MConstants.RATE_US;

    public void ShareGame()
    {
        //execute the below lines if being run on a Android device
#if UNITY_ANDROID
        //Refernece of AndroidJavaClass class for intent
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        //Refernece of AndroidJavaObject class for intent
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        //call setAction method of the Intent object created
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        //set the type of sharing that is happening
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        //add data to be passed to the other activity i.e., the data to be sent
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
        //get the current activity
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        //start the activity by sending the intent data
        currentActivity.Call("startActivity", intentObject);
#endif
        
    }
    private void Start()
    {
        /*if (AdsManager.instance.testMode)
        {
            TestAdsText.SetActive(true);
        }
        else
        {
            TestAdsText.SetActive(false);
        }
        if (AdsManager.instance != null)
        {
            
            AdsManager.instance.RequestBanner();
        }*/
    }
    public void CloseSubMenu(){
		isSubMenuVisible = false;
		subMenusList [currentSubMenu.GetHashCode ()].SetActive (false);
    }

	public void StartMenuDrive()
	{
		// StartCoroutine(ShowLevelScreenLoading());
		MConstants.CurrentGameMode = MConstants.GAME_MODES.MODE1;
		showMenu(MenuNames.LEVEL_SELECTION);
		

		if (!PlayerDataController.Instance.playerData.firstTimeTutorial)
		{
			mainMenuTutorialBg.SetActive(false);
			mainMenuClickImage.SetActive(false);
		}
  //      if (UnityAnalyticsScript.instance != null)
  //      {
		//	UnityAnalyticsScript.instance.AddUnityEvent("MainMenuPlay", new Dictionary<string, object>
		//	{
		//	});
		//}
		
	}

	public void StartSurvivalMode()
	{
		// StartCoroutine(ShowLevelScreenLoading());
		MConstants.CurrentGameMode = MConstants.GAME_MODES.MODE2_Expert;
		showMenu(MenuNames.LEVEL_SELECTION);
		Debug.Log("Mode Setup  " + MConstants.CurrentGameMode);
	}

	public void StartEndlessMode()
	{
		// StartCoroutine(ShowLevelScreenLoading());
		MConstants.CurrentGameMode = MConstants.GAME_MODES.ENDLESS_MODE;
		showMenu(MenuNames.LEVEL_SELECTION);
		Debug.Log("Mode Setup  "+ MConstants.CurrentGameMode);
	}

	public void StartZombieMode()
	{
		// StartCoroutine(ShowLevelScreenLoading());
		MConstants.CurrentGameMode = MConstants.GAME_MODES.MODE3_Zombie;
		showMenu(MenuNames.LEVEL_SELECTION);
	}
	
	public void StartSquidMode()
	{
		// StartCoroutine(ShowLevelScreenLoading());
		MConstants.CurrentGameMode = MConstants.GAME_MODES.MODE4_Squid;
		showMenu(MenuNames.LEVEL_SELECTION);
	}
	
	public void StartBattleFieldMode()
	{
		// StartCoroutine(ShowLevelScreenLoading());
		MConstants.CurrentGameMode = MConstants.GAME_MODES.MODE5_BATTLEFIELD;
		showMenu(MenuNames.LEVEL_SELECTION);
	}

	IEnumerator ShowLevelScreenLoading()
	{
		showSubMenu(SubMenuNames.LOADING);
		yield return new WaitForSeconds(1.5f);
		CloseSubMenu();
	}
	
	
    public void ShowGunMenu()
    {
        showMenu(MenuNames.GUN_MENU);
        
    }
    public void openGameModesMenu(){
		 PlayerDataSerializeable playerData;

		playerData = PlayerDataController.Instance.playerData;

	//	if (playerData.CarsList [playerData.SelectedVehicle_temp].isLocked) {
	//		openCarUnlockPopup ();
	//	} else {
            string env = "";
            if (MConstants.GAME_MODES.MODE1 == MConstants.CurrentGameMode)
            {
                env = MConstants.Env_1;
                //PlayerDataController.Instance.playerData.CurrentEnvironment = 1;
            }
            else if (MConstants.GAME_MODES.MODE2_Expert == MConstants.CurrentGameMode)
            {
                env = MConstants.Env_2;
                //PlayerDataController.Instance.playerData.CurrentEnvironment = 2;
            }
            else if (MConstants.GAME_MODES.MODE3_Zombie == MConstants.CurrentGameMode)
            {
                env = MConstants.Env_3;
                //PlayerDataController.Instance.playerData.CurrentEnvironment = 3;
            }
            else if (MConstants.GAME_MODES.MODE4_Squid == MConstants.CurrentGameMode)
            {
	            env = MConstants.Env_4_Squid;
	            //PlayerDataController.Instance.playerData.CurrentEnvironment = 3;
            }
            else if (MConstants.GAME_MODES.MODE5_BATTLEFIELD == MConstants.CurrentGameMode)
            {
	            env = MConstants.Env_5_BattleField;
	            //PlayerDataController.Instance.playerData.CurrentEnvironment = 3;
            }
            else if (MConstants.GAME_MODES.SURVIVAL_MODE == MConstants.CurrentGameMode)
            {
                env = MConstants.Env_Survival;
                //PlayerDataController.Instance.playerData.CurrentEnvironment = 3;
            }
            // else
            // {
            //     env = MConstants.Env_3;
            //     //PlayerDataController.Instance.playerData.CurrentEnvironment = 3;
            // }
            PlayerDataController.Instance.Save();
            showSubMenu(SubMenuNames.LOADING);
            //Application.LoadLevelAsync(env);
			SceneManager.LoadSceneAsync(env,LoadSceneMode.Single);
		//     }
	}
    
    public void ShowSecondModeAfterCongrats()
    { 
	    StartCoroutine(ShowSecondModeDelay());
    }
    
    public IEnumerator ShowSecondModeDelay()
    {
	    CloseSubMenu();
	    showMenu(MenuNames.MODE_SELCT_MENU);
	    yield return new WaitForSeconds(0.1f);

	    MConstants.CurrentGameMode = MConstants.GAME_MODES.MODE2_Expert;
	    showMenu(MenuNames.LEVEL_SELECTION);
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

		if (PlayerDataController.Instance.playerData.PlayerGold >= PlayerDataController.Instance.playerData.gunsList [PlayerDataController.Instance.playerData.SelectedVehicle_temp].UnlockPrice) {
			showSubMenu (SubMenuNames.CAR_UNLOCK_POPUP);
		} /*else {
			OutOfCashMenu.isCarBuy = true;
			showSubMenu (SubMenuNames.OUT_OF_CASH);
		}*/
	}


	public void openCarUpGradePopup(){
		if (PlayerDataController.Instance.playerData.PlayerCash >= PlayerDataController.Instance.playerData.gunsList [PlayerDataController.Instance.playerData.CurrentSelectedPrimaryGun - 1].UpgradePrice) {
			showSubMenu (SubMenuNames.CAR_UPGRADE_POPUP);
		} else {
			showSubMenu (SubMenuNames.OUT_OF_CASH);
		}
	}

	public void RateUs(){
		Application.OpenURL(MConstants.RATE_US);

		//UnityAnalyticsScript.instance.AddUnityEvent ("Rate Us", new Dictionary<string, object>{
		//});
	}

	public void MoreGames(){
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Frenzy+Games+Studio");

        //UnityAnalyticsScript.instance.AddUnityEvent("More Games", new Dictionary<string, object>
        //{
        //});
    }

	public void FB(){
		Application.OpenURL("https://web.facebook.com/top3Dgamessimstudio/");

		//UnityAnalyticsScript.instance.AddUnityEvent ("FB", new Dictionary<string, object>{
		//});
	}

	public void ShowLeaderBoard(){
	//	SocialManager.instance.OnShowLeaderBoard ();
		leaderBoard.SetActive(true);
		//UnityAnalyticsScript.instance.AddUnityEvent ("Leaderboard", new Dictionary<string, object>{
		//});
	}

	public void AdGold(){
		showSubMenu (SubMenuNames.STOREPANEL);
  //      if (UnityAnalyticsScript.instance != null)
  //      {
		//	UnityAnalyticsScript.instance.AddUnityEvent("FreeGold", new Dictionary<string, object>{
		//	{ "position", "MainMenu"}
		//});
		//}
		

	}

	public void ShowStore()
	{
		showSubMenu(SubMenuNames.STOREPANEL);
	}

	public void AddGold(int goldAmount)
	{
		SetCoinsText(goldAmount);
	}

	void SetCoinsText(int goldAmountNumber)
	{
		_currentNumber = _initialNumber = PlayerDataController.Instance.playerData.PlayerGold - goldAmountNumber;
		_desiredNumber = PlayerDataController.Instance.playerData.PlayerGold;
		_startAddingCoins = true;
	}

	public void ShowSideMenu(GameObject sideMenu)
	{
		sideMenu.SetActive(!sideMenu.activeSelf);
	}

	//// Umair

	public void CoinAnimation()
	{

		_startAddingCoins = true;
	}

	public void WeeklyAddGold(int goldAmount)
    {

		_currentNumber = _desiredNumber = PlayerDataController.Instance.playerData.PlayerGold;

        PlayerDataController.Instance.playerData.PlayerGold = (PlayerDataController.Instance.playerData.PlayerGold + goldAmount);
        PlayerDataController.Instance.Save();
        _desiredNumber = PlayerDataController.Instance.playerData.PlayerGold;
        //  SetCoinsText(goldAmount);
    }

	public void ShowSpinScreen()
	{
		showSubMenu(SubMenuNames.SpinScreen);
	}

	public void PlayGame()
    {
        if (!isEndlessModeSelected)
			StartSurvivalMode();
		else
        {
			StartEndlessMode();
			LevelSelectionMenuManager.Instance.PlayButton();
		}
    }

	public bool isEndlessModeSelected = false;

	public void SelectGameMode(bool endlessMode)
	{
		isEndlessModeSelected = endlessMode;
	}

}
