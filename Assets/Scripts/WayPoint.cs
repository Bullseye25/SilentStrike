using System;
using UnityEngine;
using UnityEditor;

public class WayPoint : MonoBehaviour
{
    public Transform[] waypoints;
    public Color GizmoColor;
    public float SphereSize;
    public int FontSize;

    public bool WayPointTaken;
    private void OnEnable()
    {
//        if (wayoints == null)
//        {
//            wayoints = new Transform[transform.childCount];
//            for (int i = 0; i < transform.childCount; i++)
//            {
//                wayoints[i] = transform.GetChild(i);
//            }
//        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GizmoColor;
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawSphere(waypoints[i].position, SphereSize);
            Gizmos.DrawWireSphere(waypoints[i].position, SphereSize);
            GUIStyle style = new GUIStyle();
            style.normal.textColor = GizmoColor;
            style.fontSize = FontSize;
            style.fontStyle = FontStyle.Bold;
            //Handles.Label(wayoints[i].position+Vector3.up*2f, wayoints[i].gameObject.name, style);
        }

        Gizmos.color = GizmoColor;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }

        Gizmos.DrawLine(waypoints[0].position, waypoints[waypoints.Length - 1].position);
    }

}