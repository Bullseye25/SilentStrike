using System;
using System.Collections;
using System.Collections.Generic;
using ControlFreak2.Demos.Guns;
using DG.Tweening;
using UnityEngine;

public class Level33LadderEnemy : MonoBehaviour
{
    public GameObject enemyIndicator, enemyBody, gun;
    public Animator animator;
    public GameObject civilian;
    private bool killed;
    
    public GameObject wayPointRoot;
    private Tween _myTween;
    private WayPoint _myWayPoint;
    private List<Vector3> _waypointList = new List<Vector3>();

    private void Start()
    {
        _myWayPoint = wayPointRoot.GetComponent<WayPoint>();

        foreach (var waypoint in _myWayPoint.waypoints)
        {
            _waypointList.Add(waypoint.position);
        }
    }

    void OnPatrollingWayPointChangeCallback(int wayPointIndex)
    {
        if (wayPointIndex == _waypointList.Count - 2)
        {
            animator.SetBool("end", true);
        }
        if (wayPointIndex == _waypointList.Count - 1)
        {
            gun.SetActive(true);
            animator.SetBool("shoot", true);
            transform.DOPause();
            _myTween.Kill();
            Invoke(nameof(FailLevel), 2f);
        }
        if (wayPointIndex == _waypointList.Count)
            Invoke(nameof(FailLevel), 2f);

    }

    public void StartClimbing()
    {
        animator.SetBool("start", true);
        
        transform.position = _waypointList[0];
        enemyBody.transform.localEulerAngles = Vector3.zero;
        _myTween = transform.DOPath(_waypointList.ToArray(), 20f, PathType.Linear, PathMode.Full3D, 10,
                Color.grey).SetLoops(0).SetOptions(false).SetEase(Ease.Linear)
            .OnWaypointChange(OnPatrollingWayPointChangeCallback);
    }

    public void FailLevel()
    {
        StartCoroutine(FailLevelDelay());
    }

    IEnumerator FailLevelDelay()
    {
        civilian.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        LevelsManager.instance.weaponCamera.GetComponent<Camera>().enabled = false;
        LevelsManager.instance.cfUi.SetActive(false);
        LevelsManager.instance.fpBody.SetActive(false);
        MConstants.isPlayerWin = false;
        HudMenuManager.instance.GameOver();
    }
    public void TakeDamage(int damage)
    { 
        animator.enabled = false; 
        killed = true; 
        HudMenuManager.instance.EnemyKilled();
        if(enemyIndicator != null) enemyIndicator.gameObject.SetActive(false);
    }
}
