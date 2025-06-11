using UnityEngine;
using Newtonsoft.Json;

public class QuestModal : MonoBehaviour
{
    public static QuestModal instance;
    public GameObject modal;
    public GameObject rewardRowPrefab;
    public GameObject loadingIndicator; 
    public Transform content;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HideModal()
    {
        modal.SetActive(false);
    }

    public void ShowModal(string jsonData)
    {
        QuestInstance[] questData = JsonConvert.DeserializeObject<QuestInstance[]>(jsonData);
        // Debug.Log("Modal: " + questData.Length);
        if (questData.Length == 0)
        {
            return;
        }
        Debug.Log("Show Modal: " + questData.Length);
        modal.SetActive(true);
        
        PopulateModal(questData);
    }

    private void PopulateModal(QuestInstance[] quests)
    {   
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        loadingIndicator.SetActive(true);

            loadingIndicator.SetActive(false);
            foreach (var quest in quests)
            {
                if (quest.progress == 100)
                {
                    GameObject entryObject = Instantiate(rewardRowPrefab, content);
                    QuestEntry entryUI = entryObject.GetComponent<QuestEntry>();
                    entryUI.SetEntry(quest);
                }
            }

        modal.SetActive(true);
    }

    public void QuestModalTest(){
        StartCoroutine(NetworkManager.instance.AssignThenFetchQuests(ShowModal));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && modal.activeSelf)
        {
            HideModal();
        }
    }


}
