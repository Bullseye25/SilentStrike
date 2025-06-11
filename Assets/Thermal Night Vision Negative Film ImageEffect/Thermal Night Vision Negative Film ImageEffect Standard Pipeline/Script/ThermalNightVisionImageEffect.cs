using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalNightVisionImageEffect : MonoBehaviour
{
    public enum TYPE
    {
        LUMINANCE,
        COLOR
    }

    public TYPE type = TYPE.LUMINANCE;

    [Range(0,1)]
    public float threshold = 0.5f;

    [Range(0, 10)]
    public float hotIntensity = 2;

    [Range(0, 10)]
    public float coldIntensity = 2;

    public Color coldColor = new Color(0, 0, 1);

    public Color midColor = new Color(1, 1, 0);

    public Color hotColor = new Color(1, 0, 0);

    [Tooltip("Only available when type is COLOR")]
    public Color typeColorValue = new Color(0.760f, 0.247f, 0.509f);

    public AudioClip thermalActivateSound;

    private Shader shader = null;

    private Material mtrl = null;

    private void Awake()
    {
        shader = Shader.Find("Hidden/ThermalNightVisionImageEffect");
        if (!shader.isSupported)
        {
            enabled = false;
            return;
        }

        mtrl = new Material(shader);
    }

    private void OnEnable()
    {
        if (GetComponent<AudioSource>() != null)
            GetComponent<AudioSource>().PlayOneShot(thermalActivateSound);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (mtrl == null || mtrl.shader == null || !mtrl.shader.isSupported)
        {
            enabled = false;
            return;
        }

        mtrl.SetFloat("_Threshold", threshold);
        mtrl.SetColor("_TypeColorValue", typeColorValue);
        mtrl.SetFloat("_HotIntensity", hotIntensity);
        mtrl.SetFloat("_ColdIntensity", coldIntensity);
        mtrl.SetColor("_ColdColor", coldColor);
        mtrl.SetColor("_MidColor", midColor);
        mtrl.SetColor("_HotColor", hotColor);

        if(type == TYPE.LUMINANCE)
        {
            mtrl.DisableKeyword("TYPE_COLOR");
            mtrl.EnableKeyword("TYPE_LUMINANCE");
        }
        else if(type == TYPE.COLOR)
        {
            mtrl.EnableKeyword("TYPE_COLOR");
            mtrl.DisableKeyword("TYPE_LUMINANCE");
        }

        Graphics.Blit(src, dest, mtrl, 0);
    }

    private void OnDestroy()
    {
        shader = null;

        if (mtrl != null)
        {
            DestroyImmediate(mtrl);
            mtrl = null;
        }
    }
}
