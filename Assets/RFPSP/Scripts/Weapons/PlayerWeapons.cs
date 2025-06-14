//PlayerWeapons.cs by Azuline Studios© All Rights Reserved
//Switches and drops weapons and sets weapon parent object position. 
using UnityEngine;
using System.Collections;
//
public class PlayerWeapons : MonoBehaviour {
	//objects accessed by this script
	[HideInInspector]
	public GameObject playerObj;
    public AI[] enemy;
    [HideInInspector]
	public GameObject cameraObj;
	//set up external script references
	private InputControl InputComponent;
	private FPSRigidBodyWalker FPSWalkerComponent;
	[HideInInspector]
	public FPSPlayer FPSPlayerComponent;
	[HideInInspector]
	public WeaponBehavior CurrentWeaponBehaviorComponent;
	private Ironsights IronsightsComponent;
	[HideInInspector]
	public CameraControl CameraControlComponent;

	private Animator WeaponObjAnimatorComponent;
	[HideInInspector]
	public  Animator CurWeaponObjAnimatorComponent;
	private Animator CameraAnimatorComponent;

	[Tooltip("The weaponOrder index of the first weapon that will be selected when the map loads.")]
	public int firstWeapon = 0;//the weaponOrder index of the first weapon that will be selected when the map loads
	[Tooltip("Maximum number of weapons that the player can carry.")]
	public int maxWeapons = 10;
	//backupWeapon is the weaponOrder index number of a weapon like fists, a knife, or sidearm that player will select when all other weapons are dropped
	//this weapon should also have its "droppable" and "addsToTotalWeaps" values in its WeaponBehavior.cs component set to false
	[Tooltip("The weaponOrder index of a weapon like fists, a knife, or sidearm that player will select when all other weapons are dropped.")]
	public int backupWeapon = 1;
	[HideInInspector]
	public int grenadeWeapon;//index of grenade in grenade order array for offhand throw
	[HideInInspector]
	public int totalWeapons;
	[HideInInspector]
	public int currentWeapon;//index of weaponOrder array that corresponds to current weapon 
	[HideInInspector]
	public int currentGrenade;//index of weaponOrder array that corresponds to current weapon 
	
	//Define array for storing order of weapons. This array is created in the inspector by dragging and dropping 
	//weapons from under the FPSWeapons branch in the FPS Prefab. Weapon 0 should always be the unarmed/null weapon.
	[Tooltip("Array for storing order of weapons. This array is created by dragging and dropping weapons from under the FPS Weapons Object in the FPS Prefab. Weapon 0 should always be the unarmed/null weapon.")]
	public GameObject[] weaponOrder;
	private WeaponBehavior[] weaponBehaviors;//automatically poulated
	[Tooltip("Array for storing order of grenades. This array is created by dragging and dropping grenade weapons from under the FPS Weapons Object in the FPS Prefab.")]
	public GameObject[] grenadeOrder;

	private Transform myTransform;
	private Transform mainCamTransform;
	[HideInInspector]
	public Color waterMuzzleFlashColor;
	
	//weapon switching
	[HideInInspector]
	public float switchTime = 0.0f;//time that weapon switch started
	[HideInInspector]
	public float sprintSwitchTime = 0.0f;//time that weapon sprinting animation started, set in WeaponBehavior script
	[HideInInspector]
	public bool switching = false;//true when switching weapons
	[HideInInspector]
	public bool displayingGrenade;
	private float grenDisplayTime;
	private bool sprintSwitching = false;//true when weapon sprinting animation is playing
	private bool proneMove;

	[HideInInspector]
	public bool cameraToggleState;
	[HideInInspector]
	public float cameraToggleTime;

	private bool dropWeapon;
	private bool deadDropped;
	
	//sound effects
	public AudioClip changesnd;
	private bool audioPaused;// used to pause and resume reloading sound based on timescale/game pausing state
	
	private AudioSource []aSources;
	private AudioSource aSource;

	[Tooltip("Directional sun light object checked by raycast for weapon shading in shadows.")]
	public Transform sunLightObj;
	[Tooltip("Albedo color of weapon material when in shade.")]
	public Color shadeColor;

	private int prevWepToGrenIndex; 
	[HideInInspector]
	public bool pullGrenadeState;
	[HideInInspector]
	public bool grenadeThrownState;
	[HideInInspector]
	public bool offhandThrowActive;
	private WeaponBehavior GrenadeWeaponBehaviorComponent;
	
