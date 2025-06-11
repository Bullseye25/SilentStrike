using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneDestroyer : MonoBehaviour
{
   public GameObject container;
   public GameObject explosion;
   public GameObject hitBoxCanvas;
   public EnemyAI[] enemies;
   public ZombieAICreator[] zenemies;
   public GameObject lastCam;

   public void DropCrane()
   {
      if (HudMenuManager.instance.zoomedIn)
      {
         if (ZoomSlider.instance)
         {
            ZoomSlider.instance.zoomslider.value = 6f;
            ZoomSlider.instance.ChangeZoomValue();
         }
         else
         {
            HudMenuManager.instance.zoomOut();
         }
      }
      container.transform.SetParent(null);
      hitBoxCanvas.SetActive(false);
      var rb = container.gameObject.AddComponent<Rigidbody>();
      rb.mass = 100f;
      rb.AddForce(container.transform.up * 30, ForceMode.Impulse);
      explosion.SetActive(true);

      if(MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE3_Zombie)
        {
            foreach (var enemy in zenemies)
            {
                if (enemy.GetComponent<EnemyAI>().alerted)
                {
                    return;
                }
            }
        }


        Invoke(nameof(KillEnemies), 1f);
   }

   private void KillEnemies()
   {
      lastCam.SetActive(true);
      Time.timeScale = 0.3f;
        if(MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            List<GameObject> enemiesList = new List<GameObject>(LevelsManager.instance.currentZombieWave.enemiesList);
            foreach (var enemy in enemiesList)
            {
                enemy.GetComponent<EnemyAI>().TakeDamage(999);
            }
        }
        else
        {
            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(999);
            }
        }
        
   }
}
