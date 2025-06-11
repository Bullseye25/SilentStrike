using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BompCtrl : MonoBehaviour
{
    public GameObject Explosion;
    public GameObject Spark;

    public void ApplyDamage(float damageAmt)
    {
        // MConstants.isPlayerWin = false;
        // HudMenuManager.instance.GameOver();
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        Spark.SetActive(false);
        Explosion.SetActive(true);
    }
    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    public void DestroyMango(float Duration)
    {
        Invoke("DeActivate", Duration);
    }

    void DeActivate()
    {
        this.gameObject.SetActive(false);
    }
}