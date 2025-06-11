using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneCarController : MonoBehaviour
{

    public GameObject startObjects;
    public GameObject smoke;
    public SCWave cWave;

    public float waitTime=0.5f;
    public void Start()
    {
        Invoke("ActiveObjects",waitTime);
    }

    void ActiveObjects()
    {
        startObjects.SetActive(true);
    }

    public void OnCutSceneComplete()
    {
        startObjects.SetActive(false);
        cWave.RemoveEnemy(gameObject); ;

    }
}
