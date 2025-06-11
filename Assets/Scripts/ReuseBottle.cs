using UnityEngine;
using System.Collections;

public class ReuseBottle : MonoBehaviour {
	public ThrowBottles bottlesList;
	void OnCollisionEnter(Collision collision) {
		bottlesList.bottles.Add (gameObject.GetComponent<Rigidbody>());
		gameObject.SetActive (false);
		bottlesList.InitThwoBottleAgain ();
	}
}
