using UnityEngine;
using System.Collections;

public class ScaleSample : MonoBehaviour {
	void OnEnable(){
		iTween.ScaleFrom(gameObject, iTween.Hash("x", 0, "y", 0,"easeType", "easeInOutExpo","time",1));
	}
}
