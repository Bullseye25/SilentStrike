using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomberEnableTrigger : MonoBehaviour
{

    public GameObject[] Boomber;
    bool TempWave = false;
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && TempWave == false)
        { 
            TempWave = true;
            HudMenuManager.instance.NotificationText.text = HudMenuManager.instance.Wavetext;
            HudMenuManager.instance.StartCoroutine("ShowNotification");
            foreach (var item in Boomber)
            {
                item.SetActive(true);
               
            }
        }
    }
    private void Start()
    {
        foreach (var item in Boomber)
        {
            item.SetActive(false);
            
        }
    }
}
