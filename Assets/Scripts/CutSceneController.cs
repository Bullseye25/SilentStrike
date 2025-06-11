using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneController : MonoBehaviour
{
    public static CutSceneController instance;
   // public bool isCutSceneMission;
    public GameObject cutScene;
    public float cutSceneTime;
    public GameObject nextPoint;
    public Transform target;

    private Coroutine deactivateCoroutine;
    // Start is called before the first frame update

    public void Awake()
    {   
        instance = this;
    }

    public void CutSceneFuc()
    {
       //StartCoroutine(ActivateCutScene());
       //deactivateCoroutine = StartCoroutine(DeactivateCutScene());
        
    }

    public void StopDeactivateCoroutine()
    {
        StopCoroutine(deactivateCoroutine);
        AfterCutScene();
        //gameObject.SetActive(false);
        //instance = null;
        //if (nextPoint)
        //{
        //    NextPointFunc();
        //}
    }

    public void AfterCutScene()
    {
        gameObject.SetActive(false);
        instance = null;
        if (nextPoint)
        {
            NextPointFunc();
        }
    }

 
    //public IEnumerator ActivateCutScene()
    //{
    //    HudMenuManager.instance.ObjectsToDisablesOnCutScene();
    //    cutScene.SetActive(true);
    //    if (target)
    //    {
    //        StartCoroutine(HudMenuManager.instance.SmoothMouseLook.Setrotation(target));
    //    }
    //    yield return new WaitForSeconds(0f);
    //}

    //public IEnumerator DeactivateCutScene()
    //{
    //    yield return new WaitForSeconds(cutSceneTime);
    //    //gameObject.SetActive(false);
    //    //instance = null;
    //    AfterCutScene();
    //    HudMenuManager.instance.ObjectsToEnableOnCutScene();
    //    //if(nextPoint)
    //    //{
    //    //    NextPointFunc();
    //    //}
       
    //}

    private void NextPointFunc()
    {
        if(nextPoint.GetComponent<SCWave>())
        {
            nextPoint.GetComponent<SCWave>().StartWave();
        }
        nextPoint.SetActive(true);
    }
}
