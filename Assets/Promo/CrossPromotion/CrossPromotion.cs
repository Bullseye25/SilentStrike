using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossPromotion : MonoBehaviour
{
    public Image myIconImage;
    public Image rectangleImage;
    public Text gameName;
    public Text punchLine;
    public Text rating;
    private string bundleID;

    private int randomIndex;

   
    private void OnEnable()
    {
        //if (PlayerDataController.Instance.playerData.isRemoveAds)
        // {
        // gameObject.SetActive(false);
        //}

       // AdsManager.instance.RemoveBanner();
       // Setup();
    }

    private void Setup()
    {
      

        randomIndex = Random.Range(0, PromoController.instance.adsList.Length);
        bundleID = PromoController.instance.adsList[randomIndex].BundleId;

        myIconImage.sprite = PromoController.instance.adsList[randomIndex].Icon;
        rectangleImage.sprite = PromoController.instance.adsList[randomIndex].RectangleIcon;
        gameName.text = PromoController.instance.adsList[randomIndex].Name;
        punchLine.text = PromoController.instance.adsList[randomIndex].PunchLine;
        rating.text = PromoController.instance.adsList[randomIndex].Rating;


    }


    public void OnInstallClick()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + bundleID);
        /*FinzAnalysisManager.Instance.CustomeAdAnalysis("Cross_Promo_Click");*/

        OnClose();
    }

    public void OnClose()
    {
        Destroy(gameObject);
    }
}
