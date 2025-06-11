using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Expert6PhoneBooth : MonoBehaviour
{
   public GameObject boothDoor;
   public GameObject newWayPointRoot;
   private WayPoint _newWayPoint;
   private List<Vector3> _newWaypointList = new List<Vector3>();
   private bool _once;
   private void Start()
   {
      _newWayPoint = newWayPointRoot.GetComponent<WayPoint>();
      foreach (var waypoint in _newWayPoint.waypoints)
      {
         _newWaypointList.Add(waypoint.position);
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.GetComponentInParent<EnemyAI>() && !_once)
      {
         _once = true;
         boothDoor.GetComponent<Animation>().Play("CloseDoor");
         StartCoroutine(StartRoot(other.GetComponentInParent<EnemyAI>()));
      }
   }

   IEnumerator StartRoot(EnemyAI enemy)
   {
      yield return new WaitForSeconds(4f);
      boothDoor.GetComponent<Animation>().Play("OpenDoor");
      enemy.KillTween();
      yield return new WaitForSeconds(0.5f);
      // enemy.animator.SetBool("run", true);
     var tween = enemy.transform.DOPath(_newWaypointList.ToArray(), 10f, PathType.Linear, PathMode.Full3D, 10,
            Color.grey).SetLookAt(0.01f).SetLoops(0).SetOptions(false).SetEase(Ease.Linear).OnWaypointChange(OnWayPointChangeCallback);
     tween.timeScale = 0.8f;
   }
   
   void OnWayPointChangeCallback(int wayPointIndex)
   {
      if (wayPointIndex == _newWaypointList.Count && !MConstants.isGameOver)
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
