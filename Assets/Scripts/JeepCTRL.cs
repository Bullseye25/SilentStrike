using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class JeepCTRL : MonoBehaviour
{
 
    public GameObject[] Enemies_Dummies,Enemes_AI;
    public DOTweenPath dOTween;
    public DOTweenAnimation[] Tires;
    public GameObject Player;
    public bl_MiniMapItem MMIcon;
    bool PlayerDetected;
    public DOTweenAnimation GarnadeBtn;
    public string message = "Use Garnade to Destroy enemies";
    bool IsjeepExploed = false;
    bool tANK = false;
    public bool IsTank;
    public bool isJeepTankLevel;
    public GameObject Body;
    public Texture  DestroyableTextures;
    void Start()
    {
     
        //this.GetComponent<Rigidbody>().mass = 10000;
        isJeepTankLevel = true;
        Player=FindObjectOfType<FPSPlayer>().gameObject;
        MMIcon = GetComponentInChildren<bl_MiniMapItem>();
        GarnadeBtn = HudMenuManager.instance.ThrowGrenadeButton;
        foreach (GameObject ENEMY in Enemies_Dummies)
        {

            Rigidbody[] rbs = ENEMY.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rbs)
            {
              //  rb.mass = 20000;
                rb.isKinematic = true;
            }
            Collider[] colls = ENEMY.GetComponentsInChildren<Collider>();
            foreach (var coll in colls)
            {
                coll.enabled = false;
            }

        }
    }

    public void JeepExplod()

    {
        isJeepTankLevel = false;
        this.GetComponent<Rigidbody>().isKinematic = false;
        //if (!IsTank)
        //{
        //    this.GetComponent<Rigidbody>().isKinematic = false;
        //    Invoke("FreezTank", 2f);
        //}
        if (!IsjeepExploed )
        {

            // this.GetComponent<Rigidbody>().mass = 10000;
            IsjeepExploed = true;
            PlayerDetected = true;
            StopCoroutine("dELAYtOJEEB");
            MMIcon.HideItem();
            dOTween.DOKill();
            gameObject.layer = 22;
            foreach (var item in Tires)
            {
                item.GetComponent<MeshRenderer>().materials[0].mainTexture = DestroyableTextures;
                item.DOKill();
                item.gameObject.layer = 22;
            }
            foreach (GameObject ENEMY in Enemies_Dummies)
            {
                ENEMY.transform.parent = null;
                Rigidbody[] rbs = ENEMY.GetComponentsInChildren<Rigidbody>();
                foreach (var rb in rbs)
                {
                    
                    rb.isKinematic = false;
                }
                Collider[] colls = ENEMY.GetComponentsInChildren<Collider>();
                foreach (var coll in colls)
                {
                    coll.enabled = true;
                }
                HudMenuManager.instance.EnemyKilled();


            }
           // this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
           
        }
        Body.GetComponent<MeshRenderer>().materials[0].mainTexture = DestroyableTextures;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
       
       
    }
 

    public void Enable_Ai_Disable_Jeep()
    {
        
           this.GetComponent<Rigidbody>().mass = 1;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.layer = 22;
            foreach (var item in Tires)
            {
                item.gameObject.layer = 22;
                item.DOKill();
            }
            dOTween.DOKill();
            foreach (GameObject AI in Enemes_AI)
            {
                AI.transform.parent = null;
                AI.SetActive(true);
              
            }

            foreach (GameObject ENEMY in Enemies_Dummies)
            {
                ENEMY.SetActive(false);

            }
            MMIcon.HideItem();
            this.GetComponent<JeepCTRL>().enabled = false;
        tANK = true;
        if (IsTank)
        {
            Invoke("FreezTank", 2f);
        }

    }
    void FreezTank()
    {
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
    public void Stop_Jeep_And_Enable_AI()
    {
        if (!IsjeepExploed )
        {

            IsjeepExploed = true;
            
            foreach (var item in Tires)
            {
                item.DOKill();
            }
            dOTween.DOKill();
          
           // this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            foreach (GameObject AI in Enemes_AI)
            {
                AI.transform.parent = null;
                AI.SetActive(true);
               
            }
            foreach (GameObject ENEMY in Enemies_Dummies)
            {
                ENEMY.SetActive(false);
            }
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
          //  this.GetComponent<Rigidbody>().mass = 5400000;
            MMIcon.HideItem();
           
        }
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position,Player.transform.position)<8&&!PlayerDetected)
        {
            StartCoroutine("dELAYtOJEEB");
            PlayerDetected = true;

        }
    }


    IEnumerator ShowMessageText()
    {
        yield return new WaitForSeconds(1f);
        HudMenuManager.instance.textWave.text = message;
        GarnadeBtn.DOPlay();
        yield return new WaitForSeconds(3f);
        HudMenuManager.instance.textWave.text = "";
        yield return new WaitForSeconds(2f);

        GarnadeBtn.DOKill();

    }
    private void OnEnable()
    {
        StartCoroutine("ShowMessageText");
    }

    IEnumerator dELAYtOJEEB()
    {
        dOTween.DOKill();
        foreach (var item in Tires)
        {
            item.DOKill();
        }
        this.GetComponent<Rigidbody>().mass = 10000;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSeconds(3f);
       
        Stop_Jeep_And_Enable_AI();
        yield return new WaitForSeconds(1f);
        
    }
}
