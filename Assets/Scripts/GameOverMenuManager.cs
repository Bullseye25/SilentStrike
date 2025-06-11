using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameOverMenuManager : MonoBehaviour
{
    public static GameOverMenuManager instance;
    public GameObject WinUI;
    public GameObject FailUI;
    
    public GameObject SkipUI;
    public GameObject skipButton;

    public GameObject RateUsUI;
    public GameObject promo;

    public CoinHandler coinHandler;
    public GameObject claimBtns;

    int starsCount = 0;
   // public Text winObjectiveText, looseObjectiveText;
    public Text goldReward, headShotText, totalAmountText;
    public Text freeMode;
    public Text bodyShotReward, headShotReward, heartShotReward, lungShotReward;
    private int bodyShotAmount, headShotAmount, heartShotAmount, lungShotAmount;
    public int bodyPartRewardMultiply;
    string winOrLose = "";

    //public static int looseCount =0;

    public Text bestTime;
    public GameObject bestTimeObject;
    public GameObject AlertViewForAd;
    public GameObject GameOverMenu;

    public Image[] starsList;
    public Sprite[] starsSpriteList;
    int lastLevel;
    public GameObject WinButton, nextButton, menuButton;
    public GameObject looseButton;
    public GameObject DoubleRewardButton2x;
    public AudioSource BG;
    public AudioSource[] AIs;
    int rewardGold;

    private void Awake()
    {
        instance = this;
    }
  

    void OnEnable()
    {
        if (MConstants.CurrentGameMode != MConstants.GAME_MODES.SURVIVAL_MODE)
        {
            foreach (var item in AIs)
            {
                if (item)
                {
                    item.volume = 0f;
                }
            }
        }

        if (BG == null) BG = LevelsManager.instance.gameObject.GetComponent<AudioSource>();
        
        BG.volume = 0f;
        starsCount = 0;
        HudMenuManager.instance.LapCompleteRemove();
        Time.timeScale = 1;

        var totalScore = HudMenuManager.instance.killCount;
        Debug.Log($"Total Hits: {totalScore}");
        NetworkManager.instance.UpdateScoreOnLeaderBoard(totalScore);

        //AdsManager.instance.RemoveBanner();
        //if (AdsManager.instance && MConstants.ISNATIVE_AD_LOADED)
        //{

        //    AdsManager.instance.ShowHideNativeAd(true);

        //    // nativeBg.SetActive(true && !PlayerDataController.Instance.playerData.isRemoveAds);

        //    promo.SetActive(false);

        //}
        //else if (promo)
        //{
        //    // nativeBg.SetActive(false);

        //    promo.SetActive(!PlayerDataController.Instance.playerData.isRemoveAds);
        //}

        //if (AdsManager.instance != null)
        //{
        //    AdsManager.instance.RemoveBanner();
        //    //if (InternetCheck.Instance.isInternetAvailable)
        //    //{
        //    //   // promo.SetActive(false);
        //    //}
        //    AdsManager.instance.ShowHideNativeAd(true);

        //}
        // promo.SetActive(false);


        /*if (AdsManager.instance != null && AdsManager.instance.isGamePlayAdReadyToShow())
        {
            AlertViewForAd.SetActive(true);
            GameOverMenu.SetActive(false);
            Invoke("ShowAd", 1);
        }
        else
        {
            GameOverMenu.SetActive(true);
        }*/
        GameOverMenu.SetActive(true);
        int rewardCash = Random.Range(100, 200);
        rewardGold = MConstants.rewardGold; //Random.Range (MConstants.StartRandom, MConstants.EndRandom);
        int xp = Random.Range(200, 300);
        //looseButton.SetActive(false);
        WinButton.SetActive(false);
        Invoke("ActiveNextButtons", 0.5f);
        if (MConstants.isPlayerWin)
        {
            string levelString = "level_complete";

            if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
            {
                levelString = "level_complete_twisting";
            }

            if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
            {
                levelString = "level_complete_expert";
            }

            if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
            {
                levelString = "level_complete_zombie";
            }

            if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
            {
                levelString = "level_complete_squid";
            }

            if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
            {
                levelString = "level_complete_battlefield";
            }

            if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
            {
                levelString = "Endless_mode_complete";
            }
            //if (UnityAnalyticsScript.instance)
            //{


            //    UnityAnalyticsScript.instance.AddUnityEvent(levelString, new Dictionary<string, object>
            //    {
            //        {"level_index", "" + MConstants.CurrentLevelNumber}
            //    });
            //    UnityAnalyticsScript.instance.AddFirebaseEvent(levelString, MConstants.CurrentLevelNumber);
            //}

            FailUI.SetActive(false);
            WinUI.SetActive(true);
            rewardGold = MConstants.rewardGold; //Random.Range(MConstants.rewardGold, MConstants.EndRandom);
            winOrLose = "Won";
           // winObjectiveText.text = LevelsManager.instance.currentLevel.missionStatement;
        }
        else
        {
            // HudMenuManager.instance.zoomedIn = true;
            HudMenuManager.instance.zoomOut();

            FailUI.SetActive(true);
            WinUI.SetActive(false);
            if(MConstants.CurrentGameMode != MConstants.GAME_MODES.SURVIVAL_MODE) 
               // looseObjectiveText.text = LevelsManager.instance.currentLevel.missionStatement;

            winOrLose = "Lost";
            if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
            {
                HudMenuManager.instance.TotalKill.text = HudMenuManager.instance.killCount.ToString();
                HudMenuManager.instance.BestKill.text = PlayerDataController.Instance.playerData.BestKill.ToString();
                rewardGold = HudMenuManager.instance.killCount * 15;
            }
            else
            {
                rewardGold = 0;

                string levelString = "level_fail";

                if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
                {
                    levelString = "level_fail_twisting";
                }

                if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
                {
                    levelString = "level_fail_expert";
                }

                if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
                {
                    levelString = "level_fail_zombie";
                }
                
                if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
                {
                    levelString = "level_fail_squid";
                }
                
                if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
                {
                    levelString = "level_fail_battlefield";
                }

                if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
                {
                    levelString = "Endless_mode_fail";
                }

            }


            bestTimeObject.SetActive(false);
        }


        // Reward Values
        goldReward.text = rewardGold.ToString();

        // bodyShotAmount = HudMenuManager.instance.bodyShotCount * bodyPartRewardMultiply;
        // bodyShotReward.text = bodyShotAmount.ToString();

        headShotAmount = HudMenuManager.instance.headShotCount * bodyPartRewardMultiply;
        headShotReward.text = headShotAmount.ToString();

        heartShotAmount = HudMenuManager.instance.heartShotCount * bodyPartRewardMultiply;
        heartShotReward.text = heartShotAmount.ToString();

        lungShotAmount = HudMenuManager.instance.lungShotCount * bodyPartRewardMultiply;
        lungShotReward.text = lungShotAmount.ToString();

        freeMode.text = rewardGold.ToString();

        headShotReward.text = HudMenuManager.instance.HeadShortCount * 200 + "";
        headShotReward.text = headShotAmount.ToString();

        totalAmountText.text = rewardGold /*+ bodyShotAmount*/ + headShotAmount + heartShotAmount + lungShotAmount + "";

        skipButton.SetActive(false);


        if (PlayerDataController.Instance == null)
        {
            return;
        }

         PlayerDataController.Instance.playerData.PlayerGold += rewardGold + headShotAmount;
       /* PlayerDataController.Instance.playerData.PlayerGold +=
            headShotAmount  ;*/
       // PlayerDataController.Instance.playerData.PlayerCash += rewardCash;


        lastLevel = PlayerDataController.Instance.playerData.currentSelectLevel_Mode2 - 1;
        Invoke("setStars", 1);


        /////////////////////////////// NEW //////////////////////////////////////
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
        {
            if (MConstants.CurrentLevelNumber == PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode2 &&
                MConstants.isPlayerWin)
            {
                PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode2 += 1;
                PlayerDataController.Instance.playerData.currentSelectLevel_Mode2 += 1;

               /* if (!PlayerDataController.Instance.playerData.isRateUSDone &&
                    PlayerDataController.Instance.playerData.currentSelectLevel_Mode2 % 4 == 0)
                {
                    RateUsUI.SetActive(true);
                }*/
            }
            else if (MConstants.isPlayerWin)
            {
                PlayerDataController.Instance.playerData.currentSelectLevel_Mode2 += 1;
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            if (MConstants.CurrentLevelNumber == PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode3 &&
                MConstants.isPlayerWin)
            {
                PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode3 += 1;
                PlayerDataController.Instance.playerData.currentSelectLevel_Mode3 += 1;

                /*if (!PlayerDataController.Instance.playerData.isRateUSDone &&
                    (PlayerDataController.Instance.playerData.currentSelectLevel_Mode3 % 4 == 0))
                {
                    RateUsUI.SetActive(true);
                }*/
            }
            else if (MConstants.isPlayerWin)
            {
                PlayerDataController.Instance.playerData.currentSelectLevel_Mode3 += 1;
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            if (MConstants.CurrentLevelNumber == PlayerDataController.Instance.playerData.LastUnlockedLevel_SquidMode &&
                MConstants.isPlayerWin)
            {
                PlayerDataController.Instance.playerData.LastUnlockedLevel_SquidMode += 1;
                PlayerDataController.Instance.playerData.currentSelectLevel_SquidMode += 1;

                /*if (!PlayerDataController.Instance.playerData.isRateUSDone &&
                    (PlayerDataController.Instance.playerData.currentSelectLevel_SquidMode % 4 == 0))
                {
                    RateUsUI.SetActive(true);
                }*/
            }
            else if (MConstants.isPlayerWin)
            {
                PlayerDataController.Instance.playerData.currentSelectLevel_SquidMode += 1;
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            if (MConstants.CurrentLevelNumber == PlayerDataController.Instance.playerData.LastUnlockedLevel_BattleMode &&
                MConstants.isPlayerWin)
            {
                PlayerDataController.Instance.playerData.LastUnlockedLevel_BattleMode += 1;
                PlayerDataController.Instance.playerData.currentSelectLevel_BattleMode += 1;

               /* if (!PlayerDataController.Instance.playerData.isRateUSDone &&
                    (PlayerDataController.Instance.playerData.currentSelectLevel_BattleMode % 4 == 0))
                {
                    RateUsUI.SetActive(true);
                }*/
            }
            else if (MConstants.isPlayerWin)
            {
                PlayerDataController.Instance.playerData.currentSelectLevel_BattleMode += 1;
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            if (MConstants.CurrentLevelNumber == PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode1 &&
                MConstants.isPlayerWin)
            {
                PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode1 += 1;
                PlayerDataController.Instance.playerData.currentSelectLevel_Mode1 += 1;

                /*if (!PlayerDataController.Instance.playerData.isRateUSDone &&
                    (PlayerDataController.Instance.playerData.currentSelectLevel_Mode1 % 4 == 0))
                {
                    RateUsUI.SetActive(true);
                }*/
            }
            else if (MConstants.isPlayerWin)
            {
                PlayerDataController.Instance.playerData.currentSelectLevel_Mode1 += 1;
            }
        }

        PlayerDataController.Instance.playerData.xpoints += xp;

        PlayerDataController.Instance.Save();

        
    }

    void ActiveButton()
    {
        looseButton.SetActive(true);
        WinButton.SetActive(true);

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            if (MConstants.CurrentLevelNumber >= MConstants.MAX_LEVELS_BATTLE)
            {
                nextButton.SetActive(false);
                menuButton.SetActive(true);
            }
            else
            {
                nextButton.SetActive(true);
                menuButton.SetActive(false);
            }
        }
        
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            if (MConstants.CurrentLevelNumber >= MConstants.MAX_LEVELS_SQUID)
            {
                nextButton.SetActive(false);
                menuButton.SetActive(true);
            }
            else
            {
                nextButton.SetActive(true);
                menuButton.SetActive(false);
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            if (MConstants.CurrentLevelNumber >= MConstants.MAX_LEVELS_ZOMBIE)
            {
                nextButton.SetActive(false);
                menuButton.SetActive(true);
            }
            else
            {
                nextButton.SetActive(true);
                menuButton.SetActive(false);
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
        {
            if (MConstants.CurrentLevelNumber >= MConstants.MAX_LEVELS_Expert)
            {
                nextButton.SetActive(false);
                menuButton.SetActive(true);
            }
            else
            {
                nextButton.SetActive(true);
                menuButton.SetActive(false);
            }
        }
        else
        {
            if (MConstants.CurrentLevelNumber >= MConstants.MAX_LEVELS)
            {
                nextButton.SetActive(false);
                menuButton.SetActive(true);
            }
            else
            {
                nextButton.SetActive(true);
                menuButton.SetActive(false);
            }
        }
    }
    void ActiveNextButtons()
    {
        WinButton.SetActive(true);
        claimBtns.SetActive(false);
       // ActiveButton();
    }

    void ActiveButtons()
    {
        looseButton.SetActive(true);
       // DoubleRewardButton2x.gameObject.SetActive(AdsManager.instance && AdsManager.instance.isAdReady());
        WinButton.SetActive(false);
        //claimBtns.SetActive(true);
    }

    public void OnClaim()
    {
        claimBtns.SetActive(false);
        coinHandler.OnGoldAddition(rewardGold, false);
        Invoke("ActiveNextButtons", 2f);

    }

    public void On3XRewardSuccess(bool success)
    {
        claimBtns.SetActive(false);
        Invoke("ActiveNextButtons", 2f);
        if (success)
        {
            NoIncreamentor noIncreamentor = totalAmountText.gameObject.AddComponent<NoIncreamentor>();
            noIncreamentor.goldText = totalAmountText;
            noIncreamentor.AddRemoveGold(rewardGold, rewardGold * 2);
            coinHandler.OnGoldAddition(rewardGold * 3, true);
        }
    }

    public void ShowRewardedAddForDoubleCoins()
    {
       /* if (AdsManager.instance != null && AdsManager.instance.isAdReady())
        {
            
            AdsManager.isDoubleRewardGameOver = true;

            AdsManager.instance.ShowRewardedAdd();
            UnityAnalyticsScript.instance.AddUnityEvent("ShowRewardedAdd", new Dictionary<string, object>{
            { "Positon", "Garage"}
        });
        }*/

    }
    public string getTotlalBottles()
    {
        if (PlayerDataController.Instance == null)
        {
            return "00:00";
        }


        return ((int) PlayerDataController.Instance.playerData.BestTime).ToString();
    }

    void setStars()
    {
        if (starsCount < starsList.Length)
        {
            if (starsCount < PlayerDataController.Instance.playerData.StarsList[lastLevel])
            {
                starsList[starsCount].sprite = starsSpriteList[0];
            }
            else
            {
                starsList[starsCount].sprite = starsSpriteList[1];
            }

            starsCount++;
            Invoke("setStars", 0.5f);
        }
    }

    public void Retry()
    {
        if (MConstants.isPlayerWin)
        {
            if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
            {
                PlayerDataController.Instance.playerData.currentSelectLevel_Mode1 -= 1;
            }
            else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
            {
                PlayerDataController.Instance.playerData.currentSelectLevel_Mode2 -= 1;
            }
            else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
            {
                PlayerDataController.Instance.playerData.currentSelectLevel_Mode3 -= 1;
            }
            else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
            {
                PlayerDataController.Instance.playerData.currentSelectLevel_SquidMode -= 1;
            }
            else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
            {
                PlayerDataController.Instance.playerData.currentSelectLevel_BattleMode -= 1;
            }
        }
        HudMenuManager.instance.loading.SetActive (true);
        //   StartCoroutine(LoadLevelScene());
        LoadLevelScene();
        // Invoke("SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)", 2f);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
   


    public string FloatToTime(float toConvert, string format)
    {
        switch (format)
        {
            case "00.0":
                return string.Format("{0:00}:{1:0}",
                    Mathf.Floor(toConvert) % 60, //seconds
                    Mathf.Floor((toConvert * 10) % 10)); //miliseconds
                break;
            case "#0.0":
                return string.Format("{0:#0}:{1:0}",
                    Mathf.Floor(toConvert) % 60, //seconds
                    Mathf.Floor((toConvert * 10) % 10)); //miliseconds
                break;
            case "00.00":
                return string.Format("{0:00}:{1:00}",
                    Mathf.Floor(toConvert) % 60, //seconds
                    Mathf.Floor((toConvert * 100) % 100)); //miliseconds
                break;
            case "00.000":
                return string.Format("{0:00}:{1:000}",
                    Mathf.Floor(toConvert) % 60, //seconds
                    Mathf.Floor((toConvert * 1000) % 1000)); //miliseconds
                break;
            case "#00.000":
                return string.Format("{0:#00}:{1:000}",
                    Mathf.Floor(toConvert) % 60, //seconds
                    Mathf.Floor((toConvert * 1000) % 1000)); //miliseconds
                break;
            case "#0:00":
                return string.Format("{0:#0}:{1:00}",
                    Mathf.Floor(toConvert / 60), //minutes
                    Mathf.Floor(toConvert) % 60); //seconds
                break;
            case "#00:00":
                return string.Format("{0:#00}:{1:00}",
                    Mathf.Floor(toConvert / 60), //minutes
                    Mathf.Floor(toConvert) % 60); //seconds
                break;
            case "0:00.0":
                return string.Format("{0:0}:{1:00}.{2:0}",
                    Mathf.Floor(toConvert / 60), //minutes
                    Mathf.Floor(toConvert) % 60, //seconds
                    Mathf.Floor((toConvert * 10) % 10)); //miliseconds
                break;
            case "#0:00.0":
                return string.Format("{0:#0}:{1:00}.{2:0}",
                    Mathf.Floor(toConvert / 60), //minutes
                    Mathf.Floor(toConvert) % 60, //seconds
                    Mathf.Floor((toConvert * 10) % 10)); //miliseconds
                break;
            case "0:00.00":
                return string.Format("{0:0}:{1:00}.{2:00}",
                    Mathf.Floor(toConvert / 60), //minutes
                    Mathf.Floor(toConvert) % 60, //seconds
                    Mathf.Floor((toConvert * 100) % 100)); //miliseconds
                break;
            case "#0:00.00":
                return string.Format("{0:#0}:{1:00}.{2:00}",
                    Mathf.Floor(toConvert / 60), //minutes
                    Mathf.Floor(toConvert) % 60, //seconds
                    Mathf.Floor((toConvert * 100) % 100)); //miliseconds
                break;
            case "0:00.000":
                return string.Format("{0:0}:{1:00}.{2:000}",
                    Mathf.Floor(toConvert / 60), //minutes
                    Mathf.Floor(toConvert) % 60, //seconds
                    Mathf.Floor((toConvert * 1000) % 1000)); //miliseconds
                break;
            case "#0:00.000":
                return string.Format("{0:#0}:{1:00}.{2:000}",
                    Mathf.Floor(toConvert / 60), //minutes
                    Mathf.Floor(toConvert) % 60, //seconds
                    Mathf.Floor((toConvert * 1000) % 1000)); //miliseconds
                break;
        }

        return "error";
    }

    public void skipButtonClick()
    {
        SkipUI.SetActive(true);
    }

    private void OnDisable()
    {
        //if(AdsManager.instance != null)
        //{
        //    AdsManager.instance.ShowHideNativeAd(false);
        //    AdsManager.instance.RequestBanner();
        //}
       
    }

    public void Continue()
    {
        HudMenuManager.instance.loading.SetActive(true);

        LoadLevel();
        // MainMenuManager.isGoToGrage = true;
        //  Invoke("LoadLevel", 2f);
        //  Application.LoadLevel("UIScene");
        //if (AdsManager.instance != null) {
        //	AdsManager.instance.ShowPlacementAd ();
        //}
        /* UnityAnalyticsScript.instance.AddUnityEvent("GameOverExit", new Dictionary<string, object>
         {
         });*/
    }

    public void GoToWeaponScreen()
    {
        MainMenuManager.isGoToWeaponScreen = true;
        HudMenuManager.instance.loading.SetActive(true);
        // Invoke("LoadLevel", 2f);
        LoadLevel();
        /*if (AdsManager.instance != null)
        {
            AdsManager.instance.setPosition(false);
        }*/
        // Application.LoadLevel("UIScene");
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene("UIScene");
    }

    public void skipLevelYesClick()
    {
        SkipUI.SetActive(false);

        PlayerDataController.Instance.playerData.PlayerGold -= 500;

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
        {
            MainMenuManager.FromProMode = true;
            PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode2 += 1;
            PlayerDataController.Instance.playerData.currentSelectLevel_Mode2 += 1;

            MConstants.CurrentLevelNumber = PlayerDataController.Instance.playerData.currentSelectLevel_Mode2;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            MainMenuManager.FromKidsMode = true;
            PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode3 += 1;
            PlayerDataController.Instance.playerData.currentSelectLevel_Mode3 += 1;

            MConstants.CurrentLevelNumber = PlayerDataController.Instance.playerData.currentSelectLevel_Mode3;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            MainMenuManager.FromKidsMode = true;
            PlayerDataController.Instance.playerData.LastUnlockedLevel_SquidMode += 1;
            PlayerDataController.Instance.playerData.currentSelectLevel_SquidMode += 1;

            MConstants.CurrentLevelNumber = PlayerDataController.Instance.playerData.currentSelectLevel_SquidMode;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            MainMenuManager.FromKidsMode = true;
            PlayerDataController.Instance.playerData.LastUnlockedLevel_BattleMode += 1;
            PlayerDataController.Instance.playerData.currentSelectLevel_BattleMode += 1;

            MConstants.CurrentLevelNumber = PlayerDataController.Instance.playerData.currentSelectLevel_BattleMode;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            MainMenuManager.FromKidsMode = true;
            PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode1 += 1;
            PlayerDataController.Instance.playerData.currentSelectLevel_Mode1 += 1;

            MConstants.CurrentLevelNumber = PlayerDataController.Instance.playerData.currentSelectLevel_Mode1;
        }

        PlayerDataController.Instance.Save();

        HudMenuManager.instance.loading.SetActive(true);

        MainMenuManager.isGoToGrage = true;

        if (MConstants.CurrentLevelNumber >= MConstants.MAX_LEVELS)
        {
            MainMenuManager.isGoToGrage = false;
            MainMenuManager.isModeScreen = true;
        }

        Invoke("LoadLevel", 2f);
       // Application.LoadLevel("UIScene");
    }


    public void skipLevelNoClick()
    {
        SkipUI.SetActive(false);
    }

    public void NextLevel()
    {
        HudMenuManager.instance.loading.SetActive(true);

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            MainMenuManager.FromKidsMode = true;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert )
        {
            MainMenuManager.FromProMode = true;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            MainMenuManager.FromSquidMode = true;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            MainMenuManager.FromBattleFieldMode = true;
        }


        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            MConstants.CurrentLevelNumber = PlayerDataController.Instance.playerData.currentSelectLevel_Mode1;
            MConstants.MissionObjective =  LevelSelectionMenuManager.LevelObjectivesList[MConstants.CurrentLevelNumber - 1];
            MConstants.rewardGold = LevelSelectionMenuManager.LevelRewardsList[MConstants.CurrentLevelNumber - 1];
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
        {
            MConstants.CurrentLevelNumber = PlayerDataController.Instance.playerData.currentSelectLevel_Mode2;
            MConstants.MissionObjective =  LevelSelectionMenuManager.LevelObjectivesList[MConstants.CurrentLevelNumber - 1];
            MConstants.rewardGold = LevelSelectionMenuManager.LevelRewardsList[MConstants.CurrentLevelNumber - 1];
            
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            MConstants.CurrentLevelNumber = PlayerDataController.Instance.playerData.currentSelectLevel_Mode3;
            MConstants.MissionObjective =  LevelSelectionMenuManager.LevelObjectivesList[MConstants.CurrentLevelNumber - 1];
            MConstants.rewardGold = LevelSelectionMenuManager.LevelRewardsList[MConstants.CurrentLevelNumber - 1];
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            MConstants.CurrentLevelNumber = PlayerDataController.Instance.playerData.currentSelectLevel_SquidMode;
            MConstants.MissionObjective =  LevelSelectionMenuManager.LevelObjectivesList[MConstants.CurrentLevelNumber - 1];
            MConstants.rewardGold = LevelSelectionMenuManager.LevelRewardsList[MConstants.CurrentLevelNumber - 1];
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            MConstants.CurrentLevelNumber = PlayerDataController.Instance.playerData.currentSelectLevel_BattleMode;
            MConstants.MissionObjective =  LevelSelectionMenuManager.LevelObjectivesList[MConstants.CurrentLevelNumber - 1];
            MConstants.rewardGold = LevelSelectionMenuManager.LevelRewardsList[MConstants.CurrentLevelNumber - 1];
        }
        // Invoke(" Application.LoadLevel(SceneManager.GetActiveScene().buildIndex)", 2f);
        //Application.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        // StartCoroutine(LoadNextLevelScene());
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
        {
            if (MConstants.CurrentLevelNumber >= MConstants.MAX_LEVELS_Expert)
            {
                Invoke("LoadLevel", 2f);
            }
            else
            {
                LoadNextLevelScene();
            }
        }
        else
        {
            LoadNextLevelScene();
        }
        
        // MainMenuManager.isGoToGrage = true;
        // LevelSelectionMenuManager.IsNextLevel = true;
        // Application.LoadLevel("UIScene");
    }

    public void closeRateUs()
    {
        RateUsUI.SetActive(false);
    }

    public void RateUSURL()
    {
        RateUsUI.SetActive(false);
        PlayerDataController.Instance.playerData.isRateUSDone = true;
        Application.OpenURL(MConstants.RATE_US);
    }

    public void NeverShoRateUS()
    {
        RateUsUI.SetActive(false);
        PlayerDataController.Instance.playerData.isRateUSDone = true;
    }

    void LoadLevelScene()
    {
       // yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void LoadNextLevelScene()
    {
      //  yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}