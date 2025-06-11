using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardSlotAccess : MonoBehaviour
{
    public Text textName;
    public Text textScore;
    public Text textRank;
   /* public GameObject[] rankIcons;*/

    public void SetEntry(LeaderDetails userinfo)
    {
        textName.text = userinfo.TruncateUsername(userinfo.Username);
        textScore.text = userinfo.Score.ToString();
        textRank.text = "0";
/*        foreach (var icon in rankIcons)
        {
            icon.SetActive(false);
        }*/
    }
}


[SerializeField]
public class LeaderDetails
{
    public string WalletID;
    public string Game;
    public string Username;
    public int Score;

    public string TruncateUsername(string username)
    {
        if (string.IsNullOrEmpty(username) || username.Length <= 10)
            return username;

        string firstFour = username.Substring(0, 5);
        string lastFour = username.Substring(username.Length - 5, 5);
        return firstFour + "..." + lastFour;
    }
}