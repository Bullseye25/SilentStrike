using System;
using System.Collections;
using System.Collections.Generic;
using CompleteProject;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Store : MonoBehaviour
{
    public static Store instance;
    public Button removeAdsButton;
    private int goldAmount;
    public Text goldText;

    public int goldForPurchase;
    public int cashPurchased;

    public Text pack1Price;
    public Text pack2Price;
    public Text packAdsPrice;

    
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
     
   
        var localPos = transform.localPosition;

        if (MainMenuManager.Instance.currentMenuName == MenuNames.MAIN_MENU || SceneManager.GetActiveScene().name != "UIScene")
            localPos.z = 0;
        else
            localPos.z = -1500f;
        
        transform.localPosition = localPos;
        
        removeAdsButton.interactable = !PlayerDataController.Instance.playerData.isRemoveAds;
        
        SetPricing();
    }

    public void SetPricing()
    {
        // Purchaser.instance.SetPrices();
    }
    public void BuyGoldPack1()
    {
        Purchaser.instance.BuyConsumable1();
    }

    public void BuyGoldPack2()
    {
        Purchaser.instance.BuyConsumable2();
    }

    public void BuyGoldPack3()
    {
        Purchaser.instance.BuyConsumable2();
    }
    public void BuyGoldPack4()
    {
        Purchaser.instance.BuyConsumable2();
    }
    public void BuyGoldPack5()
    {
        Purchaser.instance.BuyConsumable2();
    }


    public void BuyGoldPack6()
    {
        Purchaser.instance.BuyConsumable2();
    }

   



    //    public void BuyCashFromGold()
    //    {
    //        if (PlayerDataController.Instance.playerData.PlayerGold < goldForPurchase)
    //        {
    //            OutOfCashMenu.isStoreBuy = true;
    //            MainMenuManager.Instance.showSubMenu(SubMenuNames.OUT_OF_CASH);
    //        }
    //        else
    //        {
    //            MainMenuManager.Instance.showSubMenu(SubMenuNames.STORECONGRATSCASH);
    //        }
    //    }

    public void RemoveAds()
    {
        Purchaser.instance.BuyNonConsumable();
    }

    public void GiveGold1()
    {
        MainMenuManager.Instance.showSubMenu(SubMenuNames.STORECONGRATS);
        goldText.text = "10000";
        goldAmount = 10000;
        PlayerDataController.Instance.playerData.PlayerGold += goldAmount;
        PlayerDataController.Instance.Save();
    }

    public void GiveGold2()
    {
        MainMenuManager.Instance.showSubMenu(SubMenuNames.STORECONGRATS);
        goldText.text = "25000";
        goldAmount = 25000;
        PlayerDataController.Instance.playerData.PlayerGold += goldAmount;
        PlayerDataController.Instance.Save();
    }

    public void OkayButton()
    {
        MainMenuManager.Instance.AddGold(goldAmount);
    }

//    public void OkayCashButton()
//    {
//        MainMenuManager.Instance.AddCash(cashPurchased);
//        PlayerDataController.Instance.playerData.PlayerGold -= goldForPurchase;
//        PlayerDataController.Instance.Save();
//        MainMenuManager.Instance.RefreshData();
//    }
}