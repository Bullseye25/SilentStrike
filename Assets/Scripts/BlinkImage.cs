using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlinkImage : MonoBehaviour {

	public Image flashingText;
	public bool isBlinkWithInterVal;
	int count;
	void OnEnable(){
		StartCoroutine (BlinkTextE());
	}
	public IEnumerator BlinkTextE(){
		//blink it forever. You can set a terminating condition depending upon your requirement
		while(true){
			//set the Text's text to blank
			flashingText.enabled = true;
			count++;
			//display blank text for 0.5 seconds
			yield return new WaitForSeconds(.5f);
			//display “I AM FLASHING TEXT” for the next 0.5 seconds
			flashingText.enabled = false;
			yield return new WaitForSeconds(.5f);

			if(isBlinkWithInterVal && count > Random.Range(2,4)){

				yield return new WaitForSeconds(2f);
				count = 0;
			}
		}
	}

}
