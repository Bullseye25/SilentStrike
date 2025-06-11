using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CompleteProject;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuManager : MonoBehaviour {
	PlayerDataSerializeable playerData;
	public MainMenuManager menuManger;
	public static LevelSelectionMenuManager Instance;
    public ScrollRect ScroolViewContant;
    /*public GameObject unlockAllLevelsButton;*/
    public GameObject levelsPage1, levelsPage2, levelsPage3;
    private int _currentLevelsPage;
    public GameObject AlertViewForAd;

    [Header("Level Information Holders"), Space(10)]
    public Text modeName;
    public Text objectiveText;
    public Text rewardText;
    public Image recommendedGunImage;
    public GameObject playBtn;

    [Header("Recommended Gun Check"), Space(10)]
    public GameObject gunCheckPanel;
    public Text gunCheckText;
    public GameObject equipToPrimaryButton;
    public GameObject weaponShopBtn;
    public Image warningMark;
    public Sprite exclamationMark, tickMark;
    public GameObject purchaseGunTutBg;

    [Header("Warning Panel"), Space(10)]
    public Image warningGunImage;
    public Text warningGunText;
    
    [Header("Tutorial"), Space(5)]
    public GameObject tutorialBg;
    public GameObject clickImage;
    
    public Sprite[] playerGunsImages;
    public List<LevelUIController> levelsList;

    public static bool IsNextLevel;
    private static bool _adShown;
    
    public static List<string> LevelObjectivesList = new List<string>();
    public static List<int> LevelRewardsList = new List<int>();
    
    void Awake()
    {
        Instance = this;
        Time.timeScale = 1;
        playerData = PlayerDataController.Instance.playerData;
        /*unlockAllLevelsButton.SetActive(!playerData.unlockedAllLevels);*/
       
        if (!PlayerDataController.Instance.playerData.firstTimeTutorial)
        {
            tutorialBg.SetActive(true);
            clickImage.SetActive(true);
        }
        
        if (!playerData.buyShotgunTutorial && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1 && playerData.currentSelectLevel_Mode1 == 4)
        {
            purchaseGunTutBg.SetActive(true);
            weaponShopBtn.GetComponent<Animation>().enabled = true;
        }
    }

    void OnEnable ()
    {
        MConstants.HavingTemporaryGun = false;
        playerData = PlayerDataController.Instance.playerData;
        SetModeName();

        for (int i = 0; i < levelsList.Count; i++)
        {
            levelsList[i].gameObject.SetActive(true);
        }
        
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            for (int i = MConstants.MAX_LEVELS_ZOMBIE; i < levelsList.Count; i++)
            {
                levelsList[i].gameObject.SetActive(false);
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
        {
            for (int i = MConstants.MAX_LEVELS_Expert; i < levelsList.Count; i++)
            {
                levelsList[i].gameObject.SetActive(false);
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            for (int i = MConstants.MAX_LEVELS_SQUID; i < levelsList.Count; i++)
            {
                levelsList[i].gameObject.SetActive(false);
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            for (int i = MConstants.MAX_LEVELS_BATTLE; i < levelsList.Count; i++)
            {
                levelsList[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < levelsList.Count; i++)
            {
                levelsList[i].gameObject.SetActive(true);
            }
        }
        
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            if (playerData.currentSelectLevel_BattleMode >= MConstants.MAX_LEVELS_BATTLE)
                playerData.currentSelectLevel_BattleMode = MConstants.MAX_LEVELS_BATTLE;
            
            MConstants.CurrentLevelNumber = playerData.currentSelectLevel_BattleMode;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            if (playerData.currentSelectLevel_SquidMode >= MConstants.MAX_LEVELS_SQUID)
                playerData.currentSelectLevel_SquidMode = MConstants.MAX_LEVELS_SQUID;
            
            MConstants.CurrentLevelNumber = playerData.currentSelectLevel_SquidMode;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            if (playerData.currentSelectLevel_Mode3 >= MConstants.MAX_LEVELS_ZOMBIE)
                playerData.currentSelectLevel_Mode3 = MConstants.MAX_LEVELS_ZOMBIE;
            
            MConstants.CurrentLevelNumber = playerData.currentSelectLevel_Mode3;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
        {
            if (playerData.currentSelectLevel_Mode2 >= MConstants.MAX_LEVELS_Expert)
                playerData.currentSelectLevel_Mode2 = MConstants.MAX_LEVELS_Expert;
            
            MConstants.CurrentLevelNumber = playerData.currentSelectLevel_Mode2;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            if (playerData.currentSelectLevel_Mode1 >= MConstants.MAX_LEVELS)
                playerData.currentSelectLevel_Mode1 = MConstants.MAX_LEVELS;
            
            MConstants.CurrentLevelNumber = playerData.currentSelectLevel_Mode1;
        }
        if (MConstants.CurrentLevelNumber < 4 )
        {
            // ScroolViewContant.horizontalNormalizedPosition = (float)((float)MConstants.CurrentLevelNumber / (float)MConstants.MAX_LEVELS) - 0.1f;
        }
        else if (MConstants.CurrentLevelNumber >= 4 && MConstants.CurrentLevelNumber <= 7)
        {
            // ScroolViewContant.horizontalNormalizedPosition = (float)((float)MConstants.CurrentLevelNumber / (float)MConstants.MAX_LEVELS) - 0.1f;
        }
        else if (MConstants.CurrentLevelNumber >= 8)
        {
            // ScroolViewContant.horizontalNormalizedPosition = (float)((float)MConstants.CurrentLevelNumber / (float)MConstants.MAX_LEVELS);
        }

        // Invoke(nameof(SetLevelScreenPages), 1.2f);
        SetLevelScreenPages();
        UpdateLevelInfo();
        
        
       /* if (AdsManager.instance.isMenuAdReadyToShow() && !_adShown)
        {
            _adShown = true;
            AlertViewForAd.SetActive(true);
            Invoke("ShowAdAfterAlertView", 0.4f);
        }*/

        if (IsNextLevel)
        {
            IsNextLevel = false;
            PlayButton();
        }
    }

  /*  public void ShowAdAfterAlertView()
    {
        AdsManager.instance.showStaticIntertial();
        AlertViewForAd.SetActive(false);
    }*/

    void SetLevelScreenPages()
    {
        if (MConstants.CurrentLevelNumber < 16)
        {
            levelsPage1.GetComponent<Animation>().Play("LevelsInAnimation");
            levelsPage2.GetComponent<Animation>().Play("LevelsOutAnimation");
            levelsPage3.GetComponent<Animation>().Play("LevelsOutAnimation");
            _currentLevelsPage = 1;
            // levelsPage1.SetActive(true);
            // levelsPage2.SetActive(false);
            // levelsPage3.SetActive(false);
        }
        else if (MConstants.CurrentLevelNumber > 15 && MConstants.CurrentLevelNumber < 31)
        {
            levelsPage1.GetComponent<Animation>().Play("LevelsOutAnimation");
            levelsPage2.GetComponent<Animation>().Play("LevelsInAnimation");
            levelsPage3.GetComponent<Animation>().Play("LevelsOutAnimation");
            _currentLevelsPage = 2;
            // levelsPage1.SetActive(false);
            // levelsPage2.SetActive(true);
            // levelsPage3.SetActive(false);
        }
        else if (MConstants.CurrentLevelNumber > 30)
        {
            levelsPage1.GetComponent<Animation>().Play("LevelsOutAnimation");
            levelsPage2.GetComponent<Animation>().Play("LevelsOutAnimation");
            levelsPage3.GetComponent<Animation>().Play("LevelsInAnimation");
            _currentLevelsPage = 3;
            // levelsPage1.SetActive(false);
            // levelsPage2.SetActive(false);
            // levelsPage3.SetActive(true);
        }
    }

	public void onLevelClick(LevelUIController LUIControllr)
    {
        if (LUIControllr.levelID <= playerData.LastUnlockedLevel_Mode1 && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            playBtn.GetComponent<Button>().interactable = true;
            playerData.currentSelectLevel_Mode1 = LUIControllr.levelID;
            MConstants.CurrentLevelNumber = LUIControllr.levelID;
            MConstants.rewardGold = LUIControllr.rewardAmount1;
        }
        else if (LUIControllr.levelID <= playerData.LastUnlockedLevel_Mode2 && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert )
        {
            playBtn.GetComponent<Button>().interactable = true;
            playerData.currentSelectLevel_Mode2 = LUIControllr.levelID;
            MConstants.CurrentLevelNumber = LUIControllr.levelID;
            MConstants.rewardGold = LUIControllr.rewardAmount2;
        }
        else if (LUIControllr.levelID <= playerData.LastUnlockedLevel_Mode3 && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            playBtn.GetComponent<Button>().interactable = true;
            playerData.currentSelectLevel_Mode3 = LUIControllr.levelID;
            MConstants.CurrentLevelNumber = LUIControllr.levelID;
            MConstants.rewardGold = LUIControllr.rewardAmount3;
        }
        else if (LUIControllr.levelID <= playerData.LastUnlockedLevel_SquidMode && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            playBtn.GetComponent<Button>().interactable = true;
            playerData.currentSelectLevel_SquidMode = LUIControllr.levelID;
            MConstants.CurrentLevelNumber = LUIControllr.levelID;
            MConstants.rewardGold = LUIControllr.rewardAmountSquid;
        }
        else if (LUIControllr.levelID <= playerData.LastUnlockedLevel_BattleMode && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            playBtn.GetComponent<Button>().interactable = true;
            playerData.currentSelectLevel_BattleMode = LUIControllr.levelID;
            MConstants.CurrentLevelNumber = LUIControllr.levelID;
            MConstants.rewardGold = LUIControllr.rewardAmountBattleField;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1 && LUIControllr.levelID == playerData.LastUnlockedLevel_Mode1 + 1)
        {
            // playBtn.GetComponent<Button>().interactable = false;
            //   menuManger.showSubMenu(SubMenuNames.LEVEL_UNLOCK_POPUP);
            UnlockLevelButtonClik();
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert && LUIControllr.levelID == playerData.LastUnlockedLevel_Mode2 + 1)
        {
            // playBtn.GetComponent<Button>().interactable = false;
            //  menuManger.showSubMenu(SubMenuNames.LEVEL_UNLOCK_POPUP);
            UnlockLevelButtonClik();
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie && LUIControllr.levelID == playerData.LastUnlockedLevel_Mode3 + 1)
        {
            // playBtn.GetComponent<Button>().interactable = false;
          //  menuManger.showSubMenu(SubMenuNames.LEVEL_UNLOCK_POPUP);
            UnlockLevelButtonClik();
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid && LUIControllr.levelID == playerData.LastUnlockedLevel_SquidMode + 1)
        {
            // playBtn.GetComponent<Button>().interactable = false;
            //   menuManger.showSubMenu(SubMenuNames.LEVEL_UNLOCK_POPUP);
            UnlockLevelButtonClik();
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD && LUIControllr.levelID == playerData.LastUnlockedLevel_BattleMode + 1)
        {
            // playBtn.GetComponent<Button>().interactable = false;
            // menuManger.showSubMenu(SubMenuNames.LEVEL_UNLOCK_POPUP);
            UnlockLevelButtonClik();
        }
        
        UpdateLevelInfo();
        //RewardCashStart;
        // MConstants.EndRandom = LUIControllr.RewardCashEnd;

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            foreach (var levelUIController in levelsList)
            {
                levelUIController.RefreshState();
            }
        }
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
        {
            for (int i = 0; i < MConstants.MAX_LEVELS_Expert; i++)
            {
                levelsList[i].RefreshState();
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            for (int i = 0; i < MConstants.MAX_LEVELS_ZOMBIE; i++)
            {
                levelsList[i].RefreshState();
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            for (int i = 0; i < MConstants.MAX_LEVELS_SQUID; i++)
            {
                levelsList[i].RefreshState();
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            for (int i = 0; i < MConstants.MAX_LEVELS_BATTLE; i++)
            {
                levelsList[i].RefreshState();
            }
        }
    }

    private void UpdateLevelInfo()
    {
        
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            objectiveText.text = levelsList[playerData.currentSelectLevel_Mode1 - 1].objective1;
            MConstants.MissionObjective = levelsList[playerData.currentSelectLevel_Mode1 - 1].objective1;
            rewardText.text = levelsList[playerData.currentSelectLevel_Mode1 - 1].rewardAmount1.ToString();
            MConstants.rewardGold = levelsList[playerData.currentSelectLevel_Mode1 - 1].rewardAmount1;
            recommendedGunImage.sprite = playerGunsImages[levelsList[playerData.currentSelectLevel_Mode1 - 1].recommendedGun1];
            warningGunImage.sprite = playerGunsImages[levelsList[playerData.currentSelectLevel_Mode1 - 1].recommendedGun1];
            CheckRecommendedWeapon(levelsList[playerData.currentSelectLevel_Mode1 - 1]);
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
        {
            objectiveText.text = levelsList[playerData.currentSelectLevel_Mode2 - 1].objective2;
            MConstants.MissionObjective = levelsList[playerData.currentSelectLevel_Mode2 - 1].objective2;
            rewardText.text = levelsList[playerData.currentSelectLevel_Mode2 - 1].rewardAmount2.ToString();
            MConstants.rewardGold = levelsList[playerData.currentSelectLevel_Mode2 - 1].rewardAmount2;
            recommendedGunImage.sprite = playerGunsImages[levelsList[playerData.currentSelectLevel_Mode2 - 1].recommendedGun2];
            warningGunImage.sprite = playerGunsImages[levelsList[playerData.currentSelectLevel_Mode2 - 1].recommendedGun2];
            CheckRecommendedWeapon(levelsList[playerData.currentSelectLevel_Mode2 - 1]);
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            objectiveText.text = levelsList[playerData.currentSelectLevel_Mode3 - 1].objective3;
            MConstants.MissionObjective = levelsList[playerData.currentSelectLevel_Mode3 - 1].objective3;
            rewardText.text = levelsList[playerData.currentSelectLevel_Mode3 - 1].rewardAmount3.ToString();
            MConstants.rewardGold = levelsList[playerData.currentSelectLevel_Mode3 - 1].rewardAmount3;
            recommendedGunImage.sprite = playerGunsImages[levelsList[playerData.currentSelectLevel_Mode3 - 1].recommendedGun3];
            warningGunImage.sprite = playerGunsImages[levelsList[playerData.currentSelectLevel_Mode3 - 1].recommendedGun3];
            CheckRecommendedWeapon(levelsList[playerData.currentSelectLevel_Mode3 - 1]);
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            objectiveText.text = levelsList[playerData.currentSelectLevel_SquidMode - 1].objectiveSquid;
            MConstants.MissionObjective = levelsList[playerData.currentSelectLevel_SquidMode - 1].objectiveSquid;
            rewardText.text = levelsList[playerData.currentSelectLevel_SquidMode - 1].rewardAmountSquid.ToString();
            MConstants.rewardGold = levelsList[playerData.currentSelectLevel_SquidMode - 1].rewardAmountSquid;
            recommendedGunImage.sprite = playerGunsImages[levelsList[playerData.currentSelectLevel_SquidMode - 1].recommendedGunSquid];
            warningGunImage.sprite = playerGunsImages[levelsList[playerData.currentSelectLevel_SquidMode - 1].recommendedGunSquid];
            CheckRecommendedWeapon(levelsList[playerData.currentSelectLevel_SquidMode - 1]);
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            objectiveText.text = levelsList[playerData.currentSelectLevel_BattleMode - 1].objectiveBattleField;
            MConstants.MissionObjective = levelsList[playerData.currentSelectLevel_BattleMode - 1].objectiveBattleField;
            rewardText.text = levelsList[playerData.currentSelectLevel_BattleMode - 1].rewardAmountBattleField.ToString();
            MConstants.rewardGold = levelsList[playerData.currentSelectLevel_BattleMode - 1].rewardAmountBattleField;
            recommendedGunImage.sprite = playerGunsImages[levelsList[playerData.currentSelectLevel_BattleMode - 1].recommendedGunBattleField];
            warningGunImage.sprite = playerGunsImages[levelsList[playerData.currentSelectLevel_BattleMode - 1].recommendedGunBattleField];
            CheckRecommendedWeapon(levelsList[playerData.currentSelectLevel_BattleMode - 1]);
        }
    }
    
    private void SetModeName()
    {
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
            modeName.text = MConstants.Mode1Name;
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert) 
            modeName.text = MConstants.Mode2Name;
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
            modeName.text = MConstants.Mode3Name;
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
            modeName.text = MConstants.SquidModeName;
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
            modeName.text = MConstants.BattleFieldModeName;
    }
    public void CheckRecommendedWeapon(LevelUIController LUIControllr)
    {
        switch (MConstants.CurrentGameMode)
        {
            case MConstants.GAME_MODES.MODE1:
                if (LUIControllr.recommendedGun1 == playerData.CurrentSelectedPrimaryGun)
                {
                    gunCheckPanel.SetActive(false);
                    warningMark.sprite = tickMark;
                }
                else if (LUIControllr.recommendedGun1 == playerData.CurrentSelectedSecondaryGun &&
                         !playerData.gunsList[playerData.CurrentSelectedSecondaryGun].isLocked)
                {
                    gunCheckPanel.SetActive(true);
                    gunCheckText.text = "Switch to primary gun.";
                    equipToPrimaryButton.SetActive(true);
                    weaponShopBtn.SetActive(false);
                    warningMark.sprite = exclamationMark;
                }
                else
                {
                    gunCheckPanel.SetActive(true);
                    gunCheckText.text = playerData.gunsList[LUIControllr.recommendedGun1].Name + " recommended.";
                    warningGunText.text = playerData.gunsList[LUIControllr.recommendedGun1].Name +
                                          " is recommended for this mission.";
                    equipToPrimaryButton.SetActive(false);
                    weaponShopBtn.SetActive(true);
                    warningMark.sprite = exclamationMark;
                }
                break;
            
            case MConstants.GAME_MODES.MODE2_Expert:
                if (LUIControllr.recommendedGun2 == playerData.CurrentSelectedPrimaryGun)
                {
                    gunCheckPanel.SetActive(false);
                    warningMark.sprite = tickMark;
                }
                else if (LUIControllr.recommendedGun2 == playerData.CurrentSelectedSecondaryGun &&
                         !playerData.gunsList[playerData.CurrentSelectedSecondaryGun].isLocked)
                {
                    gunCheckPanel.SetActive(true);
                    gunCheckText.text = "Switch to primary gun.";
                    equipToPrimaryButton.SetActive(true);
                    weaponShopBtn.SetActive(false);
                    warningMark.sprite = exclamationMark;
                }
                else
                {
                    gunCheckPanel.SetActive(true);
                    gunCheckText.text = playerData.gunsList[LUIControllr.recommendedGun2].Name + " recommended.";
                    warningGunText.text = playerData.gunsList[LUIControllr.recommendedGun2].Name +
                                          " is recommended for this mission.";
                    equipToPrimaryButton.SetActive(false);
                    weaponShopBtn.SetActive(true);
                    warningMark.sprite = exclamationMark;
                }
                break;
            
            case MConstants.GAME_MODES.MODE3_Zombie:
                if (LUIControllr.recommendedGun3 == 5 || LUIControllr.recommendedGun3 == 6)
                {
                    gunCheckPanel.SetActive(false);
                    warningMark.sprite = tickMark;
                }

                else if (LUIControllr.recommendedGun3 == playerData.CurrentSelectedPrimaryGun)
                {
                    gunCheckPanel.SetActive(false);
                    warningMark.sprite = tickMark;
                }
                else if (LUIControllr.recommendedGun3 == playerData.CurrentSelectedSecondaryGun &&
                         !playerData.gunsList[playerData.CurrentSelectedSecondaryGun].isLocked)
                {
                    gunCheckPanel.SetActive(true);
                    gunCheckText.text = "Switch to primary gun.";
                    equipToPrimaryButton.SetActive(true);
                    weaponShopBtn.SetActive(false);
                    warningMark.sprite = exclamationMark;
                }
                else
                {
                    gunCheckPanel.SetActive(true);
                    gunCheckText.text = playerData.gunsList[LUIControllr.recommendedGun3].Name + " recommended.";
                    warningGunText.text = playerData.gunsList[LUIControllr.recommendedGun3].Name +
                                          " is recommended for this mission.";
                    equipToPrimaryButton.SetActive(false);
                    weaponShopBtn.SetActive(true);
                    warningMark.sprite = exclamationMark;
                }
                break;
            
            case MConstants.GAME_MODES.MODE4_Squid:
                if (LUIControllr.recommendedGunSquid == 5 || LUIControllr.recommendedGunSquid == 6)
                {
                    gunCheckPanel.SetActive(false);
                    warningMark.sprite = tickMark;
                }

                else if (LUIControllr.recommendedGunSquid == playerData.CurrentSelectedPrimaryGun)
                {
                    gunCheckPanel.SetActive(false);
                    warningMark.sprite = tickMark;
                }
                else if (LUIControllr.recommendedGunSquid == playerData.CurrentSelectedSecondaryGun &&
                         !playerData.gunsList[playerData.CurrentSelectedSecondaryGun].isLocked)
                {
                    gunCheckPanel.SetActive(true);
                    gunCheckText.text = "Switch to primary gun.";
                    equipToPrimaryButton.SetActive(true);
                    weaponShopBtn.SetActive(false);
                    warningMark.sprite = exclamationMark;
                }
                else
                {
                    gunCheckPanel.SetActive(true);
                    gunCheckText.text = playerData.gunsList[LUIControllr.recommendedGunSquid].Name + " recommended.";
                    warningGunText.text = playerData.gunsList[LUIControllr.recommendedGunSquid].Name +
                                          " is recommended for this mission.";
                    equipToPrimaryButton.SetActive(false);
                    weaponShopBtn.SetActive(true);
                    warningMark.sprite = exclamationMark;
                }
                break;
            
            case MConstants.GAME_MODES.MODE5_BATTLEFIELD:
                if (LUIControllr.recommendedGunBattleField == 5 || LUIControllr.recommendedGunBattleField == 6)
                {
                    gunCheckPanel.SetActive(false);
                    warningMark.sprite = tickMark;
                }

                else if (LUIControllr.recommendedGunBattleField == playerData.CurrentSelectedPrimaryGun)
                {
                    gunCheckPanel.SetActive(false);
                    warningMark.sprite = tickMark;
                }
                else if (LUIControllr.recommendedGunBattleField == playerData.CurrentSelectedSecondaryGun &&
                         !playerData.gunsList[playerData.CurrentSelectedSecondaryGun].isLocked)
                {
                    gunCheckPanel.SetActive(true);
                    gunCheckText.text = "Switch to primary gun.";
                    equipToPrimaryButton.SetActive(true);
                    weaponShopBtn.SetActive(false);
                    warningMark.sprite = exclamationMark;
                }
                else
                {
                    gunCheckPanel.SetActive(true);
                    gunCheckText.text = playerData.gunsList[LUIControllr.recommendedGunBattleField].Name + " recommended.";
                    warningGunText.text = playerData.gunsList[LUIControllr.recommendedGunBattleField].Name +
                                          " is recommended for this mission.";
                    equipToPrimaryButton.SetActive(false);
                    weaponShopBtn.SetActive(true);
                    warningMark.sprite = exclamationMark;
                }
                break;
        }
    }

    public void SwitchToPrimary()
    {
        var temp = playerData.CurrentSelectedSecondaryGun;
        playerData.CurrentSelectedSecondaryGun = playerData.CurrentSelectedPrimaryGun;
        playerData.CurrentSelectedPrimaryGun = temp;
        PlayerDataController.Instance.Save();
        gunCheckPanel.SetActive(false);
        warningMark.sprite = tickMark;
    }

    public void ShowGunMenuWithCurrentGun()
    {
        MConstants.ComingFromLevels = true;
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
            MConstants.ShowRecommendedWeaponNo = levelsList[playerData.currentSelectLevel_Mode1 - 1].recommendedGun1;
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
            MConstants.ShowRecommendedWeaponNo = levelsList[playerData.currentSelectLevel_Mode2 - 1].recommendedGun2;
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
            MConstants.ShowRecommendedWeaponNo = levelsList[playerData.currentSelectLevel_Mode3 - 1].recommendedGun3;
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
            MConstants.ShowRecommendedWeaponNo = levelsList[playerData.currentSelectLevel_SquidMode - 1].recommendedGunSquid;
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
            MConstants.ShowRecommendedWeaponNo = levelsList[playerData.currentSelectLevel_BattleMode - 1].recommendedGunBattleField;
        MainMenuManager.Instance.showMenu(MenuNames.GUN_MENU);
    }
    
	public void UnlockLevelButtonClik(){
		/*AdsManager.isUnLockLevel = true;
		AdsManager.instance.ShowRewardedAdd ();*/
    }

	public void UnlockLevel(){
        switch (MConstants.CurrentGameMode)
        {
            case MConstants.GAME_MODES.MODE1:
                PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode1 += 1;
                PlayerDataController.Instance.playerData.currentSelectLevel_Mode1 = PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode1;
                PlayerDataController.Instance.Save();
                MConstants.CurrentLevelNumber = playerData.LastUnlockedLevel_Mode1;
            
                objectiveText.text = levelsList[MConstants.CurrentLevelNumber - 1].objective1;
                MConstants.MissionObjective = levelsList[playerData.currentSelectLevel_Mode1 - 1].objective1;
                rewardText.text = levelsList[MConstants.CurrentLevelNumber - 1].rewardAmount1.ToString();
                recommendedGunImage.sprite = playerGunsImages[levelsList[MConstants.CurrentLevelNumber - 1].recommendedGun1];
                warningGunImage.sprite = playerGunsImages[levelsList[MConstants.CurrentLevelNumber - 1].recommendedGun1];
                CheckRecommendedWeapon(levelsList[MConstants.CurrentLevelNumber - 1]);
                break;
            case MConstants.GAME_MODES.MODE2_Expert:
                PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode2 += 1;
                PlayerDataController.Instance.playerData.currentSelectLevel_Mode2 =
                    PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode2;//+= 1;
                PlayerDataController.Instance.Save();
                MConstants.CurrentLevelNumber = playerData.LastUnlockedLevel_Mode2;
                
                objectiveText.text = levelsList[MConstants.CurrentLevelNumber - 1].objective2;
                MConstants.MissionObjective = levelsList[playerData.currentSelectLevel_Mode2 - 1].objective2;
                rewardText.text = levelsList[MConstants.CurrentLevelNumber - 1].rewardAmount2.ToString();
                recommendedGunImage.sprite = playerGunsImages[levelsList[MConstants.CurrentLevelNumber - 1].recommendedGun2];
                warningGunImage.sprite = playerGunsImages[levelsList[MConstants.CurrentLevelNumber - 1].recommendedGun2];
                CheckRecommendedWeapon(levelsList[MConstants.CurrentLevelNumber - 1]);
                break;
            case MConstants.GAME_MODES.MODE3_Zombie:
                PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode3 += 1;
                PlayerDataController.Instance.playerData.currentSelectLevel_Mode3 = PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode3;// += 1;
                PlayerDataController.Instance.Save();
                MConstants.CurrentLevelNumber = playerData.LastUnlockedLevel_Mode3;
                
                objectiveText.text = levelsList[MConstants.CurrentLevelNumber - 1].objective3;
                MConstants.MissionObjective = levelsList[playerData.currentSelectLevel_Mode3 - 1].objective3;
                rewardText.text = levelsList[MConstants.CurrentLevelNumber - 1].rewardAmount3.ToString();
                recommendedGunImage.sprite = playerGunsImages[levelsList[MConstants.CurrentLevelNumber - 1].recommendedGun3];
                warningGunImage.sprite = playerGunsImages[levelsList[MConstants.CurrentLevelNumber - 1].recommendedGun3];
                CheckRecommendedWeapon(levelsList[MConstants.CurrentLevelNumber - 1]);
                break;
            case MConstants.GAME_MODES.MODE4_Squid:
                PlayerDataController.Instance.playerData.LastUnlockedLevel_SquidMode += 1;
                PlayerDataController.Instance.playerData.currentSelectLevel_SquidMode = PlayerDataController.Instance.playerData.LastUnlockedLevel_SquidMode;// += 1;
                PlayerDataController.Instance.Save();
                MConstants.CurrentLevelNumber = playerData.LastUnlockedLevel_SquidMode;
                
                objectiveText.text = levelsList[MConstants.CurrentLevelNumber - 1].objectiveSquid;
                MConstants.MissionObjective = levelsList[playerData.currentSelectLevel_SquidMode - 1].objectiveSquid;
                rewardText.text = levelsList[MConstants.CurrentLevelNumber - 1].rewardAmountSquid.ToString();
                recommendedGunImage.sprite = playerGunsImages[levelsList[MConstants.CurrentLevelNumber - 1].recommendedGunSquid];
                warningGunImage.sprite = playerGunsImages[levelsList[MConstants.CurrentLevelNumber - 1].recommendedGunSquid];
                CheckRecommendedWeapon(levelsList[MConstants.CurrentLevelNumber - 1]);
                break;
            case MConstants.GAME_MODES.MODE5_BATTLEFIELD:
                PlayerDataController.Instance.playerData.LastUnlockedLevel_BattleMode += 1;
                PlayerDataController.Instance.playerData.currentSelectLevel_BattleMode = PlayerDataController.Instance.playerData.LastUnlockedLevel_BattleMode;// += 1;
                PlayerDataController.Instance.Save();
                MConstants.CurrentLevelNumber = playerData.LastUnlockedLevel_BattleMode;
                
                objectiveText.text = levelsList[MConstants.CurrentLevelNumber - 1].objectiveBattleField;
                MConstants.MissionObjective = levelsList[playerData.currentSelectLevel_BattleMode - 1].objectiveBattleField;
                rewardText.text = levelsList[MConstants.CurrentLevelNumber - 1].rewardAmountBattleField.ToString();
                recommendedGunImage.sprite = playerGunsImages[levelsList[MConstants.CurrentLevelNumber - 1].recommendedGunBattleField];
                warningGunImage.sprite = playerGunsImages[levelsList[MConstants.CurrentLevelNumber - 1].recommendedGunBattleField];
                CheckRecommendedWeapon(levelsList[MConstants.CurrentLevelNumber - 1]);
                break;
        }
        
        
        foreach (var levelUIController in levelsList)
        {
            levelUIController.RefreshState();
        }
        
        
        playBtn.GetComponent<Button>().interactable = true;
    }
    
    public void PlayButton()
    {
        /*if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1 && CheckGunWarning(levelsList[playerData.currentSelectLevel_Mode1 - 1].recommendedGun1))
        { 
            MainMenuManager.Instance.showSubMenu(SubMenuNames.GUN_WARNING); 
            return;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2 && CheckGunWarning(levelsList[playerData.currentSelectLevel_Mode2 - 1].recommendedGun2))
        {
            MainMenuManager.Instance.showSubMenu(SubMenuNames.GUN_WARNING);
            return;
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3 && CheckGunWarning(levelsList[playerData.currentSelectLevel_Mode3 - 1].recommendedGun3))
        {
            MainMenuManager.Instance.showSubMenu(SubMenuNames.GUN_WARNING);
            return;
        }*/

        if (!PlayerDataController.Instance.playerData.firstTimeTutorial)
        {
            tutorialBg.SetActive(false);
            clickImage.SetActive(false);
            PlayerDataController.Instance.playerData.firstTimeTutorial = true;
            PlayerDataController.Instance.Save();
        }

        PlayerDataController.Instance.playerData.CurrentEnvironment = 1;
        PlayerDataSerializeable pDta = PlayerDataController.Instance.playerData;
        string env = "";
        if (MConstants.GAME_MODES.MODE1 == MConstants.CurrentGameMode)
        {
            env = MConstants.Env_1;
            
            LevelObjectivesList.Clear();
            LevelRewardsList.Clear();
            foreach (var level in levelsList)
            {
                 LevelObjectivesList.Add(level.objective1);
                 LevelRewardsList.Add(level.rewardAmount1);
            }
        }
        else if (MConstants.GAME_MODES.MODE2_Expert == MConstants.CurrentGameMode)
        {
            env = MConstants.Env_2;
            
            LevelObjectivesList.Clear();
            LevelRewardsList.Clear();
            foreach (var level in levelsList)
            {
                LevelObjectivesList.Add(level.objective2);
                LevelRewardsList.Add(level.rewardAmount2);
            }
        }
        else if (MConstants.GAME_MODES.MODE3_Zombie == MConstants.CurrentGameMode)
        {
            env = MConstants.Env_3;
            
            LevelObjectivesList.Clear();
            LevelRewardsList.Clear();
            foreach (var level in levelsList)
            {
                LevelObjectivesList.Add(level.objective3);
                LevelRewardsList.Add(level.rewardAmount3);
            }
        }
        else if (MConstants.GAME_MODES.MODE4_Squid == MConstants.CurrentGameMode)
        {
            env = MConstants.Env_4_Squid;
            
            LevelObjectivesList.Clear();
            LevelRewardsList.Clear();
            foreach (var level in levelsList)
            {
                LevelObjectivesList.Add(level.objectiveSquid);
                LevelRewardsList.Add(level.rewardAmountSquid);
            }
        }
        else if (MConstants.GAME_MODES.MODE5_BATTLEFIELD == MConstants.CurrentGameMode)
        {
            env = MConstants.Env_5_BattleField;
            
            LevelObjectivesList.Clear();
            LevelRewardsList.Clear();
            foreach (var level in levelsList)
            {
                LevelObjectivesList.Add(level.objectiveBattleField);
                LevelRewardsList.Add(level.rewardAmountBattleField);
            }
        }
        else if (MConstants.GAME_MODES.SURVIVAL_MODE == MConstants.CurrentGameMode)
        {
            env = MConstants.Env_Survival;
        }

        else if (MConstants.GAME_MODES.ENDLESS_MODE == MConstants.CurrentGameMode)
        {
            env = MConstants.Env_Endless;
        }

        MainMenuManager.Instance.showSubMenu(SubMenuNames.LOADING);
        StartCoroutine(LoadScene(env));
    }

    IEnumerator LoadScene(string env)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(env,LoadSceneMode.Single);
        //Application.LoadLevelAsync(env);
    }
    bool CheckGunWarning(int recommendedGunNo)
    {
        if (recommendedGunNo == playerData.CurrentSelectedPrimaryGun)
        {
            return false;
        }
        else if (recommendedGunNo == playerData.CurrentSelectedSecondaryGun && !playerData.gunsList[playerData.CurrentSelectedSecondaryGun].isLocked)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AcceptTheChallengeButton()
    {
        PlayerDataController.Instance.playerData.CurrentEnvironment = 1;
        PlayerDataSerializeable pDta = PlayerDataController.Instance.playerData;
        string env = "";
        if (MConstants.GAME_MODES.MODE1 == MConstants.CurrentGameMode)
        {
            env = MConstants.Env_1;
        }
        else if (MConstants.GAME_MODES.MODE2_Expert == MConstants.CurrentGameMode)
        {
            env = MConstants.Env_2;
        }
        else if (MConstants.GAME_MODES.MODE3_Zombie == MConstants.CurrentGameMode)
        {
            env = MConstants.Env_3;
        }
        else if (MConstants.GAME_MODES.MODE4_Squid == MConstants.CurrentGameMode)
        {
            env = MConstants.Env_4_Squid;
        }
        else if (MConstants.GAME_MODES.MODE5_BATTLEFIELD == MConstants.CurrentGameMode)
        {
            env = MConstants.Env_5_BattleField;
        }
        else if (MConstants.GAME_MODES.SURVIVAL_MODE == MConstants.CurrentGameMode)
        {
            env = MConstants.Env_Survival;
        }

        // else
        // {
        //     env = MConstants.Env_3;
        // }
        MainMenuManager.Instance.showSubMenu(SubMenuNames.LOADING);
        StartCoroutine(LoadScene(env));
    }

    public void ArrowRightBtn()
    {
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            if (_currentLevelsPage == 1)
            {
                levelsPage1.GetComponent<Animation>().Play("LevelsOutAnimation");
                levelsPage2.GetComponent<Animation>().Play("LevelsInAnimation");
                _currentLevelsPage = 2;
            }
            else if (_currentLevelsPage == 2)
            {
                levelsPage2.GetComponent<Animation>().Play("LevelsOutAnimation");
                levelsPage3.GetComponent<Animation>().Play("LevelsInAnimation");
                _currentLevelsPage = 3;
            }
        }
        else  if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
        {
            if (_currentLevelsPage == 1)
            {
                levelsPage1.GetComponent<Animation>().Play("LevelsOutAnimation");
                levelsPage2.GetComponent<Animation>().Play("LevelsInAnimation");
                _currentLevelsPage = 2;
            }
        }
        else  if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            if (_currentLevelsPage == 1)
            {
                levelsPage1.GetComponent<Animation>().Play("LevelsOutAnimation");
                levelsPage2.GetComponent<Animation>().Play("LevelsInAnimation");
                _currentLevelsPage = 2;
            }
        }
    }

    public void ArrowLeftBtn()
    {
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            if (_currentLevelsPage == 2)
            {
                levelsPage2.GetComponent<Animation>().Play("LevelsOutAnimation");
                levelsPage1.GetComponent<Animation>().Play("LevelsInAnimation");
                _currentLevelsPage = 1;
            }
            else if (_currentLevelsPage == 3)
            {
                levelsPage3.GetComponent<Animation>().Play("LevelsOutAnimation");
                levelsPage2.GetComponent<Animation>().Play("LevelsInAnimation");
                _currentLevelsPage = 2;
            }
        }

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
        {
            if (_currentLevelsPage == 2)
            {
                levelsPage2.GetComponent<Animation>().Play("LevelsOutAnimation");
                levelsPage1.GetComponent<Animation>().Play("LevelsInAnimation");
                _currentLevelsPage = 1;
            }
        }
        
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            if (_currentLevelsPage == 2)
            {
                levelsPage2.GetComponent<Animation>().Play("LevelsOutAnimation");
                levelsPage1.GetComponent<Animation>().Play("LevelsInAnimation");
                _currentLevelsPage = 1;
            }
        }
    }
    
    public void BuyUnlockAllLevels()
    {
        Purchaser.instance.BuyNonConsumableUnlockLevels();
    }
    
    public void UnlockAllLevels()
    {
        PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode1 = MConstants.MAX_LEVELS;
        PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode2 = MConstants.MAX_LEVELS;
        PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode3 = MConstants.MAX_LEVELS;
        PlayerDataController.Instance.playerData.LastUnlockedLevel_SquidMode = MConstants.MAX_LEVELS;
        PlayerDataController.Instance.playerData.LastUnlockedLevel_BattleMode = MConstants.MAX_LEVELS;
        PlayerDataController.Instance.playerData.unlockedAllLevels = true;
        PlayerDataController.Instance.Save();
        /*unlockAllLevelsButton.SetActive(false);*/
        // MainMenuManager.Instance.RefreshData();
        foreach (var levelUIController in levelsList)
        {
            levelUIController.RefreshState();
        }
    }
}