	[Tooltip("Amount of time for bullet shell to stay parented to weapon object (causes shell to inherit weapon angular velocity, decrease value if shells stick with weapon model too long).")]
	public float shellParentTime = 0.5f;
	[Tooltip("Amount of time for bullet shell to stay parented to weapon object when deadzone aiming (causes shell to inherit weapon angular velocity, decrease value if shells stick with weapon model too long).")]
	public float shellDzParentTime = 0.1f;
	[Tooltip("Amount of time for bullet shell to stay parented to weapon object when bullet time is active (causes shell to inherit weapon angular velocity, decrease value if shells stick with weapon model too long).")]
	public float shellBtParentTime = 0.2f;
    PlayerDataSerializeable playerData;
    public GameObject[] AllGuns;
    public int Gun1, Gun2;
    public int defaultGun1Ammo, defaultGun2Ammo;
    public int tempMachineGun;
    private bool _showOutOfAmmoOnce;
    public static PlayerWeapons instance;
    void  Awake()
    {
        instance = this;
        if (PlayerDataController.Instance)
        {
            playerData = PlayerDataController.Instance.playerData;
            Gun1 = playerData.CurrentSelectedPrimaryGun;
            Gun2 = playerData.CurrentSelectedSecondaryGun;
            if (MConstants.HavingTemporaryGun)
            {
                Gun2 = playerData.SelectedVehicle_temp;

            }

            for (int i = 0; i < 11; i++)
            {
                if (i == Gun1 || i == Gun2)
                {
                    weaponOrder[i + 1].GetComponent<WeaponBehavior>().haveWeapon = true;
                }

            }
            Debug.Log("Gun2index" + Gun2);
            firstWeapon = Gun1 + 1;
          //  MConstants.TempSelectedWeapon = firstWeapon;
        }
        else
        {
            Gun1 = 3;
            Gun2 = 4;
        }

        defaultGun1Ammo = weaponOrder[Gun1 + 1].GetComponent<WeaponBehavior>().ammo;
        defaultGun2Ammo = weaponOrder[Gun2 + 1].GetComponent<WeaponBehavior>().ammo;
    }

    void Start (){

		myTransform = transform;//define transforms for efficiency
		mainCamTransform = Camera.main.transform;
		
		//set up external script references
		CameraControlComponent = mainCamTransform.GetComponent<CameraControl>();
		playerObj = CameraControlComponent.playerObj;
		cameraObj = CameraControlComponent.transform.parent.transform.gameObject;
		InputComponent = playerObj.GetComponent<InputControl>();
		FPSWalkerComponent = playerObj.GetComponent<FPSRigidBodyWalker>();
		FPSPlayerComponent = playerObj.GetComponent<FPSPlayer>();
		IronsightsComponent = playerObj.GetComponent<Ironsights>();
		CurrentWeaponBehaviorComponent = weaponOrder[firstWeapon].GetComponent<WeaponBehavior>();
		
		WeaponBehavior BackupWeaponBehaviorComponent = weaponOrder[backupWeapon].GetComponent<WeaponBehavior>();
		weaponBehaviors = myTransform.GetComponentsInChildren <WeaponBehavior>(true);

		aSources = transform.GetComponents<AudioSource>();
		aSource = playerObj.AddComponent<AudioSource>(); 
		aSource.spatialBlend = 0.0f;
		aSource.playOnAwake = false;
		
		//set the weapon order number in the WeaponBehavior scripts
		for(int i = 0; i < weaponOrder.Length; i++)	{
			weaponOrder[i].GetComponent<WeaponBehavior>().weaponNumber = i;
		}
		
		currentGrenade = 0; 
		
		GrenadeWeaponBehaviorComponent = grenadeOrder[currentGrenade].GetComponent<WeaponBehavior>();
		grenadeWeapon = GrenadeWeaponBehaviorComponent.weaponNumber;

        if (!weaponOrder[firstWeapon].GetComponent<WeaponBehavior>().haveWeapon)
        {
            weaponOrder[firstWeapon].GetComponent<WeaponBehavior>().haveWeapon = true;
        }
		//Select first weapon, if firstWeapon is not in inventory, player will spawn unarmed.
		// if(weaponOrder[firstWeapon].GetComponent<WeaponBehavior>().haveWeapon){
			// StartCoroutine(SelectWeapon(firstWeapon));
		//}else{
			StartCoroutine(SelectWeapon(0));	
		//}
		
		//set droppable value for backup weapon to false here if it was set to true in inspector 
		//to prevent multiple instances of backup weapon from being dropped and not selecting next weapon
		BackupWeaponBehaviorComponent.droppable = false;
		//set addsToTotalWeaps value for backup weapon to false here if it was set to true in inspector
		//to prevent picking up a backup weapon from swapping current weapon
		BackupWeaponBehaviorComponent.addsToTotalWeaps = false;
		
		UpdateTotalWeapons();

		//automatically assign sunlight object for weapon shading
		if(!sunLightObj){
			Light[] lightList = FindObjectsOfType(typeof(Light)) as Light[];
			for(int i = 0; i < lightList.Length; i++)	{
				if(lightList[i].type == LightType.Directional && lightList[i].gameObject.activeInHierarchy){
					sunLightObj = lightList[i].transform;
					break;
				}
			}
		}


	}

