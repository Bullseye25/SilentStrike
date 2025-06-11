using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSelectionSubmenu : MonoBehaviour
{
    public static GunSelectionSubmenu instance;
    public GameObject[] EquiptedSpritesForAll;
    public GameObject[] EqiptButtonsForPurchasedguns;
    public Button[] GunButtonBackGround;
    public Sprite Selected, NonSelected;

    public GameObject[] Guns;

    PlayerDataSerializeable playerData;
    public Queue<int> GunSelected;
    private int currentCar = 0;
    public GameObject EqiptBtn, BuyButton;
    public Sprite selected, unselected;
    public Text BuyPrice;
    public bool CurrentlyAGunIsBurchesed;
    public GameObject frontMenu, AllGunsMain, AllGuns;
    public Text GunsNamesText;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        MConstants.HavingTemporaryGun = false;
        AllGunsMain.transform.localScale = new Vector3(1, 1, 1);
        for (int i = 0; i < GunButtonBackGround.Length; i++)
        {
            EquiptedSpritesForAll[i] = GunButtonBackGround[i].transform.GetChild(0).gameObject;
            EqiptButtonsForPurchasedguns[i] = GunButtonBackGround[i].transform.GetChild(1).gameObject;
        }

        playerData = PlayerDataController.Instance.playerData;

        GunSelected = new Queue<int>();

        foreach (var item in Guns)
        {
            item.SetActive(false);
        }

        for (int i = 0; i < playerData.gunsList.Count; i++)
        {
            EqiptButtonsForPurchasedguns[i].SetActive(true);
            EquiptedSpritesForAll[i].SetActive(true);
        }

        for (int i = 0; i < playerData.gunsList.Count; i++)
        {
            if (playerData.gunsList[i].isLocked)
            {
                EqiptButtonsForPurchasedguns[i].SetActive(false);
                EquiptedSpritesForAll[i].SetActive(false);
            }
        }

        foreach (var item in EquiptedSpritesForAll)
        {
            item.SetActive(false);
        }

        foreach (var item in GunButtonBackGround)
        {
            item.GetComponent<Image>().sprite = NonSelected;
        }

        if (playerData.CurrentSelectedSecondaryGun == playerData.CurrentSelectedPrimaryGun)
        {
            for (int i = 0; i < playerData.gunsList.Count; i++)
            {
                if (!playerData.gunsList[i].isLocked && i != playerData.CurrentSelectedPrimaryGun)
                {
                    playerData.CurrentSelectedSecondaryGun = i;
                    break;
                }
            }
        }

        GunSelected.Enqueue(playerData.CurrentSelectedSecondaryGun);
        GunSelected.Enqueue(playerData.CurrentSelectedPrimaryGun);
        EquiptedSpritesForAll[playerData.CurrentSelectedPrimaryGun].SetActive(true);
        EqiptButtonsForPurchasedguns[playerData.CurrentSelectedPrimaryGun].SetActive(false);
        Guns[playerData.CurrentSelectedPrimaryGun].SetActive(true);
        GunsNamesText.text = playerData.gunsList[playerData.CurrentSelectedPrimaryGun].Name;
        EquiptedSpritesForAll[playerData.CurrentSelectedSecondaryGun].SetActive(true);
        EqiptButtonsForPurchasedguns[playerData.CurrentSelectedSecondaryGun].SetActive(false);
        GunButtonBackGround[playerData.CurrentSelectedPrimaryGun].GetComponent<Image>().sprite = Selected;
        Eqipted_Sprite.SetActive(true);
        BuyButton.SetActive(false);
        PlayerDataController.Instance.Save();
        if (CurrentlyAGunIsBurchesed)
        {
            SelectGun(PlayerDataController.Instance.playerData.SelectedVehicle_temp);
            CurrentlyAGunIsBurchesed = false;
        }
    }

    public void EquiptGun(int j)
    {
        GunSelected.Enqueue(j);

        EquiptedSpritesForAll[GunSelected.Peek()].SetActive(false);
        EqiptButtonsForPurchasedguns[GunSelected.Peek()].SetActive(true);
        GunButtonBackGround[GunSelected.Peek()].GetComponent<Image>().sprite = NonSelected;


        GunSelected.Dequeue();
        GunButtonBackGround[GunSelected.Peek()].GetComponent<Image>().sprite = NonSelected;
        playerData.CurrentSelectedPrimaryGun = GunSelected.Peek();
        playerData.CurrentSelectedSecondaryGun = j;


        foreach (var item in Guns)
        {
            item.SetActive(false);
        }

        for (int i = 0; i < playerData.gunsList.Count; i++)
        {
            if (playerData.gunsList[i].isLocked)
            {
                EqiptButtonsForPurchasedguns[i].SetActive(false);
                EquiptedSpritesForAll[i].SetActive(false);
            }
        }


        Guns[j].SetActive(true);
        EquiptedSpritesForAll[j].SetActive(true);
        EqiptButtonsForPurchasedguns[j].SetActive(false);
        EqiptBtn.SetActive(false);
        Eqipted_Sprite.SetActive(true);
        GunButtonBackGround[j].GetComponent<Image>().sprite = Selected;
        PlayerDataController.Instance.Save();
    }

    public GameObject Eqipted_Sprite;

    public void SelectGun(int i)
    {
        Eqipted_Sprite.SetActive(false);
        foreach (var car in Guns)
        {
            car.SetActive(false);
        }

        Guns[i].SetActive(true);
        currentCar = i;
        playerData.SelectedVehicle_temp = currentCar;
        refreshContent();
        if (currentCar == playerData.CurrentSelectedPrimaryGun || currentCar == playerData.CurrentSelectedSecondaryGun)
        {
            Eqipted_Sprite.SetActive(true);
            EqiptBtn.SetActive(false);
        }
    }

    public void EquiptGunInSecondPannel()
    {
        EquiptGun(currentCar);
    }

    public void refreshContent()
    {
        if (playerData.gunsList[currentCar].isLocked)
        {
            BuyButton.SetActive(true);
            EqiptBtn.SetActive(false);
        }
        else if (!playerData.gunsList[currentCar].isLocked)
        {
            BuyButton.SetActive(false);
            EqiptBtn.SetActive(true);
        }

        BuyPrice.text = playerData.gunsList[currentCar].UnlockPrice.ToString();
        GunsNamesText.text = playerData.gunsList[currentCar].Name;
        for (int i = 0; i < GunButtonBackGround.Length; i++)
        {
            GunButtonBackGround[i].GetComponent<Image>().sprite = unselected;
        }

        GunButtonBackGround[currentCar].GetComponent<Image>().sprite = selected;
    }

    private void OnDisable()
    {
        foreach (var item in Guns)
        {
            item.SetActive(false);
        }
    }

    public void BackgroundButtonDown()
    {
        frontMenu.SetActive(false);
//        AllGuns.transform.localScale += new Vector3(10,10,10);
        AllGuns.GetComponent<Animator>().SetBool("zoom", true);
    }

    public void BackgroundButtonUp()
    {
        frontMenu.SetActive(true);
//        AllGuns.transform.localScale -= new Vector3(10,10,10);
        AllGuns.GetComponent<Animator>().SetBool("zoom", false);
    }
}