using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Analytics;
using System;

public class UnityAnalyticsScript : MonoBehaviour {

	public static UnityAnalyticsScript instance;

	void Awake(){
		instance = this;
	}

	void Start(){
		DontDestroyOnLoad (this);
	}

	public void AddUnityEvent(string EventName, IDictionary<string, object> dictionary){
		Analytics.CustomEvent(""+EventName, dictionary);
	}

	public void AddFirebaseEvent(string EventName, int levelNum)
	{
		// Firebase.Analytics.FirebaseAnalytics.LogEvent(EventName + levelNum);
		/*FinzAnalysisManager.Instance.LogFirebaseEvent(EventName + levelNum);*/
	}
	
	public void AddFirebaseEvent(string EventName)
	{
		// Firebase.Analytics.FirebaseAnalytics.LogEvent(EventName);
	}
}