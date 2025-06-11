using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Loading : MonoBehaviour
{
    public Image fillBar;
    public Text ToolTipText;
    //public Text percentageText;
    //public GameObject promo;
    public string[] toolTips;

   /* private bool StartPercentage;
    private int Percentage;*/
    void OnEnable()
    {
        // fillBar.DOFillAmount(1, 3f).SetEase(Ease.Linear);
        ToolTipText.text = toolTips[Random.Range(0, toolTips.Length)];
       // Percentage = 1;
       // percentageText.text = Percentage + "%";
        fillBar.fillAmount = 0.01f;
        //StartPercentage = true;
        //if (AdsManager.instance != null)
        //{
        //    AdsManager.instance.RemoveBanner();
        //    if (InternetCheck.Instance.isInternetAvailable)
        //    {
        //        promo.SetActive(false);
        //    }
        //    AdsManager.instance.ShowHideNativeAd(true);
           
        //}

        //promo.SetActive(false);
    }

    private void OnDisable()
    {
        //if (AdsManager.instance != null)
        //{
        //    AdsManager.instance.ShowHideNativeAd(false);
        //    AdsManager.instance.RequestBanner();
        //}
    }

    private void Update()
    {
       /* if (StartPercentage && Percentage < 100)
        {
            Percentage++;
            //percentageText.text = Percentage + "%";
        }
        else if (Percentage == 100)
        {
            StartPercentage = false;
            Percentage = 0;
        }*/

        fillBar.fillAmount += 0.01f;
    }
}