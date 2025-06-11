﻿// Code auto-converted by Control Freak 2 on Tuesday, July 09, 2019!
using UnityEngine;
using System.Collections;

[AddComponentMenu ("Control/Orbit Camera")]
public class OrbitCamera : MonoBehaviour
{
	public Transform target;
	public bool autoRotateOn = false;
	public bool autoRotateReverse = false;
	public float autoRotateSpeed = 1f;
	float originalAutoRotateSpeed;
	public float autoRotateSpeedFast = 5f;
	float autoRotateValue = 1;
	public float distance = 1.5f;
	public float distanceMin = 1f;
	public float distanceMax = 3f;
	public float speed = 1;

	//#if UNITY_ANDROID
	public float xSpeed = 1.0f;
	public float ySpeed = 1.0f;
	//#else
	//    public float xSpeed = 15.0f;
	//    public float ySpeed = 15.0f;
	//#endif
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;
     
     
	public float smoothTime = 2f;
	public float autoTimer = 5f;
     
	float rotationYAxis = 0.0f;
	float rotationXAxis = 0.0f;
     
	float velocityX = 0.0f;
	float velocityY = 0.0f;
	bool faster;
	private bool rkeyActive;
	public bool collision = false;



	void Start ()
	{
		rkeyActive = autoRotateOn;
		autoRotateValue = 1;
		Vector3 angles = transform.eulerAngles;
		rotationYAxis = angles.y;
		rotationXAxis = angles.x;
		originalAutoRotateSpeed = autoRotateSpeed;
		if (GetComponent<Rigidbody> ()) {
			GetComponent<Rigidbody> ().freezeRotation = true;
		}
	}

	
	private void Update ()
	{

		if (autoRotateOn) {
			velocityX += (autoRotateSpeed * autoRotateValue) * Time.deltaTime;
		}
		if (ControlFreak2.CF2Input.GetKeyUp (KeyCode.R) && autoRotateOn == false) {
			autoRotateOn = true;
			rkeyActive = true;
	
		} else if (ControlFreak2.CF2Input.GetKeyUp (KeyCode.R) && autoRotateOn == true) {
			autoRotateOn = false;
			rkeyActive = false;
		}
	
		if (ControlFreak2.CF2Input.GetKeyDown (KeyCode.LeftShift) && (!faster)) {
			faster = true;
			autoRotateSpeed = autoRotateSpeedFast;
			autoRotateOn = true;
		}
	
		if (ControlFreak2.CF2Input.GetKeyUp (KeyCode.LeftShift) && (faster)) {
			faster = false;
			autoRotateSpeed = originalAutoRotateSpeed;
			if (rkeyActive == false) {
				autoRotateOn = false;
			}
		}
	
		if (autoRotateReverse == true) {
			autoRotateValue = -1;
		} else {
			autoRotateValue = 1;
		}


	
	}

	 
	void LateUpdate ()
	{
		if (target != null) {
			#if UNITY_EDITOR
						if (ControlFreak2.CF2Input.GetMouseButton (0)) {
							velocityX += xSpeed * ControlFreak2.CF2Input.GetAxis ("Mouse X") * speed * 0.02f;
							velocityY += ySpeed * ControlFreak2.CF2Input.GetAxis ("Mouse Y") * 0.02f;
			
						}
			#else
			if (ControlFreak2.CF2Input.touchCount > 0) {
			velocityX += xSpeed * ControlFreak2.CF2Input.GetTouch (0).deltaPosition.x * speed * 0.02f;
			velocityY += ySpeed * ControlFreak2.CF2Input.GetTouch (0).deltaPosition.y * 0.02f;
			}
			#endif

		
			rotationYAxis += velocityX;
			rotationXAxis -= velocityY;
     
			rotationXAxis = ClampAngle (rotationXAxis, yMinLimit, yMaxLimit);
     
			Quaternion toRotation = Quaternion.Euler (rotationXAxis, rotationYAxis, 0);
			Quaternion rotation = toRotation;
			distance = Mathf.Clamp (distance - ControlFreak2.CF2Input.GetAxis ("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
     
			if (collision == true) {
				RaycastHit hit;
				if (Physics.Linecast (target.position, transform.position, out hit)) {	
					distance -= hit.distance;
				}
			}
			Vector3 negDistance = new Vector3 (0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + target.position;
			transform.rotation = rotation;
			transform.position = position;
     
			velocityX = Mathf.Lerp (velocityX, 0, Time.deltaTime * smoothTime);
			velocityY = Mathf.Lerp (velocityY, 0, Time.deltaTime * smoothTime);
		} else {
			Debug.LogWarning ("Orbit Camera - No Target Set");
		}
	}

	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}
	
	
}
     
     

