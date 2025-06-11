using UnityEngine;
using TMPro;

public class LeaderBoardEntry : MonoBehaviour
{
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textRank;
    public GameObject[] rankIcons;

    public void SetEntry(UserInfo userinfo)
    {
        textName.text = userinfo.userName;
        textScore.text = userinfo.score.ToString();
        textRank.text = "0";
        foreach (var icon in rankIcons)
        {
            icon.SetActive(false);
        }
    }
}

[System.Serializable]
public class UserInfo 
{
    public int score;
    public string userName;
    public string privyID;
    public int reward;
    public bool isCheater;

    public string TruncateUsername(string username)
    {
        if (string.IsNullOrEmpty(username) || username.Length <= 10)
            return username;

        string firstFour = username.Substring(0, 5);
        string lastFour = username.Substring(username.Length - 5, 5);
        return firstFour + "..." + lastFour;
    }

    public UserInfo(int score, bool isCheater, string userName, string privyID, int reward = 0)
    {
        this.score = score;
        this.isCheater = isCheater;
        this.userName = TruncateUsername(userName);
        this.privyID = privyID;
        this.reward = reward;
    }

}
