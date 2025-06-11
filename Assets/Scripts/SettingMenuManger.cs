using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SettingMenuManger : MonoBehaviour {
	public Image Tilt;
	public Image Steering;
	public Image buttons;

	public Sprite []TiltSprite;
	public Sprite []SteeringSprite;
	public Sprite []buttonsSprite;

	public Toggle toggleSound;
    public Toggle toggleQuality;
    public GameObject qualityDialog;
    public GameObject highQualityWarning;


    void OnEnable(){
		if(PlayerDataController.Instance == null){
			return;
		}
		//ToggleSound (PlayerDataController.Instance.playerData.isSoundOn);
		toggleSound.isOn = PlayerDataController.Instance.playerData.isSoundOn;
        if (toggleQuality)
        {
            toggleQuality.isOn = PlayerDataController.Instance.playerData.isHighQuality;
            qualityDialog.SetActive(false);
        }

        var localPos = transform.localPosition;

        if (MainMenuManager.Instance.currentMenuName == MenuNames.MAIN_MENU || SceneManager.GetActiveScene().name != "UIScene")
	        localPos.z = 0;
        else
	        localPos.z = -1500f;
        
        transform.localPosition = localPos;
    }

    public void ToggleQuality()
    {
        qualityDialog.SetActive(true);
        if (toggleQuality.isOn)
        {
            highQualityWarning.SetActive(true);
        }
        else
        {
            highQualityWarning.SetActive(false);

        }
        StartCoroutine(DecrementCount());
    }

    private IEnumerator DecrementCount()
    {
        float pauseEndTime = Time.realtimeSinceStartup + 1f;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        changeQuality();
    }

    void changeQuality()
    {
        if (toggleQuality.isOn)
        {
            QualitySettings.SetQualityLevel(5, true);
            //toggleSound.isOn = isOn;
        }
        else
        {
            QualitySettings.SetQualityLevel(1, true);

            //toggleSound.isOn = isOn;
        }
        qualityDialog.SetActive(false);
        PlayerDataController.Instance.playerData.isHighQuality = toggleQuality.isOn;

        PlayerDataController.Instance.Save();
    }

    public void ToggleSound(){
		if(toggleSound.isOn){
			AudioListener.volume = 1;
			//toggleSound.isOn = isOn;
		}else{
			AudioListener.volume =0;
			//toggleSound.isOn = isOn;
		}
		PlayerDataController.Instance.playerData.isSoundOn = toggleSound.isOn;

		PlayerDataController.Instance.Save ();
	}

	public void ChangeController(int index){

		switch(index){

		case 0://Buttons
			//RCC_Settings.Instance.useAccelerometerForSteering = false;
			//RCC_Settings.Instance.useSteeringWheelForSteering = false;
			PlayerDataController.Instance.playerData.SelectedControl = index;
			Tilt.sprite = TiltSprite [0];
			Steering.sprite = SteeringSprite [0];
			buttons.sprite = buttonsSprite [1];
			//UnityAnalyticsScript.instance.AddUnityEvent ("ChangeController", new Dictionary<string, object>{
			//	{ "ControllerType", "Buttons"}
			//});
			break;
		case 1://Tilt
			//RCC_Settings.Instance.useAccelerometerForSteering = true;
			//RCC_Settings.Instance.useSteeringWheelForSteering = false;
			PlayerDataController.Instance.playerData.SelectedControl = index;
			Tilt.sprite = TiltSprite [1];
			Steering.sprite = SteeringSprite [0];
			buttons.sprite = buttonsSprite [0];
			//UnityAnalyticsScript.instance.AddUnityEvent ("ChangeController", new Dictionary<string, object>{
			//	{ "ControllerType", "Tilt"}
			//});
			break;
		case 2://Steering
			Tilt.sprite = TiltSprite [0];
			Steering.sprite = SteeringSprite [1];
			buttons.sprite = buttonsSprite [0];
			//RCC_Settings.Instance.useAccelerometerForSteering = false;
			//RCC_Settings.Instance.useSteeringWheelForSteering = true;
			PlayerDataController.Instance.playerData.SelectedControl = index;
			//UnityAnalyticsScript.instance.AddUnityEvent ("ChangeController", new Dictionary<string, object>{
			//	{ "ControllerType", "Steering"}
			//});
			break;

		}
		PlayerDataController.Instance.Save ();

	}

	public int clickCount;
	public void OnTestBuildUnlockClick()
	{
		clickCount++;
		Debug.Log(" Click " + clickCount);
		if (clickCount > 50)
		{
			PlayerDataController.Instance.playerData.LastUnlockedLevel_BattleMode = MConstants.MAX_LEVELS;
			PlayerDataController.Instance.playerData.LastUnlockedLevel_SquidMode = MConstants.MAX_LEVELS;
			PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode3 = MConstants.MAX_LEVELS;
			PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode2 = MConstants.MAX_LEVELS;
			PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode1 = MConstants.MAX_LEVELS;
			PlayerDataController.Instance.playerData.PlayerGold = 100000;
			PlayerDataController.Instance.playerData.PlayerCash = 100000;
			PlayerDataController.Instance.Save();
			MainMenuManager.Instance.RefreshData();
			if (PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode3 >= 4 || PlayerDataController.Instance.playerData.LastUnlockedLevel_Mode2 >= 4)
			{
				//lockOverlay.SetActive(false);
			}
		}
	}
}
