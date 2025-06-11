using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEscape : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
       

        // Update is called once per frame
       
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Boat" && !MConstants.isPlayerWin)
        {
            MConstants.isGameOver = true;
            // MConstants.isPlayerWin = false;
            // HudMenuManager.instance.GameOver();

        }
    }
}