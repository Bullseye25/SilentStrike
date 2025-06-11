using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CrowdAI : MonoBehaviour
{
    [HideInInspector] public int health;
    public float walkSpeed = 1;
    public float runningSpeed = 2;
    public GameObject escapeWayPointRoot;
    public GameObject patrollingWayPointRoot;
    
    private Animator _animator;
    public bl_MiniMapItem miniMapItem;
    private IEnumerator alertCoroutine;
    private Tween MyTween;
    private WayPoint _myWayPoint, _myEscapeWayPoint;
    private List<Vector3> waypointList = new List<Vector3>();
    private List<Vector3> escapeWaypointList = new List<Vector3>();
    private int _failCount = 0;
    [HideInInspector] public bool killed, alerted;
    
    private static readonly int Panic = Animator.StringToHash("panic");
    private static readonly int Run = Animator.StringToHash("run");

    void Start()
    {
        _animator = GetComponent<Animator>();
        miniMapItem = GetComponentInChildren<bl_MiniMapItem>();
        alertCoroutine = Alert();

        if (LevelsManager.instance.currentLevel.alertLevel == Level.AlertLevel.HighAlert)
        {
            runningSpeed *= 2;
        }

        if (patrollingWayPointRoot)
        {
            _myWayPoint = patrollingWayPointRoot.GetComponent<WayPoint>();

            foreach (var waypoint in _myWayPoint.waypoints)
            {
                waypointList.Add(waypoint.position);
            }
            transform.position = waypointList[0];

            MyTween = transform.DOPath(waypointList.ToArray(), 20f, PathType.Linear, PathMode.Full3D, 10,
                    Color.grey).SetLookAt(0.01f).SetLoops(-1).SetOptions(true).SetEase(Ease.Linear);
            MyTween.timeScale = walkSpeed;
        }
    }

    void Update()
    {
        if (HudMenuManager.instance.RegisteredEnemyToKill < LevelsManager.instance.currentLevel.RegisterEnemyToKill && !alerted && !killed)
        {
            alerted = true;
            StartCoroutine(Alert());
        }

        if (MConstants.BulletFired && !MConstants.IslastBullet &&
            Vector3.Distance(transform.position, MConstants.BulletInfo) < 10 && !killed && !alerted)
        {
            alerted = true;
            if (MyTween != null)
            {
                transform.DOPause();
                MyTween.Kill();
            }
            StartCoroutine(Alert());
        }
    }

    public void TakeDamage(int damage)
    {
        _animator.enabled = false;
        killed = true;
        MyTween.Kill();
        transform.DOPause();
        HudMenuManager.instance.EnemyKilled();
        if(miniMapItem) miniMapItem.HideItem();
    }

    private IEnumerator Alert()
    {
        if (escapeWayPointRoot == null)
        {
            _failCount++;
            if (_failCount == 2)
            {
                _failCount = 0;
                //FailTheMission();
            }
            yield break;
        }
        
        SetAnimation(Panic);
        yield return new WaitForSeconds(5f);
        SetAnimation(Run);
        
        _myEscapeWayPoint = escapeWayPointRoot.GetComponent<WayPoint>();
        _myEscapeWayPoint.waypoints[0].position = transform.position;

        foreach (var waypoint in _myEscapeWayPoint.waypoints)
        {
            escapeWaypointList.Add(waypoint.position);
        }
        
        if(MyTween != null) MyTween.Kill();
        if (killed) yield break;

        if (!MConstants.IslastBullet)
        {
            MyTween = transform.DOPath(escapeWaypointList.ToArray(), 20f, PathType.Linear, PathMode.Full3D, 10,
                    Color.grey).SetLookAt(0.01f).SetLoops(0).SetOptions(false).SetEase(Ease.Linear)
                .OnWaypointChange(OnEscapeWayPointChangeCallback);
            MyTween.timeScale = runningSpeed;
        }
    }

    void OnEscapeWayPointChangeCallback(int wayPointIndex)
    {
        if (wayPointIndex == escapeWaypointList.Count - 1)
        {
            //FailTheMission();
        }
    }
    
    void SetAnimation(int id)
    {
        if (id != Panic)
            _animator.SetBool(Panic, false);
        if (id != Run)
            _animator.SetBool(Run, false);

        _animator.SetBool(id, true);
    }

    public void WaitForGunShot()
    {
        MyTween.Kill();
        transform.DOPause();
        StopCoroutine(alertCoroutine);
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
