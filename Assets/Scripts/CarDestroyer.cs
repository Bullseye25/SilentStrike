using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarDestroyer : MonoBehaviour
{
    public float speed;
    public int explosionForce;
    public GameObject explosion;
    public GameObject wayPoints;
    // public bl_MiniMapItem miniMapItem;
    public bool startAfterObjectiveScreen; 

    private Tween MyTween;
    private WayPoint _myWayPoint;
    private List<Vector3> waypointList = new List<Vector3>();
    private EnemyIndicator _indicator;


    private void Start()
    {
        _myWayPoint = wayPoints.GetComponent<WayPoint>();
        // miniMapItem = GetComponentInChildren<bl_MiniMapItem>();
        _indicator = GetComponentInChildren<EnemyIndicator>();

        foreach (var waypoint in _myWayPoint.waypoints)
        {
            waypointList.Add(waypoint.position);
        }
        transform.position = waypointList[0];

        if (startAfterObjectiveScreen)//MConstants.CurrentLevelNumber == 16 || MConstants.CurrentLevelNumber == 7 || MConstants.CurrentLevelNumber == 23)
        {
            return;
        }
        /*if (MConstants.CurrentLevelNumber == 17)
        {
            MyTween = transform.DOPath(waypointList.ToArray(), 40f, PathType.Linear, PathMode.Full3D, 10,
                    Color.grey).SetLookAt(0.01f).SetLoops(0).SetOptions(true).SetEase(Ease.Linear)
                .OnWaypointChange(WayPointChangeCallback);
            MyTween.timeScale = 0.2f;

        }*/
        MyTween = transform.DOPath(waypointList.ToArray(), 40f, PathType.Linear, PathMode.Full3D, 10,
            Color.grey).SetLookAt(0.01f).SetLoops(-1).SetOptions(true).SetEase(Ease.Linear);
        MyTween.timeScale = speed;
    }

    private void Update()
    {
        if (MConstants.BeginLevel && (startAfterObjectiveScreen/*MConstants.CurrentLevelNumber == 16 || MConstants.CurrentLevelNumber == 7 || MConstants.CurrentLevelNumber == 23*/) 
            && MyTween == null)
        {
            MyTween = transform.DOPath(waypointList.ToArray(), 40f, PathType.Linear, PathMode.Full3D, 10,
                Color.grey).SetLookAt(0.01f).SetLoops(-1).SetOptions(true).SetEase(Ease.Linear);
            MyTween.timeScale = speed;
        }
    }

    void WayPointChangeCallback(int wayPointIndex)
    {
        if (wayPointIndex == waypointList.Count - 1)
        {
            FailTheMission();
        }
    }

    public void StopTheCar()
    {
        MyTween.Kill();
        transform.DOPause();
    }

    public void FailTheMission()
    {
        LevelsManager.instance.weaponCamera.GetComponent<Camera>().enabled = false;
        LevelsManager.instance.cfUi.SetActive(false);
        LevelsManager.instance.fpBody.SetActive(false);
        MConstants.isPlayerWin = false;
        HudMenuManager.instance.GameOver();
    }

    public void DestroyThisCar()
    {
        if(_indicator != null) _indicator.gameObject.SetActive(false);
        transform.DOPause();
        explosion.SetActive(true);
        var rb = gameObject.AddComponent<Rigidbody>();
        if (MConstants.CurrentLevelNumber == 11 || MConstants.CurrentLevelNumber == 17)
        {
            rb.mass = 2000;
            transform.position += new Vector3(0, 3, 0);
        }
        else
        {
            rb.mass = 1000;
        }
        rb.velocity = new Vector3(0, explosionForce, 0);
        // GetComponent<Animation>().enabled = true;
        // if(miniMapItem) miniMapItem.HideItem();
        StartCoroutine(DestroyDelay());
    }

    public IEnumerator DestroyDelay()
    {
        // yield return new WaitForSeconds(1f);
        GetComponent<Animation>().enabled = true;
        yield return new WaitForSeconds(3f);
        HudMenuManager.instance.EnemyKilled();
        gameObject.SetActive(false);
    }
}
