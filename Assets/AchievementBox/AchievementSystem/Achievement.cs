using UnityEngine;

[System.Serializable]
public class Achievement
{
    public Quest id;
    public string description;
    public bool isAchieved;
    public Sprite icon;
}

public enum Quest
{
    KillerInstinct = 11,    //Score 150 Kills -
    OnTheEdge = 12,        //Score 75 Kills  -
    HitMan = 13,            //Score 50 Kills   -      
    Focus = 14,      //Kill 5 under 2 mins
    DoingMyJob = 15,    //Kill 10 under 4 mins
    KillerIsBack = 16,      //Play the game at least 3 times back to back -
    DailyKills = 17,     //Score 10 Kills-
};
