using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class MovingPlayerMissionController : MonoBehaviour
{
    public GameObject wayPointRoot;
    public float speed;
    public bool looped = true;
    [HideInInspector] public bool turnOffLevelEnemiesAtStart;
    
    public int[] slowPointNumbers;
    public GameObject[] levelEnemies;
    
    private Tween _myTween;
    private WayPoint _myWayPointRoot;
    private List<Vector3> _waypointList = new List<Vector3>();

    private int _currentEnemy;
    public AudioSource helicopterSound;
    IEnumerator Start()
    {
        if (wayPointRoot)
        {
            _myWayPointRoot = wayPointRoot.GetComponent<WayPoint>();

            foreach (var waypoint in _myWayPointRoot.waypoints)
            {
                _waypointList.Add(waypoint.position);
            }
        }

        if (slowPointNumbers.Length >= 1)
        {
            HudMenuManager.instance.EnableAllUi(false);
        }

        else 
        {
            SmoothMouseLook.instance.minimumX = -360;
            SmoothMouseLook.instance.maximumX = 360;
        }
        if(LevelsManager.instance.currentLevel.isHelicopterMission)
        {
            helicopterSound = GetComponent<AudioSource>();
            LevelsManager.instance.currentLevel.helicopterSound = helicopterSound;
        }
        
        yield return new WaitForSeconds(1f);
        if (levelEnemies.Length > 0)
        {
            foreach (var levelEnemy in levelEnemies)
            {
                levelEnemy.SetActive(false);
            }

            _currentEnemy = 0;
            levelEnemies[_currentEnemy].SetActive(true);
        }

        
    }

    public void StartHelicopter()
    {
        Invoke(nameof(StartHelicopterDelay), 0.2f);
    }

    public void StartHelicopterDelay()
    {
        if (_waypointList.Count > 0)
        {
            transform.position = _waypointList[0];
        }

        PlayerCharacter.instance.walkerComponent.transform.parent = transform;
        // PlayerCharacter.instance.walkerComponent.transform.position = Vector3.zero;
        PlayerCharacter.instance.walkerComponent.CameraObj.transform.parent = transform;
        // PlayerCharacter.instance.fpBodyObj.transform.parent = transform;
        PlayerCharacter.instance.walkerComponent.CameraObj.transform.localScale = Vector3.one;
        // playerParented = true;
        // parentState = true;

        if (_waypointList.Count > 0)
        {
            _myTween = transform.DOPath(_waypointList.ToArray(), 20f, PathType.Linear, PathMode.Full3D, 10,
                    Color.grey).SetLookAt(0.01f).SetLoops(looped? -1 : 0).SetOptions(looped).SetEase(Ease.Linear)
                .OnWaypointChange(OnPatrollingWayPointChangeCallback);
            _myTween.timeScale = speed;
        }
    }

    void OnPatrollingWayPointChangeCallback(int wayPointIndex)
    {
        if (slowPointNumbers != null)
        {
            if (slowPointNumbers.Contains(wayPointIndex))
            {
                transform.DOPause();
                HudMenuManager.instance.EnableAllUi();
            }
        }
        // else
        // {
        //     
        // }
    }

    public void GoToNextEnemy()
    {
        if (levelEnemies.Length > 0)
        {
            transform.DOPlay();
            HudMenuManager.instance.EnableAllUi(false);
            _currentEnemy++;
            levelEnemies[_currentEnemy].SetActive(true);

            if (HudMenuManager.instance.zoomedIn)
            {
                if (ZoomSlider.instance)
                {
                    ZoomSlider.instance.zoomslider.value = 6f;
                    ZoomSlider.instance.ChangeZoomValue();
                }
                else
                {
                    HudMenuManager.instance.zoomOut();
                }
            }

            HudMenuManager.instance.reloadBtn.SetActive(false);
        }
    }
}
