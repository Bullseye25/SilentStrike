using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairOff : MonoBehaviour
{
    public GameObject Crosshair;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        if (Crosshair)
        {
            Crosshair.SetActive(false);
        }
    }
    private void OnDisable()
    {
        if (Crosshair)
        {
            Crosshair.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