   public void SelectWeapon()
    {
        if (weaponOrder[firstWeapon].GetComponent<WeaponBehavior>().haveWeapon)
        {
            StartCoroutine(SelectWeapon(firstWeapon));
        }
        else
        {
            StartCoroutine(SelectWeapon(0));
        }

        if (LevelsManager.instance.currentLevel.isMachineGunLevel)
        {
	        SelectTempMachineGun(LevelsManager.instance.currentLevel.machineGunType);
        }
    }

   public void SelectTempMachineGun(Level.MachineGunType gunType)
   {
	   // weaponOrder[Gun1 + 1].GetComponent<WeaponBehavior>().haveWeapon = false;
	   // weaponOrder[Gun2 + 1].GetComponent<WeaponBehavior>().haveWeapon = false;

	   if (gunType == Level.MachineGunType.SMG)
	   {
		   tempMachineGun = 6;
		   weaponOrder[tempMachineGun].GetComponent<WeaponBehavior>().haveWeapon = true;
	   }
	   else if (gunType == Level.MachineGunType.HMG)
	   {
		   tempMachineGun = 7;
		   weaponOrder[tempMachineGun].GetComponent<WeaponBehavior>().haveWeapon = true;
	   }
	   else if (gunType == Level.MachineGunType.G36)
	   {
		   tempMachineGun = 8;
		   weaponOrder[tempMachineGun].GetComponent<WeaponBehavior>().haveWeapon = true;
	   }
		else if (gunType == Level.MachineGunType.MINI_MACHINEGUN)
		{
			tempMachineGun = 9;
			weaponOrder[tempMachineGun].GetComponent<WeaponBehavior>().haveWeapon = true;
		}
		StartCoroutine(SelectWeapon(tempMachineGun));
   }
   
