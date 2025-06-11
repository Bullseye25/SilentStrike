using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TacticalAI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image healthBar;
    public Image healthBarStatic;

    [HideInInspector]
    public HealthElement healthElement;
    float MaxHealth;
    float lastHealth;
   public float initalScale=0.6f;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetHealthBar(HealthElement healthElement)
    {
        initalScale = transform.localScale.x;

       // Debug.Log("Health bar");
        this.healthElement = healthElement;
        MaxHealth = healthElement.currentHealth;
        lastHealth = healthElement.currentHealth;
        healthBar.fillAmount = lastHealth / MaxHealth;
        healthBarStatic.fillAmount = lastHealth / MaxHealth;
        healthElement.HealthChangeDelegate += OnDamage;
        this.gameObject.SetActive(false);

    }

    Vector3 distanceOffset;
    public void OnDamage(float health)
    {
        if (healthElement)
        {
            gameObject.SetActive(true);
            prefChangePercenage = healthBar.fillAmount;
            point = healthElement.currentHealth / MaxHealth;
            healthBarStatic.fillAmount = point;
            updateSpeedSecond = 0.8f;
            elapsed = 0;
            if (HudMenuManager.instance.player)
            {
                float offset=0 ;
                float distance = Vector3.Distance(HudMenuManager.instance.player.transform.position, healthElement.transform.position);
                float scale = 10/distance;
                offset = 1*(distance/250);
                if (offset>1)
                {
                    offset = 1;
                }
                if (scale<0.1)
                {
                    scale = 0.1f;
                }else if (scale> initalScale)
                {
                    scale = initalScale;
                }

                //Debug.Log("Scale "+ scale + " Dist "+ offset );
                distanceOffset = new Vector3(0, offset, 0);
                transform.localScale = new Vector3(scale, scale, scale);
            }
        }
      
        // StartCoroutine(AnimateHealth());
    }
    float updateSpeedSecond = 0;
    float elapsed = 0;
    float prefChangePercenage;
    float point;
    IEnumerator AnimateHealth()
    {
        float prefChangePercenage = healthBar.fillAmount;
        float point = healthElement.currentHealth / MaxHealth;
        healthBarStatic.fillAmount = point;

        float elapsed = 0;
        while (elapsed< updateSpeedSecond)
        {
            elapsed += Time.deltaTime;
            healthBar.fillAmount = Mathf.Lerp(prefChangePercenage, point, elapsed/updateSpeedSecond);
            yield return null;
        }
        gameObject.SetActive(false);


        //  yield return wa;

    }

    public void Update()
    {
        if (elapsed < updateSpeedSecond)
        {
            elapsed += Time.deltaTime;
            healthBar.fillAmount = Mathf.Lerp(prefChangePercenage, point, elapsed / updateSpeedSecond);
        }
        else
        {
            gameObject.SetActive(false);

        }
    }
    // Update is called once per frame
    private void LateUpdate()
    {
        if (!healthElement.IsDead)
        {
            transform.position = Camera.main.WorldToScreenPoint(healthElement.transform.position+ distanceOffset);
           

        }
        // if(HudMenuManager.instance.player!=null)
        //transform.rotation = Quaternion.LookRotation(transform.position - HudMenuManager.instance.player.transform.position);
    }

    private void OnDestroy()
    {
        healthElement.HealthChangeDelegate -= OnDamage;

    }

}
