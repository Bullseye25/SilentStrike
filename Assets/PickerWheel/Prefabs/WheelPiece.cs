﻿using UnityEngine ;

namespace EasyUI.PickerWheelUI {
   [System.Serializable]
   public class WheelPiece {
      public UnityEngine.Sprite Icon ;
      public string Label ;

      [Tooltip ("Reward amount")] public int Amount ;

      [Tooltip ("Probability in %")] 
      [Range (0f, 100f)] 
      public float Chance = 100f ;

      [HideInInspector] public int Index ;
      [HideInInspector] public double _weight = 0f ;

        [Tooltip("Item Type")] public ITEM_TYPE iTEM_TYPE;
        [Tooltip("isCan2x")] public bool isCanBe2X= true;

        public int rewardAmount;

    }

}
