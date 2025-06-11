using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{

    // Start is called before the first frame update
    float picktime ;
    void Start()
    {
        picktime = Time.time;

    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<WeaponPickup>() && Time.time >= picktime + 2f )
        {
          
            picktime = Time.time;
            other.gameObject.GetComponent<WeaponPickup>().PickUpItem();
        }
      
        if (other.gameObject.GetComponent<HealthPickup>())
        {
            
            other.gameObject.GetComponent<HealthPickup>().PickUpItem(HudMenuManager.instance.player.gameObject);
          
            
        }
    }
    
    // Update is called once per frame
   
}
