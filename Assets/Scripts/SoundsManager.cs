using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager instance;
    public AudioClip buttonClick;
    
    [HideInInspector] public AudioSource audioSource;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            audioSource = GetComponent<AudioSource>();
            return;
        }

        //DestroyImmediate(gameObject);
    }

    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClick);
    }
}
