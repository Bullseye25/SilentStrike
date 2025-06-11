using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAble : MonoBehaviour
{
    public float MaxHealth;
   [ HideInInspector ]
    public float currentHealh;

    public virtual void OnHit(float damage)
    {

    }
}
