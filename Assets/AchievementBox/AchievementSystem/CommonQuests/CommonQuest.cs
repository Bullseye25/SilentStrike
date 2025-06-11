using System;
using UnityEngine;

public class CommonQuest
{
    private const string questKey = "DailyQuest";
    private string today;
    protected AchievementManager achievementManager;

    public void Init(AchievementManager achievementManager)
    {
        this.achievementManager = achievementManager;
        today = DateTime.Now.ToString("yyyy-MM-dd"); //Dev Note: Better To Use An API In The Future..
    }

    public void ExecuteQuest(Action onSuccess, Action onFail)
    {
        if (PlayerPrefs.HasKey(questKey))
        {
            string sameDay = PlayerPrefs.GetString(questKey);

            if (sameDay == today)
            {
                onSuccess?.Invoke();
            }
            else
            {
                PlayerPrefs.SetString(questKey, today);
                PlayerPrefs.Save();

                onFail?.Invoke();
            }
        }
        else
        {
            PlayerPrefs.SetString(questKey, today);
            PlayerPrefs.Save();

            onFail?.Invoke();
        }
    }
}
