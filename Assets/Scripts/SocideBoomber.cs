using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SocideBoomber : MonoBehaviour
{

    NavMeshAgent agent;
    public GameObject Player, Explosion, AgentMesh;
    Animator animator;
    public float ShootCount;
    bool IsDie = false;
    public GameObject Body;
    bl_MiniMapItem MMIcon;

    private GameObject gunInst;

    public float maxAngle;
    public float maxRadius;
    private bool isInFov = false;

    private bool StartFolowing = false;


    void Start()
    {
        ShootCount *= 20;
        animator = this.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        MMIcon = GetComponentInChildren<bl_MiniMapItem>();
        Player = FPSPlayer.instance.gameObject;
    }
    public void DamageApplie(float Damage)
    {
        ShootCount -= Damage; ;
        if (ShootCount <= 0 && !IsDie)
        {
            if (MConstants.CurrentGameMode != MConstants.GAME_MODES.SURVIVAL_MODE)
            {
                HudMenuManager.instance.StopCoroutine("ShowNotification");
                HudMenuManager.instance.NotificationText.text = "";
            }
               IsDie = true;
            agent.isStopped = true;
            EnableAiRegdol();
            HudMenuManager.instance.EnemyKilled();
            MMIcon.HideItem();
            if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
            {
                if (FPSPlayer.instance.hitPoints < 30f)
                {
                    gunInst = Instantiate(LevelsManager.instance.HealthItem, this.gameObject.transform.position + Vector3.one, Quaternion.identity) as GameObject;


                }

                else if ((PlayerWeapons.instance.weaponOrder[PlayerWeapons.instance.Gun2 + 1].GetComponent<WeaponBehavior>().ammo <= 0 && PlayerWeapons.instance.weaponOrder[PlayerWeapons.instance.Gun2 + 1].GetComponent<WeaponBehavior>().bulletsLeft <= 0) || (PlayerWeapons.instance.weaponOrder[PlayerWeapons.instance.Gun1 + 1].GetComponent<WeaponBehavior>().ammo <= 0 && PlayerWeapons.instance.weaponOrder[PlayerWeapons.instance.Gun1 + 1].GetComponent<WeaponBehavior>().bulletsLeft <= 0))
                {
                    gunInst = Instantiate(LevelsManager.instance.HealthItem, this.gameObject.transform.position + Vector3.one, Quaternion.identity) as GameObject;
                }
                else
                {

                    if (MConstants.HealtInstLimit >= 5)
                    {
                        MConstants.GunInstLimit = 0;
                        MConstants.HealtInstLimit = 0;

                        gunInst = Instantiate(LevelsManager.instance.HealthItem, this.gameObject.transform.position + Vector3.one, Quaternion.identity) as GameObject;

                    }
                    else
                    {
                        if (MConstants.GunInstLimit >= 3)
                        {
                            int range = Random.Range(0, WaveDataController.Instance.Dropablegun.Length);
                            MConstants.GunInstLimit = 0;

                            gunInst = Instantiate(WaveDataController.Instance.Dropablegun[range], this.gameObject.transform.position + Vector3.one, Quaternion.identity) as GameObject;

                        }
                    }
                }


            }
        }
    }
    void Update()
    {
        if (MConstants.CurrentGameMode != MConstants.GAME_MODES.SURVIVAL_MODE && inFOV(transform, Player.transform, maxAngle, maxRadius)&&!StartFolowing )
        {
            StartFolowing = true;
            HudMenuManager.instance.NotificationText.text = HudMenuManager.instance.Sucide_Bomber_Detected;
            HudMenuManager.instance.StartCoroutine("ShowNotification");
            agent.SetDestination(Player.transform.position);
            animator.Play("Runing");
        }
        else
        { if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
            {
                StartFolowing = true;
                agent.SetDestination(Player.transform.position);
                animator.Play("Runing");
            }
        }


        if (Vector3.Distance(transform.position, Player.transform.position) < 3 && !IsDie && StartFolowing)
        {
            AgentMesh.SetActive(false);
            agent.isStopped = true;
            animator.enabled = false;
            Explosion.SetActive(true);

            if (Player.GetComponent<FPSPlayer>())
            {
                Player.GetComponent<FPSPlayer>().ApplyDamage(1000, null, true);
            }
            else
            {
                StartCoroutine("PlayStoryCameraWithDelay");
            }
            IsDie = true;

        }

        if (!IsDie && StartFolowing)
        {
            agent.destination = Player.transform.position;

        }
        else if (IsDie && !agent.isStopped && StartFolowing)
        {
            agent.isStopped = true;

        }


    }



    public static bool inFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
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
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, maxRadius))
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
    IEnumerator PlayStoryCameraWithDelay()
    {
        yield return new WaitForSeconds(4f);
        MConstants.isGameOver = true;
        // MConstants.isPlayerWin = false;
        // HudMenuManager.instance.GameOver();
        Debug.LogError("Issue is Here");
    }
    void EnableAiRegdol()
    {
        animator.enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Rigidbody[] rbs = Body.GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        Collider[] colls = Body.GetComponentsInChildren<Collider>();
        foreach (var coll in colls)
        {
            coll.enabled = true;
        }

        StartCoroutine("DeactiveBody");
    }



    IEnumerator DeactiveBody()
    {
        yield return new WaitForSeconds(4f);
        gameObject.transform.parent = null;
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
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
        if (Player)
        {
            Gizmos.DrawRay(transform.position, (Player.transform.position - transform.position).normalized * maxRadius);

        }

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);


    }

}

