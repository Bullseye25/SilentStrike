using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCtrl : MonoBehaviour
{
    public GameObject[] Boat_Enemy;
    public bl_MiniMapItem [] Boat_EnemyIcon;
    // Start is called before the first frame update
    void Start()
    {
        if (MConstants.CurrentLevelNumber != 4)
        {
            foreach (GameObject ENEMY in Boat_Enemy)
            {
                ENEMY.SetActive(false);

            }
            foreach (bl_MiniMapItem icon in Boat_EnemyIcon)
            {
                icon.HideItem();

            }
        }
    }

    // Update is called once per frame
  
}
