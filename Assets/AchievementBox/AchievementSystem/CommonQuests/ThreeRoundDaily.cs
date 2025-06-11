public class ThreeRoundDaily : CommonQuest
{
    private const int commonDailyRound = 3;
    private const int minimumScore = 5;
    private int currentRound = 0;

    public ThreeRoundDaily(AchievementManager achievementManager)
    {
        Init(achievementManager);
    }

    public void ProgressUpdate(int score)
    {
        ExecuteQuest( 
            () =>  //On Success
            {
                if (currentRound < commonDailyRound && score > minimumScore) //Play 3 Rounds with minimum score
                {
                    currentRound++;
                }

                achievementManager.ReportProgress(Quest.KillerIsBack, currentRound == commonDailyRound);
            }, 
            
            () => //On Fail
            {
                currentRound = 1; //Reset
            });
    }
}
