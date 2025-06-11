using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionPanel : MonoBehaviour
{
    public static InstructionPanel instance;
    public GameObject beginBtn, panelBg;
    public Text missionInfo, reward;

    private void Awake()
    {
        instance = this;
        MConstants.BeginLevel = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            LevelsManager.instance.hudNavigationSystem.SetActive(false);
        }
        // objectiveCameraThings.SetActive(true);
        HudMenuManager.instance.battleFieldCrosshair.SetActive(false);
        LevelsManager.instance.currentLevel.objectiveCam.SetActive(true);
        beginBtn.SetActive(false);
        Invoke(nameof(TurnOnBeginBtn), 0.5f);
        missionInfo.text = LevelsManager.instance.currentLevel.missionStatement;
        reward.text = MConstants.rewardGold.ToString();

        if (MConstants.CurrentGameMode != MConstants.GAME_MODES.ENDLESS_MODE)
            Invoke(nameof(CrossHairOff), 1f);

        //if (MConstants.CurrentGameMode != MConstants.GAME_MODES.ENDLESS_MODE)
        //    HudMenuManager.instance.aimReticleNew.SetActive(true);


        if(MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE){
            StartCoroutine(EndlessModeManager.PerformAfterDelayCoroutine( () => MissionPanelOff(), 0.5f));
        }
    }

    public void CrossHairOff()
    {
        HudMenuManager.instance.Croshair.SetActive(false);
    }

    public void TurnOnBeginBtn()
    {
        beginBtn.SetActive(true);
    }
    // Update is called once per frame
    public void MissionPanelOff()
    {
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie && (LevelsManager.instance.currentLevel.isMachineGunLevel))
        {
            LevelsManager.instance.hudNavigationSystem.SetActive(true);
        }
        LevelsManager.instance.currentLevel.objectiveCam.SetActive(false);

        if (MConstants.CurrentLevelNumber < 3 && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            CutSceneManager.instance.cutScenesLevels[MConstants.CurrentLevelNumber - 1].SetActive(true);
            HudMenuManager.instance.pauseBtn.SetActive(false);
            HudMenuManager.instance.missionTextBg.SetActive(false);
            panelBg.SetActive(false);
            return;
        }
        
        MConstants.StartTimer = true;
        gameObject.SetActive(false);
        if(MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            HudMenuManager.instance.battleFieldCrosshair.SetActive(false);
            HudMenuManager.instance.Croshair.SetActive(true);
        }
        
        LevelsManager.instance.EndObjectiveCamera();
        HudMenuManager.instance.CheckTutorials();
        MConstants.BeginLevel = true;
        MConstants.StartRunningInRunningMission = true;

        if (LevelsManager.instance.currentLevel.startLevelAfterInstruction)
        {
            if (LevelsManager.instance.currentZombieWave)
            {
                LevelsManager.instance.currentZombieWave.StartWayPointRoot();

            }

            if (MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE3_Zombie && MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE5_BATTLEFIELD)
            {
                foreach (var enemyAI in LevelsManager.instance.currentLevel.enemiesList)
                {
                    enemyAI.StartWayPointRoot();
                }
            }


            /*if (LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].gameObject.GetComponent<ZombieHordeManager>())
            {
                LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].startLevelAfterInstruction = false;
            }*/
        }

        foreach (var enemyAI in LevelsManager.instance.currentLevel.enemiesList)
        {
            if(enemyAI.enemyIndicator != null && MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE2_Expert)
                enemyAI.enemyIndicator.gameObject.SetActive(true);
        }
        
        if (LevelsManager.instance.gameObject.transform.GetComponentInChildren<MovingPlayerMissionController>())
        {
            LevelsManager.instance.gameObject.transform.GetComponentInChildren<MovingPlayerMissionController>().StartHelicopter();
        }
        if (LevelsManager.instance.gameObject.transform.GetComponentInChildren<Level33LadderEnemy>())
        {
            LevelsManager.instance.gameObject.transform.GetComponentInChildren<Level33LadderEnemy>().StartClimbing();
            LevelsManager.instance.gameObject.transform.GetComponentInChildren<Level33LadderEnemy>().enemyIndicator.SetActive(true);
        }

        if (LevelsManager.instance.gameObject.transform.GetComponentInChildren<Expert13BusController>())
        {
            LevelsManager.instance.gameObject.transform.GetComponentInChildren<Expert13BusController>().StartRoot();
        }

        if (LevelsManager.instance.gameObject.transform.GetComponent<SquidModeController>())
        {
            LevelsManager.instance.gameObject.transform.GetComponent<SquidModeController>().StartSquidGame();
        }

        //if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        //{
        //    LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].transform.GetComponent<BattleFieldWavesController>().ActivateEnemies();
        //}
    }

    public void StartLevelAfterCutScene()
    {
        foreach (var cutScenesLevel in CutSceneManager.instance.cutScenesLevels)
        {
            cutScenesLevel.SetActive(false);
        }
        
        gameObject.SetActive(false);
        HudMenuManager.instance.pauseBtn.SetActive(true);
        if (MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            HudMenuManager.instance.Croshair.SetActive(true);
        }
        LevelsManager.instance.EndObjectiveCamera();
        HudMenuManager.instance.CheckTutorials();
        MConstants.BeginLevel = true;
        MConstants.StartRunningInRunningMission = true;

        if (LevelsManager.instance.currentLevel.startLevelAfterInstruction)
        {
            if (LevelsManager.instance.currentZombieWave)
            {
                LevelsManager.instance.currentZombieWave.StartWayPointRoot();

            }
            if (MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE3_Zombie && MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE5_BATTLEFIELD)
            {

                foreach (var enemyAI in LevelsManager.instance.currentLevel.enemiesList)
                {
                    enemyAI.StartWayPointRoot();
                }

            }
            }

        foreach (var enemyAI in LevelsManager.instance.currentLevel.enemiesList)
        {
            if(enemyAI.enemyIndicator != null)
                enemyAI.enemyIndicator.gameObject.SetActive(true);
        }
        
        if (LevelsManager.instance.gameObject.transform.GetComponentInChildren<MovingPlayerMissionController>())
        {
            LevelsManager.instance.gameObject.transform.GetComponentInChildren<MovingPlayerMissionController>().StartHelicopter();
        }
    }
}
