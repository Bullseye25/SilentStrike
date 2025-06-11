using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunLoadOut : MonoBehaviour
{
    public static GunLoadOut instance;
    public GameObject SecondaryGunTxt;

    public void Awake()
    {
        instance = this;
    }
    public void PrimaryGunBtn()
    {
       // MainMenuManager.Instance.isLoadoutScreen = true;
        // MainMenuManager.Instance.ShowGunMenu();
        MainMenuManager.Instance.handleBackMenu();
       
    }
    public void ShowSecondaryGunInfo()
    {
        StartCoroutine(SecondaryGunInfo());
    }

    IEnumerator SecondaryGunInfo()
    {
        SecondaryGunTxt.SetActive(true);
        yield return new WaitForSeconds(3);
        SecondaryGunTxt.SetActive(false);
    }

}
