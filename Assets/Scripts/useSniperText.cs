using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class useSniperText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator ShowMessageText()
    {
        yield return new WaitForSeconds(2f);
        HudMenuManager.instance.textWave.text = "Use Sniper to Clear this Leve";
        yield return new WaitForSeconds(4f);
        HudMenuManager.instance.textWave.text = "";

    }
    private void OnEnable()
    {
        StartCoroutine("ShowMessageText");
    }
}
