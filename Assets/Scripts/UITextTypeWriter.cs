using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// attach to UI Text component (with the full text already there)

public class UITextTypeWriter : MonoBehaviour 
{

	Text txt;
	public bool isStartOnEnable = true ;
	string story;
	public AudioSource typeWriterSound;
	void OnEnable () 
	{
		if(isStartOnEnable){
			StartTextTyping ();
		}
	}

	public void StartTextTyping()
	{
		txt = GetComponent<Text> ();
		story = txt.text;
		txt.text = "";
		typeWriterSound.Play ();
		// TODO: add optional delay when to start
		StartCoroutine ("PlayText");	
	}

	IEnumerator PlayText()
	{
		foreach (char c in story) 
		{
			txt.text += c;
			yield return new WaitForSeconds (0.06f);
		}

		typeWriterSound.Stop ();
	}

}
