using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;
public class ReverseMovement : MonoBehaviour {
	public float time = 2;
	public AutoMoveAndRotate autoMoveAndRotate;
	// Use this for initialization
	void Start () {
		//Invoke ("ReveseRoatation",time);
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.CompareTag("ReversCollider")){
			autoMoveAndRotate.rotateDegreesPerSecond.value = new Vector3 (-autoMoveAndRotate.rotateDegreesPerSecond.value.x,-autoMoveAndRotate.rotateDegreesPerSecond.value.y,-autoMoveAndRotate.rotateDegreesPerSecond.value.z);
		}
	}

	public void ReveseRoatation(){
		autoMoveAndRotate.rotateDegreesPerSecond.value = new Vector3 (-autoMoveAndRotate.rotateDegreesPerSecond.value.x,-autoMoveAndRotate.rotateDegreesPerSecond.value.y,-autoMoveAndRotate.rotateDegreesPerSecond.value.z);
		//Invoke ("ReveseRoatation",time);

	}

}
