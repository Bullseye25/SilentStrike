using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillOther : MonoBehaviour
{
    public EnemyAI otherEnemy;

    private void Start()
    {
        StartCoroutine(KillOtherEnemy());
    }
    // void Update()
    // {
    //     if (MConstants.BeginLevel)
    //     {
    //         MConstants.BeginLevel = false;
    //         StartCoroutine(KillOtherEnemy());
    //     }
    // }

    IEnumerator KillOtherEnemy()
    {
        yield return new WaitForSeconds(2f);
        GetComponent<Animator>().SetBool("firegun", true);
        yield return new WaitForSeconds(2f);
        otherEnemy.animator.enabled = false;
        otherEnemy.killed = true;
    }
}
