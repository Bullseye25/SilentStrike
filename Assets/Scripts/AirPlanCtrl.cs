using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AirPlanCtrl : MonoBehaviour
{
    public GameObject[] SocideBommber;
    public GameObject[] ObjectsToDisable;
    public GameObject StoryCamera;
    public Material planeMaterial;
    
    private void OnEnable()
    {
        planeMaterial.SetColor("_Color", Color.white);
        planeMaterial.DOColor(Color.black, 4f);

        StoryCamera.SetActive(true);
        foreach (var item in ObjectsToDisable)
        {
            item.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        planeMaterial.SetColor("_Color", Color.white);

    }
    private void OnDisable()
    {
        planeMaterial.SetColor("_Color", Color.white);

    }
}
