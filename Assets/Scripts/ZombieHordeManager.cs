using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHordeManager : MonoBehaviour
{
    public Horde[] ZombieHordes;
    
    private int _currentHordeNo, _totalHordes;

    private void Start()
    {
        foreach (var horde in ZombieHordes)
        {
            _totalHordes++;
            for (int i = 0; i < horde.ZombieHorde.Length; i++)
            {
                horde.noOfZombiesInHorde++;
            }
        }
        
        foreach (var zombie in ZombieHordes[_currentHordeNo].ZombieHorde)
        {
            zombie.SetActive(true);
           var rig =  zombie.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < rig.Length; i++)
            {
                rig[i].isKinematic = true;
            }
        }
    }

    public void HordeManage()
    {
        ZombieHordes[_currentHordeNo].noOfZombiesInHorde--;
        if (ZombieHordes[_currentHordeNo].noOfZombiesInHorde == 0 && _currentHordeNo < _totalHordes - 1)
        {
            _currentHordeNo++;
            foreach (var zombie in ZombieHordes[_currentHordeNo].ZombieHorde)
            {
                zombie.SetActive(true);
                var rig = zombie.GetComponentsInChildren<Rigidbody>();
                for (int i = 0; i < rig.Length; i++)
                {
                    rig[i].isKinematic = true;
                }
                Invoke(nameof(StartNextHordeWayPoints),0.1f);
            }
        }
    }

    public void StartNextHordeWayPoints()
    {
        foreach (var zombie in ZombieHordes[_currentHordeNo].ZombieHorde)
        {
            zombie.gameObject.transform.GetComponentInChildren<EnemyAI>().StartWayPointRoot();
        }
    }
}

[System.Serializable]
public class Horde
{
    public GameObject[] ZombieHorde;
    [HideInInspector] public int noOfZombiesInHorde;
}
