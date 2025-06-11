using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageWave : MonoBehaviour
{
    public static HostageWave instance;
    public List<HostageCreator> hostageToSpawn = new List<HostageCreator>();
    public bool run;
    public Transform target;
    [HideInInspector] 
    public List<HostageController> spawnedHostages = new List<HostageController>();
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        for (int i = 0; i < hostageToSpawn.Count; i++)
        {
            hostageToSpawn[i].createHostages();
        }
    }
    
    public void EscapeHostages()
    {
        foreach(var hostage in spawnedHostages)
        {
            hostage.StartRunning(target);
        }
        Invoke(nameof(GameoverDelay), 3f);
    }

    void GameoverDelay()
    {
        MConstants.isPlayerWin = true;
        HudMenuManager.instance.GameOver();
    }
}
