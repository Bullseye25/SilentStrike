
using System.Collections.Generic;

[System.Serializable]
public class PlayerDataSerializeable  {
	public int PlayerGold = 100;
	public int PlayerCash = 100;
	public int Rank =1;
	public int xpoints =0;

	public List<PlayerCar> gunsList;
	public List<int> StarsList;

	public List<PlayerEnvironment> envioronmentList;
	public int CurrentSelectedPrimaryGun = 0;
    public int CurrentSelectedSecondaryGun = 1;
    public int SelectedVehicle_temp = 0;

    public int CurrentMode=1;
	public int CurrentEnvironment=1;
	public int SelectedControl=0;
	public bool isSoundOn= true;
	public bool isPromoClicked= false;

	public int currentSelectLevel_Mode2=1;
    public int currentSelectLevel_Mode3 = 1;
    public int currentSelectLevel_SquidMode = 1;
    public int currentSelectLevel_BattleMode = 1;
    public int currentSelectLevel_Mode1 = 1;
    public int BestKill = 0;
    public bool isRateUSDone= false;
	public bool isControlTutorialDone= false;
	public bool buyShotgunTutorial;
	public bool firstTimeTutorial;
	
	public int LastUnlockedLevel_Mode2=1;
    public int LastUnlockedLevel_Mode3 = 1;
    public int LastUnlockedLevel_SquidMode = 1;
    public int LastUnlockedLevel_BattleMode = 1;
	public int LastUnlockedLevel_Mode1 = 1;

    public int megaSaleCount = 0;

    public float BestTime = 0;
    public float SensivityValue = 1;
    public bool isHighQuality;
    public bool isRemoveAds = false;
    public bool unlockedAllGuns;
    public bool unlockedAllLevels;
    public bool unlockEverything;

    public bool FirstTimeReward;
    public long lastChestOpened;

    public int rewardDay = 1;
    public long lastWeeklyRewarded;
    public long RewardedDate;
    public int tempCoins;
  
    public long lastSpinReward;
    public int SpinCount = 0;

    public long fetch_Time = 5;
    public bool ShowAppOpen = true;
    public bool isSquidMode = true;
}
