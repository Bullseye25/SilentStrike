using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieWave : MonoBehaviour
{
    public List<ZombieAICreator> enemiesToSpawn;
    //public List<HostagesCreator> hostagesToSpawn;

   
    public List<GameObject> enemiesList;
    public GameObject nextObject;

    public bool isStartWave;
    public bool isFinalWave;
    //public bool Createhostages;


  
    // Start is called before the first frame update
    void OnEnable()
    {
        enemiesList = new List<GameObject>(); 
        for (int i = 0; i < enemiesToSpawn.Count; i++)
        {
            enemiesToSpawn[i].OnCreateEnemy(this);
        }

        if (isStartWave)
        {
            Invoke("StartWave",1);
            //StartWave();
        }
        //if(Createhostages)
        //{
        //    for (int i = 0; i < hostagesToSpawn.Count; i++)
        //    {
        //        hostagesToSpawn[i].createHostages(this);
        //    }
        //}

    }

   

    public void StartWayPointRoot()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemiesList[i].GetComponent<EnemyAI>())
            {
                enemiesList[i].GetComponent<EnemyAI>().StartWayPointRoot();
            }
        }
    }

    public void StartWave()
    {
        LevelsManager.instance.currentZombieWave = this;
        if(LevelsManager.instance.currentLevel.isMachineGunLevel)
        {
            ZombieSoundController.instance.StartZombieTauntSounds();
        }
        else
        {
            ZombieSoundController.instance.StartZombieSniperMissionSounds();
        }
        
        for (int i = 0; i < enemiesList.Count; i++)
        {
            enemiesList[i].gameObject.SetActive(true);
        }

        if (!isStartWave)
        {
            Invoke("StartWayPointRoot", 1);
            //StartWave();
        }

    }
    public void AdEnemy(GameObject baseScript)
    {
        enemiesList.Add(baseScript);

    }

   

  

    public void RemoveEnemy(GameObject baseScript)
    {

        enemiesList.Remove(baseScript);
        //HudMenuManager.instance.enemyHealthPrefab.SetActive(true);
      

        if (enemiesList.Count < 1)
        {
            if(LevelsManager.instance.currentLevel.isMachineGunLevel)
            {
                ZombieSoundController.instance.StopZombieTauntSounds();
            }
            else
            {
                ZombieSoundController.instance.StopZombieSniperMissionSounds();
            }

            if (nextObject && !isFinalWave)
            {
                nextObject.SetActive(true);
                if (nextObject.GetComponent<ZombieWave>())
                {
                    nextObject.GetComponent<ZombieWave>().StartWave();
                }
            }

            if (isFinalWave)
            {
                if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie && LevelsManager.instance.currentLevel.isHelicopterMission)
                {
                    LevelsManager.instance.currentLevel.helicopterSound.enabled = false;
                }
                if(LevelsManager.instance.currentLevel.vehicles && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
                {
                    LevelsManager.instance.currentLevel.vehicleHealthElement.ToRemoveHealthBar();
                }

                MConstants.isPlayerWin = true;
                HudMenuManager.instance.GameOver();
            }

          

          


        }

    }
}
