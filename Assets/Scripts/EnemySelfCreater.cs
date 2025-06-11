using UnityEngine;
using System.Collections;
public class EnemySelfCreater : MonoBehaviour {
	public GameObject enemy;

	public Level level;
	// Use this for initialization
	void Start () {

		GameObject newVehicle = (GameObject)GameObject.Instantiate(enemy, transform.position + (Vector3.up), transform.rotation);

	}

}
