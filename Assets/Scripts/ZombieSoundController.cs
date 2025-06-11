using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSoundController : MonoBehaviour
{
    public static ZombieSoundController instance;
    public AudioClip[] tauntsList;
    public AudioSource audioSource;
    public AudioClip sniperMissionSound , hitSound;

    private void Awake()
    {
        instance = this;
    }

    public void StartZombieTauntSounds()
    {
        StartCoroutine("PlayZombieTauntSounds");
    }

    public void StopZombieTauntSounds()
    {
        StopCoroutine("PlayZombieTauntSounds");
        // Debug.Log("stop ");

    }

    public void StartZombieSniperMissionSounds()
    {
        StartCoroutine("PlayZombieSniperMissionSounds");
    }

    public void StopZombieSniperMissionSounds()
    {
        StopCoroutine("PlayZombieSniperMissionSounds");
    }

    public void StartHitSounds()
    {
        StartCoroutine("PlayHitSounds");
    }

    public void StopHitSounds()
    {
        StopCoroutine("PlayHitSounds");
    }


    IEnumerator PlayZombieTauntSounds()
    {
        //Check to see if we can see enemy targets every x seconds
        yield return new WaitForSeconds(2);
        //  Debug.Log("Current Targes " + currentTargets.Count);

        while (LevelsManager.instance.currentZombieWave.enemiesList.Count > 0)
        {
            audioSource.PlayOneShot(tauntsList[Random.Range(0, tauntsList.Length)]);
            yield return new WaitForSeconds(Random.Range(3, 6));
        }
    }

    IEnumerator PlayZombieSniperMissionSounds()
    {
        //Check to see if we can see enemy targets every x seconds
        yield return new WaitForSeconds(1);
        //  Debug.Log("Current Targes " + currentTargets.Count);

        while (LevelsManager.instance.currentZombieWave.enemiesList.Count > 0)
        {
            audioSource.PlayOneShot(sniperMissionSound);
            yield return new WaitForSeconds(21);
        }
    }

    IEnumerator PlayHitSounds()
    {
       
        yield return new WaitForSeconds(1.5f);
        audioSource.PlayOneShot(hitSound);
        yield return new WaitForSeconds(Random.Range(3, 6));
        //while (LevelsManager.instance.currentZombieWave.enemiesList.Count > 0)
        //{
        //    audioSource.PlayOneShot(hitSound);
        //    yield return new WaitForSeconds(Random.Range(3, 6));
        //}
    }

}
