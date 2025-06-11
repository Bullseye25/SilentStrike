using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPortalCtrl : MonoBehaviour
{
    public HostageCtrl Hostage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            Hostage.HostageRlease();
            
        }
    }

   
}
