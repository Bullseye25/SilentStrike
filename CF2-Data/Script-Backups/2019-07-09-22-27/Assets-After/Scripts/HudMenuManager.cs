// Code auto-converted by Control Freak 2 on Tuesday, July 09, 2019!
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HudMenuManager : MonoBehaviour {
	public static HudMenuManager instance;
	public GameObject gameOverMenu;
	public GameObject pauseGame;
	public FPSPlayer player;

	public GameObject loading;
	public GameObject BlackCurtain;
	public GameObject reviveMenu;
	public GameObject cameraInstruction;


	public static int TotalEnemiesToKill=0;
	[HideInInspector]
	public  int TotalEnemiesKilled =0;

	//public GameObject gameControlls;
	//public GameObject miniMap;
	//public Camera mainCamera;
	public GameObject MissionInfo;
	public Text missionText;
	public GameObject okButton;
	 int TotalBulletsConsumed = 0;
	int MissShotsCount = 0;

	public Text TargetsText;
	public Text BulletsText;
	public Text TimeText;
	public Text HudTargetsText;
	public Text HudBulletsText;
	void Awake () {
		instance = this;
		MConstants.isGameOver = false;
		refreshHealth ();
		updatePos (1);
		TotalBulletsConsumed = 0;
		TotalEnemiesToKill = 0;
		if (MConstants.CurrentGameMode == MConstants.GAME_MODES.FREE) {
			StartGame ();	
		} else {
			showMissionInstruction ();
		}
	}


	void Start(){
		MissShotsCount = LevelsManager.instance.survivalLevel.TotalMissShots - (TotalBulletsConsumed - TotalEnemiesKilled);
		if(MissShotsCount<= 0){
			MissShotsCount = 0;
		}			
		refreshStats ();
	}

	void refreshHealth(){
	}

	public void doDamage(){

	}

	public  void BulletFired(){
		TotalBulletsConsumed++;

		if (MConstants.CurrentGameMode == MConstants.GAME_MODES.FREE) {

			Invoke ("refreshStats",0.15f);
		}
		refreshStats ();

		 if( MConstants.CurrentGameMode != MConstants.GAME_MODES.FREE && TotalBulletsConsumed >= TotalEnemiesToKill+LevelsManager.instance.Levels[MConstants.CurrentLevelNumber-1].TotalMissShots ){
			MConstants.isPlayerWin = false;	
			GameOver();
		}

	}

	public void GameOver(){
		if( !MConstants.isPlayerWin && AdsManager.instance != null &&  AdsManager.instance.isAdReady() ){
			//ShowRevive menu
			MConstants.isGameOver = true;

			reviveMenu.SetActive(true);
			return;
		}
	

		Invoke ("GameFail",1);

	}

	public void GameFail(){

		MConstants.isGameOver = true;
		AdsManager.isRevive = false;

		gameOverMenu.SetActive (true);

	}

	public void GameOverByNoRevive(){

		MConstants.isGameOver = true;

		gameOverMenu.SetActive (true);
	}
	public void WrongWay(){
		//wrongWay.SetActive (true);
	}

	public void RightWay(){
		//wrongWay.SetActive (false);
	}

	public void LapComplete(){
		//LapCompleteGo.SetActive (true);
		//Invoke ("LapCompleteRemove",2);
	}

	public void LapCompleteRemove(){
	}

	public void updatePos(int pos){
		//for (int i = 0; i < HudMenuManager.instance.posText.Length; i++) {
		//	HudMenuManager.instance.posText[i].text = pos + "/6 Pos";
		//}
	}

	void FreezVehicle(){
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if(player){
			player.GetComponent<Rigidbody> ().isKinematic = true;
		}
	}

	public void GameComplete(){

	    MConstants.isPlayerWin = true;	
		MConstants.isGameOver = true;	

		Invoke ("GameFail",1);

	}

	public void Revive(){
		AdsManager.isRevive = false;
		TotalBulletsConsumed = TotalBulletsConsumed - 5;
		//refreshHealth ();
		Timer.Instance.resetTime ();
		MConstants.isGameOver = false;
		Time.timeScale = 1;
		refreshStats ();
	}

	public void switchCamera(){
		
	}

	public  void AddEnemiesToKill(){
		
		TotalEnemiesToKill++;
		TargetsText.text = TotalEnemiesToKill.ToString ();
		BulletsText.text = (TotalEnemiesToKill+LevelsManager.instance.Levels[MConstants.CurrentLevelNumber-1].TotalMissShots).ToString ();
		TimeText.text =LevelsManager.instance.Levels[MConstants.CurrentLevelNumber-1].time.ToString();
		refreshStats ();
	}

	void refreshStats(){
		if (MConstants.CurrentGameMode == MConstants.GAME_MODES.FREE) {
			MissShotsCount = LevelsManager.instance.survivalLevel.TotalMissShots - (TotalBulletsConsumed - TotalEnemiesKilled);

		
			if (MissShotsCount < 0 ) {
				MConstants.isPlayerWin = false;	

				GameOver();
			}
			HudTargetsText.text = ""+TotalEnemiesKilled;
			if (MissShotsCount < 0) {
				HudBulletsText.text = "0";
			} else {
				HudBulletsText.text = ""+MissShotsCount;

			}

		} else {
			int bullets = (TotalEnemiesToKill + LevelsManager.instance.Levels [MConstants.CurrentLevelNumber - 1].TotalMissShots) - TotalBulletsConsumed;
			if(bullets < 0){
				bullets = 0;
			}
			HudTargetsText.text =(TotalEnemiesToKill - TotalEnemiesKilled).ToString ();
			HudBulletsText.text = (bullets).ToString ();
				
		}

	}
	public void EnemyKilled(){
		TotalEnemiesKilled++;
		if(TotalEnemiesKilled >= TotalEnemiesToKill && MConstants.CurrentGameMode != MConstants.GAME_MODES.FREE){
			GameComplete ();
		}else if(MConstants.CurrentGameMode == MConstants.GAME_MODES.FREE){
			LevelsManager.instance.SpanEnemy ();
		}
		refreshStats ();

	}

	public void showMissionInstruction(){
		//gameControlls.SetActive (false);
		//mainCamera.enabled = false;
		//miniMap.SetActive (false);
		MissionInfo.SetActive (true);
		okButton.SetActive (false);
		Invoke ("PausePlayer",2);
	}


	void PausePlayer(){
		okButton.SetActive (true);

		if(player == null){
			player = GameObject.FindObjectOfType (typeof(FPSPlayer)) as FPSPlayer;
		}

		if(player != null){
			player.paused = true;
		}	
	}

	public void StartGame(){
		Time.timeScale = 1;
		MConstants.isGameStarted = true;

	//	gameControlls.SetActive (true);
		//mainCamera.enabled = true;
		//miniMap.SetActive (true);
		MissionInfo.SetActive (false);
		//LevelsManager.instance.startGame ();
		if(cameraInstruction && MConstants.CurrentLevelNumber ==1 && MConstants.CurrentGameMode != MConstants.GAME_MODES.FREE){
			cameraInstruction.SetActive (true);
			Invoke ("hideCameraInstruction",3);
		}
		if(player == null){
			player = GameObject.FindObjectOfType (typeof(FPSPlayer)) as FPSPlayer;
		}

		if(player != null){
			player.paused = false;
		}
	}
	public void hideCameraInstruction(){
		cameraInstruction.SetActive (false);
	}
	public void PauseGame(){
		pauseGame.SetActive (true);
		if(player == null){
			player = GameObject.FindObjectOfType (typeof(FPSPlayer)) as FPSPlayer;
		}

		if(player != null){
			player.paused = true;
		}
	}

	public void ResumeGame(){
		if(player == null){
			player = GameObject.FindObjectOfType (typeof(FPSPlayer)) as FPSPlayer;
		}

		if(player != null){
			player.paused = false;
		}
	}


	void Update () {
		if (ControlFreak2.CF2Input.GetKeyDown (KeyCode.Escape)) {
			PauseGame ();
		}
	}

}
