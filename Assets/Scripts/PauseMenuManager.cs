using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour {
	void OnEnable () {
		Time.timeScale = 1f;
		AudioListener.volume =0;
		//AdsManager.instance.showStaticIntertial();
	}

	public void Resume(){

		gameObject.SetActive (false);
		Time.timeScale = 1;
		if(PlayerDataController.Instance!= null && PlayerDataController.Instance.playerData.isSoundOn){
			AudioListener.volume =1;
		}
		HudMenuManager.instance.ResumeGame ();
		/*UnityAnalyticsScript.instance.AddUnityEvent ("PauseResume", new Dictionary<string, object>{
		});*/
	
	}

	public void Continue(){
		HudMenuManager.instance.loading.SetActive (true);
		Time.timeScale = 1;

		MainMenuManager.isGoToGrage = false;
		MainMenuManager.isGoToWeaponScreen = false;

		//Invoke("LoadLevel", 2f);
		LoadLevel();
		//Application.LoadLevel("UIScene");
		if (PlayerDataController.Instance!= null && PlayerDataController.Instance.playerData.isSoundOn){
			AudioListener.volume =1;
		}

//		UnityAnalyticsScript.instance.AddUnityEvent ("PauseExit", new Dictionary<string, object>{
//		});

		string levelString = "level_exit";

		if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE1)
		{
			levelString = "level_exit_twisting";
		}

		if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE2_Expert)
		{
			levelString = "level_exit_expert";
		}
		
		if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE3_Zombie)
		{
			levelString = "level_exit_zombie";
		}
		
		if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE4_Squid)
		{
			levelString = "level_exit_squid";
		}

		if (MConstants.CurrentGameMode == MConstants.GAME_MODES.MODE5_BATTLEFIELD)
		{
			levelString = "level_exit_battlefield";
		}
		
		if (MConstants.CurrentGameMode == MConstants.GAME_MODES.SURVIVAL_MODE)
		{
			levelString = "Endless_mode";
		}
		/*if (UnityAnalyticsScript.instance != null)
		{
			UnityAnalyticsScript.instance.AddUnityEvent(levelString, new Dictionary<string, object>{
				{ "level_index", ""+MConstants.CurrentLevelNumber}
			});
			UnityAnalyticsScript.instance.AddFirebaseEvent(levelString, MConstants.CurrentLevelNumber);
		}*/
	}
	public void LoadLevel()
    {
		SceneManager.LoadScene("UIScene");
	}

	public void Retry(){
		HudMenuManager.instance.loading.SetActive (true);
		Time.timeScale = 1;

		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		//StartCoroutine(LoadLevelScene());
		LoadLevelScene();
		if (PlayerDataController.Instance != null && PlayerDataController.Instance.playerData.isSoundOn)
        {
            AudioListener.volume = 1;
        }
       /* UnityAnalyticsScript.instance.AddUnityEvent ("PauseRetry", new Dictionary<string, object>{
		});*/
		//MainMenuManager.Instance.showMenu (MenuNames.ENVIORNMENT_SELECTION);
	}
	void LoadLevelScene()
	{
		//yield return new WaitForSeconds(3f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
