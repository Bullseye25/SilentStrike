using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CompleteProject;
using UnityEngine.UI;

public class GunSelectionMenu : MonoBehaviour
{
    public static GunSelectionMenu instance;
    public MainMenuManager menuManger;
    public Text BuyPrice;
    PlayerDataSerializeable playerData;
    public GameObject BuyButton, allGuns;
    /*public GameObject unlockAllGunsButton;*/
    private int currentGun = 0;
    public Text gunsNamesText, gunTypeText;

    ///private int currentCar2 = 0;
    public Button primaryButton, secondaryButton;
    public Sprite  equipPrimary, equipSecondary, equippedPrimary, equippedSecondary, swapToPrimary, swapToSecondary;
    
    public Sprite selected, unselected, locked, unlocked;

    public Image lockImage;
    public Text lockText;

    public GameObject purchaseGunTutBg, equipGunAnimation;

    public GameObject[] gunsModels;
    public Button[] Guns_Buttons;
    public GameObject allGunsModels;

    void OnEnable()
    {
        playerData = PlayerDataController.Instance.playerData;
        currentGun = playerData.CurrentSelectedPrimaryGun;
        //allGunsModels.SetActive(true);
        instance = this;
        /*unlockAllGunsButton.SetActive(!playerData.unlockedAllGuns);*/
        
        /*foreach (var gun in PlayerDataController.Instance.playerData.gunsList)
        {
            if (!gun.isLocked)
            {
                unlockAllGunsButton.SetActive(false);
            }
        }*/
        // allGuns.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        if (MConstants.ComingFromLevels)
        {
            MConstants.ComingFromLevels = false;
            equipGunAnimation.SetActive(true);
            currentGun = MConstants.ShowRecommendedWeaponNo;
            playerData.SelectedVehicle_temp = currentGun;

            if (!playerData.buyShotgunTutorial && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1 && playerData.currentSelectLevel_Mode1 == 4)
            {
                BuyButton.GetComponent<Animation>().enabled = true;
                purchaseGunTutBg.SetActive(true);
            }
        }
        // foreach (var item in gunsModels)
        // {
        //     item.SetActive(false);
        // }

        // gunsModels[playerData.CurrentSelectedPrimaryGun].SetActive(true);
        // lockImage.sprite = unlocked;
        // lockText.text = "OWNED";
        // gunsNamesText.text = playerData.gunsList[playerData.CurrentSelectedPrimaryGun].Name;
        // gunTypeText.text = playerData.gunsList[playerData.CurrentSelectedPrimaryGun].Type;
        //
        // primaryButton.image.sprite = equippedPrimary;
        // secondaryButton.image.sprite = swapToSecondary;

        // for (int i = 0; i < Guns_Buttons.Length; i++)
        // {
        //     Guns_Buttons[i].GetComponent<Image>().sprite = unselected;
        //     
        //     var state = Guns_Buttons[i].GetComponent<GunsLockOwnedStateManager>();
        //     state.stateImage.sprite = playerData.gunsList[i].isLocked ? state.locked : state.owned;
        // }
        // Guns_Buttons[currentGun].GetComponent<Image>().sprite = selected;
        
        refreshContent();
    }

    public void SelectGun(int i)
    {
        // foreach (var car in gunsModels)
        // {
        //     car.SetActive(false);
        // }
        //
        // gunsModels[i].SetActive(true);
        currentGun = i;
        playerData.SelectedVehicle_temp = currentGun;
        refreshContent();
    }

    public void refreshContent()
    {
        playerData = PlayerDataController.Instance.playerData;
        foreach (var item in gunsModels)
        {
            item.SetActive(false);
        }
        gunsModels[currentGun].SetActive(true);
        
        if (playerData.gunsList[currentGun].isLocked)
        {
            BuyButton.SetActive(true);
            primaryButton.gameObject.SetActive(false);
            secondaryButton.gameObject.SetActive(false);
            // EqiptBtn.SetActive(false);
            lockImage.sprite = locked;
            lockText.text = "LOCKED";
        }
        else if (!playerData.gunsList[currentGun].isLocked)
        {
            BuyButton.SetActive(false);
            if (currentGun == playerData.CurrentSelectedPrimaryGun)
            {
                primaryButton.image.sprite = equippedPrimary;
                secondaryButton.image.sprite = swapToSecondary;
                primaryButton.gameObject.SetActive(true);
                secondaryButton.gameObject.SetActive(true);
                
                if (playerData.gunsList[playerData.CurrentSelectedSecondaryGun].isLocked)
                {
                    secondaryButton.gameObject.SetActive(false);
                }
            }
            else if (currentGun == playerData.CurrentSelectedSecondaryGun)
            {
                primaryButton.image.sprite = swapToPrimary;
                secondaryButton.image.sprite = equippedSecondary;
                primaryButton.gameObject.SetActive(true);
                secondaryButton.gameObject.SetActive(true);
            }
            else
            {
                primaryButton.image.sprite = equipPrimary;
                primaryButton.gameObject.SetActive(true);
                secondaryButton.image.sprite = equipSecondary;
                secondaryButton.gameObject.SetActive(true);
            }
            
            // EqiptBtn.SetActive(true);
            lockImage.sprite = unlocked;
            lockText.text = "OWNED";
        }

        BuyPrice.text = playerData.gunsList[currentGun].UnlockPrice.ToString();
        gunsNamesText.text = playerData.gunsList[currentGun].Name;
        gunTypeText.text = playerData.gunsList[currentGun].Type;

        for (int i = 0; i < Guns_Buttons.Length; i++)
        {
            Guns_Buttons[i].GetComponent<Image>().sprite = unselected;
            
            var state = Guns_Buttons[i].GetComponent<GunsLockOwnedStateManager>();
            state.stateImage.sprite = playerData.gunsList[i].isLocked ? state.locked : state.owned;
        }

        Guns_Buttons[currentGun].GetComponent<Image>().sprite = selected;
    }

