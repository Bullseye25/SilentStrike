using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RewardManager : MonoBehaviour
{
    public static RewardManager instance;
    public Button rewardDialogOkButton , freeSpin;
    public Text rewardTimer;
    public float nextRewardTime;
    public bool GoldCheck;
    public Button[] ButtonsToDisable , ButtonsToDeactivate;

    void Start()
    {
        instance = this;
    }

    private bool isCountAdded;
    void Update()
    {
            var diff = (DateTime.Now.Ticks - PlayerDataController.Instance.playerData.lastSpinReward) /
                       TimeSpan.TicksPerSecond;
            var timeLeft = nextRewardTime - diff;

            if (timeLeft <= 0)
            {
            rewardTimer.text = "Get Spin!";
            GoldCheck = false;
            if(!WheelManager.instance.FreeSpinCheck)
            {
                isCountAdded = true;
                if (isCountAdded && PlayerDataController.Instance.playerData.SpinCount == 0)
                {
                    isCountAdded = false;
                    WheelManager.instance.FreeSpinCheck = true;
                    WheelManager.instance.SpinWheelCount = 1;
                    PlayerDataController.Instance.playerData.SpinCount += WheelManager.instance.SpinWheelCount;
                    PlayerDataController.Instance.Save();
                    WheelManager.instance.RefreshCountData();
                }
            }
           
            if (rewardDialogOkButton && WheelManager.instance.isClaimed)
            {
                isCountAdded = true;
                WheelManager.instance.isClaimed = false;
                freeSpin.gameObject.SetActive(true);
                rewardDialogOkButton.gameObject.SetActive(false);
            }
                return;
            }
            else
            {
                GoldCheck = true;
            }

        var r = "";
            //Hours
            //r += ((int) timeLeft / 3600) + "h ";
            //timeLeft -= ((int) timeLeft / 3600) * 3600;
            //Minutes
            r += ((int) timeLeft / 60) + "m ";
            //Seconds
            r += (timeLeft % 60) + "s ";
            rewardTimer.text = r;
    }
    public void RewardDialogOkButton()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.playAudioClip(SoundManager.SoundNames.SpinWHeel_Sound.GetHashCode());
        }
        WheelManager.instance.SpinTheWheel();
        PlayerDataController.Instance.playerData.lastSpinReward = DateTime.Now.Ticks;
        PlayerDataController.Instance.Save();
        freeSpin.gameObject.SetActive(false);
        rewardDialogOkButton.gameObject.SetActive(true);
        // rewardDialogOkButton.interactable = false;
        foreach (var item in ButtonsToDisable)
        {
            item.interactable = false;
        }
        foreach (var item in ButtonsToDeactivate)
        {
            item.gameObject.SetActive(false);
        }
        MyNotificationManager.instance.ShowNotification(nextRewardTime);

    }
}