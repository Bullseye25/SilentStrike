using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDrum : MonoBehaviour
{
    public static ExplosiveDrum instance;
    public GameObject explosion;
    public EnemyIndicator indicator;
    public EnemyAI[] enemies;
    
    [HideInInspector] public bool exploded;

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        indicator.gameObject.SetActive(true);
    }

    public void Explode()
    {
        exploded = true;
        explosion.SetActive(true);
        indicator.gameObject.SetActive(false);
        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(SlowMoTimeManage());
       transform.position += new Vector3(0, 2, 0);
        // enemies = FindObjectsOfType<EnemyAI>();

        //foreach (var enemy in enemies)
        //{
        //    if (enemy.alerted)
        //    {
        //        return;
        //    }
        //}
        foreach (var enemy in enemies)
        {
            enemy.BeforeExplosion();
        }

        ExplosionForce();
        foreach (var enemy in enemies)
        {
            enemy.animator.enabled = false;
            StartCoroutine(DamageEnemy(enemy));
        }
    }

    IEnumerator DamageEnemy(EnemyAI enemy)
    {
        yield return new WaitForSeconds(3f);
        enemy.TakeDamage(999);
    }
    void ExplosionForce()
    {
       
        foreach (var enemy in enemies)
        {
            foreach (var childRb in enemy.GetComponentsInChildren<Rigidbody>())
            {
                childRb.AddForce(Vector3.forward * 2500);
            }
        }
    }

    IEnumerator SlowMoTimeManage()
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1;
    }
}
