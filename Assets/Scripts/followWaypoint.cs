using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class followWaypoint : MonoBehaviour
{
    public List<Vector3> waypointList;
    public WayPoint wayPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (var point in wayPoint.waypoints)
        {
            waypointList.Add(point.position);
        }

        transform.position = waypointList[0];
        transform.DOPath(waypointList.ToArray(), 10f, PathType.CatmullRom, PathMode.Full3D, 10,
                Color.grey).SetLookAt(0.01f).SetLoops(-1,LoopType.Restart).SetOptions(false).SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
