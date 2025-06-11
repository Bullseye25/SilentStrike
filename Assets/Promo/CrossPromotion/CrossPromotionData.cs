using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPromotionData : MonoBehaviour
{
    public AdsData[] adsList;

}

[System.Serializable]
public class AdsData
{
    public string Name;
    public Sprite Icon;
    public Sprite RectangleIcon;
    public string BundleId;
    public string PunchLine;
    public string Rating;


}