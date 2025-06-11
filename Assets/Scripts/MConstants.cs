using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

public class MConstants : MonoBehaviour
{
    public static bool ISNATIVE_AD_LOADED;
    public static int CurrentLevelNumber = 1;
    public static int MAX_LEVELS = 45;
    public static int MAX_LEVELS_Expert = 30;
    public static int MAX_LEVELS_ZOMBIE = 25;
    public static int MAX_LEVELS_SQUID = 15;
    public static int MAX_LEVELS_BATTLE = 15;
    public static int SurvivalMaxLevels = 5;
    public static int HealtInstLimit;
    public static int GunInstLimit;
    public static float currentLevelCount = 0;
    public static GameObject Tempweapon;

    //  public static int TempSelectedWeapon;
    public static bool isPlayerWin = false;
    public static string Env_1 = "Campaign"; // Mode1
    public static string Env_2 = "Expert"; // Mode2
    public static string Env_3 = "Zombie"; // Mode3
    public static string Env_4_Squid = "Squid"; // Mode4
    public static string Env_5_BattleField = "BattleField"; // Mode4
    public static string Env_Survival = "Survival"; //ENDLESS
    public static string Env_Endless = "EndelssMode"; //ENDLESS
    public static  string Env_6 = "UIScene";
    public static string MissionObjective;
    public static string FailReason;
    public static string RATE_US =
        "https://play.google.com/store/apps/details?id=com.frenzygames.fgs.snipershootinggames.fps.cityhuntershooter";

    public static int MinPickHealth = 10, MaxPickHealth = 15;
    //public static string RATE_US="itms-apps://itunes.apple.com/app/1176956813";IOS

    public static bool autoFireMode, bulletsFinished, IslastBullet, IsLastBulletHeadShot;

    public static bool isTimeOver = false;
    public static bool StartTimer;
    public static bool isGameOver = false;
    public static bool isGameStarted = false;
    public static bool HavingTemporaryGun = false;
    public static bool BeginLevel;
    public static bool StartRunningInRunningMission;
    public static bool BulletFired;
    public static bool ComingFromLevels;
    public static int ShowRecommendedWeaponNo;

    public static int rewardGold = 100, EndRandom = 200;
    public static GAME_MODES CurrentGameMode = GAME_MODES.MODE3_Zombie;
    public static GameObject LastBulletTarget;

    public static Vector3 BulletInfo;
    public static RaycastHit LastBulletHit;
    
    // public static bool AutoMove, AutoRotate;
    public enum GAME_MODES
    {
        MODE1,
        MODE2_Expert,
        MODE3_Zombie,
        MODE4_Squid,
        MODE5_BATTLEFIELD,
        SURVIVAL_MODE,
        ENDLESS_MODE,
    }

    public static string Mode1Name = "CAMPAIGN MODE";
    public static string Mode2Name = "MISSION";
    public static string Mode3Name = "ZOMBIE MODE";
    public static string SquidModeName = "SQUID MODE";
    public static string BattleFieldModeName = "BATTLE FIELD";
}