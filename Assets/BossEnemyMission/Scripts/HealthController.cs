using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : DamageTaker
{
    public UnityEngine.Events.UnityEvent OnDie;
    public UnityEngine.Events.UnityEvent Onstart;
    public Image GunHealthbar;
    public GameObject[] rigRigidbodies;
    public GameObject[] BlastEffect;
    public GameObject[] DisableObject;
    bool isdead;
    public float HitForce = 1;
    [HideInInspector]
    public SCWave JetWave;
    Transform Target;


    GameObject HealthbarBg;
    SplineInterpolator interpolator;
    public GameObject healthCanvas;
    float Maxhealth;
    // Start is called before the first frame update
    public GameObject Disabledobj;
  //  public SCWave jet;
    private void Awake()
    {
        
    }
    void Start()
    {
        Maxhealth = Health;
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
    public override void OnHit(float damage)
    {
        if (isdead)
        {
            return;
        }
        if (!isdead)
            Health -= damage / 2;
        if (Health > 1)
        {
            if (HealthbarBg)
            {
                HealthbarBg.SetActive(true);
                GunHealthbar.fillAmount = (Health / Maxhealth);

            }
            Health -= damage;
            if (Health <= 0)
            {
                OnHit(0f);
            }

        }
        else
        {
            isdead = true;
          
            //if (interpolator && interpolator.mState != "Stopped")
            //{
            //    interpolator.mState = "Stopped";
              
            //}
            if (GetComponent<BoxCollider>())
            {
                GetComponent<BoxCollider>().enabled = false;
            }
         
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
                        item.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-HitForce, HitForce), Random.Range(0, HitForce*5), Random.Range(0, HitForce)) * 0.5f);
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

            if (JetWave) JetWave.RemoveMachines();
          //  RemoveMachines(this);
        }

    }
 
    public void Destroyobj()
    {
        if (Disabledobj)
        {
            Destroy(Disabledobj);
        }
    }
}