    void Update (){

		if(cameraToggleState){//for enabling and disabling weapons when toggling cinema/alternate cameras and/or teleporting player
			SelectWeapon(CurrentWeaponBehaviorComponent.weaponNumber);
			CurWeaponObjAnimatorComponent = weaponOrder[CurrentWeaponBehaviorComponent.weaponNumber].GetComponent<Animator>();
			CurWeaponObjAnimatorComponent.SetTrigger("IdleForward");
			CurrentWeaponBehaviorComponent.gunAnglesTarget = Vector3.zero;
			CameraControlComponent.CameraAnglesAnim = Vector3.zero;
			if(CurrentWeaponBehaviorComponent.bulletsToReload <= 1 && CurrentWeaponBehaviorComponent.weaponNumber != 0){
				CurrentWeaponBehaviorComponent.WeaponAnimatorComponent.SetTrigger("Neutral");
			}
			CurrentWeaponBehaviorComponent.CameraAnimatorComponent.speed = 1.0f;
			CurrentWeaponBehaviorComponent.CameraAnimatorComponent.SetTrigger("CamIdle");
			cameraToggleState = false;
		}

       
        


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Switch Weapons
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //end offhand grenade throw and select original weapon
        if (grenadeThrownState){
			StartCoroutine(SelectWeapon(prevWepToGrenIndex, false, true));
			pullGrenadeState = false;
			offhandThrowActive = false;
			grenadeThrownState = false;
		}

		if(Time.timeSinceLevelLoad > 2.0f//don't allow weapon switching when level is still loading/fading out
	    && Time.timeScale > 0.0f//don't allow weapon switching when paused
		&& !(!FPSWalkerComponent.grounded && FPSWalkerComponent.sprintActive)//don't allow switching if player is sprinting and airborn
		&& !switching//only allow one weapon switch at once
		&& !InputComponent.toggleCameraHold//don't allow switching if toggle camera button is held
		&& (!CurrentWeaponBehaviorComponent.shooting || FPSPlayerComponent.hitPoints < 1.0f)//don't switch weapons if shooting
		//don't allow switching if player is holding an object
		&& !FPSWalkerComponent.holdingObject){
		
			//select next grenade
			if(InputComponent.selectGrenPress && !displayingGrenade){
				if(currentGrenade + 1 <= grenadeOrder.Length - 1){
					if(grenadeOrder[currentGrenade + 1].GetComponent<WeaponBehavior>().haveWeapon
					&& grenadeOrder[currentGrenade + 1].GetComponent<WeaponBehavior>().ammo > 0){
						currentGrenade ++;
					}
				}else{//start counting grenades from zero if last grenade in list
					if(grenadeOrder[0].GetComponent<WeaponBehavior>().haveWeapon
					&& grenadeOrder[0].GetComponent<WeaponBehavior>().ammo > 0){
						currentGrenade = 0;
					}
				}
				GrenadeWeaponBehaviorComponent = grenadeOrder[currentGrenade].GetComponent<WeaponBehavior>();
				if(GrenadeWeaponBehaviorComponent.haveWeapon && GrenadeWeaponBehaviorComponent.ammo > 0){
					grenadeWeapon = GrenadeWeaponBehaviorComponent.weaponNumber; 
					prevWepToGrenIndex = currentWeapon;
					displayingGrenade = true;
					grenDisplayTime = Time.time;
					StartCoroutine(SelectWeapon(grenadeWeapon, true, false));
					StartCoroutine(DisplayGrenadeSwitch());
				}	
			}
			
			//begin offhand grenade throw
			if(currentWeapon != grenadeWeapon && GrenadeWeaponBehaviorComponent.ammo > 0 && !displayingGrenade){
				if(InputComponent.grenadeHold && !pullGrenadeState && !CameraControlComponent.rotating){
					//cancel a pulled weapon attack if using offhand weapon to prevent pulled weapon fire when switching weapons
					CurrentWeaponBehaviorComponent.CancelWeaponPull();
					offhandThrowActive = true;
					prevWepToGrenIndex = currentWeapon;
					StartCoroutine(SelectWeapon(grenadeWeapon, true, false));
					grenadeThrownState = false;
					pullGrenadeState = true;
				}
			}

			//don't allow weapon switching while sprint anim is active/transitioning
			if(!sprintSwitching){

				//drop weapons
				if((InputComponent.dropPress || InputComponent.xboxDpadDownPress)
				&& currentWeapon != 0
				&& !pullGrenadeState
				&& !FPSWalkerComponent.sprintActive
				&& CurrentWeaponBehaviorComponent.droppable
				&& !CurrentWeaponBehaviorComponent.dropWillDupe){//if drop button is pressed and weapon isn't holstered (null weap 0 selected)
					DropWeapon(currentWeapon);		
				}
				
				//drop current weapon if player dies
				if(FPSPlayerComponent.hitPoints < 1.0f && !deadDropped){
					CurrentWeaponBehaviorComponent.droppable = true;
					if(CurrentWeaponBehaviorComponent.muzzleFlash){
						CurrentWeaponBehaviorComponent.muzzleFlash.GetComponent<Renderer>().enabled = false;
					}
					deadDropped = true;
					DropWeapon(currentWeapon);	
				}
			  	
				if(Time.timeScale > 0.0f){
				  	//Cycle weapons using the mousewheel (cycle through FPS Weapon children) and skip weapons that are not in player inventory.
					//weaponOrder.Length - 1 is the last weapon because the built in array starts counting at zero and weaponOrder.Length starts counting at one (index 0 of weaponOrder[] is null/unarmed weapon). 
					if (
					 InputComponent.selectPrevPress 
					){//mouse wheel down or previous weapon button pressed
						if(currentWeapon != 0){//not starting at zero
							for(int i = currentWeapon; i > -1; i--){
								WeaponBehavior ThisWeaponBehavior = weaponOrder[i].GetComponent<WeaponBehavior>();
								if(ThisWeaponBehavior.haveWeapon && ThisWeaponBehavior.cycleSelect && i != currentWeapon){//check that player has weapon and it is not currently selected weapon
									StartCoroutine(SelectWeapon(i));//run the SelectWeapon function with the next weapon index that was found
									break;
								}else if(i == 0){//reached zero, count backwards from end of list to find next weapon
									for(int n = weaponOrder.Length - 1; n > -1; n--){
										WeaponBehavior ThisWeaponBehavior2 = weaponOrder[n].GetComponent<WeaponBehavior>();
										if(ThisWeaponBehavior2.haveWeapon && ThisWeaponBehavior2.cycleSelect && n != currentWeapon){
											StartCoroutine(SelectWeapon(n));
											break;
										}
									}
								}
							}
						}else{//starting at 0
							for (int i = weaponOrder.Length - 1; i > -1; i--){
								WeaponBehavior ThisWeaponBehavior = weaponOrder[i].GetComponent<WeaponBehavior>();
								if(ThisWeaponBehavior.haveWeapon && ThisWeaponBehavior.cycleSelect && i != currentWeapon){
									StartCoroutine(SelectWeapon(i));
									break;
								}
							}
						}
					}else if(
					 InputComponent.selectNextPress//select next weapon button pressed
					
					){//drop weapon button pressed and player has weapons in their inventory
						// if (!weaponOrder[2].activeSelf && MConstants.CurrentLevelNumber == 4)
						// {
						// 	HudMenuManager.instance.EnableAllUi(true);
						// 	HudMenuManager.instance.changeGunBtn.GetComponent<Animation>().enabled = false;
						// 	HudMenuManager.instance.changeGunBtn.transform.localScale = Vector3.one;
						// }
						if(currentWeapon < weaponOrder.Length -1){//not starting at last weapon
							for(int i = currentWeapon; i < weaponOrder.Length; i++){
								WeaponBehavior ThisWeaponBehavior = weaponOrder[i].GetComponent<WeaponBehavior>();
								//cycle weapon selection manually
								if((ThisWeaponBehavior.haveWeapon && ThisWeaponBehavior.cycleSelect
								&& i != currentWeapon && !dropWeapon) 
								//do not select backupWeapon if dropping a weapon and automatically selecting the next weapon
								//but allow backupWeapon to be selected when cycling weapon selection manually
								|| (ThisWeaponBehavior.haveWeapon && ThisWeaponBehavior.cycleSelect
								&& i != currentWeapon && i != backupWeapon && dropWeapon)){
									StartCoroutine(SelectWeapon(i));
									break;
								}else if(i == weaponOrder.Length - 1){//reached end of list, count forwards from zero to find next weapon
									for(int n = 0; n < weaponOrder.Length - 1; n++){
										WeaponBehavior ThisWeaponBehavior2 = weaponOrder[n].GetComponent<WeaponBehavior>();
										//cycle weapon selection manually
										if((ThisWeaponBehavior2.haveWeapon && ThisWeaponBehavior2.cycleSelect
										&& n != currentWeapon && !dropWeapon) 
										//do not select backupWeapon if dropping a weapon and automatically selecting the next weapon
										//but allow backupWeapon to be selected when cycling weapon selection manually
										|| (ThisWeaponBehavior2.haveWeapon && ThisWeaponBehavior2.cycleSelect 
										&& n != currentWeapon && n != backupWeapon && dropWeapon)){
											StartCoroutine(SelectWeapon(n));
											break;
										}
									}
								}
							}
						}else{//starting at last weapon
							for(int i = 0; i < weaponOrder.Length - 1; i++){
								WeaponBehavior ThisWeaponBehavior = weaponOrder[i].GetComponent<WeaponBehavior>();
								//cycle weapon selection manually
								if((ThisWeaponBehavior.haveWeapon && ThisWeaponBehavior.cycleSelect
								&& i != currentWeapon && !dropWeapon) 
								//do not select backupWeapon if dropping a weapon and automatically selecting the next weapon
								//but allow backupWeapon to be selected when cycling weapon selection manually
								|| (ThisWeaponBehavior.haveWeapon && ThisWeaponBehavior.cycleSelect
								&& i != currentWeapon && i != backupWeapon && dropWeapon)){
									StartCoroutine(SelectWeapon(i));
									break;
								}
							}
						}	
					}
				}
				
				//select weapons with number keys
				if (InputComponent.holsterPress) {
					if(currentWeapon != 0){StartCoroutine(SelectWeapon(0));}
				}else if (InputComponent.selectWeap1Press && weaponOrder.Length - 1 > 0) {
					if(currentWeapon != 1){StartCoroutine(SelectWeapon(1));}
				}else if (InputComponent.selectWeap2Press && weaponOrder.Length - 1 > 1) {
					if(currentWeapon != 2){StartCoroutine(SelectWeapon(2));}
				}else if (InputComponent.selectWeap3Press && weaponOrder.Length - 1 > 2) {
					if(currentWeapon != 3){StartCoroutine(SelectWeapon(3));}
				}else if (InputComponent.selectWeap4Press && weaponOrder.Length - 1 > 3) {
					if(currentWeapon != 4){StartCoroutine(SelectWeapon(4));}
				}else if (InputComponent.selectWeap5Press && weaponOrder.Length - 1 > 4) {
					if(currentWeapon != 5){StartCoroutine(SelectWeapon(5));}
				}else if (InputComponent.selectWeap6Press && weaponOrder.Length - 1 > 5) {
					if(currentWeapon != 6){StartCoroutine(SelectWeapon(6));}
				}else if (InputComponent.selectWeap7Press && weaponOrder.Length - 1 > 6) {
					if(currentWeapon != 7){StartCoroutine(SelectWeapon(7));}
				}else if (InputComponent.selectWeap8Press && weaponOrder.Length - 1 > 7) {
					if(currentWeapon != 8){StartCoroutine(SelectWeapon(8));}
				}else if (InputComponent.selectWeap9Press && weaponOrder.Length - 1 > 8) {
					if(currentWeapon != 9){StartCoroutine(SelectWeapon(9));}
				}
				
			}
		}
		
		//check timer for switch to prevent shooting
		//this var checked in "WeaponBehavior" script in the Fire() function 
		if(switchTime + 0.87f > Time.time){
			switching = true;
		}else{
			switching = false;
		}
		
		if(grenDisplayTime + 2f < Time.time){
			displayingGrenade = false;
		}
		
		//define time that sprinting anim is active/transitioning to disable weapon switching
		if(sprintSwitchTime + 0.44f > Time.time){
			sprintSwitching = true;
		}else{
			sprintSwitching = false;
		}
		
		//pause and resume reloading sound based on timescale/game pausing state
		if(Time.timeScale > 0){
			if(audioPaused){
				aSources[1].Play();	
				audioPaused = false;
			}
		}else{
			if(!audioPaused && aSources[1].isPlaying){
				aSources[1].Pause();
				audioPaused = true;
			}
		}
		
	}
	
