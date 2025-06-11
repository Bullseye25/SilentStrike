using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldCheckPoint : MonoBehaviour
{

    public GameObject nextPoint;
    public bool isFinishPoint = false;
    public bool isStartPoint = false, isCrouched;
    public bool isTurretMission = false;
    public bool isJetMission;
    public bool jetDummyMission;
    public Transform jetDummyTarget;
    public bool isDoorOpen, isDoorClose;
    public DoorController doorMission;
    public bool isObjectDisable;
    public GameObject objectToDisable;


     void Start()
    {
        if (jetDummyMission)
        {
            HudMenuManager.instance.jetDummyTarget = jetDummyTarget;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //	HudMenuManager.instance.cutSceneEnds = false;
        if (other.gameObject.GetComponent<FPSPlayer>())
        {

            if (isDoorOpen)
            {
                doorMission.DoorMechanism(DoorController.DoorStates.DoorOpen);
            }
            if (isDoorClose)
            {
                doorMission.DoorMechanism(DoorController.DoorStates.DoorClose);
            }
            if (isTurretMission)
            {
                HudMenuManager.instance.TurretMission(true);
                if (LevelsManager.instance.currentLevel.isMachineGunLevel)
                {
                   PlayerWeapons.instance.SelectTempMachineGun(Level.MachineGunType.MINI_MACHINEGUN);
                }
            }
            if (isObjectDisable && !isCrouched)
            {
                objectToDisable.SetActive(false);
            }
            if (isFinishPoint)
            {
                gameObject.SetActive(false);
                if (HostageWave.instance)
                {
                    HostageWave.instance.EscapeHostages();
                    return;
                }
                MConstants.isPlayerWin = true;
                HudMenuManager.instance.GameOver();
            }

            else
            {
                // When we set gameobject as a player in crouch mission
                if (isCrouched)
                {
                    FPSPlayer.instance.FPSWalkerComponent.crouched = true;
                    if (isObjectDisable)
                    {
                        objectToDisable.SetActive(false);
                        Destroy(objectToDisable.GetComponent<TacticalAI.TargetScript>());
                    }
                }

                if (nextPoint.GetComponent<CutSceneController>())
                {
                    nextPoint.GetComponent<CutSceneController>().gameObject.SetActive(true);
                    nextPoint.GetComponent<CutSceneController>().CutSceneFuc();
                }

                if (nextPoint.GetComponent<SCWave>())
                {
                    if (isJetMission)
                    {
                        nextPoint.SetActive(true);
                    }
                    else
                    {

                        nextPoint.GetComponent<SCWave>().StartWave();
                    }
                }
                nextPoint.SetActive(true);
                gameObject.SetActive(false);
            }
        }

    }
}
