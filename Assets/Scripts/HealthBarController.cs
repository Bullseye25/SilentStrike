using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TacticalAI;
public class HealthBarController : MonoBehaviour
{

    public EnemyHealthBar healthBarPrefab;
    private Dictionary<HealthElement, EnemyHealthBar> healthBars = new Dictionary<HealthElement, EnemyHealthBar>();
    private void OnEnable()
    {
        HealthElement.OnHealthAdded += AddHealthBar;
        HealthElement.OnHealthRemoved += RemoveHealth;

    }

    private void OnDisable()
    {
        HealthElement.OnHealthAdded -= AddHealthBar;
        HealthElement.OnHealthRemoved -= RemoveHealth;
    }
    private void AddHealthBar(HealthElement healthScript)
    {
        if (healthBars.ContainsKey(healthScript)== false)
        {
            var healthBar = Instantiate(healthBarPrefab,transform);
            healthBars.Add(healthScript, healthBar);
            healthBar.SetHealthBar(healthScript);
        }
    }

     void RemoveHealth(HealthElement HealthScript)
    {
       // Debug.Log("Descsczc");

        if (healthBars.ContainsKey(HealthScript))
        {
           // Debug.Log("Descsczcxc");

            Destroy(healthBars[HealthScript].gameObject);
            healthBars.Remove(HealthScript);
        }
    }
}
