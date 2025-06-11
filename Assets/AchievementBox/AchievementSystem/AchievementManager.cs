using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    public ThreeRoundDaily threeRoundQuest;
    public Text totalscore;
    
    private const string achievementKey = "DailyAchievements";
    private string today;

    [Space]
    public List<Achievement> achievements = new List<Achievement>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();

            threeRoundQuest = new ThreeRoundDaily(this);
        }
        else 
            Destroy(gameObject);
    }

    public void ReportProgress(Quest achievementId, bool condition)
    {
        Achievement a = achievements.Find(x => x.id == achievementId);

        if (a == null || a.isAchieved == true) return;

        if(condition == true)
        {
            a.isAchieved = true;
            AchievementPopupManager.Instance.ShowAchievement(a);
            PlayerPrefs.SetInt(a.id.ToString(), 1);

            ///TODO: Call API To Update Quest Status in the Backend
        }
    }


    public void Init()
    {
        today = DateTime.Now.ToString("yyyy-MM-dd");

        if (PlayerPrefs.HasKey(achievementKey))
        {
            string sameDay = PlayerPrefs.GetString(achievementKey);

            if (sameDay == today)
            {
                for (int index = 0; index < achievements.Count; index++)
                {
                    var value = PlayerPrefs.GetInt(achievements[index].id.ToString());

                    achievements[index].isAchieved = value == 1;
                }
            }
            else
            {
                PlayerPrefs.SetString(achievementKey, today);
                PlayerPrefs.Save();

                achievements.ForEach((a) => 
                {
                    PlayerPrefs.SetInt(a.id.ToString(), 0);
                    a.isAchieved = false; 
                });
            }
        }
        else
        {
            PlayerPrefs.SetString(achievementKey, today);
            PlayerPrefs.Save();

            achievements.ForEach((a) =>
            {
                PlayerPrefs.SetInt(a.id.ToString(), 0);
                a.isAchieved = false;
            });
        }
    }

    public void ReturningKiller()
    {
        threeRoundQuest.ProgressUpdate(int.Parse(totalscore.text));
    }
}