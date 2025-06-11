using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModeSelectionMenu : MonoBehaviour
{
    public GameObject endlessModeInfo;
    public GameObject lockOverlay;
    public GameObject thirdModeInfo;
    public GameObject thirdLockOverlay;
    public GameObject AlertViewForAd;
    
    [Header("Tutorial"), Space(5)]
    public GameObject tutorialBg;
    public GameObject clickImage;

    void OnEnable()
    {
        if (PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode1 > 5)
        {
            lockOverlay.SetActive(false);
        }
        
        if (PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode2 > 10)
        {
            thirdLockOverlay.SetActive(false);
        }
        
        if (!PlayerDataController.Instance.playerData.firstTimeTutorial)
        {
            tutorialBg.SetActive(true);
            clickImage.SetActive(true);
        }

        /*if (AdsManager.instance != null)
        {
        //    AdsManager.instance.RequestBanner();
        }*/

        /*if (AdsManager.instance.isMenuAdReadyToShow())
        {
            AlertViewForAd.SetActive(true);
            Invoke("ShowAdAfterAlertView", 0.4f);
        }*/
    }

    public void ShowAdAfterAlertView()
    {
        /*AdsManager.instance.showStaticIntertial();
        AlertViewForAd.SetActive(false);*/
    }

    public void showEndlessLockInfo()
    {
        endlessModeInfo.SetActive(true);
    }
    public void hideEndlessLockInfo()
    {
        endlessModeInfo.SetActive(false);
    }
    
    
    public void Show3rdModeLockedInfo()
    {
        thirdModeInfo.SetActive(true);
    }
    public void hideThirdLockInfo()
    {
        thirdModeInfo.SetActive(false);
    }

    public void SelectMode(int id)
    {
        LoadLevel(id);
        tutorialBg.SetActive(false);
        clickImage.SetActive(false);
        
        //UnityAnalyticsScript.instance.AddUnityEvent("SelectMode", new Dictionary<string, object>
        //{
        //    {"GameMode", "" + id}
        //});
    }

    void LoadLevel(int id)
    {
        PlayerDataController.Instance.playerData.CurrentEnvironment = id;

        switch (id)
        {
            case 1:
                MConstants.CurrentGameMode = MConstants.GAME_MODES.MODE1;
                MainMenuManager.Instance.showMenu(MenuNames.LEVEL_SELECTION);
                //MConstants.CurrentLevelNumber = 1;
                //Application.LoadLevelAsync(MConstants.Env_1);


                break;

            case 2:
                MConstants.CurrentGameMode = MConstants.GAME_MODES.MODE2_Expert;

                MainMenuManager.Instance.showMenu(MenuNames.LEVEL_SELECTION);

                break;
            case 3:


                MConstants.CurrentGameMode = MConstants.GAME_MODES.MODE3_Zombie;

                MainMenuManager.Instance.showMenu(MenuNames.LEVEL_SELECTION);

                break;
            case 4:


                MConstants.CurrentGameMode = MConstants.GAME_MODES.SURVIVAL_MODE;

                MainMenuManager.Instance.showSubMenu(SubMenuNames.SECODORY_GUN_SELECTION);

                break;
        }
    }
}