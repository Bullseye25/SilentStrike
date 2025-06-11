using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using DG.Tweening;
public class TargetDetection : MonoBehaviour
{
    //private Transform myTransform;
    //  public LayerMask playerMask;
    // public bl_MiniMapItem MMIcon;

    public float maxAngle;
    public float MinTargetRadius=10;
    public float MaxTargetRadius=20;
    public float MinShootDuration=3;
    public float MaxShootDuration = 8;
    public float MinAttackDelay;
    public float MaxAttackDelay;
   // public GameObject parentRoot;
    public float Damagetoplayer;

    public LayerMask Fovdetectedlayer;
  public Transform player;
    bool isInFov = false, isDead = false;
    RobotController Robot;
    public bool leftGunType = true, RightGunType = true, CenterGunType;
    public Guncontroller LeftGun, RightGun, CenterGun;
    public Guncontroller []GunsArray;

    float GunActivetime = 0;
    public float maxRadius;
    public bool IsHeli, Isrobot=true;
    public SplineInterpolator splineinterpolatorobject;
    [HideInInspector]
    public bool IsbossAi=true;
    float Attackdelay;
    bool StartAttack=false;
    bool startfire = true;
    public bl_MiniMapItem MMIcon;
    public bool stoponfire=true;
    public GameObject turretSoundEffect;
    private void Awake()
    {
        if (MMIcon == null)
        {
            MMIcon = GetComponentInChildren<bl_MiniMapItem>();
        }

        if (splineinterpolatorobject == null && IsHeli)
        {
            splineinterpolatorobject = GetComponent<SplineInterpolator>();
        }
       
      // MaxAttackDelay = 
        Invoke("startfiredelay", Random.Range(1, MinAttackDelay));
    }

    
    void startfiredelay()
    {
        StartAttack = true;
    }
    //private void OnDrawGizmos()
    //{
    //    if (player)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawWireSphere(transform.position, maxRadius);

    //        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
    //        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawRay(transform.position, fovLine1);
    //        Gizmos.DrawRay(transform.position, fovLine2);

    //        if (!isInFov)
    //            Gizmos.color = Color.red;
    //        else
    //            Gizmos.color = Color.green;
    //        Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * maxRadius);

    //        Gizmos.color = Color.black;
    //        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);

    //    }


    //}
  
    private void Start()
    {
        maxRadius = MinTargetRadius;
        if (HudMenuManager.instance.player != null)
        {
            player = HudMenuManager.instance.player.transform;
        }
        // GunActivetime = Random.Range(MinAttackDelay, MaxAttackDelay);

    }
    public bool inFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        if (HudMenuManager.instance.player.hitPoints <= 0|| !StartAttack)
        {
            return false;
        }

