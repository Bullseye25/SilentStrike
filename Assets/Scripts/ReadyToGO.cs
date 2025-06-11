using UnityEngine;
using System.Collections;

public class ReadyToGO : MonoBehaviour {

	public static int count = 0;
	public GameObject[] TextArray;
	public GameObject blackImage;
	public GameObject MissionInfo;

	void Start () {
		count = 5;
		//MissionInfo.SetActive (true);
		//Time.timeScale = 0;


		StartGame();

		//readyToGo();
	}



	public void readyToGo(){

		   blackImage.SetActive (true);
			Counter();
	

	}

	public void Counter(){
		if( count < 4 ){
			blackImage.SetActive (false);

			blackImage.SetActive (true);

			for (int i = 0; i < TextArray.Length; i++) {
				TextArray [i].SetActive (false);
			}
			TextArray [count].SetActive (true);

			count+=1;

			StartCoroutine(DecrementCount());
		
			    
		}else{
			for (int i = 0; i < TextArray.Length; i++) {
				TextArray [i].SetActive (false);
			}

			//LevelsManager.instance.Levels [MConstants.CurrentLevelNumber - 1].DeActiveCameras();  
			//LevelsManager.instance.SetStartTime ();
			blackImage.SetActive (false);
			//Start Game
		}
	}

	private IEnumerator  DecrementCount()
	{
		float pauseEndTime = Time.realtimeSinceStartup + 2f;
		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}
			Counter();
	}


	public void StartGame(){
		MissionInfo.SetActive (false);

		readyToGo ();
	}
}
