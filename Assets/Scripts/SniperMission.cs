using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperMission : MonoBehaviour
{
    public GameObject JoyStick,RunButton;
    public bool IsRunButtonOn;
    private void Start()
       
    {
        RunButton = GameObject.Find("Button-Run");
        JoyStick = LevelsManager.instance.touchjoyStick.transform.parent.gameObject;
        JoyStick.SetActive(false);
        //if (!IsRunButtonOn)
        //{
            RunButton.SetActive(false);
       // }
    }
    //private void OnEnable()
    //{

    //}
    private void Update()
    {
        if(RunButton!=null && RunButton.activeSelf)
        {
            RunButton.SetActive(false);
        }
    }
}
