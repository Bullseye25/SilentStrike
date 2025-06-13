using Newtonsoft.Json;
using System.Collections;
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

        StartCoroutine(FetchAndDisplayLeaderboard());
    }

    private IEnumerator FetchAndDisplayLeaderboard()
    {
        yield return StartCoroutine(NetworkManager.instance.GetLeaderboard());

        string jsonData = NetworkManager.instance.leaderboardJson;
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogError("Leaderboard JSON is empty or null");
            loadingCircle.SetActive(false);
            yield break;
        }

        yield return StartCoroutine(HandleLeaderboard(jsonData));
    }

    [ContextMenu("Create 25 Leaderboard Slots")]
    private void CreateLeaderboardSlots()
    {
        foreach (Transform child in content)
        {
            if (child.gameObject != loadingCircle)
                DestroyImmediate(child.gameObject);
        }

        for (int i = 0; i < 25; i++)
        {
            GameObject entryObject = Instantiate(entryPrefab, content);
            entryObject.name = "LeaderboardSlot_" + (i + 1);
            entryObject.GetComponent<LeaderBoardSlotAccess>().Clear();
            entryObject.SetActive(false);
        }

        Debug.Log("Created 25 leaderboard slots.");
    }

    private IEnumerator HandleLeaderboard(string jsonData)
    {
        // Wrap raw array in a dummy root object
        jsonData = "{\"leaderboard\":" + jsonData + "}";

        // Parse using Unity's JsonUtility
        LeaderboardWrapper wrapper = JsonUtility.FromJson<LeaderboardWrapper>(jsonData);
        LeaderDetails[] leaderboardData = wrapper.leaderboard;

        int length = leaderboardData.Length;
        Debug.Log("Received leaderboard: " + length);

        loadingCircle.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        List<Transform> children = new List<Transform>();
        int i = 0;

        foreach (Transform child in content)
        {
            if (child.gameObject == loadingCircle) continue;

            if (i < leaderboardData.Length)
            {
                child.gameObject.SetActive(true);
                var entryUI = child.GetComponent<LeaderBoardSlotAccess>();
                entryUI.SetEntry(leaderboardData[i]);
                children.Add(child);
            }
            else
            {
                child.gameObject.SetActive(false);
            }

            i++;
            if (i >= 22) break;
        }

        yield return new WaitForSeconds(0.5f);

        // Sort entries by Score descending
        children = children
            .OrderByDescending(child =>
            {
                var details = child.GetComponent<LeaderBoardSlotAccess>();
                return int.TryParse(details.textScore.text, out int score) ? score : int.MinValue;
            })
            .ToList();

        for (int j = 0; j < children.Count; j++)
        {
            children[j].SetSiblingIndex(j);
            children[j].GetComponent<LeaderBoardSlotAccess>().textRank.text = (j + 1).ToString();
        }
    }
}
