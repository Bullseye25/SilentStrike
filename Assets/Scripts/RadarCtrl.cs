using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarCtrl : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            HudMenuManager.instance.Radar.GetComponent<CanvasGroup>().alpha = 1;
            this.gameObject.SetActive(false);
        }
    }
    
}
