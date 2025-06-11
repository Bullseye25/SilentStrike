using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class PlayerDataController : MonoBehaviour {
	public static PlayerDataController Instance;
	[HideInInspector]
	public PlayerDataSerializeable playerData;
	public GameObject defaultPlayerDataObject;

	string fileName = "/GameStatebunker.Gd" ;
	void Awake(){
		if (Instance == null)
		{
			Instance = this;
			Load();
			DontDestroyOnLoad(this);
			return;
		}
		DestroyImmediate(gameObject);
	}

	void LoadSaveDeafultData(){
		playerData = new PlayerDataSerializeable ();
		PlayerData defaultPlayerData = defaultPlayerDataObject.GetComponent<PlayerData> ();
		playerData.PlayerCash = defaultPlayerData.PlayerCash;
		playerData.PlayerGold = defaultPlayerData.PlayerGold;

		playerData.gunsList = new List<PlayerCar> ();
		playerData.StarsList = new List<int> ();

		for (int i = 0; i < defaultPlayerData.TotalLevels; i++) {
			playerData.StarsList.Add (0);
		}

		for (int i = 0; i < defaultPlayerData.CarsList.Count; i++) {
			PlayerCar pCar = new PlayerCar ();
			pCar.ID = defaultPlayerData.CarsList [i].ID;
			pCar.Name = defaultPlayerData.CarsList[i].Name;
			pCar.Type = defaultPlayerData.CarsList[i].type;
			pCar.isLocked = defaultPlayerData.CarsList [i].isLocked;
			pCar.N2O = defaultPlayerData.CarsList [i].N2O;
			pCar.Speed = defaultPlayerData.CarsList [i].Speed;
			pCar.UnlockPrice = defaultPlayerData.CarsList [i].UnlockPrice;
			pCar.UpgradeLevel = defaultPlayerData.CarsList [i].UpgradeLevel;
			pCar.UpgradePrice = defaultPlayerData.CarsList [i].UpgradePrice;
			pCar.Control = defaultPlayerData.CarsList [i].Control;
			pCar.Acceleration = defaultPlayerData.CarsList [i].Acceleration;
			pCar.isLocked = defaultPlayerData.CarsList [i].isLocked;
			playerData.gunsList.Add (pCar);

		}

		playerData.envioronmentList = new List<PlayerEnvironment> ();

		for (int i = 0; i < defaultPlayerData.envioronmentList.Count; i++) {
			PlayerEnvironment pCar = new PlayerEnvironment ();
			pCar.ID = defaultPlayerData.envioronmentList [i].ID;
			pCar.UnlockPrice = defaultPlayerData.envioronmentList [i].UnlockPrice;
			pCar.isLocked = defaultPlayerData.envioronmentList [i].isLocked;
			playerData.envioronmentList.Add (pCar);
			
		}

		Save ();
		//Load ();
	
	}

    public void HandleIncreamentData()
    {
        PlayerData defaultPlayerData = defaultPlayerDataObject.GetComponent<PlayerData>();

        //		if(playerData.StarsList.Count < defaultPlayerData.TotalLevels){    
        //			for (int i = playerData.StarsList.Count; i < defaultPlayerData.TotalLevels; i++) {
        //				playerData.StarsList.Add (0);
        //			}
        //		}
        if (playerData.gunsList.Count < defaultPlayerData.CarsList.Count)
        {
            for (int i = playerData.gunsList.Count; i < defaultPlayerData.CarsList.Count; i++)
            {
                PlayerCar pCar = new PlayerCar();
                pCar.ID = defaultPlayerData.CarsList[i].ID;

                pCar.isLocked = defaultPlayerData.CarsList[i].isLocked;
                pCar.N2O = defaultPlayerData.CarsList[i].N2O;
                pCar.Speed = defaultPlayerData.CarsList[i].Speed;
                pCar.UnlockPrice = defaultPlayerData.CarsList[i].UnlockPrice;
                pCar.UpgradeLevel = defaultPlayerData.CarsList[i].UpgradeLevel;
                pCar.UpgradePrice = defaultPlayerData.CarsList[i].UpgradePrice;
                pCar.Control = defaultPlayerData.CarsList[i].Control;
                pCar.Acceleration = defaultPlayerData.CarsList[i].Acceleration;
                pCar.isLocked = defaultPlayerData.CarsList[i].isLocked;
                playerData.gunsList.Add(pCar);

            }
        }
        
        if (playerData.currentSelectLevel_SquidMode < 1)
	        playerData.currentSelectLevel_SquidMode = 1;
        if (playerData.LastUnlockedLevel_SquidMode < 1)
	        playerData.LastUnlockedLevel_SquidMode = 1;
        
        if (playerData.currentSelectLevel_BattleMode < 1)
	        playerData.currentSelectLevel_BattleMode = 1;
        if (playerData.LastUnlockedLevel_BattleMode < 1)
	        playerData.LastUnlockedLevel_BattleMode = 1;
    }

    //it's static so we can call it from anywhere
    public  void Save() {
		//BinaryFormatter bf = new BinaryFormatter();
		////Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
		//FileStream file = File.Create (Application.persistentDataPath + fileName); //you can call it anything you want
		//bf.Serialize(file, playerData);
		//file.Close();

		string json = JsonUtility.ToJson(playerData);
		PlayerPrefs.SetString("PlayerData", json);
		PlayerPrefs.Save();  // Ensure data is immediately saved
	}

    public void Load()
    {
        //try
        //{
        //    if (File.Exists(Application.persistentDataPath + fileName))
        //    {
        //        BinaryFormatter bf = new BinaryFormatter();
        //        FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);
        //        Debug.Log("Path " + Application.persistentDataPath + fileName);
        //        playerData = (PlayerDataSerializeable)bf.Deserialize(file);
        //        file.Close();
        //        HandleIncreamentData();

        //    }
        //    else
        //    {
        //        LoadSaveDeafultData();
        //    }
        //}
        //catch (System.Exception ex)
        //{
        //    LoadSaveDeafultData();

        //}

        try
        {
            if (PlayerPrefs.HasKey("PlayerData"))
            {
                string json = PlayerPrefs.GetString("PlayerData");
                playerData = JsonUtility.FromJson<PlayerDataSerializeable>(json);
                HandleIncreamentData();
            }
            else
            {
                LoadSaveDeafultData();
            }
        }
        catch (System.Exception ex)
        {
            LoadSaveDeafultData();
        }


    }



}
