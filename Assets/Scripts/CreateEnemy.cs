using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{

    public GameObject enemyPrefab;
    public Transform taregtTransform;
    public TacticalAI.BaseScript.AIType aIType;
    public TacticalAI.BaseScript.IdleBehaviour idleBehaviour;

    public bool isHuman= true;

    // Start is called before the first frame update
    public void OnCreateEnemy(SCWave sCWave)
    {
        GameObject go = Instantiate(enemyPrefab , transform.position, transform.rotation);
        if (!isHuman)
        {
            go.GetComponent<SplineController>().SplineRoot = taregtTransform.gameObject;

            if (go.GetComponent<CutSceneCarController>())
            {
                go.GetComponent<CutSceneCarController>().cWave = sCWave;
                go.SetActive(false);

            }
            else
            {
                go.SetActive(true);
                go.transform.GetComponentInChildren<HealthController>().JetWave = sCWave;
            }
        }
        else
        {
            enemyPrefab.SetActive(false);
            go.GetComponent<TacticalAI.BaseScript>().keyTransform = taregtTransform;
            go.GetComponent<TacticalAI.BaseScript>().myAIType = aIType;
            go.GetComponent<TacticalAI.BaseScript>().myIdleBehaviour = idleBehaviour;

            go.GetComponent<TacticalAI.BaseScript>().cWave = sCWave;
            go.SetActive(false);
        }
        sCWave.AdEnemy(go);

    }


}
