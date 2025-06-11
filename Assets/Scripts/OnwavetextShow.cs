using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnwavetextShow : MonoBehaviour
{
    public string WaveOnAlerText;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("TextChange", 1f);
    }

    // Update is called once per frame
    void TextChange()
    {
        SpawnerCtrl.instance.EnemyText = WaveOnAlerText;
        HudMenuManager.instance.textWave.text = WaveOnAlerText;
        SpawnerCtrl.instance.StartCoroutine("SpawnWave");
    }
}
