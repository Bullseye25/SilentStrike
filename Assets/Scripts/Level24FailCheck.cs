using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level24FailCheck : MonoBehaviour
{
    public GameObject muzzle;
    
    private bool _once;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 15 && !_once)
        {
            _once = true;
            StartCoroutine(StartShooting());
        }
    }

    IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(2f);
        muzzle.SetActive(true);
        yield return new WaitForSeconds(20f);
        FailTheMission();
    }
    
    void FailTheMission()
    {
        LevelsManager.instance.weaponCamera.GetComponent<Camera>().enabled = false;
        LevelsManager.instance.cfUi.SetActive(false);
        LevelsManager.instance.fpBody.SetActive(false);
        MConstants.isPlayerWin = false;
        HudMenuManager.instance.GameOver();
    }
}
