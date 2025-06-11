using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class GiftRewards : MonoBehaviour
{
    public static GiftRewards instance;
    public GameObject rewards,victory,nextButton,clameButton,equip;
    public GameObject WatchVideoButton;
    public int []gunsIdsToUnlock;
    public int[] levelsCountToUnlock;

    public Sprite[] gunsSprites;

    public Image  gunImage;
    public Image gunImageClaim;
    public Image gunImageCongratulation;
    public Image filbarPrimary;
    
    public Image filbarScondary;

    public Text totalCountsText;
    public Text remainingCountsToUnlockText;

    PlayerDataSerializeable playerData;
    float totalLevelCountToUnlock= 5;
    public int currentGunId;
    public GameObject starsEffect;
    public Canvas[] CanvasesToOff;
    public Canvas canvaseToActiveOnEnd;
    public AudioClip victorySound;
    public GameObject loading;
    public GameObject noVideoPopShow;
    public GameObject areYouSure;
    public GameObject congratulationPop;
   
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        currentGunId = -1;
        playerData = PlayerDataController.Instance.playerData;

        MConstants.currentLevelCount++;
        for (int i = 0; i < CanvasesToOff.Length; i++) {
            CanvasesToOff[i].enabled = false;
        }
            
        StartCoroutine(ActiveVictory());
        //AdsManager.instance.setPositionNative(1);
    }

   
    public void SetGunData()
    {
        for (int i=0;i<gunsIdsToUnlock.Length;i++)
        {
            if (playerData.gunsList[gunsIdsToUnlock[i]].isLocked && !playerData.gunsList[gunsIdsToUnlock[i]].isSkip)
            {
                currentGunId = i;
                break;
            }
        }

       
        if (currentGunId >= 0)
        {
          
            totalLevelCountToUnlock = levelsCountToUnlock[currentGunId];
            if ( MConstants.currentLevelCount == totalLevelCountToUnlock)
            {
                
                gunImageClaim.sprite = gunsSprites[currentGunId];
                victory.SetActive(false);
                rewards.SetActive(false);
                congratulationPop.SetActive(false);
                areYouSure.SetActive(false);
                equip.SetActive(true);
                MConstants.currentLevelCount = 0;
                PlayerDataController.Instance.Save();
                //GunClaim0
            }
            else
            {

                filbarPrimary.fillAmount = (MConstants.currentLevelCount +0.7f) / totalLevelCountToUnlock;
               
                filbarScondary.fillAmount = (MConstants.currentLevelCount-1) / totalLevelCountToUnlock;

                StartCoroutine(FillAfterDelay());
                gunImage.sprite = gunsSprites[currentGunId];
                totalCountsText.text = totalLevelCountToUnlock.ToString();
                remainingCountsToUnlockText.text = (totalLevelCountToUnlock- MConstants.currentLevelCount).ToString() + " LEVEL LEFT FOR THIS GUN REWARD  / ";
                victory.SetActive(false);
                rewards.SetActive(true);
                equip.SetActive(false);
                congratulationPop.SetActive(false);
                areYouSure.SetActive(false);
                PlayerDataController.Instance.Save();
                //fill gun detail;
            }


        }
        else
        {
            GameCompleteButton();
        }

        


    }
    public void Congratulation()
    {
        gunImageCongratulation.sprite = gunsSprites[currentGunId];
        areYouSure.SetActive(false);
        rewards.SetActive(false);
        equip.SetActive(false);
        congratulationPop.SetActive(true);
        PlayerDataController.Instance.Save();
    }
    public IEnumerator FillAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        
        float currentValue =  (MConstants.currentLevelCount - 1) / totalLevelCountToUnlock;
        float targetValue = (MConstants.currentLevelCount) / totalLevelCountToUnlock;
       
        float lerpDuration = 1;
      
        float valueToLerp;

        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(currentValue, targetValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            filbarScondary.fillAmount = valueToLerp;

            yield return null;
        }
        filbarScondary.fillAmount = targetValue;
        PlayerDataController.Instance.Save();
    }


    public void OnClaimBtn()
    {
        rewards.SetActive(false);
        equip.SetActive(true);
      
    }
    public void OnSkipBtn()
    {
        areYouSure.SetActive(true);
        rewards.SetActive(false);
        equip.SetActive(false);
        
        // GameCompleteButton();
        playerData.gunsList[gunsIdsToUnlock[currentGunId]].isSkip = true;
        PlayerDataController.Instance.Save();
    }
    public void OnEquipBtn()
    {
       
        /*if (AdsManager.instance != null && AdsManager.instance.isAdReady())
        {
            playerData.gunsList[gunsIdsToUnlock[currentGunId]].isLocked = false;
           // AdsManager.instance.ResetBols();
            AdsManager.instance.isRewardsGun = true;
            AdsManager.instance.ShowRewardedAdd();
            playerData.CurrentSelectedPrimaryGun = gunsIdsToUnlock[currentGunId];
            MConstants.currentLevelCount = 0;
            PlayerDataController.Instance.Save();
            // GameCompleteButton();
        }
        else
        {
            playerData.gunsList[gunsIdsToUnlock[currentGunId]].isLocked = false;
            playerData.CurrentSelectedPrimaryGun = gunsIdsToUnlock[currentGunId];
            gunImageCongratulation.sprite = gunsSprites[currentGunId];
            MConstants.currentLevelCount = 0;
            congratulationPop.SetActive(true);
            noVideoPopShow.SetActive(false);
            rewards.SetActive(false);
            areYouSure.SetActive(false);
            equip.SetActive(false);
            PlayerDataController.Instance.Save();
           
        }*/

    }
    public void OnEquipFillBarBtn()
    {
/*
        if (AdsManager.instance != null && AdsManager.instance.isAdReady())
        {
            playerData.gunsList[gunsIdsToUnlock[currentGunId]].isLocked = false;
            //AdsManager.instance.ResetBols();
            AdsManager.instance.isRewardsGun = true;
            AdsManager.instance.ShowRewardedAdd();
            playerData.CurrentSelectedPrimaryGun = gunsIdsToUnlock[currentGunId];
            MConstants.currentLevelCount = 0;
            PlayerDataController.Instance.Save();
            // GameCompleteButton();
        }
        else
        {
            rewards.SetActive(false);
            noVideoPopShow.SetActive(true);
        }*/

    }
    public void CloseNoVideopop()
    {
        noVideoPopShow.SetActive(false);
        rewards.SetActive(false);
        congratulationPop.SetActive(false);
        areYouSure.SetActive(false);
        GameCompleteButton();
    }
    public IEnumerator ActiveVictory()
    {
        yield return new WaitForSeconds(1f);
        starsEffect.SetActive(true);
        
        victory.SetActive(true);
        rewards.SetActive(false);
        equip.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        // victorySound.Play();
        if (SoundManager.instance)
        {
            SoundManager.instance.playAudioClip(victorySound);
        }
       // yield return new WaitForSeconds(3f);
        starsEffect.SetActive(false);
        victory.SetActive(false);
        
        SetGunData();

      
        yield return new WaitForSeconds(2f);
        if (MConstants.currentLevelCount < totalLevelCountToUnlock)
        {
            nextButton.SetActive(true);
            WatchVideoButton.SetActive(true);
        }
        else
        {
           // clameButton.SetActive(true);
        }
        
    }

    void ActiveControles()
    {
         canvaseToActiveOnEnd.enabled = true;
        loading.SetActive(false);

    }
    public void GameCompleteButton()
    {
        rewards.SetActive(false);
        equip.SetActive(false);
        areYouSure.SetActive(false);
       // loading.SetActive(true);
        congratulationPop.SetActive(false);
        HudMenuManager.instance.GameComplete();
        canvaseToActiveOnEnd.enabled = true;
        // Invoke("ActiveControles",2f);
    }

    
}
