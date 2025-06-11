using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCtrl : MonoBehaviour
{
    public static SpawnerCtrl instance;
    public int EnemiesInWave;
    public int Count = 0;
    public GameObject[] Waves;
  [HideInInspector]
    public string EnemyText = "Enemy Wave is Coming";
   public void Start()
    {
        instance = this;
        StartCoroutine("SpawnWave");

    }

   
     IEnumerator SpawnWave()
    {
        foreach (var wave in Waves)
        {
            yield return new WaitForSeconds(2f);
            HudMenuManager.instance.textWave.text = EnemyText;
            wave.SetActive(true);

            yield return new WaitForSeconds(2f);
            HudMenuManager.instance.textWave.text = "";


            yield return new WaitUntil(() => Count == EnemiesInWave);
            Count = 0;
        }
        


    }


}
