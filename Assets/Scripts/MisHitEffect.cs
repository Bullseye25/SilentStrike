using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisHitEffect : MonoBehaviour
{
    public AudioClip[] recoList;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void OnEnable()
    {
        audioSource.PlayOneShot(recoList[Random.Range(0, recoList.Length)]);

    }


}
