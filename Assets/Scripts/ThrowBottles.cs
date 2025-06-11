using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThrowBottles : MonoBehaviour {

	public float bulletSpeed = 20;
	public float RandomValue = 300;

	public float timeToThrow = 1;
	public List<Rigidbody> bottles;
	bool isDone ;
	void Start(){
		Invoke ("ThrowBottle",timeToThrow);
	}

	void ThrowBottle(){
		Rigidbody rigidBody = bottles [0];
		bottles.Remove (rigidBody);
		rigidBody.transform.position = gameObject.transform.position;
		rigidBody.gameObject.SetActive (true);
		rigidBody.isKinematic = false;
		rigidBody.transform.rotation = gameObject.transform.rotation;
		rigidBody.AddForce (rigidBody.transform.forward * Random.Range(bulletSpeed-(RandomValue/2),bulletSpeed+RandomValue));
		if (bottles.Count > 0) {
			Invoke ("ThrowBottle", timeToThrow);
		} else {
			isDone = true;
		}
	}

	public void InitThwoBottleAgain(){
		if(isDone){
			isDone = false;
			Invoke ("ThrowBottle",timeToThrow);

		}
	}
//	void Update () {
//		// Put this in your update function
//		if (Input.GetButtonDown("Fire1")) {
//
//			// Instantiate the projectile at the position and rotation of this transform
//			var clone : Transform;
//			clone = Instantiate(projectile, transform.position, transform.rotation);
//
//			// Add force to the cloned object in the object's forward direction
//			clone.rigidbody.AddForce(clone.transform.forward * shootForce);
//		}
//	}
}
