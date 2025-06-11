using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    private bool _once;

    private void Start()
    {
        gameObject.SetActive(MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE);
    }

    void Update()
    {
        if (!(Camera.main is null))
            transform.LookAt(Camera.main.transform);
    }
}
