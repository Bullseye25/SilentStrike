using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnlockEverythingScript : MonoBehaviour
{
    void Start()
    {
         var localPos = transform.localPosition;
        
                if (MainMenuManager.Instance.currentMenuName == MenuNames.MAIN_MENU || SceneManager.GetActiveScene().name != "UIScene")
        	        localPos.z = 0;
                else
        	        localPos.z = -1500f;
                
                transform.localPosition = localPos;
    }
    
}
