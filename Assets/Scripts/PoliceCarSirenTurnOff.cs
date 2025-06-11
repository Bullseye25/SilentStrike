using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceCarSirenTurnOff : MonoBehaviour
{
    private bool _once;

    void Update()
    {
        if (MConstants.isGameOver && !_once)
        {
            _once = true;
            if (GetComponent<AudioSource>())
            {
                GetComponent<AudioSource>().mute = true;
            }
        }
    }
}
