using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PromoButton : MonoBehaviour
{
    public Image myIconImage;
    private string bundleID;
    private Button button;
    private int randomIndex;
    public bool RectangleIcon;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }
    private void OnEnable()
    {
        //if (PlayerDataController.Instance.playerData.isRemoveAds)
       // {
           // gameObject.SetActive(false);
        //}
        
    
        Setup();
        /*FinzAnalysisManager.Instance.CustomeAdAnalysis("Native_Promo");*/

    }

    private void Setup()
    {
        while (true)
        {
            randomIndex = Random.Range(0, PromoController.instance.adsList.Length);
            bundleID = PromoController.instance.adsList[randomIndex].BundleId;
            if (RectangleIcon && PromoController.instance.adsList[randomIndex].RectangleIcon == null)
            {
                continue;
            }
            myIconImage.sprite = RectangleIcon ? PromoController.instance.adsList[randomIndex].RectangleIcon : PromoController.instance.adsList[randomIndex].Icon;
            break;
        }
    }

    private void OnButtonClick()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + bundleID);
        Invoke(nameof(Setup),2);
       /* FinzAnalysisManager.Instance.CustomeAdAnalysis("Native_Promo_click");*/


    }
}