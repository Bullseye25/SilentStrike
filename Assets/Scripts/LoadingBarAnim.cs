using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBarAnim : MonoBehaviour
{
    public Text percentageText;
    //public GameObject[] barPoints;

    private bool StartPercentage;
    private int Percentage, count;
    //IEnumerator Start()
    //{
    //    Percentage = 0;
    //    StartPercentage = true;
    //    foreach (var point in barPoints)
    //    {
    //        point.SetActive(false);
    //    }

    //    foreach (var point in barPoints)
    //    {
    //        point.SetActive(true);
    //        yield return new WaitForSeconds(0.3f);
    //    }
    //}

    private void Start()
    {
        Percentage = 0;
        StartPercentage = true;
    }
    private void Update()
    {
        if (StartPercentage && Percentage < 100)
        {
            count++;
            if (count % 3 == 0)
            {
                Percentage++;
                percentageText.text = Percentage + "%";
            }
        }
        else if (Percentage == 100)
        {
            StartPercentage = false;
            Percentage = 0;
        }
    }
}
