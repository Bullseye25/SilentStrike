using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleFieldWavesController : MonoBehaviour
{
    public static BattleFieldWavesController instance;

    public EnemyWaves[] enemyWaves;
    
    private int _currentWaveNo, _totalWaves;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        foreach (var enemyWave in enemyWaves)
        {
            _totalWaves++;
            for (int i = 0; i < enemyWave.enemiesInWave.Length; i++)
            {
                enemyWave.noOfEnemiesInWave++;
            }
        }
    }

    public void ActivateEnemies()
    {
        foreach (var enemy in enemyWaves[_currentWaveNo].enemiesInWave)
        {
            Instantiate(LevelsManager.instance.tacticalEnemies[Random.Range(0, LevelsManager.instance.tacticalEnemies.Length)], enemy.transform);
        }
    }

    public void HandleNextWave()
    {
        enemyWaves[_currentWaveNo].noOfEnemiesInWave--;

        if (enemyWaves[_currentWaveNo].noOfEnemiesInWave == 0 && _currentWaveNo < _totalWaves - 1)
        {
            if (enemyWaves[_currentWaveNo].nextMovePoint)
            {
                enemyWaves[_currentWaveNo].nextMovePoint.SetActive(true);
            }
            else
            {
                ActivateNextWave();
            }
        }
        // if (_currentWaveNo < _totalWaves - 1)
        // {
        //     if (enemyWaves[_currentWaveNo].noOfEnemiesInWave == 0 && enemyWaves[_currentWaveNo].nextMovePoint)
        //     {
        //         enemyWaves[_currentWaveNo].nextMovePoint.SetActive(true);
        //     }
        //     else if (enemyWaves[_currentWaveNo].noOfEnemiesInWave == 1 && !enemyWaves[_currentWaveNo].nextMovePoint)
        //     {
        //         ActivateNextWave();
        //     }
        // }
        else if (enemyWaves[_currentWaveNo].noOfEnemiesInWave == 0 && _currentWaveNo == _totalWaves - 1)
        {
            HudMenuManager.instance.GameComplete();
        }
    }

    public void ActivateNextWave()
    {
        _currentWaveNo++;
        foreach (var enemy in enemyWaves[_currentWaveNo].enemiesInWave)
        {
           // enemy.SetActive(true);
           Instantiate(LevelsManager.instance.tacticalEnemies[Random.Range(0, LevelsManager.instance.tacticalEnemies.Length)], enemy.transform);
        }
    }
}

[System.Serializable]
public class EnemyWaves
{
    public GameObject[] enemiesInWave;
    public GameObject nextMovePoint;
    [HideInInspector] public int noOfEnemiesInWave;
}
