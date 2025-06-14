﻿// Code auto-converted by Control Freak 2 on Tuesday, July 09, 2019!
//CameraControl.cs by Azuline Studios© All Rights Reserved
//Camera positioning and angle management for smooth camera kicks and animations.
using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	//other objects accessed by this script
	[HideInInspector]
	public GameObject gun;//this variable updated by PlayerWeapons script
	public GameObject playerObj;
	[HideInInspector]
	public GameObject weaponObj;
	private Transform myTransform;
	[HideInInspector]
	public FPSRigidBodyWalker FPSWalkerComponent;
	private Ironsights IronsightsComponent;
	[HideInInspector]
	public SmoothMouseLook MouseLookComponent;
	private WorldRecenter WorldRecenterComponent;
	[HideInInspector]
	public FPSPlayer FPSPlayerComponent;
	private InputControl InputComponent;
	[HideInInspector]
	public GunSway GunSwayComponent;
	[HideInInspector]
	public CamAndWeapAnims CamAndWeapAnimsComponent;
	private Animation AnimationComponent;
	private Animator AnimatorComponent;	
	private PlayerWeapons PlayerWeaponsComponent;
	private WeaponBehavior CurrentWeaponBehaviorComponent;
	[HideInInspector]
	public GameObject PlayerCharacterObj;
	[HideInInspector]
	public PlayerCharacter PlayerCharacterComponent;	
	[HideInInspector]
	public bool constantLooking;
	//camera angles
	[HideInInspector]
	public Vector3 CameraAnglesAnim = Vector3.zero;//these values are modified by animations and added to camera angles
	[HideInInspector]
	public Vector3 bobAngles = Vector3.zero;//view bobbing angles are sent here from the HeadBob script
	private Quaternion tempCamAngles;
	private float deltaAmt;
	private float returnSpeedAmt = 4.0f;//speed that camera angles return to neutral
	//to move gun and view down slightly on contact with ground
	private bool  landState = false;
	private float landStartTime = 0.0f;
	private float landTime = 0.35f;
	private float landAmt = 1.0f;
	private float landValue = 0.0f;
	//weapon position
	private float gunDown = 0.0f;
	[HideInInspector]
	public float dampOriginX = 0.0f;//Player X position is passed from the GunBob script
	[HideInInspector]
	public float dampOriginY = 0.0f;//Player Y position is passed from the HeadBob script
	
	[Tooltip("True if third person mode is allowed.")]
	public bool allowThirdPerson = true;
	[Tooltip("True if first person mode is allowed.")]
	public bool allowFirstPerson = true;
	[Tooltip("True third person mode is active, otherwise, first person mode is active.")]
	public bool thirdPersonActive;//other scripts read this var to check if third person mode is active
	
	//camera position vars
	private Vector3 dampVel;
	[Tooltip("Speed to smooth the camera angles in third person mode.")]
	public float camSmoothSpeed = 0.075f;
	[HideInInspector]
	public float lerpSpeedAmt;
	private Transform playerObjTransform;
	[HideInInspector]
	public Transform mainCameraTransform;
	
	[HideInInspector]
	public float movingTime;//for controlling delay in lerp of camera position
	[HideInInspector]
	public Vector3 targetPos = Vector3.one;//point to orbit camera around in third person mode
	private Vector3 targetPos2;//secondary orbit point to prevent wobbling of angles if smoothed earlier in camera position calculation
	[HideInInspector]
	public Vector3 camPos;//camera position
	[HideInInspector]
	public Vector3 tempLerpPos = Vector3.one;
	
	//camera roll angle smoothing amounts
	private float rollAmt;
	private float lookRollAmt;
	
	[Tooltip("Horizontal input speed for rotating the camera in third person mode.")]
	public float horizontalRotateSpeed = 5;
	[Tooltip("Vertical input speed for rotating the camera in third person mode.")]
	public float verticalRotateSpeed = 5;
	[Tooltip("Amount to offset the camera from the player in third person mode (horizontal, vertical).")]
	public Vector2 offset;
	private Vector2 offsetAmt;
	
	[Tooltip("Minumum allowed distance to zoom the camera in third person mode.")]
	public float minZoom = 1.45f;
	[Tooltip("Maximum allowed distance to zoom the camera in third person mode.")]
	public float maxZoom = 10f;

	//input values that are scaled by sensitivity amounts
	private float vertical;
	private float horizontal;

	[Tooltip("Minumum allowed vertical camera angle in third person mode.")]
	public float verticalMin = -25f;
	[Tooltip("Maximum allowed vertical camera angle in third person mode.")]
	public float verticalMax = 85f;
	
	private bool angleState;//to initialize angles only once when changing camera modes
	private bool tpState;//for initilization of third person settings
	[HideInInspector]
	public float tpPressTime;//to detect when camera should orbit freely after holding camera toggle button
	private bool tpPressState = true;
	[HideInInspector]
	public bool rotating;//true if toggle camera button is held
	private bool idleRotating;//true if camera is rotating and character is idle in third person
	private bool rotated;//for tracking if angles need to be restored on camera mode change

	[HideInInspector]
	public float zoomDistance;
	private float previousZoomDist;
	private bool zoomState;
	[Tooltip("Radius of sphere collider for detecting collisions with camera in third person mode.")]
	public float sphereSizeTpCol = 0.3f;
	[Tooltip("Distance behind camera to check for obstacles in third person mode (for fine tuning to preventing clipping into scene geometry).")]
	public float rayTestPadding = -0.5f;
	private float smoothedDistance;//smoothed distance between player and camera position
	private float targetDistance;//distance between player and camera after reducing to avoid obstruction, if needed
	[HideInInspector]
	public float currentDistance;//smoothed obstruction reduced distance in third person mode
	
	private RaycastHit hit;
	private Vector3 direction;//direction from camera to player
	private Quaternion camRotation;

	private bool tpSwitching;//true if camera is transitioning camera modes
	private bool tpScopeState;//true if player has zoomed in third person with a scoped weapon

	[Tooltip("Height to zoom to and from in transition to third person mode.")]
	public float fpZoomHeadHeight = 1.27f;
	[Tooltip("Distance to zoom to and from in transition to third person mode.")]
	public float tpModeSwitchZoomDist = 0.65f;

	private float distanceVelocity = 0.0f;
	private float dampVelocity;
	private float dampOrg;//smoothed vertical camera postion

	private float stancePos;
	private float stancePosAmt;
	
