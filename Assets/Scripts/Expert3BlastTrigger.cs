using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expert3BlastTrigger : MonoBehaviour
{
    public RagdollController player1, player2;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<CarDestroyer>())
        {
            other.gameObject.GetComponentInParent<CarDestroyer>().StopTheCar();
            other.gameObject.GetComponentInParent<CarDestroyer>().explosion.SetActive(true);
            player1.RagDoll(true);
            player2.RagDoll(true);
            Invoke(nameof(FailTheMission),1f);
        }
    }
    
    public void FailTheMission()
    {
        LevelsManager.instance.weaponCamera.GetComponent<Camera>().enabled = false;
        LevelsManager.instance.cfUi.SetActive(false);
        LevelsManager.instance.fpBody.SetActive(false);
        MConstants.isPlayerWin = false;
        HudMenuManager.instance.GameOver();
    }
}
