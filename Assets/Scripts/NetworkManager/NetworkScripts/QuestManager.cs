using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public GameObject questPanel;
    public GameObject questPrefab;
    public GameObject completedPrefab;
    public GameObject loadingCirclePrefab;
    private GameObject loadingCircle;
    public Transform content;
    public QuestInstance[] Quests;
    public int LatestScore = 0;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
            GameObject loadingCircleObject = Instantiate(loadingCirclePrefab, content);
            loadingCircle = loadingCircleObject;
            loadingCircle.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HideQuests()
    {

        questPanel.SetActive(false);
        this.enabled = false;
    }

    public void ShowQuests()
    {

        this.enabled = true;
        questPanel.SetActive(true);
        loadingCircle.SetActive(true);
        if (NetworkManager.instance == null)
        {
            Debug.LogError("NetworkManager is null");
        }
        StartCoroutine(NetworkManager.instance.AssignThenFetchQuests(CallbackGetQuests));
    }

    // Callback from networkManager
    public void CallbackGetQuests(string jsonData)
    {

        QuestInstance[] questData = JsonConvert.DeserializeObject<QuestInstance[]>(jsonData);
        Quests = questData;
        Debug.Log("recieved quests: " + questData.Length);

        // Clear previous entries
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        List<QuestInstance> completedQuests = new List<QuestInstance>();
        List<QuestInstance> incompleteQuests = new List<QuestInstance>();

        // Populate new quest entries
        for (int i = 0; i < questData.Length; i++)
        {
            if (questData[i].progress == 100)
            {
                completedQuests.Add(questData[i]);
            }
            else
            {
                incompleteQuests.Add(questData[i]);
            }
        }

        loadingCircle.SetActive(false);

        foreach (QuestInstance quest in incompleteQuests)
        {
            GameObject entryObject = Instantiate(questPrefab, content);
            QuestEntry entryUI = entryObject.GetComponent<QuestEntry>();
            entryUI.SetEntry(quest);
        }

        foreach (QuestInstance quest in completedQuests)
        {
            GameObject completedObject = Instantiate(completedPrefab, content);
            QuestEntry completedUI = completedObject.GetComponent<QuestEntry>();
            completedUI.SetEntry(quest);
        }

    }

    private void Update()
    {

        if (loadingCircle != null && loadingCircle.activeInHierarchy)
        {
            loadingCircle.transform.Rotate(0, 0, -200 * Time.deltaTime);
        }
    }

}
