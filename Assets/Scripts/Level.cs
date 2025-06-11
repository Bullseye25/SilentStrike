using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    public static Level instance;
    public bool isTimedMission;
    public float levelTime;
    public int RegisterEnemyToKill;
    // public GameObject FinalPortal;
   
    public Transform playerSpawnPoint;
    public Transform playerLookAtPosition;
    public GameObject objectiveCam;
    public AlertLevel alertLevel;
    public bool runningLevel;
    public bool startLevelAfterInstruction;

    public bool isMachineGunLevel;
    public bool isZombiesAttack;
    public MachineGunType machineGunType;

    public bool isTurretMission;
    public bool isHelicopterMission;
    public AudioSource helicopterSound;
    public GameObject vehicles;
    [HideInInspector]
    public HealthElement vehicleHealthElement;
    [HideInInspector]
    public float vehicleHealth = 100; 
    //public Transform jetDummyTarget;

    [Header("-----Num Of Targets Range For Squid Mode-----"), Space(10)]
    public int minTargetRange;
    public int maxTargetRange;
    
    [HideInInspector] public string missionStatement;
    [HideInInspector] public GameObject MissionInfoPanel;

    [HideInInspector] public EnemyAI[] enemiesList;
    public enum MachineGunType
    {
        SMG,
        HMG,
        G36,
        MINI_MACHINEGUN
    }
    public enum AlertLevel
    {
        MediumAlert,
        HighAlert
    }
    private void Awake()
    {
        instance = this;
    }
    void OnEnable()
    {

        //if ((MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE))
        //{
        //    enemiesList = GetComponentsInChildren<EnemyAI>();
        //    HudMenuManager.instance.Croshair.SetActive(true);
        //    return;
        //}

        if (MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            Invoke(nameof(ObjectiveCamDelay), 0.5f);
        }
       
        missionStatement = MConstants.MissionObjective;
        MissionInfoPanel = HudMenuManager.instance.missionInfoPanel;
       
        enemiesList = GetComponentsInChildren<EnemyAI>();
        
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            HudMenuManager.instance.missionText.text = missionStatement;
            MissionInfoPanel.SetActive(false);
            objectiveCam.SetActive(false);
            LevelsManager.instance.EndObjectiveCamera();
            // InstructionPanel.instance.MissionPanelOff();
        }
       
        else
        {
            HudMenuManager.instance.missionText.text = missionStatement + "\n" + " ( " + RegisterEnemyToKill + " / " + RegisterEnemyToKill + " )";
            MissionInfoPanel.SetActive(true);
        }

           
    }

    private void Start()
    {
        if(vehicles && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            vehicleHealthElement = GetComponentInChildren<HealthElement>();
            vehicleHealthElement.currentHealth = vehicleHealth;
        }
        //if (jetDummyMission)
        //{
        //    HudMenuManager.instance.jetDummyTarget = jetDummyTarget;
        //}

        //if (isMachineGunLevel)
        //{
        //    LevelsManager.instance.GetComponent<AudioSource>().clip = LevelsManager.instance.bgMusicForMachineGunLevel;
        //    LevelsManager.instance.GetComponent<AudioSource>().Play();
        //    if (GetComponent<ZombieHordeManager>())
        //    {
        //        var zombieHordeManager = GetComponent<ZombieHordeManager>();
        //        foreach (var zombie in zombieHordeManager.ZombieHordes[0].ZombieHorde)
        //        {
        //            zombie.SetActive(true);
        //        }
        //    }

        //    if (GetComponent<BattleFieldWavesController>())
        //    {
        //        var battleFieldController = GetComponent<BattleFieldWavesController>();
        //        foreach (var enemy in battleFieldController.enemyWaves[0].enemiesInWave)
        //        {
        //            //enemy.SetActive(true);
        //            // Instantiate(battleFieldController.enemyPrefabs[Random.Range(0, battleFieldController.enemyPrefabs.Length)], enemy.transform);
        //        }
        //    }
        //}
    }

    private void ObjectiveCamDelay()
    {
        LevelsManager.instance.StartObjectiveCamera();
    }
}