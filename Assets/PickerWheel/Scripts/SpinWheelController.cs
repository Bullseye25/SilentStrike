using EasyUI.PickerWheelUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinWheelController : MonoBehaviour
{
    [Header("Spin panel :")]
    [SerializeField] private Button uiSpinButton;
    [SerializeField] private Text uiSpinButtonText;
    [SerializeField] private Button uiSpinGoldButton;

    public NumberAnimatorScript numberAnimatorScript;
    public GameObject coinEffect;
    public GameObject enoughGold;
    public GameObject notEnoughGold;

    [SerializeField] private PickerWheel pickerWheel;
    public Text spinAmountText;
    public float nextRewardTime;
    public Text rewardTimer;
    public GameObject freeDailySpin;
    public GameObject noFreeDailySpin;
    public int SpinAmout = 100;

    [Header("Reward panel :")]

    public GameObject rewardClaimPanel;
    public GameObject claimBtns;

    public Image rewardIcon;
    public Text rewardAmmount;
    [SerializeField] private Button twoXBtn;
    public AudioClip rewardClip;
    public AudioClip buttonClip;
    public AudioSource audioSource;
    public NumberAnimatorScript rewardAnim;

    [HideInInspector]
    public WheelPiece wheelPieceReward;
    long lastTimeClicked;
    string prefName = "LastDateTime";
    public bool isDailySpin;
    float lastUpdateTime;

   public bool canSpin = true;
    bool is2Xreward = false;

    [Header("Reward Piece:")]
    public UIPiece[] rewardItems;

    public int currentGold = 5000;
    public int rewardSpinCount = 4;

    bool freeSpin;
    int spinCount;
    public GameObject getFreeSpinBtn;
    public Text freeSpinCount;
    public bool IsinFinalProject;

    public GameObject closeBtn;
    private void OnEnable()
    {

        rewardClaimPanel.SetActive(false);

        if (!PlayerPrefs.HasKey(prefName))
        {

            PlayerPrefs.SetString(prefName, (DateTime.Now.Ticks).ToString());
        }
        isDailySpinAwailable();
        RefreshSpin(false);
        spinAmountText.text = SpinAmout.ToString();
        freeSpinCount.text = spinCount.ToString();

        PlayButtonSound();
        coinEffect.SetActive(false);

        numberAnimatorScript.goldText.text = getGold().ToString();
        Debug.Log("numberAnimatorScript..." + getGold());

        rewardTimer.text = "00:00";
    }

    int getGold()
    {

       return PlayerDataController.Instance.playerData.PlayerGold;

       // return currentGold;


    }

    void RefreshSpin(bool isCountWin)
    {
        int totalGold = getGold();
        freeDailySpin.SetActive((spinCount > 0));
        noFreeDailySpin.SetActive(!(spinCount > 0));
        //int xReward = 0;
        //if (isCountWin && wheelPieceReward != null)
        //{
        //    xReward = wheelPieceReward.rewardAmount;
        //}
        uiSpinGoldButton.interactable = ((totalGold ) >= SpinAmout);

        freeSpinCount.text = spinCount.ToString();

        enoughGold.SetActive(uiSpinGoldButton.interactable);
        notEnoughGold.SetActive(!uiSpinGoldButton.interactable);
    }


    bool isRewardedAd = false;
    public void OnSpinClick(bool isFree)
    {
        if (!isFree && SpinAmout>getGold())
        {
            RefreshSpin(false);
            return;
        }
        closeBtn.SetActive(false);
        uiSpinButton.interactable = false;
        uiSpinButtonText.text = "Spinning...";
        freeDailySpin.SetActive(false);
        noFreeDailySpin.SetActive(false);
        canSpin = false;
        coinEffect.SetActive(false);

        pickerWheel.OnSpinEnd(wheelPiece => {

            OnSpinEnd(wheelPiece);

            uiSpinButton.interactable = true;
            uiSpinButtonText.text = "";
            closeBtn.SetActive(true);
        });

        pickerWheel.Spin();

        if (!isFree)
        {
            AddRemoveGold(-SpinAmout, false);
            //DeuctCoins
        }
        else if(!isRewardedAd)
        {
            spinCount -= 1;
        }
        isRewardedAd = false;
        freeSpinCount.text = spinCount.ToString();
        
        PlayButtonSound();
    }

    void AddRemoveGold(int amount, bool isPeffect = true)
    {
        numberAnimatorScript.AddRemoveGold(amount, getGold());
        PlayerDataController.Instance.playerData.PlayerGold += amount;
        PlayerDataController.Instance.Save();
        MainMenuManager.Instance.RefreshData();
    }


    public bool isDailySpinAwailable()
    {

        if (!PlayerPrefs.HasKey(prefName))
        {
            PlayerPrefs.SetString(prefName, (DateTime.Now.Ticks - TimeSpan.TicksPerSecond * nextRewardTime).ToString());
        }
        long.TryParse(PlayerPrefs.GetString(prefName), out lastTimeClicked);

        var diff = (DateTime.Now.Ticks - lastTimeClicked) /
                       TimeSpan.TicksPerSecond;
        var timeLeft = nextRewardTime - diff;

        if ((timeLeft <= 0))
        {
            PlayerPrefs.SetString(prefName, DateTime.Now.Ticks.ToString());
            spinCount += 1;
            isDailySpin = false;
        }
        // Debug.Log("Next " + (timeLeft <= 0));
        return (timeLeft <= 0);

    }

    void Update()
    {
        if (!isDailySpin && canSpin && Time.time > lastUpdateTime + 1)
        {
            lastUpdateTime = Time.time;
            var diff = (DateTime.Now.Ticks - lastTimeClicked) /
                       TimeSpan.TicksPerSecond;
            var timeLeft = nextRewardTime - diff;

            if (timeLeft <= 0)
            {
                isDailySpin = true;
                isDailySpinAwailable();
                RefreshSpin(false);

                // return;
            }

            var r = "";
            //Hours
            r += ((int)timeLeft / 3600) + ":";
            timeLeft -= ((int)timeLeft / 3600) * 3600;
            //Minutes
            r += ((int)timeLeft / 60) + ":";
            //Seconds
            r += (timeLeft % 60);
            rewardTimer.text = r;
        }
    }


    public void OnSpinEnd(WheelPiece wheelPiece)
    {
        wheelPieceReward = wheelPiece;
        rewardClaimPanel.SetActive(true);
        claimBtns.SetActive(true);

        for (int i = 0; i < rewardItems.Length; i++)
        {
            rewardItems[i].gameObject.SetActive(false);
        }

        rewardItems[wheelPieceReward.iTEM_TYPE.GetHashCode()].gameObject.SetActive(true);
        wheelPieceReward.rewardAmount = wheelPieceReward.Amount;

        rewardItems[wheelPieceReward.iTEM_TYPE.GetHashCode()].amountText.text = wheelPieceReward.rewardAmount.ToString();
        if (rewardItems[wheelPieceReward.iTEM_TYPE.GetHashCode()].amountText.gameObject.GetComponent<NumberAnimatorScript>())
        {
            rewardAnim = rewardItems[wheelPieceReward.iTEM_TYPE.GetHashCode()].amountText.gameObject.GetComponent<NumberAnimatorScript>();

        }

        twoXBtn.gameObject.SetActive(wheelPieceReward.isCanBe2X);
        /*twoXBtn.interactable = isAdReady();*/
        PlayRewardClip();

    }

    void PlayRewardClip()
    {
        audioSource.PlayOneShot(rewardClip);
    }
   /* bool isAdReady()
    {
       // return false;
       return AdsManager.instance.isAdReady();
    }*/
    public void OnClaim()
    {
        StartCoroutine(OnContinue());
    }

    IEnumerator OnContinue()
    {
        claimBtns.SetActive(false);
        if (wheelPieceReward.iTEM_TYPE == ITEM_TYPE.GOLD)
        {
            rewardAnim.AddRemoveGold(-wheelPieceReward.rewardAmount, wheelPieceReward.rewardAmount);
            coinEffect.SetActive(true);


            yield return new WaitForSeconds(1.5f);
            AddRemoveGold(wheelPieceReward.rewardAmount);

        }
        else if (wheelPieceReward.iTEM_TYPE == ITEM_TYPE.FREE_SPIN)
        {
            spinCount++;
        }
        else if (wheelPieceReward.iTEM_TYPE == ITEM_TYPE.VEHICLE)
        {
            if (PlayerDataController.Instance.playerData.gunsList[3].isLocked)
                PlayerDataController.Instance.playerData.gunsList[3].isLocked = false;
        }
        rewardClaimPanel.SetActive(false);
        isDailySpin = false;
        canSpin = true;
        RefreshSpin(true);
        PlayButtonSound();

    }

    public void OnRewardSpin()
    {

       /*AdsManager.instance.ShowRewardedAddCallBack(OnRewardSpinSucces);*/
        PlayButtonSound();

    }

    

    public void OnRewardSpinSucces(bool isSuccess)
    {
        if (isSuccess)
        {
            isRewardedAd = true;

            OnSpinClick(true);
        }
    }

    void On2XSuccess(bool isSuccess)
    {
        StartCoroutine(GiveGoldAndPlay(isSuccess));

    }
    public void On2XRewardClick()
    {

       /*AdsManager.instance.ShowRewardedAddCallBack(On2XSuccess);*/
        PlayButtonSound();
    }

    public void OnGetFreeSpinSuccess(bool isSuccess)
    {
        if (isSuccess)
        {
            spinCount += rewardSpinCount;

        }
        RefreshSpin(false);

    }
    public void OnGetFreeSpin()
    {

        /*AdsManager.instance.ShowRewardedAddCallBack(OnGetFreeSpinSuccess);*/

        PlayButtonSound();

    }

    void PlayButtonSound()
    {
        audioSource.PlayOneShot(buttonClip);

    }


    public IEnumerator GiveGoldAndPlay(bool is2xReward)
    {
        if (is2xReward)
        {
            //  wheelPieceReward.Amount *= 2;
            wheelPieceReward.rewardAmount *= 2;

            rewardAmmount.text = (wheelPieceReward.rewardAmount).ToString();
            twoXBtn.interactable = false;
            yield return new WaitForSeconds(0.5f);
            rewardAnim.AddRemoveGold(wheelPieceReward.Amount, wheelPieceReward.Amount);
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(OnContinue());
    }

}

public enum ITEM_TYPE
{
    GOLD,
    FREE_SPIN,
    TRY_AGAIN,
    VEHICLE

}


//System.Action<bool> rewardCallBack;

//public void ShowRewardedAddCallBack(System.Action<bool> callback)
//{

//    if (AdmobAdListener.instance.IsRewardedAdLoaded())
//    {
//        rewardCallBack = callback;
//        AdmobAdListener.instance.ShowRewardedAd();
//    }
//    else
//    {
//        callback(false);
//    }


//}

//public void admobRewardedAdCallBack(bool isSucces)
//{
//    if (rewardCallBack != null)
//    {
//        rewardCallBack(isSucces);
//        rewardCallBack = null;
//    }
//    else

