using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip[] AudioClips;
    public AudioSource audioSource;

    void Awake()
    {
        instance = this;
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    public void playAudioClip(int index)
    {
        
         audioSource.PlayOneShot(AudioClips[index],1f);
         
    }
    public void playAudioClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip, 1f);
        //if (SCGameManager.Instance.SoundCheck())
        //{

        //}


    }

    public enum SoundNames
    {
        Main_Clicks_Sound,
        Small_Buttons_Sound,
        Back_Buttons_Sound,
        Weapon_Selection_Sound,
        Panel_Buttons_Sound,
        Lock_Button_Sound,
        SpinWHeel_Sound,
    }
}
