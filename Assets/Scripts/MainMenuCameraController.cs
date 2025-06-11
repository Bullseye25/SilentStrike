using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraController : MonoBehaviour
{

    public Canvas canvas;

    private void OnEnable()
    {
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    private void OnDisable()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
    }
}
