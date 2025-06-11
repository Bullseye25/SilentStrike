using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeleCopCtrl : MonoBehaviour
{
    public GameObject Rope;
    public GameObject[] HeleChara;
    public float StartDeley, DelaybetweenSpawning;
    void Start()
    {
        StartCoroutine("DropChracter");
        
       
    }
    
    IEnumerator DropChracter()
    {
        yield return new WaitForSeconds(DelaybetweenSpawning);
        Rope.SetActive(true);
        yield return new WaitForSeconds(StartDeley-3f);

        HeleChara[0].SetActive(true);
        yield return new WaitForSeconds(StartDeley);

        HeleChara[1].SetActive(true);
        yield return new WaitForSeconds(StartDeley);
        if (HeleChara.Length>2)
        {
            HeleChara[2].SetActive(true);
        }
        // item.GetComponent<BoxCollider>().enabled = true;


    }
}
