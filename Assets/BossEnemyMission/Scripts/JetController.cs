using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetController : DamageAble
{

    public JetGun[] jetGuns;
    public float Fov = 30;
    public float distanceToShoot=50;
    public Cinemachine.CinemachineDollyCart cinemachine;
    //public Cinemachine.CinemachineDollyCart speed;
    public GameObject explosion;
    public Rigidbody planeRigidBody;
    public GameObject smoke;

    public AudioSource machineGunSound;
    public AudioSource planeSound;
    public bool isRandomAttck;

    bool isdead;
   // bool isInFOV;

    Transform target;
    float lastAttackTime;
    float attackDelay=10;
    float attackDuration = 5;
    private void Start()
    {
        target= HudMenuManager.instance.player.transform;
       // gameObject.tag = MConstants.enemyTag;

        currentHealh = MaxHealth;
        lastAttackTime = Time.time;
    }

    private void Update()
    {
        if (MConstants.isGameOver)
        {
            planeSound.mute = true;
        }
        if (!isdead &&((isRandomAttck && Time.time>lastAttackTime+ attackDelay) || ((Vector3.Distance(target.position , transform.position)< distanceToShoot  && CheckFov()))))
        {
            lastAttackTime = Time.time;
            for (int i= 0;i<jetGuns.Length;i++)
            {
                jetGuns[i].isFiring = true;
            }
            if (machineGunSound && !machineGunSound.isPlaying)
            {
                machineGunSound.Play();

            }
            if (isRandomAttck)
            {
                Invoke("StopFirong", attackDuration);
            }
        } 
        else if(!isRandomAttck)
        {
            for (int i = 0; i < jetGuns.Length; i++)
            {
                jetGuns[i].isFiring = false;
            }
            if (machineGunSound && machineGunSound.isPlaying)
            {
                machineGunSound.Stop();

            }
        }
    }

    void StopFirong()
    {
        lastAttackTime = Time.time;

        for (int i = 0; i < jetGuns.Length; i++)
        {
            jetGuns[i].isFiring = false;
        }
        if (machineGunSound && machineGunSound.isPlaying)
        {
            machineGunSound.Stop();

        }
    }
    public override void OnHit(float damage)
    {
        if (isdead)
        {
            return;
        }
        if (currentHealh < MaxHealth / 2)
        {
            smoke.SetActive(true);

        }
        currentHealh -= damage;
        if (currentHealh<=0)
        {
            isdead = true;
            explosion.SetActive(true);
            StopFirong();
            planeRigidBody.transform.parent = null;
            planeRigidBody.isKinematic = false;
            planeRigidBody.AddForce(transform.forward*planeRigidBody.mass*20,ForceMode.Impulse);
            smoke.SetActive(false);
            planeSound.Stop();
            if (cinemachine)
            {
                Destroy(cinemachine);
            }
            Invoke("PlaneDestroye", 4);
            HudMenuManager.instance.EnemyKilled();

        }
    }

    void PlaneDestroye()
    {
        // DestroyChild.instance.DestroyArrowPlane();
        // yield return new WaitForSeconds(1f);
        Destroy(planeRigidBody.gameObject);
        Destroy(this.gameObject);
    }

    bool CheckFov()
    {
        Vector3 targetDir = target.position - transform.position;
        float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));

        if (angleToPlayer >= -Fov && angleToPlayer <= Fov)
        {
            return  true;
        }

        return false;
    }
  
}
