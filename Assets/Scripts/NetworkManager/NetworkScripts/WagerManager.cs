using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;

public class WagerManager : MonoBehaviour{
    public static WagerManager instance;
    [DllImport("__Internal")] private static extern void SendToFrontEnd(string message);
    public bool wagerPaid;
    [SerializeField] private Button freePlayButton;

    private void wagerResponse(bool response)
    {
        if (!response) return;
        
        wagerPaid = true;
        Debug.Log("wagerpaid: " + wagerPaid);
        freePlay();
    }

    public void sendWager()
    {
        Debug.Log("wager button clicked");
        SendToFrontEnd("SendWager");
        // wagerResponse(true);
    }

    public void freePlay(){
        freePlayButton.onClick.Invoke();
    }
    private void Awake(){
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
            Debug.Log("WagerManager initialized");
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
