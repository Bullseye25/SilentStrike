using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCtrl : MonoBehaviour
{
    public bl_MiniMapItem MMIcon;

    private void Start()
    {
        if (MMIcon == null)
        {
            MMIcon = GetComponentInChildren<bl_MiniMapItem>();
        }
    }

    // Update is called once per frame
    public void EnemyKilled()
    {
        HudMenuManager.instance.EnemyKilled();
        if (MMIcon)
        {
            MMIcon.HideItem();
        }
    }
    public void EnemyKilledInWave()
    {
        SpawnerCtrl.instance.Count++;
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
        {
            HudMenuManager.instance.EnemyKilled();
            MMIcon.HideItem();
        }
    }

}