        Collider[] overlaps = new Collider[100];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps, Fovdetectedlayer);

        for (int i = 0; i < count + 1; i++)
        {

            if (overlaps[i] != null)
            {

                if (overlaps[i].transform == target)
                {

                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                    if (angle <= maxAngle+2f)
                    {

                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                      //  return true;
                        if (Physics.Raycast(ray, out RaycastHit hit, maxRadius,Fovdetectedlayer))
                        {

                            if (hit.transform == target)
                                return true;

                        }

                    }


                }

            }

        }
        //if(MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE && Isrobot && !Robot.isMoving)
        //{
        //    Robot.isMoving = true;
        //}
        return false;
    }

    private void Update()
    {
        if (!player && HudMenuManager.instance.player)
        {
            player = HudMenuManager.instance.player.transform;
        }
       
        if (GunActivetime > 0)
        {
            GunActivetime -= 1 * Time.deltaTime;
        }
        else
        {
            turretSoundEffect.SetActive(false);

        }
        if (!LeftGun && !RightGun && CenterGun && !CenterGun.gameObject.activeSelf)
        {
            CenterGun.gameObject.SetActive(true);
            CenterGunType = true;
        }
        if (!isDead)
        {
            isInFov = inFOV(transform, player, maxAngle, maxRadius);
            if (isInFov)
            {
               
               
               
               if (splineinterpolatorobject != null && splineinterpolatorobject.mState != "Stopped"&&stoponfire)
                    {
                        splineinterpolatorobject.mState = "Stopped";
                    
                        GunActivetime = Random.Range(MinShootDuration, MaxShootDuration);
                        StopCoroutine("StartFiring");
                        StartCoroutine("StartFiring");
                        return;
                  }
                else if(startfire&&!stoponfire)
                 {
                    startfire = false;
                    GunActivetime = Random.Range(MinShootDuration, MaxShootDuration);
                    StopCoroutine("StartFiring");
                    StartCoroutine("StartFiring");
                    return;
                }
                   
                
                
            if(GunActivetime <=0)
                    {
                    if (stoponfire)
                    {
                        splineinterpolatorobject.mState = "Loop";
                    }
                    else
                    {
                        startfire = true;
                    }
                        
                            
                            StartCoroutine(StartSearch(Attackdelay= Random.Range(MinAttackDelay, MinAttackDelay)));
                            StopCoroutine("StartFiring");
                        
                          maxRadius = 0f;
                        // Invoke("ResetRotion", 1f);
                        
                    }
                  
                }
           
            else if (IsHeli &&(!startfire|| (splineinterpolatorobject != null && splineinterpolatorobject.mState == "Stopped"))&&player!=null)
            {
              // var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
             //  transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
                if (GunActivetime <= 0)
                {
                    StartCoroutine(StartMove());
                }
                //StopCoroutine("StartFiring");

            }
        }
     
    }
    IEnumerator StartMove( )
        {
      
            if (isInFov)
            {
           // StopCoroutine("StartFiring");
          //  StartCoroutine("StartFiring");
            yield return true;
           
        }
        GunActivetime = 0f;
       
      
            maxRadius = 0f;
        if (stoponfire)
        {
            ResetRotion();
            splineinterpolatorobject.mState = "Loop";
        }
        else
        {
            startfire = true;
        }
            StartCoroutine(StartSearch(Attackdelay));
       
        }
    IEnumerator StartSearch(float Delay)
    {
       
        yield return new WaitForSeconds(Delay);
        maxRadius = Random.Range(MinTargetRadius, MaxTargetRadius);
       
    }

    //IEnumerator SurvivalmodeFiring()
    //{
    //    while (player.GetComponent<FPSPlayer>().hitPoints >= 0f)
    //    {
    //        yield return new WaitUntil(() => isInFov == true);
    //        yield return new WaitForSeconds(0.1f);
    //        if (!LeftGun && !RightGun && !CenterGun)
    //        {
    //            isDead = true;

    //            yield return false;
    //        }
    //        int random = Random.Range(0, 100);
    //        if (RightGunType && RightGun)
    //        {
    //            RightGun.Fire(player);
    //        }
    //        if (leftGunType && LeftGun)
    //        {
    //            LeftGun.Fire(player);
    //        }
    //        if (CenterGun && CenterGunType)
    //        {
    //            CenterGun.Fire(player);
    //        }
        


    //        if (random > 80)
    //        {
    //            player.GetComponent<FPSPlayer>().ApplyDamage(Damagetoplayer);
               
    //        }

    //    }
    //}
    IEnumerator StartFiring()
        {
            while (GunActivetime >= 0f)
            {
                yield return new WaitUntil(() => isInFov == true);
                yield return new WaitForSeconds(0.1f);
                if ((!LeftGun && !RightGun && !CenterGun) && GunsArray.Length<0)
                {
                isDead = true;
               
                yield return false;
                }
                int random = Random.Range(0, 100);
            if (RightGunType && RightGun)
                {
                    RightGun.Fire(player);
                }
            if (leftGunType && LeftGun)
            {
                LeftGun.Fire(player);
            }
            if (CenterGun && CenterGunType)
            {
                CenterGun.Fire(player);
            }
            turretSoundEffect.SetActive(true);
            for (int i=0; i<GunsArray.Length; i++)
            {
                GunsArray[i].Fire(player);
            }
            //if (!LeftGun && !RightGun && CenterGun && !CenterGun.gameObject.activeSelf)
            //    {
            //        CenterGun.gameObject.SetActive(true);
            //        CenterGunType = true;
            //    }
           

            //    if (random > 80)
            //    {
            //    player.GetComponent<FPSPlayer>().ApplyDamage(Damagetoplayer);
            //    //  var effect = ObjectPool.instance.GetObjectForType("Blood12Slash_1", false);
            //    //if (effect != null)
            //    //{
            //    //    effect.transform.SetParent(null);
            //    //    effect.transform.position = new Vector3(HudMenuManager.instance.CurentPlayer.transform.position.x, HudMenuManager.instance.CurentPlayer.transform.position.y + 1.2F, HudMenuManager.instance.CurentPlayer.transform.position.z);
            //    //    effect.SetActive(true);
            //    //    ObjectPool.instance.PoolObject(effect, 4);
            //    //}

            //    //  var hit = new Hit(HudMenuManager.instance.CurentPlayer.transform.position, Vector3.zero, 5F, this.gameObject, HudMenuManager.instance.CurentPlayer, HitType.Rifle, 0);
            //    //  HudMenuManager.instance.CurentPlayer.SendMessage("OnHit", hit, SendMessageOptions.DontRequireReceiver);
            //}

            }

    }

    public void EnemyDie()
        {
           StopCoroutine("StartFiring");
        if(MMIcon)
            MMIcon.HideItem();
            isInFov = false;
            isDead = true; 
    //    GameObject projectile = AzuObjectPool.instance.SpawnPooledObj(19, this.transform.position, Quaternion.identity) as GameObject;//Spa
        if (this.gameObject.GetComponent<AudioSource>())
            {
                this.gameObject.GetComponent<AudioSource>().enabled = false;
            }
            //HudMenuManager.instance.EnemyKilled();
        //if (MConstants.CurrentGameMode == MConstants.GAME_MODES.ENDLESS_MODE)
        //{
        //    if (IsHeli&&GetComponent<SmoothFollow>()&& GetComponent<SmoothFollow>().enabled==true)
        //    {
        //        GetComponent<SmoothFollow>().enabled = false;
        //    }
        
        //}

        Destroy(GetComponent<FOVDetection>(),0.2f);
        
    }
    public void ResetRotion()
    {
        if (LeftGun)
        {
           LeftGun .transform.localRotation = Quaternion.identity;
        }
        if (RightGun)
        {
            RightGun.transform.localRotation = Quaternion.identity;
        }
        if (CenterGun)
        {
            CenterGun.transform.localRotation = Quaternion.identity;
        }
    }

   
    } 
  
       
         
   
  
