using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplosionController : MonoBehaviour {
	public List<GameObject> partsToDeactive;
	public List<GameObject> partsToExplode;

	public GameObject particle;

	void OnEnable(){
		//Explode ();
	}
	// Use this for initialization
	public void Explode () {
		particle.transform.parent = null;
		particle.SetActive (true);
	
		for (int i = 0; i < partsToExplode.Count; i++) {
			partsToExplode [i].transform.parent = null;
			Rigidbody rb = partsToExplode [i].GetComponent<Rigidbody> ();
			rb.AddExplosionForce(100, transform.position, 20,20, ForceMode.Impulse);

		}

		for (int i = 0; i < partsToDeactive.Count; i++) {
			partsToDeactive [i].SetActive (false);

		}
	}

}
