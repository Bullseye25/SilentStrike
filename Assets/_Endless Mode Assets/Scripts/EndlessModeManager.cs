using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class EndlessModeManager : MonoBehaviour
{
    public Transform spawnedEnemies;
    public static EndlessModeManager Instance { get; private set; }
    public static IEnumerator PerformAfterDelayCoroutine(Action performThis, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Perform the action
        performThis?.Invoke();
    }

    public List<GameObject> EnemyAIPrefab;
    public List<SpawnPoint> SpawnPoints;
    public int MaxEnemyCount = 4;
    public int rewardAmmoForEachKill = 1;
    internal EndlessPlayerStats endlessPlayerStats;
    private int currentEnemyCount = 0;
    private SpawnPointManager spawnPointManager;
    private Timer timer;
    private WeaponBehavior weaponBehavior;

    // Dictionary to track which enemy belongs to which spawn point
    private Dictionary<GameObject, SpawnPoint> enemySpawnPointMap = new Dictionary<GameObject, SpawnPoint>();

    void Awake() {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Optional: Keeps the instance alive across scenes
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }

        Initialize();
    }
    void Start()
    {   
        MConstants.CurrentGameMode = MConstants.GAME_MODES.ENDLESS_MODE;
        timer = FindObjectOfType<Timer>();

        if(timer == null){
            Debug.Log("Timer is null");
        }
        SpawnEnemies();
    }

    void Initialize(){
        spawnPointManager = new SpawnPointManager(SpawnPoints);

        endlessPlayerStats = new EndlessPlayerStats();
        endlessPlayerStats.enemiesKilled = 0;
        endlessPlayerStats.headShots = 0;
        endlessPlayerStats.timeSurvived = "00:00";

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE)
            HudMenuManager.instance.Croshair.SetActive(true);
    }

    void SpawnEnemies()
    {
        for (int i = currentEnemyCount; i < MaxEnemyCount; i++)
        {
            SpawnPoint targetSpawnPoint = spawnPointManager.GetRandomWayPoint();
            if (targetSpawnPoint == null)
            {
                return;
            }

            GameObject enemyGO = Instantiate(EnemyAIPrefab[currentEnemyCount % EnemyAIPrefab.Count], targetSpawnPoint.transform.position, targetSpawnPoint.transform.rotation,spawnedEnemies);
            // GameObject enemyGO = Instantiate(EnemyAIPrefab[UnityEngine.Random.Range(0, EnemyAIPrefab.Count)]);
            //enemyGO.transform.position = targetSpawnPoint.transform.position;
            AI enemyAI = enemyGO.GetComponent<AI>();
            
            CharacterDamage characterDamage = enemyGO.GetComponent<CharacterDamage>();
            characterDamage.onDie.AddListener(() => OnEnemyDeath(enemyGO)); // Pass enemy GameObject

            enemyAI.waypointGroup = targetSpawnPoint.waypointGroup;
            enemyAI.attackRange = targetSpawnPoint.attackRange;
            enemyAI.shootRange = targetSpawnPoint.shootRange;

            if(targetSpawnPoint.isSideHide){
                enemyAI.AnimatorComponent.runtimeAnimatorController = targetSpawnPoint.SideHideAnimatorController;
            }

            enemyGO.SetActive(true);

            currentEnemyCount++;

            // Store the enemy and its spawn point
            enemySpawnPointMap[enemyGO] = targetSpawnPoint;
        }
    }

    public void RewardAlert(Text rewardtext, int rewardAmount)
    {
        rewardtext.text = "+" + rewardAmount;
    }

    public void SetEndlessGameStats()
    {
        var killingReward = endlessPlayerStats.enemiesKilled * 10;
        var headshotReward = endlessPlayerStats.headShots * 50;
        var totalReward = killingReward + headshotReward;

        HudMenuManager.instance.killingRewardText.text = killingReward.ToString();
        HudMenuManager.instance.headShotRewardText.text = headshotReward.ToString();
        HudMenuManager.instance.totalRewardText.text = totalReward.ToString();
        PlayerDataController.Instance.playerData.PlayerGold += totalReward;
    }

    void OnEnemyDeath(GameObject enemy)
    {
        currentEnemyCount--;

        endlessPlayerStats.enemiesKilled++;

       
        HudMenuManager.instance.CoinRewardAlert();
        HudMenuManager.instance.EnemyKilled();
        // if(weaponBehavior == null){
        //     weaponBehavior = GameObject.Find("Sniper (Mid)").GetComponent<WeaponBehavior>();
        // }

        // weaponBehavior.ammo = 999;

        // Find the spawn point and mark it as available again
        if (enemySpawnPointMap.TryGetValue(enemy, out SpawnPoint spawnPoint))
        {
            spawnPoint.isOccupied = false; // Mark the spawn point as free
            enemySpawnPointMap.Remove(enemy);
        }

        enemy.GetComponent<AI>().enemyIndicator.gameObject.SetActive(false);
        

        if (HudMenuManager.instance.player.hitPoints <= 50)
        {
            var randomNumber = UnityEngine.Random.Range(0, 20);
            if (randomNumber <= 5)
                HudMenuManager.instance.player.HealPlayer(25);
        }

        SpawnEnemies(); // Spawn a new enemy
    }

    void GameOver(){
        endlessPlayerStats.timeSurvived = timer.getTime();
    }

    public void RegisterHeadShot(){
        endlessPlayerStats.headShots++;
    }

    public SpawnPoint GetSpawnPointForEnemy(GameObject self){
        return enemySpawnPointMap[self];
    }
}



public class SpawnPointManager{
    public List<SpawnPoint> spawnPoints;

    public SpawnPointManager(List<SpawnPoint> spawnPoints)
    {
        this.spawnPoints = spawnPoints;
    }

    public SpawnPoint GetRandomWayPoint()
    {
        List<SpawnPoint> availableSpawnPoints = spawnPoints.FindAll(sp => !sp.isOccupied);

        if (availableSpawnPoints.Count == 0)
        {
            return null; // No vacant spawn points available
        }

        // Select a random available spawn point
        SpawnPoint selectedSpawnPoint = availableSpawnPoints[UnityEngine.Random.Range(0, availableSpawnPoints.Count)];

        // Mark it as occupied
        selectedSpawnPoint.isOccupied = true;

        // Return its transform
        return selectedSpawnPoint;
    }
}

public struct EndlessPlayerStats{
    public int enemiesKilled;
    public int headShots;
    public string timeSurvived;
    public int goldReward;
}