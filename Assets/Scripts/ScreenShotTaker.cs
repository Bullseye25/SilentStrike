// Code auto-converted by Control Freak 2 on Tuesday, July 09, 2019!
using UnityEngine;
using System.Collections;

public class ScreenShotTaker : MonoBehaviour {


	
	//// Update is called once per frame
	//void Update () {
	//	if (ControlFreak2.CF2Input.GetKeyDown (KeyCode.P)) {
	//		Time.timeScale = 0.0001f;
	//		//Application.CaptureScreenshot(Application.persistentDataPath + "Screenshot.png",1);

	//	}else if (ControlFreak2.CF2Input.GetKeyDown (KeyCode.R)) {
	//		Time.timeScale = 1;

	//		//Application.CaptureScreenshot(Application.persistentDataPath + "Screenshot.png",1);

	//	}else if (ControlFreak2.CF2Input.GetKeyDown (KeyCode.S)) {

	//		ScreenCapture.CaptureScreenshot(ScreenShotName(),4);

	//	}

	//}

	public static string ScreenShotName() {
		return Application.persistentDataPath+
			System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")+".png";
	}


}
