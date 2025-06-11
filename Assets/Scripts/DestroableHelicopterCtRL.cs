using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DestroableHelicopterCtRL : MonoBehaviour
{
    public GameObject Explosion;
    public GameObject[] Smoke;
    public GameObject[] Pieces;
   public DOTweenPath DOTweenPath;
     int FireNumber = 0;
    public float Health=100;
    private Transform myTransform;

    public Transform player;
    public float maxAngle;
    public float maxRadius;

    public bool isInFov = false, isDead = false;
    public GameObject gun, parentRoot, muzzle;
    public float PlayerDamage;
   

    private WeaponEffects WeaponEffectsComponent;
    private Vector3 rayOrigin;
    private Vector3 targetDir;
    private Vector3 targetPos;
    public LayerMask playerMask;

    public bl_MiniMapItem MMIcon;
   

  //  public Collider[] rigColliders;
  //  public Rigidbody[] rigRigidbodies;
    private void OnDrawGizmos()
    {
        if (player)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, maxRadius);

            Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
            Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, fovLine1);
            Gizmos.DrawRay(transform.position, fovLine2);

            if (!isInFov)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * maxRadius);

            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position, transform.forward * maxRadius);

        }


    }

    public bool inFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {

        Collider[] overlaps = new Collider[100];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        for (int i = 0; i < count + 1; i++)
        {

            if (overlaps[i] != null)
            {

                if (overlaps[i].transform == target)
                {

                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                    if (angle <= maxAngle)
                    {

                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);

                        if (Physics.Raycast(ray, out RaycastHit hit, maxRadius, playerMask))
                        {

                            if (hit.transform == target)
                                return true;

                        }

                    }


                }

            }

        }

        return false;
    }

    private void Update()
    {
        if (!isDead && !MConstants.isGameOver)
        {
            isInFov = inFOV(transform, player, maxAngle, maxRadius);
            if (isInFov)
            {
                gun.transform.LookAt(player.transform);
                parentRoot.transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
                if (GetComponent<SmoothFollow>() != null)
                {
                    GetComponent<SmoothFollow>().target = player;
                    if (DOTweenPath != null)
                    {
                        DOTweenPath.DOKill();
                    }
                }
                //parentRoot.transform.eulerAngles = Vector3.RotateTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z),0.5f,1);
            }
            else
            {
                muzzle.SetActive(false);
            }
        }

    }

    private void Start()
    {
        myTransform = transform;
        player = FPSPlayer.instance.gameObject.transform;
        targetPos = player.transform.position;
        WeaponEffectsComponent = player.GetComponent<FPSPlayer>().weaponObj.GetComponent<WeaponEffects>();

        MMIcon = GetComponentInChildren<bl_MiniMapItem>();
        StartCoroutine("StartFiring");
        if(MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
        {
            this.gameObject.transform.LookAt(player.transform);
        }
      //  DisableRagdol();
    }
    //public void DisableRagdol()
    //{
    //    rigColliders = Ragdol.GetComponentsInChildren<Collider>();
    //    rigRigidbodies = Ragdol.GetComponentsInChildren<Rigidbody>();


    //    foreach (Rigidbody rb in rigRigidbodies)
    //    {
    //        rb.isKinematic = true;
    //    }
    //}

    IEnumerator StartFiring()
    {
        while (player.GetComponent<FPSPlayer>().hitPoints >= 0f)
        {
            yield return new WaitUntil(() => isInFov == true);
            muzzle.SetActive(!muzzle.activeSelf);
            yield return new WaitForSeconds(0.03f);
            int random = Random.Range(0, 100);
            if (random > 85)
            {
                player.GetComponent<FPSPlayer>().ApplyDamage(PlayerDamage);

            }
            rayOrigin = new Vector3(myTransform.position.x, myTransform.position.y + 2f, myTransform.position.z);
            targetPos = player.transform.position;
            targetDir = (targetPos - rayOrigin).normalized;
            if (muzzle)
            {
                WeaponEffectsComponent.BulletTracers(targetDir, muzzle.transform.position, -3.0f, 0.0f, false);
            }
        }
    }
    public bool isADrown = false;
    public void EnemyDie()
    {
        //StopCoroutine("StartFiring");
        //MMIcon.HideItem();
        //isInFov = false;
        //isDead = true;
        //muzzle.SetActive(false);
        //if (this.gameObject.GetComponent<AudioSource>())
        //{
        //    this.gameObject.GetComponent<AudioSource>().enabled = false;
        //}
        //if (!isADrown)
        //{
        // //   HudMenuManager.instance.EnemyKilled();

        //    //foreach (Rigidbody rb in rigRigidbodies)
        //    //{
        //    //    rb.isKinematic = false;
        //    //}

        //   // Invoke("DisableBody", 10f);
        //}
        //else
        //{
        //    DOTweenPath.DOKill();
        //    GetComponent<SmoothFollow>().enabled = false;

        //    //foreach (var item in rigColliders)
        //    //{
        //    //    item.enabled = true;
        //    //    if (item.GetComponent<DOTweenAnimation>())
        //    //    {
        //    //        item.GetComponent<DOTweenAnimation>().DOKill();

        //    //    }
        //    //    item.gameObject.AddComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 1f)) * 100);

        //    //}
        //   // Invoke("DisableBody", 10f);
        //}
    }

    //void DisableBody()
    //{
    //    Ragdol.SetActive(false);
    //}
    //private void Start()
    //{
        
    //}
    public void HelicoperGotShotted(Vector3 pos,float damage)
    {
        if (isDead == false )
        {
            Health -=damage;
            if(Health > 0)
            {
                if (FireNumber < 3)
                {
                    Smoke[FireNumber].transform.position = pos;
                    Smoke[FireNumber].SetActive(true);

                }
            }
           
            else
            {
                Explosion.SetActive(true);
                foreach (var item in Pieces)
                {
                   item.GetComponent<Rigidbody>().gameObject.layer = 22;
                    item.transform.parent = null;
                    item.GetComponent<DOTweenAnimation>().DOKill();
                    item.GetComponent<Rigidbody>().isKinematic = false;
                    item.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1000, 1000), Random.Range(0, 1000), Random.Range(-1000, 1000)));
                    if (DOTweenPath != null)
                    {
                        DOTweenPath.DOKill();
                    }

                }
                StopCoroutine("StartFiring");
                MMIcon.HideItem();
                isInFov = false;
                isDead = true;
                muzzle.SetActive(false);
                GetComponent<Rigidbody>().gameObject.layer = 22;
                GetComponent<Rigidbody>().isKinematic = false;
                if (this.gameObject.GetComponent<AudioSource>())
                {
                    this.gameObject.GetComponent<AudioSource>().enabled = false;
                }
                StartCoroutine("ExplosionAfterDeley");
            }
            FireNumber++;
        }
    }

    IEnumerator ExplosionAfterDeley()
    {
        yield return new WaitForSeconds(3f);
        this.GetComponent<SmoothFollow>().enabled = false;
        HudMenuManager.instance.EnemyKilled();
        yield return new WaitForSeconds(3f);
        GetComponent<Rigidbody>().isKinematic = true;
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
        {
            foreach (var item in Pieces)
            {
                Destroy(item.gameObject, 2f);

            }
        }
    }
}
