using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class HudMenuManager : MonoBehaviour
{
    public static HudMenuManager instance;
    public GameObject gameOverMenu;
    public GameObject pauseGame;
    public GameObject pauseBtn;
    public FPSPlayer player;
    public GameObject organsCamera;
    public static float sensitivity;

    public Slider sensitbitySlider;
    public GameObject loading;
    public GameObject BlackCurtain;
    public GameObject reviveMenu;
    public GameObject howToPlayScreen, missionInfoPanel;
    public Text missionText;
    public GameObject missionTextBg;
    public GameObject disableMissionBg;
    public GameObject totalKillBg;
    public Text killCountText;


    public GameObject okButton;
    public GameObject[] fireButtons;

    public SmoothMouseLook SmoothMouseLook;
    public Gradient Gradient;
    public int RegisteredEnemyToKill = 0;
    public GameObject Radar;

    public Text textWave, TotalEnemyKilled, outOfAmmoText, NotificationText, MissionStatmentText;

    public GameObject outOfAmmoDialouge;
    public GameObject noVideoPopup;
    //public FPSPlayer FPSPlayer;
    public Slider playerhealthBar;
    // public Image thermalViewButton, thermalSlideImage;
    public float thermalDecreaseRatio;
    public GameObject[] meshesToDisable;
    public bool zoomedIn = false;
    public GameObject[] UI;
    public GameObject[] Snipers , buttosToDisAble , battleFieldButtons; //, buttosToDisAble;
    public GameObject HeadShortObj;
    public GameObject[] ObjectsToDisableWhenGameOver;
    public int HeadShortCount = 0;
    public Toggle autoFireToggle;
    public Image InstrutionImage;
    public DOTweenAnimation ThrowGrenadeButton;
    public Camera FpsCamera;
    public GameObject LastBulletPrefab, Croshair, battleFieldCrosshair, machineGunCrosshair;
    public GameObject bloodExplodeFx;
    public GameObject redAlert, greyAlert;
    public string Sucide_Bomber_Detected, Wavetext, HostageRescue_text;
    public GameObject headShotAlertImage, killShotAlertImage, coinRewardAlert;
    public Text rewardAmountText;
    public Sprite headShotImage, heartShotImage, lungShotImage;

    [Header("Endless Mode"), Space(10)]
    public int killCount;
    public Text TotalKill;
    public Text BestKill;

    [Header(" UI Buttons To Disable For Tutorial"), Space(10)]
    public GameObject moveLeft;

    public GameObject moveRight;
    public GameObject zoomBtn;
    public GameObject fireBtn;
    public GameObject changeGunBtn;
    public GameObject reloadBtn;
    public GameObject bullets;
    public GameObject zoomTut;
    public GameObject fireTut;
    public GameObject zoomSlider;
    public GameObject zoomSliderTut;
    public GameObject thermalTut;
   

    private bool startThermalTime;
    private AudioSource _audioSource;

    [HideInInspector] public int bodyShotCount, headShotCount, heartShotCount, lungShotCount;
    [HideInInspector]
    public bool isFirebtnActive = false;
    public Transform jetDummyTarget;

    public GameObject dataManager;

    public Text killingRewardText;
    public Text headShotRewardText;
    public Text totalRewardText;

    public GameObject aimReticleNew;

    private AchievementManager achievementManager;
    private Timer timer;

    public void ShowHeadShort(Transform pos)
    {
        HeadShortObj.SetActive(false);
        HeadShortObj.transform.position = pos.position;
        HeadShortObj.transform.LookAt(Camera.main.transform);
        HeadShortObj.SetActive(true);
        HeadShortCount++;
    }

    public void HeadShotAlert()
    {
        StartCoroutine(HeadShotAlertCoroutine());
    }

    public void CoinRewardAlert()
    {
        StartCoroutine(CoinRewardAlertCoroutine());
    }

    IEnumerator HeadShotAlertCoroutine()
    {
        if (EndlessModeManager.Instance)
            EndlessModeManager.Instance.RewardAlert(rewardAmountText, 50);

        headShotAlertImage.SetActive(true);
        yield return new WaitForSeconds(5f);
        headShotAlertImage.SetActive(false);
    }

    IEnumerator CoinRewardAlertCoroutine()
    {
        if (EndlessModeManager.Instance)
            EndlessModeManager.Instance.RewardAlert(rewardAmountText, 10);

        coinRewardAlert.SetActive(true);
        yield return new WaitForSeconds(2f);
        coinRewardAlert.SetActive(false);

    }

    public void KillShotAlert()
    {
        StartCoroutine(KillShotAlertCoroutine());
    }

    IEnumerator KillShotAlertCoroutine()
    {
        killShotAlertImage.SetActive(true);
        yield return new WaitForSeconds(5f);
        killShotAlertImage.SetActive(false);
    }

    public void SniperZoomPreCheck()
    {
        // if (MConstants.CurrentLevelNumber == 1 && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        // {
        //     ShowZoomSliderTutorial();
        // }

        // if (MConstants.CurrentLevelNumber == 3 && zoomTut.activeSelf &&
        //     MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        // {
        //     zoomTut.SetActive(false);
        //     zoomBtn.SetActive(false);
        //     thermalTut.SetActive(true);
        //     thermalViewButton.gameObject.SetActive(true);
        // }

        //foreach (var item in Snipers)
        //{
        //    if (item.activeSelf)
        //    {
        //        SniperZoom();
        //    }
        //}
    }

    public void SniperZoom()
    {
        zoomedIn = true;

        if (zoomedIn)
        {
            
            foreach (var item in meshesToDisable)
            {
                item.SetActive(false);
            }

            foreach (var item in UI)
            {
                item.SetActive(true);
            }

            changeGunBtn.SetActive(false);
            reloadBtn.SetActive(false);
        }
        else
        {
           // enemyHealthBar.SetActive(true);
            foreach (var item in meshesToDisable)
            {
                item.SetActive(true);
            }

            foreach (var item in UI)
            {
                item.SetActive(false);
            }
        }
    }

    public void zoomOut()
    {
        FPSPlayer.instance.zoomed = false;
        foreach (var item in meshesToDisable)
        {
            item.SetActive(true);
        }

        foreach (var item in UI)
        {
            item.SetActive(false);
        }

        reloadBtn.SetActive(true);

        if (PlayerDataController.Instance)
        {
            for (int x = 1; x < PlayerDataController.Instance.playerData.gunsList.Count; x++)
            {
                if (!PlayerDataController.Instance.playerData.gunsList[x].isLocked)
                {
                    changeGunBtn.SetActive(true);
                }
            }
        }
       

        if (MConstants.IslastBullet)
        {
            if(MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie && LevelsManager.instance.currentLevel.vehicles)
            {
                LevelsManager.instance.currentLevel.vehicleHealthElement.ToRemoveHealthBar();
            }
            changeGunBtn.SetActive(false);
            reloadBtn.SetActive(false);
        }
    }

    //public void ZoomForMachineGuns()
    //{
    //    if(!WeaponBehavior.instance.IronsightsComponent.reloading && HudMenuManager.instance.machineGunCrosshair)
    //        machineGunCrosshair.SetActive(!machineGunCrosshair.activeSelf);
    //}

    IEnumerator ShowNotification()
    {
        yield return new WaitForSeconds(2f);
        NotificationText.text = "";
    }

    void Awake()
    {
        MConstants.isPlayerWin = false;
        MConstants.IslastBullet = false;
        MConstants.BulletFired = false;
        MConstants.LastBulletTarget = null;
        // MConstants.LastBulletHit = null;
        MConstants.GunInstLimit = 0;
        MConstants.HealtInstLimit = 0;
        headShotCount ++;
        missionTextBg.SetActive(MConstants.CurrentGameMode != MConstants.GAME_MODES.SURVIVAL_MODE);
        totalKillBg.SetActive(MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE);
        totalKillBg.SetActive(MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE);
        aimReticleNew.SetActive(MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE);

        if (SmoothMouseLook == null)
        {
            SmoothMouseLook = GameObject.FindObjectOfType(typeof(SmoothMouseLook)) as SmoothMouseLook;
        }
        if (PlayerDataController.Instance == null)
        {
            Instantiate(dataManager);
        }


        instance = this;
        MConstants.isGameOver = false;
        MConstants.bulletsFinished = false;
        // updatePos(1);
        if (PlayerDataController.Instance)
        {
            sensitivity = PlayerDataController.Instance.playerData.SensivityValue;
        }
        else
        {
            sensitivity = 4f;
        }

        SmoothMouseLook.sensitivity = sensitivity / 2;

//        if (MConstants.CurrentLevelNumber == 1)
//        {
        MConstants.autoFireMode = false;
//        }
//        else
//        {
//            MConstants.autoFireMode = true;
//        }
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            playerhealthBar.gameObject.SetActive(true);
            missionTextBg.SetActive(false);
            disableMissionBg.SetActive(false);
        }
        if(MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            playerhealthBar.gameObject.SetActive(true);
        }
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
        {
            MConstants.autoFireMode = false;
            TotalKill.gameObject.transform.parent.gameObject.SetActive(true);
            BestKill.gameObject.transform.parent.gameObject.SetActive(true);
        }

        if(MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE){
            playerhealthBar.gameObject.SetActive(true);
        }

        autoFireToggle.isOn = MConstants.autoFireMode;
        foreach (var fireButton in fireButtons)
        {
            fireButton.SetActive(!MConstants.autoFireMode);
        }

        changeGunBtn.SetActive(false);
        if (PlayerDataController.Instance)
        {
            for (int x = 1; x < PlayerDataController.Instance.playerData.gunsList.Count; x++)
            {
                if (!PlayerDataController.Instance.playerData.gunsList[x].isLocked)
                {
                    changeGunBtn.SetActive(true);
                }
            }
        }
       
        // if (MConstants.CurrentLevelNumber < 4 && PlayerDataController.Instance.playerData
        //     .gunsList[PlayerDataController.Instance.playerData.CurrentSelectedSecondaryGun].isLocked)
        // {
        //     changeGunBtn.SetActive(false);
        // }

        // if (MConstants.CurrentLevelNumber == 5 && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        // {
        //     changeGunBtn.SetActive(false);
        // }
    }
    public void InfoMission()
    {
        missionInfoPanel.SetActive(true);
        Level.instance.objectiveCam.SetActive(true);
    }
    public void EnableRadar()
    {
        Radar.SetActive(true);
    }

    void Start()
    {
        achievementManager = AchievementManager.Instance;
        timer = Timer.Instance;

        _audioSource = GetComponent<AudioSource>();
        string levelString = "level_start";

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            levelString = "level_start_twisting";
        }

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
        {
            levelString = "level_start_expert";
        }

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            levelString = "level_start_zombie";
        }

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            levelString = "level_start_squid";
        }

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            levelString = "level_start_battlefield";
        }

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
        {
            levelString = "Endless_mode";
        }
        //if (UnityAnalyticsScript.instance)
        //{


        //    UnityAnalyticsScript.instance.AddUnityEvent(levelString, new Dictionary<string, object>
        //    {
        //        {"level_index", "" + MConstants.CurrentLevelNumber}
        //    });
        //    UnityAnalyticsScript.instance.AddFirebaseEvent(levelString, MConstants.CurrentLevelNumber);
        //}

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
        {
           /* if (AdsManager.instance != null)
            {
                AdsManager.instance.RequestBanner();
            }*/

            Invoke("StartGame", 1);
        }
        else
        {
            /*if (AdsManager.instance != null)
            {
                AdsManager.instance.RequestBanner();
                // AdsManager.instance.RemoveBanner();
            }*/

            RegisteredEnemyToKill =
                LevelsManager.instance.currentLevel.RegisterEnemyToKill;
            TotalEnemyKilled.text = RegisteredEnemyToKill + " / " +
                                    LevelsManager.instance.currentLevel
                                        .RegisterEnemyToKill;
            Invoke("StartGame", 1);
        }

        /*if (MConstants.CurrentLevelNumber <= 2 && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            thermalViewButton.gameObject.SetActive(false);
            thermalViewButton.gameObject.GetComponent<Button>().interactable = false;
        }*/
