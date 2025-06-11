using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuCtrl : MonoBehaviour
{
    public GameObject Smoke;

    private void OnEnable()
    {
        Smoke.SetActive(true);
    }
    private void OnDisable()
    {
        Smoke.SetActive(false);

    }
}
