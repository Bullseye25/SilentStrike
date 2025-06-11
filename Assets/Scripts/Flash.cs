using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
     public float waitingTime;     // The total of seconds the flash wil last
     public float maxIntensity;     // The maximum intensity the flash will reach
     public Light myLight;        // Your light
  void OnEnable(){
StartCoroutine(flashNow ());
}
     public IEnumerator flashNow ()
     {
         while (true)
        {
myLight.enabled=!myLight.enabled;
       yield  return new WaitForSeconds(waitingTime);
         }
     }
}
