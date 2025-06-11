using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelUIController : MonoBehaviour {
	public int levelID =1;
	/*public GameObject freeLockedImage;*/
	public GameObject lockImage;
	public Button levelButton;
	public Button levelButtonCurrent;

	public Text levelNoText;
    
	public Image []starsList;
	public Sprite []starsSpriteList;

	PlayerDataSerializeable playerData;
    public string ForKids, ForPro;
    // public int RewardCashStart, RewardCashEnd;

    [Header("Level Information Mode 1"), Space(10)]
    public string objective1;
    public int rewardAmount1;
    public int recommendedGun1;
    
    [Header("Level Information Mode 2"), Space(10)]
    public string objective2;
    public int rewardAmount2;
    public int recommendedGun2;
    
    [Header("Level Information Mode 3"), Space(10)]
    public string objective3;
    public int rewardAmount3;
    public int recommendedGun3;
    
    [Header("Level Information Squid Mode"), Space(10)]
    public string objectiveSquid;
    public int rewardAmountSquid;
    public int recommendedGunSquid;
    
    [Header("Level Information Battle Field Mode"), Space(10)]
    public string objectiveBattleField;
    public int rewardAmountBattleField;
    public int recommendedGunBattleField;

    void OnEnable () {
		playerData = PlayerDataController.Instance.playerData;
		levelNoText.text = levelID.ToString ();
        RefreshState();
    }
    

    public void RefreshState()
    {
        playerData = PlayerDataController.Instance.playerData;

        /////////////////////////////////////////////////////////
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
        {
            if (levelID <= playerData.LastUnlockedLevel_Mode2)
            {
                /*freeLockedImage.SetActive(false);*/
                /*lockImage.SetActive(false);*/
                if (levelID == playerData.currentSelectLevel_Mode2)
                {
                    levelButtonCurrent.gameObject.SetActive(true);
                    levelButton.gameObject.SetActive(false);
                }
                else
                {
                    levelButtonCurrent.gameObject.SetActive(false);
                    levelButton.gameObject.SetActive(true);
                }


            }
            else if ((levelID == playerData.LastUnlockedLevel_Mode2 + 1) /*&& AdsManager.instance.isAdReady()*/)
            {
                /*freeLockedImage.SetActive(true);*/
                lockImage.SetActive(true);
                levelButtonCurrent.gameObject.SetActive(false);
                levelButton.gameObject.SetActive(true);

            }
            else
            {
                /*freeLockedImage.SetActive(false);*/
                lockImage.SetActive(true);
                levelButtonCurrent.gameObject.SetActive(false);
                levelButton.gameObject.SetActive(false);
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            if (levelID <= playerData.LastUnlockedLevel_Mode3)
            {
                /*freeLockedImage.SetActive(false);*/
                /*lockImage.SetActive(false);*/
                if (levelID == playerData.currentSelectLevel_Mode3)
                {
                    levelButtonCurrent.gameObject.SetActive(true);
                    levelButton.gameObject.SetActive(false);
                }
                else
                {
                    levelButtonCurrent.gameObject.SetActive(false);
                    levelButton.gameObject.SetActive(true);
                }


            }
            else if ((levelID == playerData.LastUnlockedLevel_Mode3 + 1) /*&& AdsManager.instance.isAdReady()*/)
            {
                /*freeLockedImage.SetActive(true);*/
                /*lockImage.SetActive(false);*/
                levelButtonCurrent.gameObject.SetActive(false);
                levelButton.gameObject.SetActive(true);

            }
            else
            {
                /*freeLockedImage.SetActive(false);*/
                lockImage.SetActive(true);
                levelButtonCurrent.gameObject.SetActive(false);
                levelButton.gameObject.SetActive(false);
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            if (levelID <= playerData.LastUnlockedLevel_SquidMode)
            {
                /*freeLockedImage.SetActive(false);*/
                /*lockImage.SetActive(false);*/
                if (levelID == playerData.currentSelectLevel_SquidMode)
                {
                    levelButtonCurrent.gameObject.SetActive(true);
                    levelButton.gameObject.SetActive(false);
                }
                else
                {
                    levelButtonCurrent.gameObject.SetActive(false);
                    levelButton.gameObject.SetActive(true);
                }


            }
            else if ((levelID == playerData.LastUnlockedLevel_SquidMode + 1) /*&& AdsManager.instance.isAdReady()*/)
            {
                /*freeLockedImage.SetActive(true);*/
                /*lockImage.SetActive(false);*/
                levelButtonCurrent.gameObject.SetActive(false);
                levelButton.gameObject.SetActive(true);

            }
            else
            {
                /*freeLockedImage.SetActive(false);*/
                lockImage.SetActive(true);
                levelButtonCurrent.gameObject.SetActive(false);
                levelButton.gameObject.SetActive(false);
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            if (levelID <= playerData.LastUnlockedLevel_BattleMode)
            {
                /*freeLockedImage.SetActive(false);*/
                /*lockImage.SetActive(false);*/
                if (levelID == playerData.currentSelectLevel_BattleMode)
                {
                    levelButtonCurrent.gameObject.SetActive(true);
                    levelButton.gameObject.SetActive(false);
                }
                else
                {
                    levelButtonCurrent.gameObject.SetActive(false);
                    levelButton.gameObject.SetActive(true);
                }


            }
            else if ((levelID == playerData.LastUnlockedLevel_BattleMode + 1) /*&& AdsManager.instance.isAdReady()*/)
            {
                /*freeLockedImage.SetActive(true);*/
                /*lockImage.SetActive(false);*/
                levelButtonCurrent.gameObject.SetActive(false);
                levelButton.gameObject.SetActive(true);

            }
            else
            {
                /*freeLockedImage.SetActive(false);*/
                lockImage.SetActive(true);
                levelButtonCurrent.gameObject.SetActive(false);
                levelButton.gameObject.SetActive(false);
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            if (levelID <= playerData.LastUnlockedLevel_Mode1)
            {
                /*freeLockedImage.SetActive(false);*/
                /*lockImage.SetActive(false);*/
                if (levelID == playerData.currentSelectLevel_Mode1)
                {
                    levelButtonCurrent.gameObject.SetActive(true);
                    levelButton.gameObject.SetActive(false);
                    
                }
                else
                {
                    levelButtonCurrent.gameObject.SetActive(false);
                    levelButton.gameObject.SetActive(true);
                    
                }


            }
            else if ((levelID == playerData.LastUnlockedLevel_Mode1 + 1) /*&& AdsManager.instance.isAdReady()*/)
            {
                /*freeLockedImage.SetActive(true);*/
                lockImage.SetActive(true);
                levelButtonCurrent.gameObject.SetActive(false);
                levelButton.gameObject.SetActive(true);

            }
            else
            {
                /*freeLockedImage.SetActive(false);*/
                lockImage.SetActive(true);
                levelButtonCurrent.gameObject.SetActive(false);
                levelButton.gameObject.SetActive(false);
            }
        }
        ////////////////////////////////////////////////////////
    }
    
    
}
