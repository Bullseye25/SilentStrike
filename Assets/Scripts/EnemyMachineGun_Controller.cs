using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMachineGun_Controller : DamageAble
{
    public JetGun[] jetGuns;
    public float Fov = 30;
    public float distanceToShoot = 50;
    public float alertRange = 50;

    public float damping=10;
    public GameObject explosion;

    public GameObject smoke;
    public GameObject EnemyMesh;

    public AudioSource machineGunSound;
    public Transform turret;
    public Animator animatorController;
    public GameObject[] nonDestructable;

    bool isdead;
    public bool overrideFOV;
    public Vector3 offSet;
    // bool isInFOV;

    HealthElement healthElement;
    Transform target;
    float lastAttackTime;
    float lastPauseTime;
    float attackDelay = 10;
    float attackDuration = 5;
    public float inaccuracy = 1;
    public float burstTime=4;
    public float reloadTime = 2;

    bool isFiring;
    private void Start()
    {
        healthElement = GetComponentInChildren<HealthElement>();
        target = HudMenuManager.instance.player.transform;

         currentHealh = MaxHealth;
        healthElement.currentHealth = currentHealh;
        lastAttackTime = Time.time;
    }

    float distance;
    private void Update()
    {
        if (MConstants.isGameOver)
        {
          //  planeSound.mute = true;
        }
        distance = Vector3.Distance(target.position, transform.position);

        if (!isdead  && distance < distanceToShoot && CheckFov() && !isFiring && (Time.time > lastPauseTime + reloadTime))
        {
         
            lastAttackTime = Time.time;
            isFiring = true;
            for (int i = 0; i < jetGuns.Length; i++)
            {
                jetGuns[i].isFiring = true;
            }
            animatorController.SetBool("Shoot", true);
            if (machineGunSound && !machineGunSound.isPlaying)
            {
                machineGunSound.Play();

            }
           

        }
        else if((isFiring && Time.time > lastAttackTime + burstTime) || isdead)
        {
            isFiring = false;
            lastPauseTime = Time.time;
            for (int i = 0; i < jetGuns.Length; i++)
            {
                jetGuns[i].isFiring = false;
            }
            animatorController.SetBool("Shoot", false);
            if (machineGunSound && machineGunSound.isPlaying)
            {
                machineGunSound.Stop();

            }
        }

        if (!isdead && distance < alertRange && overrideFOV)
        {
        
            LookAtTarget();
        }

    }

    public void LookAtTarget()
    {
        //Movement in x , z direction
        var lookPos = target.position - transform.position ;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

        if (lookPos.magnitude > 4f)
        {
            lookPos = target.position - turret.position + offSet + new Vector3(Random.Range(-inaccuracy, inaccuracy), 0, Random.Range(-inaccuracy, inaccuracy));
            rotation = Quaternion.LookRotation(lookPos);
            turret.rotation = Quaternion.Slerp(turret.rotation, rotation, Time.deltaTime * damping);
        }
        

    }

    void StopFirong()
    {
        lastAttackTime = Time.time;

        for (int i = 0; i < jetGuns.Length; i++)
        {
            jetGuns[i].isFiring = false;
        }
        animatorController.SetBool("Shoot", false);
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
        healthElement.OnHealthChanged(currentHealh);
        if (currentHealh <= 0)
        {
            isdead = true;
            explosion.SetActive(true);
            StopFirong();
            //smoke.SetActive(false);
            StartCoroutine(AfterExplosion());
           // Invoke("PlaneDestroye", 4);
        //    HudMenuManager.instance.EnemyKilled();

        }
    }
    IEnumerator AfterExplosion()
    {
        if(GetComponent<BoxCollider>())
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        yield return new WaitForSeconds(2f);
        explosion.SetActive(false);
        EnemyMesh.SetActive(false);
        foreach(var item in nonDestructable)
        {
            item.GetComponent<Renderer>().material.color = Color.black;
        }
    }


    bool CheckFov()
    {
        if (overrideFOV)
        {
            return true;
        }
        Vector3 targetDir = target.position - transform.position;
        float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));

        if (angleToPlayer >= -Fov && angleToPlayer <= Fov)
        {
            return true;
        }

        return false;
    }

}

