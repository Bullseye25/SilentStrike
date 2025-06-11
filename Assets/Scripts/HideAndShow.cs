using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndShow : MonoBehaviour
{
    public GameObject[] ToHide;
   
    void Start()
    {
       
        foreach (var item in ToHide)
        {
            item.SetActive(false);

        }
    }

}
