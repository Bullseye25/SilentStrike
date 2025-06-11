using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class WeeklyRewardManager : MonoBehaviour
{
    public static WeeklyRewardManager instance;
    public GameObject CollectButton,collectButton2x,WeeklyRewardPanel;
    public Text rewardTimer;
    public float nextRewardTime;
    public bool ActivePressState;
    public GameObject[] PressedSprites , PopUpSprites;
    private bool rewardClaimed , claimed2X = false;
    private AudioSource gunSound;
    void Start()
    {
        instance = this;
        gunSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Reward should not be claimed after 2 days
        // var PresentDate = DateTime.Today.Ticks;
        var PresentDate = DateTime.Now.Ticks;
        var DateDiff = (PresentDate - PlayerDataController.Instance.playerData.RewardedDate) / TimeSpan.TicksPerSecond;
        if ( DateDiff > 172800)
        {
            PlayerDataController.Instance.playerData.rewardDay = 1;
            PlayerDataController.Instance.Save();
        }

        var diff = (DateTime.Now.Ticks - PlayerDataController.Instance.playerData.lastWeeklyRewarded) /
                   TimeSpan.TicksPerSecond;
        var timeLeft = nextRewardTime - diff;
        if (timeLeft <= 0 && !rewardClaimed)
        {
            rewardClaimed = true;
            ActivePressState = true;
            PressedState();
            /*MainMenuManager.Instance.showSubMenu(SubMenuNames.WeeklyReward);*/
            rewardTimer.text = "Reward Ready!";
            return;
        }

        var r = "";
        //Hours
        r += ((int)timeLeft / 3600) + "h ";
        timeLeft -= ((int)timeLeft / 3600) * 3600;
        //Minutes
        r += ((int)timeLeft / 60) + "m ";
        //Seconds
        r += (timeLeft % 60) + "s ";
        rewardTimer.text = r;
    }
    public void PressedState()
    {
        if (ActivePressState)
        {
            switch (PlayerDataController.Instance.playerData.rewardDay)
            {
                case 1:
                    PressedSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                    break;

                case 2:
                    PressedSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                    break;

                case 3:                    
                    PressedSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                    break;

                case 4:                   
                    PressedSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                    break;

                case 5:                   
                    PressedSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                    break;

                case 6:                   
                    PressedSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                    break;

                case 7:               
                    PressedSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                    break;
            }
            ActivePressState = false;

        }
    }

    public void ClaimRewardBtn()
    {
        if(claimed2X)
        {
            claimed2X = false;
            PlayerDataController.Instance.playerData.lastWeeklyRewarded = DateTime.Now.Ticks;
            PlayerDataController.Instance.playerData.RewardedDate = DateTime.Now.Ticks;
            PlayerDataController.Instance.Save();
            MyNotificationManager.instance.ShowNotification(nextRewardTime);
            rewardClaimed = false;
        }
        else
        {
            PlayerDataController.Instance.playerData.lastWeeklyRewarded = DateTime.Now.Ticks;
            PlayerDataController.Instance.playerData.RewardedDate = DateTime.Now.Ticks;
            PlayerDataController.Instance.Save();
            MyNotificationManager.instance.ShowNotification(nextRewardTime);
            rewardClaimed = false;
            switch (PlayerDataController.Instance.playerData.rewardDay)
            {
                case 1:
                    gunSound.enabled = true;
                    PlayerDataController.Instance.playerData.gunsList[1].isLocked = false;
                    PlayerDataController.Instance.Save();
                    break;

                case 2:
                    MainMenuManager.Instance.WeeklyAddGold(100);
                    MainMenuManager.Instance.CoinAnimation();
                    break;

                case 3:

                    MainMenuManager.Instance.WeeklyAddGold(200);
                    MainMenuManager.Instance.CoinAnimation();
                    break;

                case 4:

                    MainMenuManager.Instance.WeeklyAddGold(500);
                    MainMenuManager.Instance.CoinAnimation();
                    break;

                case 5:

                    MainMenuManager.Instance.WeeklyAddGold(1000);
                    MainMenuManager.Instance.CoinAnimation();
                    break;

                case 6:

                    MainMenuManager.Instance.WeeklyAddGold(1500);
                    MainMenuManager.Instance.CoinAnimation();
                    break;

                case 7:
                    gunSound.enabled = true;
                    MainMenuManager.Instance.WeeklyAddGold(2000);
                    MainMenuManager.Instance.CoinAnimation();
                    PlayerDataController.Instance.playerData.gunsList[4].isLocked = false;
                    PlayerDataController.Instance.Save();
                    break;
            }
        }
       
       
        MainMenuManager.Instance.CloseSubMenu();
        CollectButton.SetActive(false);
        collectButton2x.SetActive(false);
        HandleRewardDay();
    }


    void HandleRewardDay()
    {
        PlayerDataController.Instance.playerData.rewardDay++;
        RefreshPressedState();

        if (PlayerDataController.Instance.playerData.rewardDay == 8)
            PlayerDataController.Instance.playerData.rewardDay = 1;
      
        PlayerDataController.Instance.Save();
    }

    public void OpenClaimRewardPopUp()
    {
        CollectButton.SetActive(true);
        collectButton2x.SetActive(true);
        switch (PlayerDataController.Instance.playerData.rewardDay)
        {
            case 1:
                // gunSound.enabled = true;
                collectButton2x.SetActive(false);
                PopUpSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                break;

            case 2:
                PlayerDataController.Instance.playerData.tempCoins = 100;
                PopUpSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                break;

            case 3:
                PlayerDataController.Instance.playerData.tempCoins = 200;
                PopUpSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                break;

            case 4:
                PlayerDataController.Instance.playerData.tempCoins = 500;
                PopUpSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                break;

            case 5:
                PlayerDataController.Instance.playerData.tempCoins = 1000;
                PopUpSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                break;

            case 6:
                PlayerDataController.Instance.playerData.tempCoins = 1500;
                PopUpSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                break;

            case 7:
                collectButton2x.SetActive(false);
                PlayerDataController.Instance.playerData.tempCoins = 2000;
                PopUpSprites[PlayerDataController.Instance.playerData.rewardDay - 1].SetActive(true);
                break;
        }
    }

    void RefreshPressedState()
    {
      
        foreach (var item in PressedSprites)
        {
            item.SetActive(false);
        }
        foreach (var item in PopUpSprites)
        {
            item.SetActive(false);
        }
    }

    public void ShowRewardedAddForDoubleCoins()
    {
        /*AdsManager.isUnLockLevel = false;
        AdsManager.isRevive = false;
        AdsManager.HaveTempGun = false;
        AdsManager.isShowMenuAd = false;
        AdsManager.isRewarads = false;
        AdsManager.isspin = false;
        AdsManager.isDoubleReward = true;

        AdsManager.instance.ShowRewardedAdd();
        UnityAnalyticsScript.instance.AddUnityEvent("ShowRewardedAdd", new Dictionary<string, object>{
            { "Positon", "Garage"}
        });*/
    }

    public void DoubleReward()
    {
        MainMenuManager.Instance.WeeklyAddGold(PlayerDataController.Instance.playerData.tempCoins * 2);
        MainMenuManager.Instance.CoinAnimation();
        collectButton2x.SetActive(false);
        claimed2X = true;
    }
}
