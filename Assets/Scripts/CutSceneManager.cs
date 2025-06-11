using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager instance;
    public GameObject[] cutScenesLevels;
    private void Awake()
    {
        instance = this;
    }
}
