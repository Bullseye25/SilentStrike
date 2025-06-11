using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CivilianAI : MonoBehaviour
{
    public GameObject wayPoints;
    public float speed = 1;
    public bool looped = true;
    public bool startAfterInstruction;
    
    private Tween MyTween;
    private WayPoint _myWayPoint;
    private List<Vector3> waypointList = new List<Vector3>();


    private void Start()
    {
        if(wayPoints == null)
            return;
        
        _myWayPoint = wayPoints.GetComponent<WayPoint>();

        foreach (var waypoint in _myWayPoint.waypoints)
        {
            waypointList.Add(waypoint.position);
        }
        transform.position = waypointList[0];

        if (startAfterInstruction) 
            return;
        
        GetComponent<Animator>().SetBool("walk", true);

        MyTween = transform.DOPath(waypointList.ToArray(), 40f, PathType.Linear, PathMode.Full3D, 10,
            Color.grey).SetLookAt(0.01f).SetLoops(looped? -1 : 0).SetOptions(looped).SetEase(Ease.Linear);
        MyTween.timeScale = speed;
    }

    private void Update()
    {
        if (MConstants.BeginLevel && MyTween == null && startAfterInstruction)
        {
            GetComponent<Animator>().SetBool("walk", true);

            MyTween = transform.DOPath(waypointList.ToArray(), 40f, PathType.Linear, PathMode.Full3D, 10,
                Color.grey).SetLookAt(0.01f).SetLoops(looped? -1 : 0).SetOptions(looped).SetEase(Ease.Linear);
            MyTween.timeScale = speed;
        }
    }

    public void CivilianDeath()
    {
        transform.DOPause();
        // GetComponent<Animator>().SetBool("die", true);
        GetComponent<Animator>().enabled = false;
        MConstants.FailReason = "You Killed An Innocent Civilian";
        FailTheMission();
    }

    void FailTheMission()
    {
        LevelsManager.instance.weaponCamera.GetComponent<Camera>().enabled = false;
        LevelsManager.instance.cfUi.SetActive(false);
        LevelsManager.instance.fpBody.SetActive(false);
        MConstants.isPlayerWin = false;
        HudMenuManager.instance.GameOver();
    }
}
