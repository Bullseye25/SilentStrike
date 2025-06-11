using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expert11VIP : MonoBehaviour
{
    public CivilianAI hostage;

    private bool _once;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 15 && !_once)
        {
            _once = true;
            GetComponent<BoxCollider>().enabled = false;
            hostage.GetComponent<Animator>().SetBool("kneel", true);
            Invoke(nameof(FailTheMission),4f);
        }
    }

    void FailTheMission()
    {
        if (MConstants.isGameOver)
            return;

        LevelsManager.instance.weaponCamera.GetComponent<Camera>().enabled = false;
        LevelsManager.instance.cfUi.SetActive(false);
        LevelsManager.instance.fpBody.SetActive(false);
        MConstants.isPlayerWin = false;
        HudMenuManager.instance.GameOver();
    }
}
