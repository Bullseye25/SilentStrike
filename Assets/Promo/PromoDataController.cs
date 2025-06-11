using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PromoDataController : MonoBehaviour {
	public static PromoDataController Instance;
	[HideInInspector]
	public PromoDataSrialized playerData;
	public GameObject defaultPlayerDataObject;

	string fileName = "/Promodata.Gd" ;
	//void Awake(){
	//	Instance = this;
	//	Load ();
	//	DontDestroyOnLoad (this);
	//}

	//void LoadSaveDeafultData(){
	//	playerData = new PromoDataSrialized ();
	//	PromoData defaultPlayerData = defaultPlayerDataObject.GetComponent<PromoData> ();


	//	playerData.PromoListList = new List<PromoDataSerializeableScript> ();

	//	for (int i = 0; i < defaultPlayerData.PromoListList.Count; i++) {
	//		PromoDataSerializeableScript pCar = new PromoDataSerializeableScript ();
	//		pCar.gameLink = defaultPlayerData.PromoListList [i].gameLink;
	//		pCar.Id = defaultPlayerData.PromoListList [i].Id;
	//		pCar.isPromoClicked = defaultPlayerData.PromoListList [i].isPromoClicked;

	//		playerData.PromoListList.Add (pCar);

	//	}


	//	Save ();
	//	//Load ();

	//}

	////it's static so we can call it from anywhere
	//public  void Save() {
	//	BinaryFormatter bf = new BinaryFormatter();
	//	//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
	//	FileStream file = File.Create (Application.persistentDataPath + fileName); //you can call it anything you want
	//	bf.Serialize(file, playerData);
	//	file.Close();
	//}   

	//public  void Load() {
	//	if (File.Exists (Application.persistentDataPath + fileName)) {
	//		BinaryFormatter bf = new BinaryFormatter ();
	//		FileStream file = File.Open (Application.persistentDataPath + fileName, FileMode.Open);
	//		Debug.Log ("Path "+Application.persistentDataPath + fileName);
	//		playerData = (PromoDataSrialized)bf.Deserialize (file);
	//		file.Close ();
	//	} else {
	//		LoadSaveDeafultData ();
	//	}

	//}



}
