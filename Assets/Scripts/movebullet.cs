using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movebullet : MonoBehaviour
{
    Rigidbody rb;
    bool run, camDeattach;
    public float bulletSpeed;
    public float ParticleLifeTime = 5;
    public float DecayDuration = 10;

    public GameObject bulletMesh, Cam1, bulletImpact;
    public AudioClip bulletHitSound;
    
    public GameObject ParticleHit;
    public GameObject DecayFX;
    
    public GameObject[] sniperBullet;
    public GameObject[] bulletSparks;
    public GameObject[] dummyGuns;

    private int _bulletNo;
    private GameObject _currentGun;
    private AudioSource _audioSource;
    [HideInInspector] public int lastBulletDamage;
    private void Awake()
    {
        HudMenuManager.instance.Croshair.SetActive(false);
       // HudMenuManager.instance.machineGunCrosshair.SetActive(false);
        transform.SetParent(null);
        transform.LookAt(MConstants.LastBulletTarget.transform);
        _audioSource = GetComponent<AudioSource>();

        if (InputControl.instance.zoomHold) InputControl.instance.zoomHold = false;
        HudMenuManager.instance.zoomOut();

        foreach (var weaponNo in PlayerWeapons.instance.weaponOrder)
        {
            if (weaponNo.activeSelf)
            {
                _bulletNo = System.Array.IndexOf(PlayerWeapons.instance.weaponOrder, weaponNo);
                sniperBullet[_bulletNo - 1].SetActive(true);
            }
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Time.timeScale = 0.06f;
        for (int i = 0; i < PlayerWeapons.instance.weaponOrder.Length; i++)
        {
            if (PlayerWeapons.instance.weaponOrder[i].activeSelf)
            {
                _currentGun = dummyGuns[i - 1];
                _currentGun.SetActive(true);
            }
            else
            {
                // dummyGuns[i].SetActive(false);
            }
        }
        StartCoroutine(ShootBullet());
    }
    

    private void Update()
    {
        if (camDeattach && !run)
        {
            Cam1.transform.LookAt(MConstants.LastBulletTarget.transform);
        }

        if (run)
        {
            // transform.LookAt(MConstants.LastBulletTarget.transform);
            transform.position = Vector3.MoveTowards(transform.position, MConstants.LastBulletTarget.transform.position,
                Time.deltaTime * bulletSpeed);
            float dist = Vector3.Distance(transform.position, MConstants.LastBulletTarget.transform.position);
            // float playerDist = Vector3.Distance( /*HudMenuManager.instance.player.gameObject.*/transform.position,
            //     MConstants.LastBulletTarget.transform.position);


            if (dist <= 3f && !camDeattach)
            {
                camDeattach = true;
                Time.timeScale = 0.3f;
                // bulletSpeed = 15;
                bulletSparks[_bulletNo - 1].SetActive(false);
                sniperBullet[_bulletNo - 1].transform.localScale = new Vector3(2, 2, 2);
                Cam1.transform.SetParent(null);
                Cam1.GetComponent<Animation>().Play("LastBulletZoomOutAnim");
                Destroy(Cam1,2.5f);
                
            }

            else if (dist < 1f)
            {
                Cam1.transform.LookAt(MConstants.LastBulletTarget.transform);
                OnHit(MConstants.LastBulletHit, gameObject);
                // bloodParticles.SetActive(true);
                // bloodParticles.transform.position = MConstants.LastBulletTarget.transform.position;
                // bloodParticles.transform.SetParent(MConstants.LastBulletTarget.transform);
                // Destroy(bloodParticles,2f);
                // MConstants.LastBulletTarget.gameObject.GetComponent<AnimalLocationDamage>().animalController.TakeDamage(999);
                MConstants.LastBulletTarget.GetComponentInParent<EnemyAI>().TakeDamage(999);
                if (MConstants.IsLastBulletHeadShot)
                {
                    MConstants.IsLastBulletHeadShot = false;
                    HudMenuManager.instance.headShotCount++;
                    HudMenuManager.instance.HeadShotAlert();
                    // HudMenuManager.instance.ShowHeadShort(MConstants.LastBulletTarget.transform);
                }
                else
                {
                    HudMenuManager.instance.KillShotAlert();
                }
                // if (MConstants.LastBulletTarget.transform.gameObject.GetComponent<LocationDamage>().damageMultiplier > 15f)
                    // HudMenuManager.instance.ShowHeadShort(MConstants.LastBulletTarget.gameObject.transform);
                
                // MConstants.LastBulletTarget.gameObject.GetComponent<LocationDamage>().ApplyDamage(500, Vector3.forward, WeaponBehavior.instance.mainCamTransform.position, WeaponBehavior.instance.myTransform, true, false);
                WeaponBehavior.instance.FPSPlayerComponent.UpdateHitTime();//used for hitmarker}

                
                _audioSource.PlayOneShot(bulletHitSound);
                // _audioSource.clip = animalDieSound;
                _audioSource.PlayDelayed(0.2f);

                run = false;
                rb.isKinematic = true;
                bulletMesh.SetActive(false);
                Invoke("DestroyThis", 2f);
            }
        }
    }

    public IEnumerator ShootBullet()
    {
        bulletMesh.SetActive(false);
        WeaponBehavior.instance.FPSPlayerComponent.UpdateHitTime(); //used for hitmarker}
        _currentGun.SetActive(true);

        yield return new WaitForSeconds(0.05f);

        bulletMesh.SetActive(true);
        run = true;
        // bulletImpact.SetActive(true);
        
        yield return new WaitForSeconds(0.1f);
        
        foreach (var gun in dummyGuns)
        {
            gun.SetActive(false);
        }
    }
    
    public  void OnHit(RaycastHit hit, GameObject bullet)
    {

        if (DecayFX)
        {
            GameObject decay = (GameObject)GameObject.Instantiate(DecayFX, hit.point, Quaternion.identity);
            decay.transform.forward = bullet.transform.forward - (Vector3.up * 0.2f);
            GameObject.Destroy(decay, DecayDuration);
        }
        if (ParticleHit)
        {
            GameObject hitparticle = (GameObject)Instantiate(ParticleHit, hit.point, hit.transform.rotation);
            hitparticle.transform.forward = hit.normal;
            hitparticle.transform.parent = MConstants.LastBulletTarget.transform;
            hitparticle.name = "FX";
            GameObject.Destroy(hitparticle, ParticleLifeTime);
        }
        /*if (ParticleFlow)
        {
            GameObject flowparticle = (GameObject)Instantiate(ParticleFlow, this.transform.position, hit.transform.rotation);
            flowparticle.transform.SetParent(bullet.transform);
            GameObject.Destroy(flowparticle, ParticleParticleFlowLifeTime);
        }*/
        
        
        /*if (hit.rigidbody && hit.rigidbody.useGravity){
            hit.rigidbody.AddForceAtPosition(force * directionArg / (Time.fixedDeltaTime * 100.0f), hit.point);//scale the force with the Fixed Timestep setting
        }*/
    }


    private void DestroyThis()
    {
        FPSPlayer.instance.CameraControlComponent.thirdPersonActive = false;
        if (MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            HudMenuManager.instance.Croshair.SetActive(true);
        }
        HudMenuManager.instance.pauseBtn.SetActive(true);
        FPSPlayer.instance.painFadeObj.GetComponent<Image>().enabled = true;
        HudMenuManager.instance.FpsCamera.farClipPlane = 12000f;
        MConstants.IslastBullet = false;
        Destroy(this.gameObject);
    }
}