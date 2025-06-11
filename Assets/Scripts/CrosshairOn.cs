using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairOn : MonoBehaviour
{
    public GameObject Crosshair;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        if (!Crosshair.activeSelf && MConstants.CurrentGameMode != MConstants.GAME_MODES.MODE5_BATTLEFIELD)
        {
            Crosshair.SetActive(true);
        }
    }
    private void OnDisable()
    {
        // if (Crosshair.activeSelf)
        {
//            Crosshair.SetActive(false);
        }
    }
  
}
