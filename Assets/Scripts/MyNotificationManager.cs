using System;
using NotificationSamples;
/*using Unity.Notifications.Android;*/
using UnityEngine;
using UnityEngine.UI;

public class MyNotificationManager : MonoBehaviour
{
    public static MyNotificationManager instance;
    public GameNotificationsManager NotificationsManager;
    public string Title, Description;

    // Start is called before the first frame update
    void Start()
    {
       /* instance = this;
        StartNotifications();
       *//* AndroidNotificationCenter.CancelAllNotifications();*/

       /* var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();*//*

        if (notificationIntentData != null)
        {
            NotificationOpenedEvent();
        }*/
    }

    void StartNotifications()
    {
        GameNotificationChannel channel =
            new GameNotificationChannel("DailyReward", "Daily Reward", "Daily Reward");
        NotificationsManager.Initialize(channel);
    }

    public void ShowNotification(float hour)
    {
        ShowNotificationAfterDelay(Title, Description, DateTime.Now.AddSeconds(hour));
    }

    void ShowNotificationAfterDelay(string title, string body, DateTime time)
    {
        IGameNotification Notification = NotificationsManager.CreateNotification();

        if (Notification != null)
        {
            Notification.Title = title;
            Notification.Body = body;
            Notification.DeliveryTime = time;
            Notification.SmallIcon = "icon_0";
            Notification.LargeIcon = "icon_1";

            NotificationsManager.ScheduleNotification(Notification);
        }
    }

    public void NotificationOpenedEvent()
    {
      //  RewardManager.instance.rewardDialog.SetActive(true);
      //  RewardManager.instance.confettiParticle.SetActive(true);

    }
}