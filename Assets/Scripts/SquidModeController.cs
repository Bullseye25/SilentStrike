using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using Random = UnityEngine.Random;

public class SquidModeController : MonoBehaviour
{
    public static SquidModeController instance;
    
    public GameObject dollHead;
    [SerializeField] private GameObject lineRendererPrefab;
    public GameObject[] squidEnemies;
    public GameObject[] enemiesSpawnPoints;
    public Transform[] squidMovePos;
    public GameObject[] redGuards;
    
    [HideInInspector] public int noOfTargets, remainingEnemies;
    private Animator _dollHeadAnimator;
    private AudioSource _currentAudioSource;

    [HideInInspector] public List<EnemyAI> TotalEnemiesList = new List<EnemyAI>();

    private void Awake()
    {
        instance = this;
    }

    public void StartSquidGame()
    {
        _currentAudioSource = transform.GetComponent<AudioSource>();
        _dollHeadAnimator =  dollHead.GetComponent<Animator>();
        Timer.Instance.startSquidTimer = false;
        Timer.Instance.timerLabel.gameObject.SetActive(false);
        
        for(int i = 0; i < LevelsManager.instance.currentLevel.enemiesList.Length; i++)
        {
            TotalEnemiesList.Add(LevelsManager.instance.currentLevel.enemiesList[i]);
            LevelsManager.instance.currentLevel.enemiesList[i].movePos = squidMovePos[i];
            LevelsManager.instance.currentLevel.enemiesList[i].StartSquidRun();
        }

       
        remainingEnemies = TotalEnemiesList.Count;
        StartCoroutine(StartDollMusic());
    }
    
    IEnumerator StartDollMusic()
    {
        HudMenuManager.instance.fireBtn.SetActive(false);
        _currentAudioSource.Play();

        yield return new WaitForSeconds(Random.Range(4, 7));
        _currentAudioSource.Stop();
       _dollHeadAnimator.SetBool("TurnToPlayer",true);
       _dollHeadAnimator.SetBool("TurnAwayFromPlayer",false);

       yield return new WaitForSeconds(1f);
        if(TotalEnemiesList.Count > 0)
       SetTargets();
    }

    public void StartDollMusicAgain()
    {
        StartCoroutine(StartDollMusicAgainCo());
    }

    IEnumerator StartDollMusicAgainCo()
    {
        HudMenuManager.instance.fireBtn.SetActive(false);
        
        _dollHeadAnimator.SetBool("TurnToPlayer",false);
        _dollHeadAnimator.SetBool("TurnAwayFromPlayer",true);
        Timer.Instance.startSquidTimer = false;
        Timer.Instance.timerLabel.gameObject.SetActive(false);
        
        yield return new WaitForSeconds(1f);
        _currentAudioSource.Play();
        foreach (var enemyAI in TotalEnemiesList)
        {
            enemyAI.PlayForSquidMode();
        }
        
        yield return new WaitForSeconds(Random.Range(4, 7));
        _currentAudioSource.Stop();
        _dollHeadAnimator.SetBool("TurnToPlayer",true);
        _dollHeadAnimator.SetBool("TurnAwayFromPlayer",false);

        // yield return new WaitForSeconds(1f);
        if (TotalEnemiesList.Count > 0)
            SetTargets();
    }

    void SetTargets()
    {
        HudMenuManager.instance.fireBtn.SetActive(true);

        if (TotalEnemiesList.Count > 3)
            noOfTargets = Random.Range(LevelsManager.instance.currentLevel.minTargetRange,
                LevelsManager.instance.currentLevel.maxTargetRange);
        else if (TotalEnemiesList.Count > 1)
            noOfTargets = Random.Range(1, TotalEnemiesList.Count);
        else
        {
            int x = Random.Range(0, 100);

            if (x < 50)
            {
                noOfTargets = 0;
                Invoke(nameof(StartDollMusicAgain), 2f);
            }
            else
            {
                noOfTargets = 1;
            }
           
        }

        foreach (var enemy in TotalEnemiesList)
        {
            enemy.PauseForSquidMode();
        }
        
        for (int i = 0; i < noOfTargets; i++)
        {
            int rand = Random.Range(0, TotalEnemiesList.Count);
            TotalEnemiesList[rand].highlighted = true;
            TotalEnemiesList[rand].GetComponent<Outlinable>().enabled = true;
            
            var line = Instantiate(lineRendererPrefab, dollHead.transform);
            TotalEnemiesList[rand]._myLineRenderer = line;
            line.GetComponent<AnimatedLineRenderer>().SetLinePoints(dollHead.transform, 0, TotalEnemiesList[rand].transform, 1);
            
            TotalEnemiesList.RemoveAt(rand);
        }

        Timer.Instance.resetTime();
        Timer.Instance.startSquidTimer = true;
        Timer.Instance.timerLabel.gameObject.SetActive(true);
    }

    public void GuardsShootAtPlayer()
    {
        foreach (var redGuard in redGuards)
        {
            redGuard.GetComponent<Animator>().SetBool("shoot", true);
            redGuard.GetComponent<IkTarget>().enabled = true;
            
            var line = Instantiate(lineRendererPrefab, redGuard.transform);
            line.GetComponent<AnimatedLineRenderer>()
                .SetLinePoints(redGuard.transform, 1f, Camera.main.transform, -0.5f);
        }
    }
}
