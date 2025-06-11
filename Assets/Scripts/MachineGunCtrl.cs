using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunCtrl : MonoBehaviour
{
    public float health;
    public FOVDetection  FOVDetection;
   
    public void ApplyDamage(float damage)
    {
        if (health>0)
        {
            health -= damage;
            if (health <= 0)
            {
                
                FOVDetection.EnemyDie();
                
            }
        }
        
    }
}
