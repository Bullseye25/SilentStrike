using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expert16AlertOthers : MonoBehaviour
{
    public EnemyAI[] otherEnemies;

    public void AlertOtherEnemies()
    {
        foreach (var otherEnemy in otherEnemies)
        {
            if (!otherEnemy.alerted)
            {
                otherEnemy.StartRunning();
            }
        }
    }
}
