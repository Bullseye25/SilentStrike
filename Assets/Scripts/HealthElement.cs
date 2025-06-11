using System;
using System.Collections;
using System.Collections.Generic;
using TacticalAI;
using UnityEngine;

public class HealthElement : MonoBehaviour
{

    public static event Action<HealthElement> OnHealthAdded = delegate { };
    public static event Action<HealthElement> OnHealthRemoved = delegate { };
   public event Action<float> HealthChangeDelegate = delegate { };

    [HideInInspector]
    public bool IsDead;

    [HideInInspector]
    public float currentHealth = 100;

    private void Start()
    {
        OnHealthAdded(this);
       // healthScript.OnDie += OnDie;
        //healthScript.OnHealthChanged += OnDamage;
    }

    public void OnHealthChanged(float health)
    {
        this.currentHealth = health;
        HealthChangeDelegate(health);
    }

    public void OnDie()
    {
        IsDead = true;
        transform.parent = null;
        Invoke("RemoveHealthBar",0.8f);
    }

    public void ToRemoveHealthBar()
    {
        RemoveHealthBar();
    }

      void RemoveHealthBar()
      {
            OnHealthRemoved(this);

      }



}
