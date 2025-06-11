using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;
    [SerializeField] private GameObject leaderboardScreen;
    public GameObject entryPrefab;
    [SerializeField] private GameObject loadingCircle;
    public Transform content;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            loadingCircle.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HideLeaderboard()
    {
        leaderboardScreen.SetActive(false);
        this.enabled = false;  
    }

    public void ShowLeaderboard()
    {
        Debug.Log("Opening Leaderboard");
        this.enabled = true;  
        leaderboardScreen.SetActive(true);
        loadingCircle.SetActive(true);

        if (NetworkManager.instance == null)
        {
            Debug.LogError("NetworkManager is null");
            return;
        }

        StartCoroutine(NetworkManager.instance.GetLeaderboard(CallbackGetLeaderboard));
    }

    // Callback from networkManager
    public void CallbackGetLeaderboard(string jsonData)
    {
        LeaderDetails[] leaderboardData = JsonConvert.DeserializeObject<LeaderDetails[]>(jsonData);

        var length = leaderboardData.Length;
        Debug.Log("Received leaderboard: " + length);

        #region Clear previous entries
        foreach (Transform child in content)
        {
            if (child.gameObject == loadingCircle) continue;
            Destroy(child.gameObject);
        }
        #endregion

        loadingCircle.SetActive(false);

        #region Create entries
        List<Transform> children = new List<Transform>();

        // Populate new leaderboard entries
        for (int i = 0; i < length; i++)
        {
            GameObject entryObject = Instantiate(entryPrefab, content);
            LeaderBoardSlotAccess entryUI = entryObject.GetComponent<LeaderBoardSlotAccess>();
            entryUI.SetEntry(leaderboardData[i]);
            children.Add(entryObject.transform);
        }
        #endregion

        #region Sort
        children = children
            .OrderByDescending(child =>
            {
                var details = child.GetComponent<LeaderBoardSlotAccess>();
                int score;
                return int.TryParse(details.textScore.text, out score) ? score : int.MinValue;
            })
            .ToList();

        // Reorder in hierarchy
        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
            children[i].GetComponent<LeaderBoardSlotAccess>().textRank.text = (i + 1).ToString();
        }
        #endregion
    }
}