	//set weapon parent position in LateUpdate to sync with CameraControl.cs LateUpdate actions
	void LateUpdate (){
		//align weapon parent origin with player camera origin
		Vector3 tempGunPosition = new Vector3(mainCamTransform.position.x, mainCamTransform.position.y, mainCamTransform.position.z);
		myTransform.position = tempGunPosition;
	}
	
	private IEnumerator DisplayGrenadeSwitch(){
		yield return new WaitForSeconds(1.0f);
		StartCoroutine(SelectWeapon(prevWepToGrenIndex, false, true));
	}
	
	public void DropWeapon ( int weapon){

		if(!weaponOrder[weapon].GetComponent<WeaponBehavior>().haveWeapon){
			return;
		}
		
		float dropVel;//var to allow velocity to be added to weapon if dropped while moving
		
		//set haveWeapon value to false for this weapon to remove it from player's inventory
		weaponOrder[weapon].GetComponent<WeaponBehavior>().haveWeapon = false;
		
		aSources[1].Stop();//prevent reload sound from playing after dropping weapon
		
		//modify drop velocity based on player speed so the weapon doesn't fly far away 
		//if player is moving backwards or drop behind player when moving
		if(!deadDropped){
			if(FPSWalkerComponent.inputY > 0){
				dropVel = 8.0f;	
			}else if(FPSWalkerComponent.inputY < 0){
				dropVel = 2.0f;	
			}else{
				dropVel = 4.0f;
			}
		}else{
			if(FPSWalkerComponent.inputY > 0){
				dropVel = 4.0f;
			}else{
				dropVel = 2.0f;
			}
		}
		
		if((currentWeapon != backupWeapon || deadDropped) && currentWeapon != 0){//only drop backup weapon if player dies
			//set dropWeapon value to true for weapon switch code to check below
			dropWeapon = true;
		}
		
		UpdateTotalWeapons();
		
		//instantiate weaponDropObj from WeaponBehavios.cs at camera position
		if(weaponOrder[weapon].GetComponent<WeaponBehavior>().weaponDropObj){
			GameObject weaponObjDrop;
			if(!CameraControlComponent.thirdPersonActive){
				weaponObjDrop = Instantiate(weaponOrder[weapon].GetComponent<WeaponBehavior>().weaponDropObj, mainCamTransform.position + playerObj.transform.forward * 0.25f + Vector3.up * -0.25f, Quaternion.identity) as GameObject;
			}else{
				weaponObjDrop = Instantiate(weaponOrder[weapon].GetComponent<WeaponBehavior>().weaponDropObj, playerObj.transform.position + playerObj.transform.forward * 0.25f + Vector3.up * -0.25f, Quaternion.identity) as GameObject;//mainCamTransform.rotation
            }
			//add forward velocity to dropped weapon in the direction that the player currently faces 
			weaponObjDrop.GetComponent<Rigidbody>().AddForce(playerObj.transform.forward * dropVel, ForceMode.Impulse);
			//add random rotation to the dropped weapon
			float rotateAmt;
			if(Random.value > 0.5f){rotateAmt = 7.0f;}else{rotateAmt = -7.0f;}
			weaponObjDrop.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up * rotateAmt,ForceMode.Impulse);
			if(Random.value > 0.5f){rotateAmt = 7.0f;}else{rotateAmt = -7.0f;}
			weaponObjDrop.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.right * rotateAmt,ForceMode.Impulse);
		}

		//if player dropped their last weapon, just select the null/holstered weapon 
		if(!deadDropped){
          
			if(totalWeapons == 0 && currentWeapon != 0 && currentWeapon != backupWeapon){
				if(weaponOrder[backupWeapon].GetComponent<WeaponBehavior>().haveWeapon && FPSPlayerComponent.hitPoints > 1.0f){
					StartCoroutine(SelectWeapon(backupWeapon));
				}else{
					StartCoroutine(SelectWeapon(0));	
				}
			}
		}else{
			StartCoroutine(SelectWeapon(0));//don't select another weapon if player died
            Debug.Log("dead");
            FPSPlayer.instance.playerDaed = true;
           

        }

    }
	
	public void UpdateTotalWeapons (){
        Debug.Log("Forcefull assign totl weopon");
		totalWeapons = 3;//initialize totalWeapons value at zero because a weapon could have been picked up since we checked last
		//for(int i = 1; i < weaponOrder.Length; i++){//iterate through weaponOrder array and count total weapons in player's inventory
		//	if(weaponOrder[i].GetComponent<WeaponBehavior>().haveWeapon && weaponOrder[i].GetComponent<WeaponBehavior>().addsToTotalWeaps){
		//		totalWeapons ++;//increment totalWeapons by one if player has this weapon	
		//	}
		//}
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Select Weapons
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public IEnumerator SelectWeapon ( int index, bool isOffhandThrow = false, bool endOffhandThrow = false){
		CameraAnimatorComponent = Camera.main.GetComponent<Animator>();
		WeaponObjAnimatorComponent = weaponOrder[currentWeapon].GetComponent<Animator>();
		
		//we are not dropping a weapon anymore if one has been selected
		dropWeapon = false;
		
		//do not proceed with selecting weapon if player doesn't have it in their inventory
		//but make an exception for the null/unarmed weapon for when the player presses the holster button
		//also dont allow weapon switch if player is climbing, swimming, or holding object and their weapon is lowered
		WeaponBehavior ThisWeaponBehavior = weaponOrder[index].GetComponent<WeaponBehavior>();
		if((!ThisWeaponBehavior.haveWeapon && index != 0) 
		|| (!ThisWeaponBehavior.cycleSelect && !isOffhandThrow)
		|| FPSWalkerComponent.hideWeapon
		|| pullGrenadeState){
			yield break;
		}
		
		if(index != 0){//if a weapon is selected, prevent unarmed/null weapon from being selected in selection cycle 
			weaponOrder[0].GetComponent<WeaponBehavior>().haveWeapon = false;
		}
		
		//cancel zooming when switching
		FPSPlayerComponent.zoomed = false;
        if (HudMenuManager.instance.UI[0].activeSelf)
        {

            HudMenuManager.instance.zoomOut();
        }
        if (CurrentWeaponBehaviorComponent.useLight){
			if(CurrentWeaponBehaviorComponent.lightConeMesh){CurrentWeaponBehaviorComponent.lightConeMesh.enabled = false;}
			if(CurrentWeaponBehaviorComponent.spot){CurrentWeaponBehaviorComponent.spot.enabled = false;}
			if(CurrentWeaponBehaviorComponent.point){CurrentWeaponBehaviorComponent.point.enabled = false;}
		}
		
		//reset non-magazine reload if interrupted by weapon switch
		if(CurrentWeaponBehaviorComponent.bulletsToReload != CurrentWeaponBehaviorComponent.bulletsPerClip 
		&& IronsightsComponent.reloading){
			//play neutral animation when putting weapon away to prevent neutral anim glitch at start of next reload
			CurrentWeaponBehaviorComponent.WeaponAnimatorComponent.SetTrigger("Neutral");
			//reset bulletsReloaded to prevent delay of reloading the next time we reload this weapon
			CurrentWeaponBehaviorComponent.bulletsReloaded = 0;
		}
		
		//cancel reloading when switching
		IronsightsComponent.reloading = false;//set IronSights Reloading var to false
		CurrentWeaponBehaviorComponent.StopCoroutine("Reload");//stop the Reload function if it is running

		switchTime = Time.time;
		
		if(Time.timeSinceLevelLoad > 1){

			if(!offhandThrowActive && !displayingGrenade){
				//play weapon switch sound if not the first call to this function after level load
				aSource.clip = changesnd;
				aSource.volume = 1f;
				aSource.Play();
			}
		
			//play camera weapon switching animation
			CameraAnimatorComponent.speed = 1f;
			CameraAnimatorComponent.SetTrigger("CamSwitch");
		}

		if(WeaponObjAnimatorComponent.gameObject.activeSelf){
			//animate previous weapon down
			if(!FPSWalkerComponent.canRun && !FPSWalkerComponent.proneMove){
				WeaponObjAnimatorComponent.SetTrigger("Switch");
			}else{
				WeaponObjAnimatorComponent.SetTrigger("SprintBack");
				CurrentWeaponBehaviorComponent.sprintAnimState = true;//do this to keep weapons in back sprinting position while sprinting and switching
			}
		}
			
		if(Time.timeSinceLevelLoad > 2){
			if(!CurrentWeaponBehaviorComponent.verticalWeapon || isOffhandThrow){
				//move weapon down while switching
				IronsightsComponent.switchMove = -0.4f;
			}else{
				//move vertical oriented weapons down further while switching because they take more vertical screen space than guns
				IronsightsComponent.switchMove = -1.2f;
			}
			
			//wait for weapon down animation to play before switching weapons and animating weapon up
			yield return new WaitForSeconds(0.2f);
			
		}
		
		//immediately switch weapons (activate called weaponOrder index and deactivate all others)
		for (int i = 0; i < weaponOrder.Length; i++){

			CurWeaponObjAnimatorComponent = weaponOrder[i].GetComponent<Animator>();

			if (i == index){

				//update transform reference of active weapon object in other scipts
				IronsightsComponent.gun = weaponOrder[i].transform;
				//update active weapon object reference in other scipts
				IronsightsComponent.gunObj = weaponOrder[i];
				IronsightsComponent.WeaponBehaviorComponent = weaponOrder[i].GetComponent<WeaponBehavior>();
				FPSPlayerComponent.WeaponBehaviorComponent = weaponOrder[i].GetComponent<WeaponBehavior>();
				CameraControlComponent.gun = weaponOrder[i];
				CurrentWeaponBehaviorComponent = weaponOrder[i].GetComponent<WeaponBehavior>();

				// Activate the selected weapon
				weaponOrder[i].SetActive(true);

				if(endOffhandThrow){
					CurrentWeaponBehaviorComponent.isOffhandThrow = true;
					switchTime = Time.time - 0.5f;//allow weapon to start firing sooner after switching from offhand grenade throw than when recovering from sprinting
				}

				CurrentWeaponBehaviorComponent.InitializeWeapon();
				
				//get current weapon value from index
				currentWeapon = index;
			
				//synchronize current and previous weapon's y pos for correct offscreen switching, use localPosition not position for correct transforms
				weaponOrder[i].transform.localPosition = weaponOrder[i].transform.localPosition + new Vector3(0, weaponOrder[i].transform.localPosition.y - 0.3f, 0);
				
				if(Time.timeSinceLevelLoad > 2){
					//move weapon up when switch finishes
					IronsightsComponent.switchMove = 0;

					//animate selected weapon up by setting time of animation to it's end and playing in reverse
					if(!FPSWalkerComponent.canRun && !FPSWalkerComponent.proneMove){
						CurWeaponObjAnimatorComponent.SetTrigger("SwitchReverse");
					}else{
						//if player is sprinting, keep weapon in sprinting position during weapon switch
						CurWeaponObjAnimatorComponent.SetTrigger("SprintBack");
						CurrentWeaponBehaviorComponent.sprintAnimState = true;//do this to keep weapons in back sprinting position while sprinting and switching
					}

				}else{
					CurWeaponObjAnimatorComponent.SetTrigger("IdleForward");
				}

			}else{
				
				//reset transform of deactivated gun to make it in neutral position when selected again
				//use weapon parent transform.position instead of Camera.main.transform.position
				//or Camera.main.transform.localPosition to avoid positioning bugs due to camera pos changing with walking bob and kick 
				weaponOrder[i].transform.position = myTransform.position;

				if(CurWeaponObjAnimatorComponent.gameObject.activeSelf){
					CurWeaponObjAnimatorComponent.SetTrigger("SprintBack");
					CurrentWeaponBehaviorComponent.sprintAnimState = true;//do this to keep weapons in back sprinting position while sprinting and switching
				}

				//synchronize sprintState var in WeaponBehavior script
				weaponOrder[i].GetComponent<WeaponBehavior>().sprintState = true;

				// Activate the selected weapon
				weaponOrder[i].SetActive(false);
			}	
		}



    }
    bool Empty_Gun1, Empty_Gun2;
    
    public void CheckAllWeaponsAmmo()
    {
        bool switchGun = false;
        if (currentWeapon == 10) {
            // grenade
            MConstants.bulletsFinished = false;
            return;
        }
        int index = -1;
        if (currentWeapon == Gun1 + 1)
        {
           if ((weaponOrder[Gun2 + 1].GetComponent<WeaponBehavior>().ammo > 0 || weaponOrder[Gun2 + 1].GetComponent<WeaponBehavior>().bulletsLeft > 0)&& Empty_Gun1==false)
           {
	           switchGun = true;
	           index = Gun2 + 1;
	           Empty_Gun1 =true;
           }
        }
        else if (currentWeapon == Gun2 + 1)
        {
            if ((weaponOrder[Gun1 + 1].GetComponent<WeaponBehavior>().ammo > 0 || weaponOrder[Gun1 + 1].GetComponent<WeaponBehavior>().bulletsLeft > 0)&& Empty_Gun2==false)
            {
                switchGun = true;
                index = Gun1 + 1;
                Empty_Gun2 = true;
            }
        }

        if (switchGun && index != -1)
        {
            switchGun = false;
            HudMenuManager.instance.zoomOut();
            StartCoroutine(SelectWeapon(index));
            HudMenuManager.instance.outOfAmmoText.gameObject.SetActive(false);
            Invoke("bullets", 2f);
        }
        else {
            if (weaponOrder[Gun1 + 1].GetComponent<WeaponBehavior>().ammo > 0 || weaponOrder[Gun1 + 1].GetComponent<WeaponBehavior>().bulletsLeft > 0 ||
                weaponOrder[Gun2 + 1].GetComponent<WeaponBehavior>().ammo > 0 || weaponOrder[Gun2 + 1].GetComponent<WeaponBehavior>().bulletsLeft > 0)
            {
                HudMenuManager.instance.outOfAmmoText.gameObject.SetActive(false);

            }
            else
            {
                if (!MConstants.IslastBullet && !_showOutOfAmmoOnce)
                {
                    //both guns are empty
                    _showOutOfAmmoOnce = true;
                    HudMenuManager.instance.outOfAmmoDialouge.SetActive(true);
                }
            }
            Invoke("bullets", 2f);

        }

    }

    void bullets() {
        MConstants.bulletsFinished = false;
    }

    public void GiveAllWeaponsAndAmmo(){
		foreach(WeaponBehavior wb in weaponBehaviors){
			wb.haveWeapon = true;
			wb.ammo = wb.maxAmmo;
		}
	}

    public void AddBulletsAfterVideo()
    {
	    HudMenuManager.instance.outOfAmmoDialouge.SetActive(false);
	    _showOutOfAmmoOnce = false;
	    weaponOrder[Gun1 + 1].GetComponent<WeaponBehavior>().ammo = defaultGun1Ammo;
	    weaponOrder[Gun2 + 1].GetComponent<WeaponBehavior>().ammo = defaultGun2Ammo;
    }

}