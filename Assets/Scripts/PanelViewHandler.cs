using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelViewHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        var localPos = transform.localPosition;
        if (MainMenuManager.Instance.currentMenuName == MenuNames.MAIN_MENU || SceneManager.GetActiveScene().name != "UIScene")
            localPos.z = 0;
        else
            localPos.z = -1500f;
        
        transform.localPosition = localPos;
    }
    
}
