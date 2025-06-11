using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Expert13BusController : MonoBehaviour
{
    public GameObject wayPointRoot;
    public GameObject enemyTarget, civilian1, civilian2;
    
    private WayPoint _wayPoint;
    private List<Vector3> _waypointList = new List<Vector3>();
    private int _rootNo;

    public void Start()
    {
        _wayPoint = wayPointRoot.GetComponent<WayPoint>();
        foreach (var waypoint in _wayPoint.waypoints)
        {
            _waypointList.Add(waypoint.position);
        }

        _rootNo = 1;
    }

    public void StartRoot()
    {
        transform.position = _waypointList[0];
        var tween = transform.DOPath(_waypointList.ToArray(), 10f, PathType.Linear, PathMode.Full3D, 10,
            Color.grey).SetLookAt(0.01f).SetLoops(0).SetOptions(false).SetEase(Ease.Linear).OnWaypointChange(OnWayPointChangeCallback);
        tween.timeScale = 0.5f;
    }

    IEnumerator PauseRoot()
    {
        transform.DOPause();
        yield return new WaitForSeconds(3f);
        transform.DOPlay();
    }

    void OnWayPointChangeCallback(int wayPointIndex)
    {
        if (wayPointIndex == _waypointList.Count && _rootNo <= 3)
        {
            _rootNo++;
            StartRoot();
        }
        else if (wayPointIndex == _waypointList.Count && !MConstants.isGameOver)
        {
            FailTheMission();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Expert13BusTrigger>())
        {
            StartCoroutine(PauseRoot());
            if (_rootNo == 1)
            {
                civilian1.SetActive(true);
                civilian2.SetActive(true);
                enemyTarget.SetActive(true);
            }
            else if (_rootNo == 2)
            {
                civilian1.SetActive(false);
                civilian2.SetActive(false);
            }
            else if (_rootNo == 3)
            {
                enemyTarget.SetActive(false);
            }
        }
        
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
