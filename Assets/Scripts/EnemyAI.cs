using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EPOOutline;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public int health = 100;
    public float walkSpeed = 1;
    public float runningSpeed = 2;
    public EnemyIndicator enemyIndicator;
    public GameObject muzzleFlash;
    public GameObject escapeWayPointRoot;
    public GameObject patrollingWayPointRoot;
    public AudioClip dieSound;
    public bool patrolWalkLoop = true;
    public bool dontLookAt;
    public bool shootAtPlayerIfMiss, shootAtPlayerWithoutMiss;

    private Outlinable _outlinable;
    private HealthBarLookAt _healthBar;
    private int _fullHealth;
    private bool _runningEnemy;
    [HideInInspector] public Animator animator;
    private IEnumerator _alertCoroutine;
    private Tween _myTween;
    private WayPoint _myPatrollingWayPoint, _myEscapeWayPoint;
    private List<Vector3> _patrollingWaypointList = new List<Vector3>();
    private List<Vector3> _escapeWaypointList = new List<Vector3>();
    private int _failCount = 0;
    private float _distance, _walkTime, _runTime;
    [HideInInspector] public bool killed, alerted, highlighted;
    [HideInInspector] public GameObject _myLineRenderer;
   
    //----------Squid Mode Variables
    [HideInInspector] public Transform movePos;
    private NavMeshAgent _navMeshAgent;
    private bool reached = true;
    //----------
    
    private static readonly int Idle = Animator.StringToHash("idle");
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int Panic = Animator.StringToHash("panic");
    private static readonly int Run = Animator.StringToHash("run");
    private static readonly int Walk = Animator.StringToHash("walk");

    public ZombieWave myWave;
    [HideInInspector]
    public Rigidbody[] rigidBodies;
    [HideInInspector]
    HealthElement healthElement;
    [HideInInspector]
    public bool isVehicleHealthChange = false;
    public GameObject fakeShadow;

    void Start()
    {
        if(MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            healthElement = GetComponentInChildren<HealthElement>();
            healthElement.currentHealth = health;
        }
        animator = GetComponent<Animator>();
        _outlinable = GetComponent<Outlinable>();
        _healthBar = GetComponentInChildren<HealthBarLookAt>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        
        if (_healthBar) _healthBar.gameObject.SetActive(false);
        _fullHealth = health;
        _alertCoroutine = Alert();

        if (LevelsManager.instance.currentLevel.alertLevel == Level.AlertLevel.HighAlert)
        {
            runningSpeed *= 2;
        }

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            int x = Random.Range(1, 1000);
            
            if (x <= 330)
                walkSpeed *= 5;
            else if (x <= 660)
                walkSpeed *= 3;
            else
                walkSpeed *= 2;
        }

        if (patrollingWayPointRoot)
        {
            _myPatrollingWayPoint = patrollingWayPointRoot.GetComponent<WayPoint>();

            foreach (var waypoint in _myPatrollingWayPoint.waypoints)
            {
                _patrollingWaypointList.Add(waypoint.position);
            }

            for (int i = 0; i < _myPatrollingWayPoint.waypoints.Length - 1; i++)
            {
                _distance += Vector3.Distance(_myPatrollingWayPoint.waypoints[i].position, _myPatrollingWayPoint.waypoints[i + 1].position);
            }
            _walkTime = _distance / 0.5f;
            // _runTime = _distance / runningSpeed;
            // transform.position = _patrollingWaypointList[0];

            if (!(LevelsManager.instance.currentLevel.startLevelAfterInstruction ))
            {
                if (dontLookAt)
                {
                    _myTween = transform.DOPath(_patrollingWaypointList.ToArray(), _walkTime, PathType.Linear, PathMode.Full3D, 10,
                            Color.grey).SetLoops(patrolWalkLoop? -1 : 0).SetOptions(patrolWalkLoop).SetEase(Ease.Linear)
                        .OnWaypointChange(OnPatrollingWayPointChangeCallback);
                }
                else
                {
                    _myTween = transform.DOPath(_patrollingWaypointList.ToArray(), _walkTime, PathType.Linear, PathMode.Full3D, 10,
                            Color.grey).SetLookAt(0.01f).SetLoops(patrolWalkLoop? -1 : 0).SetOptions(patrolWalkLoop).SetEase(Ease.Linear)
                        .OnWaypointChange(OnPatrollingWayPointChangeCallback);
                }
                
                _myTween.timeScale = walkSpeed;
            }
        }

        if (LevelsManager.instance.currentLevel.runningLevel)
        {
            _runningEnemy = true;
            MConstants.StartRunningInRunningMission = false;
        }

        //if(MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE1)
        //{
            rigidBodies = GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < rigidBodies.Length; i++)
            {
                rigidBodies[i].isKinematic = true;
                rigidBodies[i].transform.tag = "Flesh";
            }
       // }
      
    }

    public void StartWayPointRoot()
    {
        if (!patrollingWayPointRoot) return;
        
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            return;
        }
        
        SetAnimation(Walk);
        
        if (dontLookAt)
        {
            _myTween = transform.DOPath(_patrollingWaypointList.ToArray(), _walkTime, PathType.Linear, PathMode.Full3D, 10,
                    Color.grey).SetLoops(patrolWalkLoop? -1 : 0).SetOptions(patrolWalkLoop).SetEase(Ease.Linear)
                .OnWaypointChange(OnPatrollingWayPointChangeCallback);
        }
        else
        {
            _myTween = transform.DOPath(_patrollingWaypointList.ToArray(), _walkTime, PathType.Linear, PathMode.Full3D, 10,
                    Color.grey).SetLookAt(0.01f).SetLoops(patrolWalkLoop? -1 : 0).SetOptions(patrolWalkLoop).SetEase(Ease.Linear)
                .OnWaypointChange(OnPatrollingWayPointChangeCallback);
        }

        _myTween.timeScale = walkSpeed;
    }

    void Update()
    {
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie && (LevelsManager.instance.currentLevel.vehicles && isVehicleHealthChange))
        {
            float random = Random.Range(0, 100);
            if (random < 5)
            {
                LevelsManager.instance.currentLevel.vehicleHealth -= 0.1f;
                LevelsManager.instance.currentLevel.vehicleHealthElement.OnHealthChanged(LevelsManager.instance.currentLevel.vehicleHealth);
                if (LevelsManager.instance.currentLevel.vehicleHealth <= 0)
                {
                    isVehicleHealthChange = false;
                    LevelsManager.instance.currentLevel.vehicleHealthElement.ToRemoveHealthBar();
                    MConstants.isPlayerWin = false;
                    HudMenuManager.instance.GameOver();
                }

            }
        }

        if (ExplosiveDrum.instance)
        {
            if (ExplosiveDrum.instance.exploded)
            {
                return; //for level 19
            }
        }
        
        if (HudMenuManager.instance.RegisteredEnemyToKill < LevelsManager.instance.currentLevel.RegisterEnemyToKill 
            && !alerted && !killed && !LevelsManager.instance.currentLevel.runningLevel)
        {
            alerted = true;
            StartCoroutine(Alert());
        }

        if (MConstants.BulletFired && !MConstants.IslastBullet &&
            Vector3.Distance(transform.position, MConstants.BulletInfo) < 10 && !killed && !alerted 
            && !LevelsManager.instance.currentLevel.runningLevel)
        {
            alerted = true;
            StartCoroutine(Alert());
        }
        
        if (((shootAtPlayerIfMiss && Vector3.Distance(transform.position, MConstants.BulletInfo) < 50) || shootAtPlayerWithoutMiss)
            && !MConstants.IslastBullet && !killed)
        {
            if (LevelsManager.instance.fpBody != null)
            {
                Vector3 lookAtPosition = LevelsManager.instance.fpBody.transform.position;
                lookAtPosition.y = transform.position.y;
                transform.LookAt(lookAtPosition);
            }
        }
        
        if (_runningEnemy && MConstants.StartRunningInRunningMission)
        {
            _runningEnemy = false;
            StartRunning();
        }

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid && !reached)
        {
            float dis = Vector3.Distance(transform.position, movePos.position);
            if(dis < 1f && !killed)
            {
                reached = true;
                _navMeshAgent.speed = 0;
                    
                animator.SetBool("celebrate", true);
                SquidModeController.instance.remainingEnemies--;
                SquidModeController.instance.TotalEnemiesList.Remove(this);
                if (SquidModeController.instance.remainingEnemies == 0)
                {
                    HudMenuManager.instance.GameComplete();
                }
                    
            }
        }
    }

    public void BeforeExplosion()
    {
        for (int i = 0; i < rigidBodies.Length; i++)
        {
            rigidBodies[i].isKinematic = false;
        }
        if (enemyIndicator != null)
        {
            enemyIndicator.gameObject.SetActive(false);
        }
        if(fakeShadow != null)
        {
            fakeShadow.SetActive(false);
        }
        killed = true;
        _myTween.Kill();
        transform.DOPause();
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            healthElement.OnHealthChanged(health);
        }
           
        UpdateHealthBar(health);
        
        if (health > 0)
            return;

    
        for (int i = 0; i < rigidBodies.Length; i++)
        {
                rigidBodies[i].isKinematic = false;
        }

        animator.enabled = false;
        killed = true;
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            healthElement.ToRemoveHealthBar();
        }

        if (enemyIndicator != null) enemyIndicator.gameObject.SetActive(false);
        if(muzzleFlash != null) muzzleFlash.SetActive(false);
        if (dieSound) LevelsManager.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(dieSound);

        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid && highlighted)
        {
            CheckSquidModeEnemiesCount();
        }
        else  if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            FailSquidMode();
        }

        if (myWave)
        {
            myWave.RemoveEnemy(gameObject);
        }
        _myTween.Kill();
        transform.DOPause();
        HudMenuManager.instance.EnemyKilled();
        DestroyBody();

        if (GetComponent<RagdollController>())
        {
            GetComponent<RagdollController>().RagDoll(true);
        }
        
        if (GetComponentInParent<CarDestroyer>())
        {
            GetComponentInParent<CarDestroyer>().transform.DOPause();
        }
        
        if (GetComponent<HostageRelease>())
        {
            GetComponent<HostageRelease>().Release();
        }

        if (GetComponent<Expert16AlertOthers>())
        {
            GetComponent<Expert16AlertOthers>().AlertOtherEnemies();
        }

        if (LevelsManager.instance.currentLevel.transform.GetComponentInChildren<MovingPlayerMissionController>()
        && HudMenuManager.instance.RegisteredEnemyToKill > 0)
        {
            LevelsManager.instance.currentLevel.transform.GetComponentInChildren<MovingPlayerMissionController>().GoToNextEnemy();
        }

        if (LevelsManager.instance.currentLevel.gameObject.GetComponent<ZombieHordeManager>())
        {
            LevelsManager.instance.currentLevel.gameObject.GetComponent<ZombieHordeManager>().HordeManage();
        }
        /*else if (LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].transform.GetComponentInChildren<HelicopterMissionController>()
                 && HudMenuManager.instance.RegisteredEnemyToKill <= 0)
        {
            LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].transform
                    .GetComponentInChildren<HelicopterMissionController>().gameObject.GetComponent<AudioSource>().mute =
                true;
        }*/
    }
    
    void UpdateHealthBar(int _health)
    {
        if (_healthBar == null) return;
        
        if (_health > 0)
        {
            _healthBar.gameObject.SetActive(true);
            _healthBar.fillBar.fillAmount = (float) health / _fullHealth;
        }
        else if (_health < 1)
        {
            _healthBar.gameObject.SetActive(false);
        }
    }

    void DestroyBody()
    {
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
            if(LevelsManager.instance.currentLevel.isMachineGunLevel
            || HudMenuManager.instance.RegisteredEnemyToKill != 0)
                gameObject.SetActive(false);

        // Destroy(gameObject, 2f);
    }

    private IEnumerator Alert()
    {
        if (escapeWayPointRoot == null)
        {
            _failCount++;
            if (_failCount == 2 && !MConstants.isGameOver)
            {
                _failCount = 0;
                FailTheMission();
            }
            yield break;
        }
        if (_myTween != null)
        {
            transform.DOPause();
            _myTween.Kill();
        }
        SetAnimation(Panic);
        yield return new WaitForSeconds(2f);
        SetAnimation(Run);
        
        _myEscapeWayPoint = escapeWayPointRoot.GetComponent<WayPoint>();
        _myEscapeWayPoint.waypoints[0].position = transform.position;

        foreach (var waypoint in _myEscapeWayPoint.waypoints)
        {
            _escapeWaypointList.Add(waypoint.position);
        }
        
        if(_myTween != null) _myTween.Kill();

        if (!MConstants.IslastBullet)
        {
            _myTween = transform.DOPath(_escapeWaypointList.ToArray(), 20f, PathType.Linear, PathMode.Full3D, 10,
                    Color.grey).SetLookAt(0.01f).SetLoops(0).SetOptions(false).SetEase(Ease.Linear)
                .OnWaypointChange(OnEscapeWayPointChangeCallback);
            _myTween.timeScale = runningSpeed;
        }
    }

    void OnPatrollingWayPointChangeCallback(int wayPointIndex)
    {
        if (!patrolWalkLoop)
        {
            if (wayPointIndex == _patrollingWaypointList.Count)
            {
                transform.DOPause();
                _myTween.Kill();
                SetAnimation(Idle);
            }
            // print(gameObject.name + " = " + wayPointIndex);
            if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie && wayPointIndex == _patrollingWaypointList.Count - 1)
            {
                transform.DOPause();
                _myTween.Kill();
                SetAnimation(Attack);
                isVehicleHealthChange = true;
                // HudMenuManager.instance.zoomOut();
               // if(LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].gameObject.GetComponent<ZombieHordeManager>()) 
              
               if(LevelsManager.instance.currentLevel.isZombiesAttack)
                    StartAttacking();
            }
        }
    }
    
    void OnEscapeWayPointChangeCallback(int wayPointIndex)
    {
        if (wayPointIndex == _escapeWaypointList.Count && !MConstants.isGameOver)
        {
            FailTheMission();
        }
        
        if (wayPointIndex == _escapeWaypointList.Count - 1 && !MConstants.isGameOver
        && MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            FailTheMission();
        }
    }

    public void StartRunning()
    {
        if(escapeWayPointRoot == null) return;
        alerted = true;
        SetAnimation(Run);
        
        _myEscapeWayPoint = escapeWayPointRoot.GetComponent<WayPoint>();
        _myEscapeWayPoint.waypoints[0].position = transform.position;

        foreach (var waypoint in _myEscapeWayPoint.waypoints)
        {
            _escapeWaypointList.Add(waypoint.position);
        }
        transform.position = _escapeWaypointList[0];

        _myTween = transform.DOPath(_escapeWaypointList.ToArray(), 20f, PathType.Linear, PathMode.Full3D, 10,
                Color.grey).SetLookAt(0.01f).SetLoops(0).SetOptions(false).SetEase(Ease.Linear)
            .OnWaypointChange(OnEscapeWayPointChangeCallback);
        _myTween.timeScale = runningSpeed;
    }
    
    public void SetAnimation(int id)
    {
        if (id != Idle)
            animator.SetBool(Idle, false);
        if (id != Panic)
            animator.SetBool(Panic, false);
        if (id != Run)
            animator.SetBool(Run, false);
        if (id != Walk)
            animator.SetBool(Walk, false);
        if (id != Attack)
            animator.SetBool(Attack, false);
        
        animator.SetBool(id, true);
    }

    public void WaitForGunShot()
    {
        _myTween.Kill();
        transform.DOPause();
        StopCoroutine(_alertCoroutine);
        animator.speed = 0;
        if (GetComponentInParent<CarDestroyer>())
        {
            GetComponentInParent<CarDestroyer>().transform.DOPause();
        }
    }

    public void CheckSquidModeEnemiesCount()
    {
        GetComponent<Outlinable>().enabled = false;
        Destroy(_myLineRenderer);
        Destroy(gameObject, 2f);
        SquidModeController.instance.noOfTargets--;
        SquidModeController.instance.remainingEnemies--;
        if (SquidModeController.instance.remainingEnemies == 0)
        {
            HudMenuManager.instance.GameComplete();
        }
        
        if (SquidModeController.instance.noOfTargets == 0)
        {
            SquidModeController.instance.StartDollMusicAgain();
            if (ZoomSlider.instance)
            {
                ZoomSlider.instance.zoomslider.value = 6f;
                ZoomSlider.instance.ChangeZoomValue();
            }
        }
    }

    public void StartSquidRun()
    {
        reached = false;
        _navMeshAgent.SetDestination(movePos.position);
        PlayForSquidMode();
    }

    public void PlayForSquidMode()
    {
        SetAnimation(Walk);
        var moveSpeed = Random.Range(0.8f, 1f);
        _navMeshAgent.speed = moveSpeed;
    }
    public void PauseForSquidMode()
    {
        SetAnimation(Idle);
        _navMeshAgent.speed = 0;
    }

    public void KillTween()
    {
        _myTween.Kill();
        transform.DOPause();
        StopCoroutine(_alertCoroutine);
    }

    void FailTheMission()
    {
        LevelsManager.instance.weaponCamera.GetComponent<Camera>().enabled = false;
        LevelsManager.instance.cfUi.SetActive(false);
        LevelsManager.instance.fpBody.SetActive(false);
        MConstants.isPlayerWin = false;
        HudMenuManager.instance.GameOver();
    }

    void FailSquidMode()
    {
        LevelsManager.instance.weaponCamera.GetComponent<Camera>().enabled = false;
        LevelsManager.instance.cfUi.SetActive(false);
        LevelsManager.instance.fpBody.SetActive(false);
        MConstants.isPlayerWin = false;
        SquidModeController.instance.GuardsShootAtPlayer();
        if (ZoomSlider.instance)
        {
            ZoomSlider.instance.zoomslider.value = 6f;
            ZoomSlider.instance.ChangeZoomValue();
        }
        Invoke(nameof(FailTheMission), 2f);
        // HudMenuManager.instance.GameOver();
    }

    void StartAttacking()
    {
        if (!killed)
        {
            ZombieSoundController.instance.StartHitSounds();
            HudMenuManager.instance.redAlert.SetActive(!MConstants.isGameOver);
            int random = Random.Range(0, 100);
            if (random > 85)
            {
                FPSPlayer.instance.ApplyDamage(10);
            }
        }
        if (!killed)
        {
            Invoke(nameof(StartAttacking),1f);
        }
        else
        {
            HudMenuManager.instance.redAlert.SetActive(false);
        }
        if (!killed && MConstants.isGameOver)
        {
            if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
            {
                if (LevelsManager.instance.currentLevel.isMachineGunLevel)
                {
                    ZombieSoundController.instance.StopZombieTauntSounds();
                    ZombieSoundController.instance.StopHitSounds();
                }
                else
                {
                    ZombieSoundController.instance.StopZombieSniperMissionSounds();
                }
            }
        }
      
    }
}