using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq;

public class QuestEntry : MonoBehaviour
{
    public TextMeshProUGUI textQuest;  
    public TextMeshProUGUI textReward; 

    public void SetEntry(QuestInstance quest)
    {
        textQuest.text = quest.template.title;
        textReward.text = quest.template.reward.ToString();
    }
}
[System.Serializable]
public class QuestTemplate
{
    public string _id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public int reward { get; set; }
    public JObject script { get; set; }
    public int __v { get; set; }
}

[System.Serializable]
public class QuestInstance
{
    public string _id { get; set; }
    public QuestTemplate template { get; set; }
    public string userWallet { get; set; }
    public string game { get; set; }
    public int progress { get; set; }
    public int __v { get; set; }

}