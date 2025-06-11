using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SCGameManager : MonoBehaviour
{

    public static SCGameManager Instance;
    [Header("SoundMixer")]
    public AudioMixer soundMixer;
    [Header("Music")]
    public AudioMixer musicMixer;

    //public event Action RefreshAudioSettings = delegate { };


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnRefreshAudioSettings();
    }
    public void OnRefreshAudioSettings()
    {
        soundMixer.SetFloat("SoundVol", PlayerDataController.Instance.playerData.isSoundOn ? 0 : -80);

       // musicMixer.SetFloat("MusicVol", PlayerDataController.Instance.playerData.isMusicOn ? 0 : -80);

       // gameObject.GetComponent<AudioSource>().outputAudioMixerGroup = soundMixer.outputAudioMixerGroup;
    }


}
