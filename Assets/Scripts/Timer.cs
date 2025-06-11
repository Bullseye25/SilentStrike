using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerLabel;
    public AudioSource clockTickingAudioSource;
    public bool startSquidTimer;
    private float time;
    float tempTime;
    public static Timer Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MConstants.isTimeOver = false;
        MConstants.StartTimer = false;
        // clockTickingAudioSource.Stop ();
    }

    void FixedUpdate()
    {
        if (LevelsManager.instance == null || MConstants.isGameOver || !MConstants.isGameStarted)
        {
            // clockTickingAudioSource.Stop ();
            return;
        }

        if (!MConstants.StartTimer || MConstants.IslastBullet)
            return;
        
        time += Time.deltaTime;

        if (LevelsManager.instance.currentLevel.isTimedMission)/*&& MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2)*/
        {
            tempTime = LevelsManager.instance.currentLevel.levelTime - time;
            
            if (tempTime <= 0)
            {
                tempTime = 0;
                MConstants.isPlayerWin = false;
                MConstants.isTimeOver = true;
                HudMenuManager.instance.GameOver();
                // clockTickingAudioSource.Stop ();
            }
        }
        else if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid && startSquidTimer)
        {
            tempTime = LevelsManager.instance.currentLevel.levelTime - time;
            
            if (tempTime <= 0)
            {
                tempTime = 0;
                startSquidTimer = false;
                MConstants.isPlayerWin = false;
                MConstants.isTimeOver = true;
                HudMenuManager.instance.GameOver();
            }
        }
        else
        {
            tempTime = time;
        }

        int minutes = (int) (tempTime / 60); //Divide the guiTime by sixty to get the minutes.
        int seconds = (int) (tempTime % 60); //Use the euclidean division for the seconds.
        //var fraction = (time * 100) % 100;


        //update the label value
        timerLabel.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void resetTime()
    {
        MConstants.isTimeOver = false;
        time = 0;
    }


    public string getTime()
    {
        if (PlayerDataController.Instance == null)
        {
            return "00:00";
        }

        if (PlayerDataController.Instance.playerData.BestTime < time)
        {
            PlayerDataController.Instance.playerData.BestTime = time;
        }

        int minutes =
            (int) (PlayerDataController.Instance.playerData.BestTime /
                   60); //Divide the guiTime by sixty to get the minutes.
        int seconds =
            (int) (PlayerDataController.Instance.playerData.BestTime %
                   60); //Use the euclidean division for the seconds.
        //var fraction = (time * 100) % 100;

        //update the label value
        return string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public bool IsUnderTwoMinutes()
    {
        return tempTime < 120f;
    }

    public bool IsUnderFourMinutes()
    {
        return tempTime < 240f;
    }

}