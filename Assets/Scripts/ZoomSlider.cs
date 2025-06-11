using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomSlider : MonoBehaviour
{
    public GameObject[] Snipers;
    [HideInInspector] public Slider zoomslider;
    public float sliderMin, SliderMax;
    private float lastTime;
    public static ZoomSlider instance;
    
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        Invoke(nameof(GetGunZoomValues),0.1f);
    }

    private void Update()
    {
        if (ControlFreak2.CF2Input.GetButtonDown("Select Next Weapon"))
        {
            Invoke(nameof(GetGunZoomValues),1f);
            lastTime = Time.time;
        }
    }

    public void GetGunZoomValues()
    {
        zoomslider = this.GetComponent<Slider>();
        foreach (var item in Snipers)
        {
            if (item.activeSelf)
            {
                zoomslider.minValue = item.GetComponent<GunScopeZoomValues>().zoomMin;
                zoomslider.maxValue = SliderMax = item.GetComponent<GunScopeZoomValues>().zoomMax;

                item.GetComponent<WeaponBehavior>().zoomFOV = 32 - item.GetComponent<GunScopeZoomValues>().zoomMin;
                zoomslider.value = item.GetComponent<GunScopeZoomValues>().zoomMin;
            }
        }
    }

    public void ChangeZoomValue()
    {
        if (Time.time > lastTime + 1f)
        {
            foreach (var item in Snipers)
            {
                item.GetComponent<WeaponBehavior>().zoomFOV = 32 - zoomslider.value;
            }

            if (MConstants.CurrentLevelNumber == 1 && zoomslider.value >= (SliderMax - (SliderMax / 4)) &&
                MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
            {
                HudMenuManager.instance.ShowFireTutorial();
            }

            if (zoomslider.value > 6.2f)
            {
                HudMenuManager.instance.SniperZoom();
                InputControl.instance.zoomHold = true;
            }
            else if (zoomslider.value < 6.2f)
            {
                HudMenuManager.instance.zoomOut();
                InputControl.instance.zoomHold = false;
            }
        }
    }
}