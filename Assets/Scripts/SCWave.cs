using System.Collections;
using System.Collections.Generic;
using TacticalAI;
using UnityEngine;

public class SCWave : MonoBehaviour
{
    public List<CreateEnemy> enemiesToSpawn;
    //public List<HostagesCreator> hostagesToSpawn;

    List<GameObject> baseScriptsList;
    public GameObject nextObject;
    public List<AttackPositions> positionsList;

    public bool isSlomo;
    public bool startWaveView;
    public bool isFinalWave;
    public bool TurretMissionEnd;
    //public bool Createhostages;


    public GameObject movePoint;
    public GameObject targetPoint;
    public bool ObjectToActive;
    public GameObject GameObjectToActive;
    // Start is called before the first frame update
    void OnEnable()
    {
        baseScriptsList = new List<GameObject>();
        for (int i=0; i<enemiesToSpawn.Count; i++)
        {
            enemiesToSpawn[i].OnCreateEnemy(this);
        }
        //if(Createhostages)
        //{
        //    for (int i = 0; i < hostagesToSpawn.Count; i++)
        //    {
        //        hostagesToSpawn[i].createHostages(this);
        //    }
        //}

    }

    public void StartWave()
    {
        TacticalAI.ControllerScript.currentController.SetAttackPositions(positionsList);

        TacticalAI.ControllerScript.currentController.StartTauntSounds();

        for (int i = 0; i < baseScriptsList.Count; i++)
        {
            baseScriptsList[i].gameObject.SetActive(true);
        }

        if (startWaveView)
        {
           // MovePlayerAndCamera.Instance.StartAttackAnimation(movePoint, targetPoint, 8, 5f, false, Vector3.zero);
           // MovePlayerAndCamera.Instance.StartAttackAnimation(movePoint, targetPoint, 20, 2f, false, Vector3.zero);
        }
       
    }
    public void AdEnemy(GameObject baseScript)
    {
        baseScriptsList.Add(baseScript);
        
    }

    public bool IsSlomoTime()
    {
        if (baseScriptsList.Count == 1 && isSlomo)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

        public void RemoveMachines()
        {
       
        //  PlayerWeapons.instance.weaponOrder[19].GetComponent<WeaponBehavior>().haveWeapon = false;
        //   StartCoroutine(PlayerWeapons.instance.SelectWeapon(PlayerWeapons.instance.TempGun));

        if (HudMenuManager.instance.isFirebtnActive && TurretMissionEnd)
        {
            if (LevelsManager.instance.currentLevel.isMachineGunLevel)
            {
                PlayerWeapons.instance.SelectTempMachineGun(Level.MachineGunType.SMG);
            }
            HudMenuManager.instance.TurretMission(false);
            TurretMissionEnd = false;
        }

        TacticalAI.ControllerScript.currentController.StopTauntSounds();
            if (nextObject && !isFinalWave)
            {
                nextObject.SetActive(true);
                if (nextObject.GetComponent<SCWave>())
                {
                    nextObject.GetComponent<SCWave>().StartWave();
                }
            }

            if (isFinalWave)
            {
                MConstants.isPlayerWin = true;
                HudMenuManager.instance.GameOver();
            }
        }

        public void RemoveEnemy(GameObject baseScript)
        {
      
        baseScriptsList.Remove(baseScript);

        if (baseScriptsList.Count == 0 && isSlomo)
        {
            isSlomo = false;
            GameObject obj = new GameObject();
            obj.transform.parent = baseScript.GetComponent<BaseScript>().animationScript.animator.GetBoneTransform(HumanBodyBones.Head);//baseScript.GetComponent<BaseScript>().animationScript.myAIBodyTransform;
            obj.transform.localPosition = new Vector3(0,0f,0f);
            Vector3 Offset = new Vector3(0, 0, 0);
      //      MovePlayerAndCamera.Instance.StartAttackAnimation(obj, obj,60,0.5f, true,Offset);
        }

        if (baseScriptsList.Count<1)
        {
            
            TacticalAI.ControllerScript.currentController.StopTauntSounds();

            if (HudMenuManager.instance.isFirebtnActive && TurretMissionEnd)
            {
                if (LevelsManager.instance.currentLevel.isMachineGunLevel)
                {
                    PlayerWeapons.instance.SelectTempMachineGun(Level.MachineGunType.SMG);
                }
                HudMenuManager.instance.TurretMission(false);
                TurretMissionEnd = false;
            }

            if (nextObject && !isFinalWave)
            {
                nextObject.SetActive(true);
                if (nextObject.GetComponent<SCWave>())
                {
                    nextObject.GetComponent<SCWave>().StartWave();
                }
            }

            if (isFinalWave)
            {
                MConstants.isPlayerWin = true;
                HudMenuManager.instance.GameOver();
            }

            if (ObjectToActive)
            {
                GameObjectToActive.SetActive(true);
            }

            if(FPSPlayer.instance.FPSWalkerComponent.crouched)
            {
                FPSPlayer.instance.FPSWalkerComponent.crouched = false;
            }
            

        }
       
        }


}
