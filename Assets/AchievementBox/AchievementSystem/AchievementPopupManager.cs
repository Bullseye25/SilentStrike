using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementPopupManager : MonoBehaviour
{
    public static AchievementPopupManager Instance;

    [SerializeField] private GameObject popup;
    [SerializeField] private AchievementPopupUI popupUI;
    [SerializeField] private float displayDuration = 2.5f;
    [SerializeField] private float delayBetweenPopups = 0.5f;

    private Queue<Achievement> popupQueue = new Queue<Achievement>();
    private bool isShowing = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowAchievement(Achievement achievement)
    {
        popupQueue.Enqueue(achievement);
        if (!isShowing) StartCoroutine(ShowNextPopup());
    }

    private IEnumerator ShowNextPopup()
    {
        while (popupQueue.Count > 0)
        {
            isShowing = true;

            Achievement achievement = popupQueue.Dequeue();

            popup.SetActive(true);
            popupUI.Initialize(achievement);
            
            yield return new WaitForSeconds(displayDuration);
            
            popup.SetActive(false);

            yield return new WaitForSeconds(delayBetweenPopups);
        }
        isShowing = false;
    }
}