/*
        if (AdsManager.instance != null)
        {
            AdsManager.instance.RequestInterstitial();
        }
        if (AdsManager.instance != null)
        {
            AdsManager.instance.setPosition(true);
        }*/
    }


    public void GameOver()
    {
        if (NotificationText != null)
        {
            NotificationText.text = "";
        }

        if (redAlert.activeSelf) redAlert.SetActive(false);

        Invoke("GameFail", 1);
    }
   
    public void GameFail()
    {
        MConstants.isGameOver = true;
       /* AdsManager.isRevive = false;*/
       gameOverMenu.SetActive(true);
       if (MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE)
        {
            if (EndlessModeManager.Instance != null)
            {
                EndlessModeManager.Instance.SetEndlessGameStats();
            }
        }

    }

    void HostageText()
    {
        HudMenuManager.instance.NotificationText.text = HudMenuManager.instance.HostageRescue_text;
        HudMenuManager.instance.StartCoroutine("ShowNotification");
    }

    public void EnemyKilled()
    {
        if (MConstants.CurrentGameMode != MConstants.GAME_MODES.SURVIVAL_MODE && MConstants.CurrentGameMode != MConstants.GAME_MODES.ENDLESS_MODE)
        {
            RegisteredEnemyToKill--;
            if (RegisteredEnemyToKill <= 0)
            {
                GameComplete();
                
                // if (LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].FinalPortal == null)
                // {
                //     GameComplete();
                // }
                // else
                // {
                //     LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].FinalPortal.SetActive(true);
                //
                //     Invoke("HostageText", 3f);
                // }
            }

            missionText.text = LevelsManager.instance.currentLevel.missionStatement + "\n"
                               + " ( " + RegisteredEnemyToKill + "/ " + LevelsManager.instance.currentLevel.RegisterEnemyToKill + " )";
            // TotalEnemyKilled.text = RegisteredEnemyToKill + " / " + LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].RegisterEnemyToKill;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE)
        {
            killCount++;
            
            achievementManager.ReportProgress(Quest.KillerInstinct, killCount > 150);
            achievementManager.ReportProgress(Quest.OnTheEdge, killCount > 75);
            achievementManager.ReportProgress(Quest.HitMan, killCount > 50);
            achievementManager.ReportProgress(Quest.DailyKills, killCount > 10);

            if (timer.IsUnderTwoMinutes())
            {
                achievementManager.ReportProgress(Quest.Focus, killCount >= 5);
            }

            if (timer.IsUnderFourMinutes()) 
            {
                achievementManager.ReportProgress(Quest.DoingMyJob, killCount >= 10);
            }

            killCountText.text = "Total Kills = " + killCount;
            Debug.Log("Here i am in ENdlesss Mode");
        }
        else
        {
            RegisteredEnemyToKill--;
            killCount++;
            if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
            {
                killCountText.text = "Total Kills = " + killCount;
            }

            if (killCount > PlayerDataController.Instance.playerData.BestKill)
            {
                PlayerDataController.Instance.playerData.BestKill = killCount;
                PlayerDataController.Instance.Save();
            }
        }
        /*else
        {
            RegisteredEnemyToKill++;
            TotalEnemyKilled.text = RegisteredEnemyToKill.ToString();
            TotalKill.text = RegisteredEnemyToKill.ToString();
            WaveDataController.Instance.wavecount();
            MConstants.GunInstLimit++;
            MConstants.HealtInstLimit++;
            if (RegisteredEnemyToKill > PlayerDataController.Instance.playerData.BestKill)
            {
                PlayerDataController.Instance.playerData.BestKill = RegisteredEnemyToKill;
                BestKil.text = PlayerDataController.Instance.playerData.BestKill.ToString();
                PlayerDataController.Instance.Save();
            }
            else
            {
                BestKil.text = PlayerDataController.Instance.playerData.BestKill.ToString();
            }
        }*/
    }

    public void GameOverByNoRevive()
    {
        MConstants.isGameOver = true;
        gameOverMenu.SetActive(true);
    }

    public void LapCompleteRemove()
    {
    }

    public void PlayFastBgMusic()
    {
        if (_audioSource != null)
        {
            // _audioSource.clip = fastBgMusic;
            _audioSource.Play();
        }
    }

    void FreezVehicle()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            player.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void GameComplete()
    {
        if (NotificationText != null)
        {
            NotificationText.text = "";
        }

        foreach (var item in ObjectsToDisableWhenGameOver)
        {
            //      item.SetActive(false);
        }

        if (zoomedIn)
        {
            if (ZoomSlider.instance)
            {
                ZoomSlider.instance.zoomslider.value = 6f;
                ZoomSlider.instance.ChangeZoomValue();
            }
            else
            {
                zoomOut();
            }
        }

        MConstants.isPlayerWin = true;
        MConstants.isGameOver = false;

        Invoke("GameFail", 1);
    }

    public void Revive()
    {
       /* AdsManager.isRevive = false;*/
        //refreshHealth ();
        Timer.Instance.resetTime();
        MConstants.isGameOver = false;
        Time.timeScale = 1;
    }

    public void WatchVideoToAddBullets()
    {
        /*AdsManager.isRevive = AdsManager.isUnLockLevel = AdsManager.HaveTempGun = false;
        AdsManager.AddBullets = true;
        AdsManager.instance.ShowRewardedAdd();*/
    }

    public void CloseWatchVideoToAddBullets()
    {
        /*AdsManager.AddBullets = false;*/
        outOfAmmoDialouge.SetActive(false);
        GameOverByNoRevive();
    }
    //public void showMissionInstruction(){
    //
    //	MissionInfo.SetActive (true);
    //	okButton.SetActive (false);
    //	Invoke ("PausePlayer",0.5f);
    //}


    void PausePlayer()
    {
        okButton.SetActive(true);

        if (player == null)
        {
            player = GameObject.FindObjectOfType(typeof(FPSPlayer)) as FPSPlayer;
        }

        if (player != null)
        {
            player.paused = true;
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        MConstants.isGameStarted = true;

        /*if (howToPlayScreen && MConstants.CurrentLevelNumber == 1 &&
            MConstants.CurrentGameMode != MConstants.GAME_MODES.ENDLESS_MODE)
        {
            howToPlayScreen.SetActive(true);
            //Invoke ("hideCameraInstruction",3);
        }*/

        if (player == null)
        {
            player = GameObject.FindObjectOfType(typeof(FPSPlayer)) as FPSPlayer;
        }

        if (player != null)
        {
            player.paused = false;
        }

        player.PlayerWeaponsComponent.SelectWeapon();
    }

    public void hideCameraInstruction()
    {
        howToPlayScreen.SetActive(false);
    }

    public void PauseGame()
    {
        pauseGame.SetActive(true);
        if (player == null)
        {
            player = GameObject.FindObjectOfType(typeof(FPSPlayer)) as FPSPlayer;
        }

        if (player != null)
        {
            player.paused = true;
        }
    }

    public void ResumeGame()
    {
        sensitivity = PlayerDataController.Instance.playerData.SensivityValue;
        SmoothMouseLook.sensitivity = sensitivity / 2;
        if (player == null)
        {
            player = GameObject.FindObjectOfType(typeof(FPSPlayer)) as FPSPlayer;
        }

        if (player != null)
        {
            player.paused = false;
        }
    }


    void Update()
    {
        // if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.Escape))
        // {
        //     PauseGame();
        // }

        if (MConstants.CurrentLevelNumber == 1 && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            if (ControlFreak2.CF2Input.GetButton("Fire"))
            {
                 HideFireTutorial();
            }
        }

        playerhealthBar.value = player.hitPoints;

#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.F))
            InputControl.instance.fireHold = true;
        else
            InputControl.instance.fireHold = false;	
#endif
        /*if (startThermalTime)
        {
            thermalSlideImage.fillAmount -= thermalDecreaseRatio;
            if (thermalSlideImage.fillAmount <= 0.1f)
            {
                ActivateThermalView();
            }
        }*/
    }

    public void TimeScaleToOne()
    {
        Time.timeScale = 1;

        if (PlayerDataController.Instance != null && PlayerDataController.Instance.playerData.isSoundOn)
        {
            AudioListener.volume = 1;
        }
    }

    public void ToggleAutoFire()
    {
        MConstants.autoFireMode = autoFireToggle.isOn;

        foreach (var fireButton in fireButtons)
        {
            fireButton.SetActive(!MConstants.autoFireMode);
            reloadBtn.SetActive(!MConstants.autoFireMode);
        }
    }

    //public void ToggleAutoFire()
    //{
    //    //  SoundManager.instance.playAudioClip(SoundManager.SoundNames.Panel_Buttons_Sound.GetHashCode());
    //    MConstants.autoFireMode = autoFireToggle.isOn;
    //    // if (MConstants.CurrentGameMode == MConstants.GAME_MODES.PVP_MODE)
    //    //{
    //    if (MConstants.autoFireMode)
    //    {
    //        if (isFirebtnActive)
    //        {
    //            foreach (GameObject firbtn in fireButtons)
    //            {
    //                firbtn.SetActive(true);
    //            }
    //            MConstants.autoFireMode = false;
    //            //    isFirebtnActive = false;
    //        }
    //        else
    //        {
    //            foreach (GameObject firbtn in fireButtons)
    //            {
    //                firbtn.SetActive(false);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        foreach (GameObject firbtn in fireButtons)
    //        {
    //            firbtn.SetActive(true);
    //        }
    //    }
    //    //}
    //}


    public void CheckTutorials()
    {
        if (MConstants.CurrentLevelNumber == 1 && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            EnableAllUi(false);
            changeGunBtn.SetActive(false);
            reloadBtn.SetActive(false);
            StartCoroutine(StartLevel1Tutorial());
        }
    }

    public void EnableAllUi(bool enable = true)
    {
        // zoomBtn.SetActive(enable);
        fireBtn.SetActive(enable);
        changeGunBtn.SetActive(enable);
        bullets.SetActive(enable);
        reloadBtn.SetActive(enable);
        zoomSlider.SetActive(enable);
       
        
    }

    #region level 1 Tutorial

    IEnumerator StartLevel1Tutorial()
    {
       
        moveLeft.SetActive(true);
        moveRight.SetActive(true);
        yield return new WaitForSeconds(4f);
        moveLeft.SetActive(false);
        moveRight.SetActive(false);
        SmoothMouseLook.instance.SetCameraTutorialTarget();
        ShowZoomSliderTutorial();
        if (MConstants.CurrentLevelNumber == 1 && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            changeGunBtn.SetActive(false);
            reloadBtn.SetActive(false);
        }
        // zoomBtn.SetActive(true);
        // zoomTut.SetActive(true);
    }

    void ShowZoomSliderTutorial()
    {
        zoomBtn.SetActive(false);
        zoomTut.SetActive(false);
        zoomSlider.SetActive(true);
        zoomSliderTut.SetActive(true);
    }

    public void ShowFireTutorial()
    {
        zoomSlider.SetActive(false);
        zoomSliderTut.SetActive(false);
        fireBtn.SetActive(true);
        fireTut.SetActive(true);
    }

    public void HideFireTutorial()
    {
        fireBtn.SetActive(false);
        fireTut.SetActive(false);
    }

    #endregion

    
    public void SlowMotionLastBullet()
    {
        StartCoroutine(SlowMotionLastBulletCoroutine());
    }

    IEnumerator SlowMotionLastBulletCoroutine()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1f;
    }

    public void TurretMission(bool isFireBtn)
    {
       // PlayerWeapons.instance.weaponOrder[19].GetComponent<WeaponBehavior>().haveWeapon = false;
       // StartCoroutine(PlayerWeapons.instance.SelectWeapon(PlayerWeapons.instance.TempGun));

        if (isFireBtn)
        {
            foreach (var item in buttosToDisAble)
            {
                item.SetActive(false);
            }
        }
        else
        {
            foreach (var item in buttosToDisAble)
            {
                item.SetActive(true);
            }
        }
        isFirebtnActive = isFireBtn;
        ToggleAutoFire();
    }
    /*public void ActivateThermalView()
    {
        MConstants.ThermalVision = !MConstants.ThermalVision;
        Camera.main.GetComponent<ThermalNightVisionImageEffect>().enabled = MConstants.ThermalVision;

        if (MConstants.ThermalVision)
        {
            startThermalTime = true;
        }
        else
        {
            thermalSlideImage.fillAmount = 1;
            startThermalTime = false;
        }
    }*/
}