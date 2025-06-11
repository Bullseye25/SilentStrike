using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HeliFlyAway : MonoBehaviour
{
    public GameObject heliDoor;
    public GameObject heliFlyWayPoints;

    private WayPoint _flyWayPoint;
    private List<Vector3> _flyWayPointsList = new List<Vector3>();
    private bool _triggerCheckOnce;

    private void Start()
    {
        if (heliFlyWayPoints)
        {
            _flyWayPoint = heliFlyWayPoints.GetComponent<WayPoint>();
            foreach (var flyPoints in _flyWayPoint.waypoints)
            {
                _flyWayPointsList.Add(flyPoints.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 15 && !_triggerCheckOnce)
        {
            _triggerCheckOnce = true;
            other.gameObject.GetComponentInParent<EnemyAI>().transform.parent = this.transform;//transform.root.SetParent(this.transform);
            StartCoroutine(StartFlying());
        }
    }

    IEnumerator StartFlying()
    {
        yield return new WaitForSeconds(2f);
        heliDoor.GetComponent<Animation>().Play("HeliDoorAnimation");
        yield return new WaitForSeconds(3f);
        HudMenuManager.instance.zoomOut();
        GetComponent<Animation>().Play("HeliFlyAnimation");
        yield return new WaitForSeconds(4f);
        FailTheMission();

        // transform.DOPath(_flyWayPointsList.ToArray(), 20f, PathType.Linear, PathMode.Full3D, 10,
        // Color.grey).SetLookAt(0.01f).SetLoops(0).SetOptions(true).SetEase(Ease.Linear)
        // .OnWaypointChange(WayPointChangeCallback);
    }
    
    void WayPointChangeCallback(int wayPointIndex)
    {
        if (wayPointIndex == _flyWayPointsList.Count - 1)
        {
            FailTheMission();
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
