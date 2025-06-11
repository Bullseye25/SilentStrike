using UnityEngine;
using System.Collections;

public class PlayerCreator : MonoBehaviour
{
    public GameObject selectablePlayer;
    Vector3 lastKnownPos;
    Quaternion lastKnownRot;
    PlayerDataSerializeable playerData;
 
    void Awake()

    {
        Spawn();
    }


    public void Spawn()
    {
        // int FreeModeSpown =  Random.Range(0, LevelsManager.instance.survivalModeSpwanPoints.Length);

        if (MConstants.CurrentLevelNumber > MConstants.MAX_LEVELS)
        {
            MConstants.CurrentLevelNumber = MConstants.MAX_LEVELS;
        }
       
        // if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
        {
            lastKnownPos = LevelsManager.instance.currentLevel.playerSpawnPoint.position;
            lastKnownRot = LevelsManager.instance.currentLevel.playerSpawnPoint.rotation;
        }
        
        /*if (MConstants.CurrentGameMode== MConstants.GAME_MODES.MODE2_Expert)
        {
            lastKnownPos = LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].playerSpawnPoint.position;
            lastKnownRot = LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].playerSpawnPoint.rotation;
        }
        
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
        {
            lastKnownPos = LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].playerSpawnPoint.position;
            lastKnownRot = LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].playerSpawnPoint.rotation;
        }
        
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
        {
            lastKnownPos = LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].playerSpawnPoint.position;
            lastKnownRot = LevelsManager.instance.Levels[MConstants.CurrentLevelNumber - 1].playerSpawnPoint.rotation;
        }*/
        
        if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
        {
            // lastKnownPos = SurvivalLevelsManager.instance.survivalLevels[0].playerSpawnPoint.position;
            // lastKnownRot = SurvivalLevelsManager.instance.survivalLevels[0].playerSpawnPoint.rotation;
        }



        lastKnownRot.x = 0f;
        lastKnownRot.z = 0f;
        selectablePlayer.transform.position = lastKnownPos;
        selectablePlayer.transform.rotation = lastKnownRot;
        selectablePlayer.SetActive(true);

        if (MConstants.CurrentGameMode != MConstants.GAME_MODES.SURVIVAL_MODE)
        {
            if (LevelsManager.instance.currentLevel.playerLookAtPosition != null)
            {
                selectablePlayer.transform.LookAt(LevelsManager.instance.currentLevel.playerLookAtPosition);
            }
        }
    }
}