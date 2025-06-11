using UnityEngine;
using System.Collections;

public class WeatherEffcetManager : MonoBehaviour {
	public Material clearDaySkyBox;
	public Material rainyDaySkyBox;

	public GameObject rainEffect;
	public GameObject clearDayEffect;
	public bool isRainMode;
	// Use this for initialization
	void Start () {
		int random = Random.Range (0, 20);
		if(isRainMode){
			random = 20;
		}
		if (random < 15) {
			RenderSettings.skybox = clearDaySkyBox;
			clearDayEffect.SetActive (true);
			rainEffect.SetActive (false);

		} else {
			RenderSettings.skybox = rainyDaySkyBox;
			clearDayEffect.SetActive (false);
			rainEffect.SetActive (true);
			RenderSettings.ambientLight = new Color(0.3088235f,0.2997405f,0.2997405f,1);
		}
	}
}
