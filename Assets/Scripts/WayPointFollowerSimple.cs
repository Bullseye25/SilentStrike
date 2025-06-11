using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WayPointFollowerSimple : MonoBehaviour
{
    public float speed = 1;
    public GameObject wayPoints;
    public bool loopedPath;
        
    private Tween MyTween;
    private WayPoint _myWayPoint;
    private List<Vector3> waypointList = new List<Vector3>();

    void Start()
    {
        _myWayPoint = wayPoints.GetComponent<WayPoint>();

        foreach (var waypoint in _myWayPoint.waypoints)
        {
            waypointList.Add(waypoint.position);
        }
        transform.position = waypointList[0];

        MyTween = transform.DOPath(waypointList.ToArray(), 40f, PathType.Linear, PathMode.Full3D, 10,
            Color.grey).SetLookAt(0.01f).SetLoops(loopedPath? -1 : 0).SetOptions(loopedPath).SetEase(Ease.Linear);
        MyTween.timeScale = speed;

    }
}
