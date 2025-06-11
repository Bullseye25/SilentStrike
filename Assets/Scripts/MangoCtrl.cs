using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MangoCtrl : MonoBehaviour
{
   
    void Start()
    {
        
    }

    // Update is called once per frame
    public void DestroyMango(float Duration)
    {
        Invoke("DeActivate", Duration);
    }

  void DeActivate()
    {
        this.gameObject.SetActive(false);
    }
}
