using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromoController : MonoBehaviour
{
   public static PromoController instance;
   
   public AdsList[] adsList;

   private void Awake()
   {
      instance = this;
        DontDestroyOnLoad(this);
   }
}


[System.Serializable]
public class AdsList
{
   public string Name;
   public Sprite Icon;
   public Sprite RectangleIcon;
   public string BundleId;
   public string PunchLine;
   public string Rating;
}
