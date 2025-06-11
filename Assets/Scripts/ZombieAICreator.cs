using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAICreator : MonoBehaviour
{
    public RuntimeAnimatorController AttackBehaviour;
    public GameObject enemyPrefab;
    public GameObject patrollingWayPointRoot;
    public int health = 200;
    public float walkSpeed = 1;
    public float runningSpeed = 2;

    public void OnCreateEnemy(ZombieWave sCWave)
    {
        GameObject go = Instantiate(enemyPrefab, transform.position, transform.rotation);
        if (AttackBehaviour != null)
        {
            go.GetComponent<Animator>().runtimeAnimatorController = AttackBehaviour;
        }
         EnemyAI ai= go.GetComponent<EnemyAI>();
        ai.patrollingWayPointRoot = patrollingWayPointRoot;
        ai.health = health;
        ai.walkSpeed = walkSpeed;
        ai.runningSpeed = runningSpeed;
        ai.myWave = sCWave;
        go.SetActive(false);

        sCWave.AdEnemy(go);

    }
}
