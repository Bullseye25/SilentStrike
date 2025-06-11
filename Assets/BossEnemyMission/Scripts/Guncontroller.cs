using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Guncontroller : MonoBehaviour
{
    public static Guncontroller Instance;
   public UnityEngine.Events.UnityEvent OnDie;
    public UnityEngine.Events.UnityEvent Onstart;
    public float Health;
    public Image GunHealthbar;
    public Animator animator;
    public GameObject[] rigRigidbodies;
    public GameObject[] BlastEffect;
    public GameObject[] DisableObject;
    public AudioSource Gunsound;
    public float HitForce=1;
    bool isdead;
 
    public GameObject muzzle;
    public ParticleSystem bulletTrace;
    private Vector3 rayOrigin;
    private Vector3 targetDir;
    private Vector3 targetPos;

    GameObject HealthbarBg;
    public bool isjeep;
    SplineInterpolator interpolator;
  public  GameObject healthCanvas;
    Transform Target;
    float Maxhealth;
    public bool Isminirobot;
    // Start is called before the first frame update
    private WeaponEffects WeaponEffectsComponent;
    public bl_MiniMapItem MMIcon;
    public LayerMask Detectedlayer;
    public GameObject Disabledobj;
    private void Awake()
    {
        Instance = this;
        if (isjeep)
        {
       

            interpolator = GetComponent<SplineInterpolator>();
        }
    }
    void Start()
    {
        WeaponEffectsComponent =HudMenuManager.instance.player.weaponObj.GetComponent<WeaponEffects>();
        Maxhealth =Health;
        if (GunHealthbar)
        {
            HealthbarBg = GunHealthbar.transform.parent.gameObject;
            GunHealthbar.fillAmount = (Health / Maxhealth);
            if (!healthCanvas)
            {
                healthCanvas = GetComponentInChildren<Canvas>().gameObject;
            }
            if (HudMenuManager.instance.player)
            {
                Target = HudMenuManager.instance.player.transform;
            }
        }
        muzzle.SetActive(false);
        if (!Gunsound)
        {
            Gunsound = GetComponent<AudioSource>();
        }
        if (!HudMenuManager.instance.player)
        {
            StartCoroutine(SearchTarget());
        }
        else
        {
            Onstart.Invoke();
        }
    }

    IEnumerator SearchTarget()
    {
        yield return new WaitUntil(() => HudMenuManager.instance.player);
        if (!HudMenuManager.instance.player)
        {
            yield return false;
        }
        yield return new WaitForSeconds(0f);
        Target = HudMenuManager.instance.player.transform;
        Onstart.Invoke();
        StopCoroutine(SearchTarget());
    }
    // Update is called once per frame
    void Update()
    {
       
        if (healthCanvas && Target)
        {
            healthCanvas.transform.LookAt(Target);
        }
        
    }
    public void OnHit(float damage)
    {
        if (isdead)
        {
            return;
        }
        if (!isdead)
            Health -= damage/2;
          if (Health > 1)
            {
                HealthbarBg.SetActive(true);
                Health -= damage;
                GunHealthbar.fillAmount = (Health / Maxhealth);
            if (Health<=0)
            {
                OnHit(0f);
            }

        }
            else
            {
                isdead = true;
                if (animator)
                {
                    animator.enabled = false;
                }
                //if (interpolator && interpolator.mState != "Stopped")
                //{
                //    interpolator.mState = "Stopped";
                //    //if (isjeep)
                //    //{
                //    //    OnDie.Invoke();
                //    //}
              //  }
            if (GetComponent<BoxCollider>())
            {
                GetComponent<BoxCollider>().enabled = false;
            }
            if (MMIcon)
                MMIcon.HideItem();
            //if (!isjeep)
            //{
           
                //}
                if (rigRigidbodies.Length > 0)
                {
                    foreach (var item in rigRigidbodies)
                    {
                        if (item != null)
                        {
                            if (!item.GetComponent<Collider>())
                            {
                                item.AddComponent<BoxCollider>();
                            }
                            item.transform.parent = null;
                            item.AddComponent<Rigidbody>();
                            item.GetComponent<Rigidbody>().mass = 1;
                            item.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-HitForce, HitForce), Random.Range(0, HitForce), Random.Range(0, HitForce))*0.5f);
                            Destroy(item.gameObject, 3f);
                        }
                    }
                }
                if (BlastEffect.Length > 0)
                {
                    foreach (var item in BlastEffect)
                    {
                        item.SetActive(true);
                    }
                }
                if (DisableObject.Length > 0)
                {
                    foreach (var item in DisableObject)
                    {
                        item.SetActive(false);
                    }
                }
                OnDie.Invoke();
            Destroy(GetComponent<Guncontroller>(), 0.2f);
            }

    }
    Quaternion fireRotation;
    float inaccuracy=5;
   static int index=0;
    public void Fire(Transform target)
    {
        if (!target||Isminirobot)
        {
            return;
        }
       // Ray ray; ;//= new Ray(muzzle.transform.position, target.position);
        //  return true;
      
      
     
        Target = target;
        muzzle.SetActive(false);
        muzzle.SetActive(true);
        if (index%2==0)
        {
            fireRotation = Quaternion.LookRotation(bulletTrace.transform.forward);
            fireRotation *= Quaternion.Euler(Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy), 0);

            GameObject bullet = AzuObjectPool.instance.SpawnPooledObj(21, muzzle.transform.position, fireRotation) as GameObject;
            bullet.transform.parent = null;
            bullet.gameObject.SetActive(false);
            bullet.gameObject.SetActive(true);
            bullet.GetComponent<TacticalAI.BulletScript>().Sender = transform;
        }
        index++;

        bulletTrace.Play();

        // transform.LookAt(Target);
        //   rayOrigin = new Vector3(muzzle.transform.position.x, muzzle.transform.position.y, muzzle.transform.position.z);
        //   targetPos = Target.transform.position;
        //   targetDir = (targetPos - rayOrigin).normalized;
        ////   var bullet = ObjectPool.instance.GetObjectForType("BulletRobot", false);
        //   float randomBullet = Random.Range(-0.3f, 0.3f);
        //   if (Physics.Raycast(muzzle.transform.position + new Vector3(0, 0, 0.2f), muzzle.transform.forward, out RaycastHit hit, 5f, Detectedlayer))
        //   {
        //       // muzzle.SetActive(false);
        //       if (hit.transform.gameObject != target.gameObject)
        //       {
        //           return;
        //       }
        //   }
        //   //  bullet.transform.position = new Vector3(muzzle.transform.position.x + randomBullet, muzzle.transform.position.y + randomBullet, muzzle.transform.position.z);
        //   if (muzzle)
        //   {
        //       WeaponEffectsComponent.BulletTracers(targetDir, new Vector3(muzzle.transform.position.x + randomBullet, muzzle.transform.position.y + randomBullet, muzzle.transform.position.z), -3.0f, 0.0f, false);
        //   }

        //  bullet.transform.TransformDirection(targetDir);



        //if (projectile != null)
        //{
        //    float randomtarget = Random.Range(-0.8f, 0.8f);
        //    projectile.Speed = 3;
        //    projectile.Distance = Vector3.Distance(this.transform.position,Target.transform.position);
        //    projectile.Direction = vector.normalized+new Vector3(vector.x+ randomtarget, vector.y+ randomtarget, vector.z+ randomtarget);
        //    projectile.Target = null;


        //}
    }
    public void Destroyobj()
    {
        if (Disabledobj)
        {
            Destroy(Disabledobj);
        }
    }
}