//	public bool drawCamSphere;

	[HideInInspector]
	public bool viewUnderwater;//true if camera is underwater (used for increased camera rolling effect)
	[Tooltip("Layers that the camera will collide with in third person mode.")]
	public LayerMask clipMask;
	
	void Start (){
		//set up object and component references
		myTransform = transform;//store this object's transform for optimization
		playerObjTransform = playerObj.transform;
		mainCameraTransform = Camera.main.transform;
		FPSPlayerComponent = playerObj.GetComponent<FPSPlayer>();
		weaponObj = FPSPlayerComponent.weaponObj;
		MouseLookComponent = transform.parent.transform.GetComponent<SmoothMouseLook>();
		GunSwayComponent = weaponObj.GetComponent<GunSway>();
		FPSWalkerComponent = playerObj.GetComponent<FPSRigidBodyWalker>();
		IronsightsComponent = playerObj.GetComponent<Ironsights>();
		CamAndWeapAnimsComponent = transform.parent.transform.GetComponent<CamAndWeapAnims>();
		PlayerWeaponsComponent = weaponObj.GetComponent<PlayerWeapons>();
		InputComponent = playerObj.GetComponent<InputControl>();
		if(playerObj.GetComponent<WorldRecenter>()){
			WorldRecenterComponent = playerObj.GetComponent<WorldRecenter>();
		}
		AnimatorComponent = GetComponent<Animator>();

		offsetAmt = offset;
		currentDistance = 0.0f;
		zoomDistance = 0.0f;
	}
	
	void Update (){
		
		if(Time.timeScale > 0 && Time.deltaTime > 0 && Time.smoothDeltaTime > 0 && Time.time > 0.0f){//allow pausing by setting timescale to 0

			CurrentWeaponBehaviorComponent = PlayerWeaponsComponent.CurrentWeaponBehaviorComponent;
			
			if(allowThirdPerson){
				if(InputComponent.toggleCameraDown){
					tpPressTime = Time.time;
					tpPressState = false;
					if(!thirdPersonActive){
						thirdPersonActive = true;
						tpPressState = true;
					}
				}
				
				//detect if camera toggle button is held and if camera should orbit when moving, after a delay
				//also, determine state of camera rotation and pass to rest of script
				if(!InputComponent.toggleCameraHold){
					if(tpPressTime + (Time.deltaTime * 30.0f) > Time.time && !tpPressState){
						if(!thirdPersonActive){
							thirdPersonActive = true;
						}else if(allowFirstPerson){
							if(!tpSwitching){
								tpSwitching = true;
							}
						}
					}
					rotating = false;
					if(idleRotating){
						rotated = false;
					}
					tpPressState = true;
				}else{
					if(tpPressTime + 0.2f < Time.time && !FPSPlayerComponent.zoomed){
						rotating = true;
					}
				}
			}


			if(CurrentWeaponBehaviorComponent.fpZoomForTp){
				if(FPSPlayerComponent.zoomed && thirdPersonActive && PlayerWeaponsComponent.switchTime + CurrentWeaponBehaviorComponent.readyTimeAmt < Time.time){
					tpScopeState = true;
					thirdPersonActive = false;
				}

				if(tpScopeState && !FPSPlayerComponent.zoomed){
					thirdPersonActive = true;
					tpScopeState = false;
				}
			}else{
				tpScopeState = false;
			}

			
			//make sure that animated camera angles zero-out when not playing an animation
			//this is necessary because sometimes the angle amounts did not return to zero
			//which resulted in the gun and camera angles becoming misaligned
			if(AnimatorComponent.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f){
				CameraAnglesAnim = Vector3.zero;
			}
			
			if(!thirdPersonActive){
				//set up camera position with horizontal lean amount
				targetPos = playerObjTransform.position + (playerObjTransform.right * FPSWalkerComponent.leanPos);
			}else{
				targetPos = playerObjTransform.position;
			}
			
			//if world has just been recentered, don't lerp camera position to prevent lagging behind FPS Player object position
			if(WorldRecenterComponent && WorldRecenterComponent.worldRecenterTime + (0.1f * Time.timeScale) > Time.time){
				tempLerpPos = playerObjTransform.position;
			}else{//lerp camera normally if not on elevator or moving platform
				if(movingTime + 0.75f > Time.time){
					lerpSpeedAmt = 0.0f;
				}else{
					if((FPSPlayerComponent.removePrefabRoot && playerObj.transform.parent == null)
					   ||(!FPSPlayerComponent.removePrefabRoot && playerObj.transform.parent == FPSWalkerComponent.mainObj.transform)){
						lerpSpeedAmt = Mathf.MoveTowards(lerpSpeedAmt, camSmoothSpeed, Time.deltaTime);//gradually change lerpSpeedAmt for smoother lerp transition
					}else{//faster camera lerp speed on platforms or elevators to minimize camera position lag
						lerpSpeedAmt = Mathf.MoveTowards(lerpSpeedAmt, 0.01f, Time.deltaTime);
					}
				}
			}
			
			//smooth player position before applying bob effects
			tempLerpPos = Vector3.SmoothDamp(tempLerpPos, targetPos, ref dampVel, lerpSpeedAmt, Mathf.Infinity, Time.smoothDeltaTime);
			
			//side to side bobbing/moving of camera (stored in the dampOriginX) needs to added to the right vector
			//of the transform so that the bobbing amount is correctly applied along the X and Z axis.
			//If added just to the x axis, like done with the vertical Y axis, the bobbing does not rotate
			//with camera/mouselook and only moves on the world axis. 

			dampOrg = Mathf.SmoothDamp(dampOrg, FPSWalkerComponent.midPos, ref dampVelocity, FPSWalkerComponent.camDampSpeed, Mathf.Infinity, Time.smoothDeltaTime);

			//offset vertical camera height to compensate for changing height of character animations in third person mode
			if(FPSWalkerComponent.crouched && FPSWalkerComponent.moving){
				stancePos = offsetAmt.y * 0.5f;
			}else if(FPSWalkerComponent.prone){
				stancePos = offsetAmt.y * -0.7f;
			}else if(FPSWalkerComponent.swimming && FPSPlayerComponent.zoomed){
				stancePos = offsetAmt.y * -0.3f;
			}else{
				stancePos = 0.0f;
			}

			stancePosAmt = Mathf.Lerp(stancePosAmt, stancePos, Time.deltaTime * 4.0f);
			
			if(!thirdPersonActive){
				camPos = tempLerpPos + (playerObjTransform.right * (CamAndWeapAnimsComponent.camPosAnim.x * IronsightsComponent.camPositionBobAmt.x)) 
					+ new Vector3(0.0f, dampOrg + (CamAndWeapAnimsComponent.camPosAnim.y * IronsightsComponent.camPositionBobAmt.y), 0.0f);
			}else{
				camPos = tempLerpPos + (playerObjTransform.up * stancePosAmt);
			}
				
			
			/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			//Third Person Mode
			/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////			
			
			if(thirdPersonActive){
				if(!tpState){
					//Set view angles on tp mode init
					MouseLookComponent.playerMovedTime = Time.time;
					MouseLookComponent.rotationX = mainCameraTransform.parent.transform.eulerAngles.y;
					//make sure that camera in tp mode starts aligned with back of player
					camRotation = mainCameraTransform.parent.transform.rotation;

					currentDistance = 0.0f;
					if(FPSWalkerComponent.sprintActive){
						smoothedDistance = tpModeSwitchZoomDist - 0.35f;
					}else{
						smoothedDistance = tpModeSwitchZoomDist - 0.2f;
					}
					zoomDistance = maxZoom * 0.65f;

					GunSwayComponent.enabled = false;
					
					tpState = true;
				}
				
				//sync weapon angles with main camera, even when in third person mode, for smoother transition back to first person
				weaponObj.transform.rotation = mainCameraTransform.rotation;

				MouseLookComponent.thirdPerson = true;
				
				//Calculate thirdperson input and clamp angles
				horizontal = MouseLookComponent.rotationX;
				vertical = MouseLookComponent.rotationY + MouseLookComponent.recoilY;
				vertical = ClampAngle(vertical, verticalMin, verticalMax);
				horizontal = ClampAngle(horizontal, -360, 360);
				
				//calculate camera position and angles in third person mode, with offsets and distances
				smoothedDistance = Mathf.Lerp(smoothedDistance, zoomDistance, Time.smoothDeltaTime * 9f);
				targetPos2 = camPos + (playerObjTransform.up * offsetAmt.y * 1.25f);
				
				camRotation = Quaternion.Slerp(camRotation , Quaternion.Euler(-vertical, horizontal, 0.0f), 0.35f * Time.smoothDeltaTime * 60f / Time.timeScale);
				if(smoothedDistance != 0){
					direction = camRotation * (-Vector3.forward - (Vector3.right * (offsetAmt.x / smoothedDistance)));
				}
	
				if(!tpSwitching){
					targetDistance = AdjustLineOfSight(targetPos2, direction);
					currentDistance = Mathf.Min(targetDistance + 0.5f, Mathf.SmoothDamp(currentDistance, targetDistance, ref distanceVelocity, 0.15f));
					offsetAmt.x = Mathf.Lerp(offsetAmt.x, offset.x, Time.smoothDeltaTime * 6f);
					offsetAmt.y = Mathf.Lerp(offsetAmt.y, offset.y, Time.smoothDeltaTime * 6f);
					if(!FPSPlayerComponent.zoomed){
						if(zoomState){
							zoomDistance = previousZoomDist;
							zoomState = false;
						}
						if(rotating){
							zoomDistance = Mathf.Clamp(zoomDistance - ControlFreak2.CF2Input.GetAxis("Mouse Scroll Wheel") * 5f, minZoom, maxZoom);
						}
					}else{
						if(!zoomState){
							previousZoomDist = zoomDistance;
							zoomState = true;
						}
						if(PlayerWeaponsComponent.switchTime + CurrentWeaponBehaviorComponent.readyTimeAmt < Time.time 
						&& CurrentWeaponBehaviorComponent.meleeSwingDelay == 0.0f){
							zoomDistance = minZoom;
						}
					}
				}else{
					//gradually transition between first and third person
					offsetAmt.x = Mathf.Lerp(offsetAmt.x, 0.0f, Time.smoothDeltaTime * 6f);
					offsetAmt.y = Mathf.Lerp(offsetAmt.y, fpZoomHeadHeight, Time.smoothDeltaTime * 6f);
					zoomDistance = tpModeSwitchZoomDist;
					currentDistance = Mathf.SmoothDamp(currentDistance, 0.0f, ref distanceVelocity, 0.075f);
					//now instantly change back to first person mode
					if(smoothedDistance < tpModeSwitchZoomDist + 0.2f){
						smoothedDistance = 0.0f;
						currentDistance = 0.0f;
						targetDistance = 0.0f;
						zoomDistance = 0.0f;
						if(PlayerCharacterComponent){
							PlayerCharacterComponent.tpswitchTime = Time.time;
						}
						targetPos2 = camPos + (playerObjTransform.up * offsetAmt.y * (FPSWalkerComponent.capsule.height * 0.5f));
						thirdPersonActive = false;
						tpSwitching = false;
					}
				}
				
				if(!rotating &&
				(InputComponent.moveX != 0 
				|| InputComponent.moveY != 0 
				|| InputComponent.fireHold
				|| InputComponent.grenadeHold
				|| InputComponent.meleePress
				|| CurrentWeaponBehaviorComponent.releaseTimer + 0.1f > Time.time
				|| PlayerWeaponsComponent.offhandThrowActive
				|| constantLooking
				|| FPSPlayerComponent.zoomed)){//rotate camera locked behind player when moving, firing, or zooming
				
					if(!angleState){//initialize third person angles
						if(rotated){
							MouseLookComponent.rotationX = mainCameraTransform.parent.transform.eulerAngles.y;
							rotated = false;
						}
						
						mainCameraTransform.position = mainCameraTransform.parent.transform.position;
						mainCameraTransform.rotation = mainCameraTransform.parent.transform.rotation;

						angleState = true;
						
					}
					
					idleRotating = false;
					
					MouseLookComponent.tpIdleCamRotate = false;

					//rotate main camera parent and main camera as one object for locked rotataion
					mainCameraTransform.parent.transform.rotation = camRotation;
					mainCameraTransform.parent.transform.position = (camPos + direction * (currentDistance + smoothedDistance)) + (playerObjTransform.up * offsetAmt.y);

				}else{//orbit camera around player when idle or camera toggle button held (after delay)
				
					if(angleState){
						mainCameraTransform.parent.transform.rotation = mainCameraTransform.rotation;
						angleState = false;
					}
					
					//reset angles if toggle camera button was held when moving
					if(rotating && (InputComponent.moveX != 0|| InputComponent.moveY != 0)){
						rotated = true;
						idleRotating = false;
					}else{
						idleRotating = true;
					}
					
					MouseLookComponent.tpIdleCamRotate = true;
					
					//orbit only main camera around player, which does not set player looking direction, 
					//unlike the main camera parent, which also has the mouse look component
					mainCameraTransform.rotation = camRotation;
					mainCameraTransform.position = (camPos + direction * (currentDistance + smoothedDistance)) + (playerObjTransform.up * offsetAmt.y);
	
				}
				
				if(!FPSWalkerComponent.moving){
					IronsightsComponent.side = 0.0f;
					IronsightsComponent.raise = 0.0f;
				}
				
			}else{//third person mode inactive
			
				if(tpState){
									
					//set the time that the player moved in these scripts, to reduce lerp/smoothing times for instant transition
					MouseLookComponent.playerMovedTime = Time.time;
					
					MouseLookComponent.enabled = false;
					//Set view angles to those defined by camYaw and camPitch parameters
					mainCameraTransform.parent.transform.rotation = Quaternion.Euler(-vertical, horizontal, 0.0f);
					mainCameraTransform.rotation = mainCameraTransform.parent.transform.rotation;

					MouseLookComponent.enabled = true;
					
					MouseLookComponent.rotationX = horizontal;
					MouseLookComponent.rotationY = vertical;
					//Reset angle amounts below to zero, to allow view movement to resume at new angles
					MouseLookComponent.horizontalDelta = 0.0f;
					MouseLookComponent.recoilY = 0.0f;
					MouseLookComponent.recoilX = 0.0f;
					MouseLookComponent.xQuaternion = Quaternion.Euler(0f, 0.0f, 0.0f);
					MouseLookComponent.yQuaternion = Quaternion.Euler(0f, 0.0f, 0.0f);
					MouseLookComponent.originalRotation = Quaternion.Euler(0f, 0.0f, 0.0f);
					
					GunSwayComponent.enabled = true;
					
					GunSwayComponent.localRaise = 0.0f;
					GunSwayComponent.localSide = 0.0f;

					tpState = false;
				}
				
				mainCameraTransform.parent.transform.position = camPos;
				mainCameraTransform.position = camPos;
				
				MouseLookComponent.thirdPerson = false;
				
				//initialize camera position/angles quickly before fade out on level load
				if(Time.timeSinceLevelLoad < 1){
					returnSpeedAmt = 32.0f;
				}else{
					if(!viewUnderwater){
						returnSpeedAmt = IronsightsComponent.rollReturnSpeed;
					}else{
						returnSpeedAmt = IronsightsComponent.rollReturnSpeedSwim;
					}
				}
				
				//caculate camera roll angle amounts
				if(FPSWalkerComponent.sprintActive){
					rollAmt = IronsightsComponent.sprintStrafeRoll;	
					//view rolls more with horizontal looking during bullet time for dramatic effect
					lookRollAmt = (-1000f * (1f - Time.timeScale)) * IronsightsComponent.btLookRoll;
				}else{
					rollAmt = IronsightsComponent.walkStrafeRoll;
					if(!viewUnderwater){
						if(Time.timeScale < 1.0f){
							//view rolls more with horizontal looking during bullet time for dramatic effect
							lookRollAmt = (-500f * (1f - Time.timeScale)) * IronsightsComponent.btLookRoll;	
						}else{
							lookRollAmt = -100 * IronsightsComponent.lookRoll;	
						}
					}else{
						lookRollAmt = -100 * IronsightsComponent.swimLookRoll;	
					}
				}
				
				/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				//Camera Angle Assignment
				/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////			
				
				//apply a force to the camera that returns it to neutral angles (Quaternion.identity) over time after being changed by code or by animations
				myTransform.localRotation = Quaternion.Slerp(myTransform.localRotation, Quaternion.identity, Time.deltaTime * returnSpeedAmt);

				deltaAmt = Mathf.Clamp01(Time.deltaTime) * 75.0f;

				//store camera angles in temporary Quaternion and add yaw and pitch from animations 
				tempCamAngles = Quaternion.Euler(mainCameraTransform.localEulerAngles.x - (CamAndWeapAnimsComponent.camAngleAnim.y * IronsightsComponent.camAngleBobAmt.y * deltaAmt) + (CameraAnglesAnim.x * deltaAmt),//camera pitch modifiers
					mainCameraTransform.localEulerAngles.y - (CamAndWeapAnimsComponent.camAngleAnim.x * IronsightsComponent.camAngleBobAmt.x * deltaAmt) + (CameraAnglesAnim.y * deltaAmt),//camera yaw modifiers
					mainCameraTransform.localEulerAngles.z - (CamAndWeapAnimsComponent.camAngleAnim.z * IronsightsComponent.camAngleBobAmt.z * deltaAmt) + (CameraAnglesAnim.z * deltaAmt)//camera roll modifiers 
					- (FPSWalkerComponent.leanAmt * 3.0f * Time.deltaTime * returnSpeedAmt) 
					- (FPSWalkerComponent.inputX * rollAmt * Time.deltaTime * returnSpeedAmt)
					- (IronsightsComponent.side * lookRollAmt * Time.deltaTime * returnSpeedAmt)); 

				//apply tempCamAngles to camera angles
				myTransform.localRotation = tempCamAngles;
			}
		}		
		
		//Track time that player has landed from jump or fall for gun kicks
		if(FPSWalkerComponent.fallingDistance < 1.25f && !FPSWalkerComponent.jumping){
			if(!landState){
				//init timer amount
				landStartTime = Time.time;
				//set landState only once
				landState = true;
			}
		}else{
			if(landState){
				//if land time has elapsed
				if(landStartTime + landTime < Time.time){ 
					//reset landState
					landState = false;
				}
			}
		}
		
		//perform jump of gun when landing
		if(landStartTime + landTime > Time.time){
			//only rise for half of landing time for quick rising and slower lowering
			if(landStartTime + (landTime * 0.5f) < Time.time){//move up gun
				landValue += landAmt * Mathf.Min(0.1f, Time.deltaTime);
			}else{//for remaining half of landing time, move down gun
				landValue -= landAmt * Mathf.Min(0.1f, Time.deltaTime);
			}
		}else{
			//reset vars
			landValue = 0.0F;
		}
		
		//make landing kick less when zoomed
		if(!FPSPlayerComponent.zoomed){
			gunDown = landValue * 0.3f;
		}else{
			gunDown = landValue * 0.1f;
		}

		//pass value of gun kick to IronSights script where it will be added to gun position
		if(movingTime + 0.75f < Time.time){
			IronsightsComponent.jumpAmt = gunDown;
		}else{
			IronsightsComponent.jumpAmt = 0.0f;
		}
		
	}
	
	public static float ClampAngle (float angle, float min, float max){
		angle = angle % 360;
		if((angle >= -360F) && (angle <= 360F)){
			if(angle < -360F){
				angle += 360F;
			}
			if(angle > 360F){
				angle -= 360F;
			}         
		}
		return Mathf.Clamp (angle, min, max);
	}
	
	//Detect if third person camera is block by obstruction between player and camera
	//and shorten distance to player until the collision is avoided
	private float AdjustLineOfSight (Vector3 target, Vector3 direction){
		RaycastHit hit;
		if (Physics.SphereCast (target, sphereSizeTpCol, direction, out hit, zoomDistance, clipMask)){
			if (hit.collider.gameObject.tag != "Usable" && hit.collider.gameObject.tag != "NoHitMark") {//extra check to prevent arrows from blocking camera
				return hit.distance - zoomDistance + rayTestPadding;
			}else{
				return 0.0f;
			}
		}else{
			return 0.0f;
		}
	}
	
//	void OnDrawGizmos() {
//		Gizmos.color = Color.yellow;
//		Gizmos.DrawSphere(hit.point, 0.2f);
//		Gizmos.color = Color.green;
//		Gizmos.DrawSphere(camPos, 0.2f);
//		if(drawCamSphere){
//			Gizmos.color = Color.cyan;
//			Gizmos.DrawSphere(Camera.main.transform.position, sphereSizeTpCol);
////			Gizmos.color = Color.blue;
////			Gizmos.DrawSphere(targetPos + (playerObjTransform.up * offsetAmt.y) + (playerObjTransform.right * (offsetAmt.x * -0.12)) + (direction * (currentDistance + smoothedDistance - sphereSizeTpCol)), sphereSizeTpCol);
////			Gizmos.DrawSphere(targetPos + (playerObjTransform.up * offsetAmt.y) + (direction * (currentDistance + smoothedDistance)), sphereSizeTpCol);
//		}
//		
//	}
	
}