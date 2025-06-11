using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageCreator : MonoBehaviour
{
    public GameObject hostagePrefab;

    public void createHostages()
    {
        GameObject go =  Instantiate(hostagePrefab, transform.position, transform.rotation);
        HostageWave.instance.spawnedHostages.Add(go.GetComponent<HostageController>());
    }
    
}
