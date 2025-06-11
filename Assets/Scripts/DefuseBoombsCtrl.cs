
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DefuseBoombsCtrl : MonoBehaviour//, IDetectable
{
    public bl_MiniMapItem MMIcon;
    public Image cooldown;

    public float FillUpTime;
   
    public GameObject BlastEffect;

    // Update is called once per frame
    private void Start()
    {
        FillUpTime = 1f;
       // cooldown = HudMenuManager.instance.BombFillbar.GetComponent<Image>();
        MMIcon = GetComponentInChildren<bl_MiniMapItem>();
      //  string minutes = Mathf.Floor(timer / 60).ToString("00");
      //  string seconds = (timer % 60).ToString("00");
      //  counterText.text = "00:" + string.Format("{0}:{1}", minutes, seconds);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GunPickup>())
        {
            cooldown.fillAmount = 0f;
            cooldown.gameObject.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<GunPickup>())
        {
            cooldown.fillAmount += 1.0f / FillUpTime * Time.deltaTime;
            if (cooldown.fillAmount >= 1f)
            {
               // HudMenuManager.instance.BombDefuse();
                cooldown.gameObject.SetActive(false);
                MMIcon.HideItem();
                this.gameObject.GetComponent<AudioSource>().enabled = false;
                this.gameObject.SetActive(false);

               // counterText.text = "Defused";
                
                //  Level.defusedAction -= DefuseEventUnSubscribed;
                
                // LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].BoombIsDefusedStartOtherTimer();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GunPickup>())
        {
            
            cooldown.gameObject.SetActive(false);
        }
    }
    //void Update()
    //{
    //    if (Trigged == true)
    //    {
    //        cooldown.fillAmount += 1.0f / FillUpTime * Time.deltaTime;
    //    }
    //    else if (cooldown.fillAmount > 0)
    //    {
    //        cooldown.fillAmount -= 1.0f / FillDownTime * Time.deltaTime;
    //    }

    //    if (cooldown.fillAmount == 1 && UseTimer && cooldown.enabled)
    //    {
    //        UseTimer = false;
    //        cooldown.enabled = false;
    //        counterText.color = Color.green;
    //        counterText.text = "Defused";
    //        this.gameObject.GetComponent<AudioSource>().enabled = false;
    //      //  Level.defusedAction -= DefuseEventUnSubscribed;
    //        MMIcon.HideItem();
    //       // LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].BoombIsDefusedStartOtherTimer();
    //    }
    //    if (UseTimer && cooldown.enabled)
    //    {
    //        if (timer <= 0f)
    //        {
    //            this.gameObject.GetComponent<AudioSource>().enabled = false;
    //            counterText.text = "00:00:00";
    //            UseTimer = false;
    //            BlastEffect.SetActive(true);
    //          //  HudMenuManager.instance.ReasonToFailText.text = "";
    //             //   HudMenuManager.instance.ReasonToFailText.text = HudMenuManager.instance.ReasonToFailTime;
                
    //            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    //            counterText.transform.gameObject.SetActive(false);
    //        }
    //        else
    //        {
    //            timer -= Time.deltaTime;
    //            string minutes = Mathf.Floor(timer / 60).ToString("00");
    //            string seconds = (timer % 60).ToString("00");
    //            counterText.text = "00:" + string.Format("{0}:{1}", minutes, seconds);
    //        }

    //    }

    //}

    //private void OnEnable()
    //{
    //    LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].TotalTimeBomb++;

    //    HudMenuManager.instance.TotalEnemyKilled.text = LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].TotalTimeBomb + " / " + LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].TotalTimeBomb;
    //}
   
   
}
