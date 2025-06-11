using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ControlFreak2;

public class LevelsManager : MonoBehaviour {
	public static LevelsManager instance;
	public GameObject environment;
	public AudioClip bgMusicForMachineGunLevel;
	public Level []Levels;
    [HideInInspector]
	public float startTime = 0f;
	[HideInInspector]
	public float endTime = 0f;
	public Transform []survivalModeSpwanPoints;
	[HideInInspector]
	public Material greenCheckPointMaterial;
    public AI[] enemy;
    public GameObject[] tacticalEnemies;
    public TouchJoystick touchjoyStick;
    int pickpoint;
    public GameObject HealthItem;

    
    [Header("Story Camera Objects"), Space (10)]
    public GameObject weaponCamera;
    public GameObject cfUi, joyStick;
    public GameObject fpBody;

	[HideInInspector]
	public ZombieWave currentZombieWave;
	public GameObject hudNavigationSystem;

	[HideInInspector]
	public Level currentLevel;

	//public AudioSource helicopterSound;
	void Awake () {
      
		instance = this;
        if (touchjoyStick == null)
        {
            touchjoyStick = FindObjectOfType(typeof(TouchJoystick)) as TouchJoystick;
        }

        if (environment) Instantiate(environment);

        if (MConstants.CurrentGameMode != MConstants.GAME_MODES.SURVIVAL_MODE)
        {
			if (MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE)
			{
				currentLevel = Instantiate(Levels[0], transform);
				currentLevel.gameObject.SetActive(true);
				return;
			}

			currentLevel = Instantiate(Levels[MConstants.CurrentLevelNumber - 1], transform);
            //currentLevel.gameObject.SetActive(true);


			if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
			{
				foreach (var spawnEnemy in SquidModeController.instance.enemiesSpawnPoints)
				{
					Instantiate(SquidModeController.instance.squidEnemies[Random.Range(0, SquidModeController.instance.squidEnemies.Length)],
						spawnEnemy.transform.position, spawnEnemy.transform.rotation,
						currentLevel.gameObject.transform);
				}
			}

			currentLevel.gameObject.SetActive(true);
		}
	}

    void Start(){

		SetStartTime ();
        if (MConstants.CurrentGameMode != MConstants.GAME_MODES.SURVIVAL_MODE)
        {
            Invoke("PickupItem", 3f);
        }
       
    }
   
    public void PickupItem()
    {
        enemy = FindObjectsOfType<AI>();
        //for (int i = 0; i < enemy.Length; i++)
        //{
        //    enemy[i].gameObject.GetComponent<CharacterDamage>().gunItem = null;
        //    enemy[i].GetComponent<CharacterDamage>().gunItem = HealthItem;
        //}
        if (enemy.Length!=0)
        {
            pickpoint = Random.Range(0, enemy.Length);
            if (HealthItem != null && enemy[pickpoint].GetComponent<CharacterDamage>())
            {
                enemy[pickpoint].GetComponent<CharacterDamage>().gunItem = HealthItem;
                // enemy[pickpoint].GetComponent<CharacterDamage>().gunObj = null;
                Debug.Log("Assign Randam Dropable Health Item");
            }
        }
       
    }

	public void SetStartTime(){
		startTime = Time.time;
	}

	public void SetEndTime(){
		endTime = Time.time;

	}

	//public void SpanEnemy(){
	//	Transform bestTarget = GetClosestEnemy(survivalModeSpwanPoints);

	//	if(bestTarget){
	//		GameObject.Instantiate(enemiesList[Random.Range(0,enemiesList.Length)], bestTarget.position + (Vector3.up), bestTarget.rotation);
	//	}

	//}
	Transform GetClosestEnemy (Transform[] enemies)
	{
		Transform bestTarget = null;
		return bestTarget;
	}
	
	public void StartObjectiveCamera()
	{
		// Levels[MConstants.CurrentLevelNumber - 1].StoryCamera.SetActive(true);
		weaponCamera.GetComponent<Camera>().enabled = false;
		cfUi.SetActive(false);
		fpBody.SetActive(false);
		HudMenuManager.instance.zoomSlider.SetActive(false);
		HudMenuManager.instance.bullets.SetActive(false);
		if (MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE5_BATTLEFIELD)
			HudMenuManager.instance.Croshair.SetActive(false);

		foreach (var meshes in HudMenuManager.instance.meshesToDisable)
		{
			meshes.SetActive(false);
		}

		if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
			joyStick.SetActive(false);

		if (MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE)
			HudMenuManager.instance.Croshair.SetActive(true);
	}
	
	public void EndObjectiveCamera()
	{
		// Levels[MConstants.CurrentLevelNumber - 1].StoryCamera.SetActive(false);
		weaponCamera.GetComponent<Camera>().enabled = true;
		cfUi.SetActive(true);
		
		if (MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
			fpBody.SetActive(true);
			HudMenuManager.instance.zoomSlider.SetActive(true);
			HudMenuManager.instance.Croshair.SetActive(true);
		}
		
		HudMenuManager.instance.bullets.SetActive(true);
		
		foreach (var meshes in HudMenuManager.instance.meshesToDisable)
		{
			meshes.SetActive(true);
		}

		if (currentLevel.isMachineGunLevel && currentLevel.isTurretMission)
		{
			HudMenuManager.instance.changeGunBtn.SetActive(false);
			HudMenuManager.instance.zoomSlider.SetActive(false);
			//HudMenuManager.instance.zoomBtn.SetActive(false);
			HudMenuManager.instance.Croshair.SetActive(false);
			//	HudMenuManager.instance.machineGunCrosshair.SetActive(true);
			HudMenuManager.instance.autoFireToggle.gameObject.SetActive(true);
			HudMenuManager.instance.autoFireToggle.isOn = true;
			HudMenuManager.instance.TurretMission(true);
		}
		else
        {
			if (currentLevel.isMachineGunLevel)
		{
			HudMenuManager.instance.changeGunBtn.SetActive(false);
			HudMenuManager.instance.zoomSlider.SetActive(false);
			HudMenuManager.instance.zoomBtn.SetActive(true);
			HudMenuManager.instance.Croshair.SetActive(false);
			HudMenuManager.instance.battleFieldCrosshair.SetActive(true);
		//	HudMenuManager.instance.machineGunCrosshair.SetActive(true);
			HudMenuManager.instance.autoFireToggle.gameObject.SetActive(true);
			HudMenuManager.instance.autoFireToggle.isOn = true;
		}
        }
		
		if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD && !currentLevel.isTurretMission)
        {
			joyStick.SetActive(true);
			foreach (var buttons in HudMenuManager.instance.battleFieldButtons)
		    {
		        buttons.SetActive(true);
		    }
		//	HudMenuManager.instance.Croshair.SetActive(false);
		}

		if (MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE)
			HudMenuManager.instance.Croshair.SetActive(true);

	}

}