    public void EquipPrimary()
    {
        // SWAP PRIMARY <=> SECONDARY GUNS
        if (currentGun != playerData.CurrentSelectedPrimaryGun && currentGun == playerData.CurrentSelectedSecondaryGun)
        {
            playerData.CurrentSelectedSecondaryGun = playerData.CurrentSelectedPrimaryGun;
            playerData.CurrentSelectedPrimaryGun = currentGun;
            primaryButton.image.sprite = equippedPrimary;
            secondaryButton.image.sprite = swapToSecondary;
        }
        // SET PRIMARY GUN
        else
        {
            playerData.CurrentSelectedPrimaryGun = currentGun;
            primaryButton.image.sprite = equippedPrimary;
            secondaryButton.image.sprite = swapToSecondary;
        }
        PlayerDataController.Instance.Save();
        equipGunAnimation.SetActive(false);
        print(playerData.CurrentSelectedPrimaryGun + " --- " + playerData.CurrentSelectedSecondaryGun);
    }

    public void EquipSecondary()
    {
        // SWAP PRIMARY <=> SECONDARY GUNS
        if (currentGun != playerData.CurrentSelectedSecondaryGun && currentGun == playerData.CurrentSelectedPrimaryGun)
        {
            playerData.CurrentSelectedPrimaryGun = playerData.CurrentSelectedSecondaryGun;
            playerData.CurrentSelectedSecondaryGun = currentGun;
            primaryButton.image.sprite = swapToPrimary;
            secondaryButton.image.sprite = equippedSecondary;
        }
        // SET SECONDARY GUN
        else
        {
            playerData.CurrentSelectedSecondaryGun = currentGun;
            primaryButton.image.sprite = swapToPrimary;
            secondaryButton.image.sprite = equippedSecondary;
        }
        PlayerDataController.Instance.Save();
    }

    public void EuiptThisGun()
    {
        // playerData.CurrentSelectedPrimaryGun = currentGun;
        // recentEquiptedGun = currentGun;
        // PlayerDataController.Instance.Save();
    }

    public void Unlock()
    {
        if (GunSelectionSubmenu.instance)
        {
            GunSelectionSubmenu.instance.CurrentlyAGunIsBurchesed = true;
        }

        PlayerDataController.Instance.playerData.PlayerGold -= PlayerDataController.Instance.playerData
            .gunsList[PlayerDataController.Instance.playerData.SelectedVehicle_temp].UnlockPrice;
        PlayerDataController.Instance.playerData.gunsList[PlayerDataController.Instance.playerData.SelectedVehicle_temp]
            .isLocked = false;
        PlayerDataController.Instance.Save();
        equipGunAnimation.SetActive(true);

        menuManger.RefreshData();
        refreshContent();
        
        var state = Guns_Buttons[PlayerDataController.Instance.playerData.SelectedVehicle_temp].GetComponent<GunsLockOwnedStateManager>();
        state.stateImage.sprite = state.owned;
        
        if (!playerData.buyShotgunTutorial && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1 && playerData.currentSelectLevel_Mode1 == 4)
        {
            BuyButton.GetComponent<Animation>().enabled = false;
            purchaseGunTutBg.SetActive(false);
            LevelSelectionMenuManager.Instance.purchaseGunTutBg.SetActive(false);
            LevelSelectionMenuManager.Instance.weaponShopBtn.GetComponent<Animation>().enabled = false;
            playerData.buyShotgunTutorial = true;
            PlayerDataController.Instance.Save();
            EquipPrimary();
            Invoke(nameof(CloseGunMenu), 0.5f);
            // backBtn.GetComponent<Animation>().enabled = true;
        }
    }

    public void BuyUnlockAllGuns()
    {
        Purchaser.instance.BuyNonConsumableUnlockGuns();
    }
    
    public void UnlockAllGuns()
    {
        foreach (var gun in PlayerDataController.Instance.playerData.gunsList)
        {
            gun.isLocked = false;
        }

       /* unlockAllGunsButton.SetActive(false);*/
        PlayerDataController.Instance.playerData.unlockedAllGuns = true;
        PlayerDataController.Instance.Save();
        refreshContent();
    }
    void CloseGunMenu()
    {
        MainMenuManager.Instance.handleBackMenu();
    }

    public void ShowRewardedAdd()
    {
       /* AdsManager.isUnLockLevel = false;
        AdsManager.instance.ShowRewardedAdd();*/
        //UnityAnalyticsScript.instance.AddUnityEvent("ShowRewardedAdd", new Dictionary<string, object>
        //{
        //    {"Positon", "Garage"}
        //});
    }

    public void ShowRewardedAddForTempGun()
    {
       /* AdsManager.HaveTempGun = true;
        AdsManager.instance.ShowRewardedAdd();*/
        // UnityAnalyticsScript.instance.AddUnityEvent("ShowRewardedAdd", new Dictionary<string, object>{
        //     { "Positon", "Garage"}
        // });
    }

    public void HideSpriteOfEquiptGun()
    {
        // Eqipted_Sprite.SetActive(false);
    }

    private void OnDisable()
    {
        // allGuns.transform.localScale = new Vector3(145, 145, 145);
       
        foreach (var item in gunsModels)
        {
            item.SetActive(false);
        }

        if (LevelSelectionMenuManager.Instance)
        {
            LevelSelectionMenuManager.Instance.CheckRecommendedWeapon(LevelSelectionMenuManager.Instance.levelsList[MConstants.CurrentLevelNumber - 1]);
        }
        //allGunsModels.SetActive(false);
    }

}