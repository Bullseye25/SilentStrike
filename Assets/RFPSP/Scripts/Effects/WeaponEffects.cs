//WeaponEffects.cs by Azuline Studios© All Rights Reserved
//Emits particles and plays sound effects for impacts by surface type,
//instantiates weapon mark objects, and emits tracer effects. 
using UnityEngine;
using System.Collections;

public class WeaponEffects : MonoBehaviour {
	private PlayerWeapons PlayerWeaponsComponent;
	private WeaponBehavior WeaponBehaviorComponent;
	private FPSRigidBodyWalker FPSWalkerComponent;
	[HideInInspector]
	public GameObject weaponObj;

	//Particle Emitters
	//used for weapon fire and bullet impact effects
	[Tooltip("Particle effect to use for dirt impacts.")]
	public GameObject dirtImpact;
	[Tooltip("Particle effect to use for metal impacts.")]
	public GameObject metalImpact;
	[Tooltip("Particle effect to use for wood impacts.")]
	public GameObject woodImpact;
	[Tooltip("Particle effect to use for water impacts.")]
	public GameObject waterImpact;
	[Tooltip("Particle effect to use for glass impacts.")]
	public GameObject glassImpact;
	[Tooltip("Particle effect to use for flesh impacts.")]
	public GameObject fleshImpact;
	[Tooltip("Particle effect to use for stone impacts.")]
	public GameObject stoneImpact;
	[Tooltip("Particle effect to use for explosions.")]
	public GameObject explosion;
	public GameObject CarExplosion;
	private GameObject explosionObj;
	[Tooltip("Particle effect to use for water splashes.")]
	public GameObject waterSplash;
	[Tooltip("Particles emitted around player treading water.")]
	public ParticleSystem rippleEffect;
	[Tooltip("Particles emitted underwater for ambient bubbles/particles.")]
	public ParticleSystem bubblesEffect;
	[Tooltip("Particles to emit when player is swimming on water surface.")]
	public ParticleSystem splashTrail;


	private int impactObjID;
	private GameObject impactObj;
	[Tooltip("Particle effect to use for NPC tracers.")]
	public ParticleSystem npcTracerParticles;
	[Tooltip("Particle effect to use for player tracers.")]
	public ParticleSystem playerTracerParticles;
	[Tooltip("Particle effect to use for underwater tracers.")]
	public ParticleSystem bubbleParticles;

	//impact marks to be placed on objects where projectiles hit
	[Tooltip("Index in the object pool of dirt mark objects.")]
	public int[] dirtMarksID;
	[Tooltip("Index in the object pool of metal mark objects.")]
	public int[] metalMarksID;
	[Tooltip("Index in the object pool of wood mark objects.")]
	public int[] woodMarksID;
	[Tooltip("Index in the object pool of glass mark objects.")]
	public int[] glassMarksID;
	[Tooltip("Index in the object pool of stone mark objects.")]
	public int[] stoneMarksID;

	[Tooltip("Index in the object pool of dirt mark objects for melee weapons.")]
	public int[] dirtMarksMeleeID;
	[Tooltip("Index in the object pool of metal mark objects for melee weapons.")]
	public int[] metalMarksMeleeID;
	[Tooltip("Index in the object pool of wood mark objects for melee weapons.")]
	public int[] woodMarksMeleeID;

	private int markObjID;

	//impact sound effects for different materials
	[Tooltip("Sounds to use for default impacts.")]
	public AudioClip[] defaultImpactSounds;
	[Tooltip("Sounds to use for metal impacts.")]
	public AudioClip[] metalImpactSounds;
	[Tooltip("Sounds to use for wood impacts.")]
	public AudioClip[] woodImpactSounds;
	[Tooltip("Sounds to use for water impacts.")]
	public AudioClip[] waterImpactSounds;
	[Tooltip("Sounds to use for glass impacts.")]
	public AudioClip[] glassImpactSounds;
	[Tooltip("Sounds to use for flesh impacts.")]
	public AudioClip[] fleshImpactSounds;
	[Tooltip("Sounds to use for stone impacts.")]
	public AudioClip[] stoneImpactSounds;

	[Tooltip("Sounds to use for default melee impacts.")]
	public AudioClip[] defaultImpactSoundsMelee;
	[Tooltip("Sounds to use for metal melee impacts.")]
	public AudioClip[] metalImpactSoundsMelee;
	[Tooltip("Sounds to use for wood melee impacts.")]
	public AudioClip[] woodImpactSoundsMelee;
	[Tooltip("Sounds to use for flesh melee impacts.")]
	public AudioClip[] fleshImpactSoundsMelee;
	[Tooltip("Sounds to use for stone melee impacts.")]
	public AudioClip[] stoneImpactSoundsMelee;

	private AudioClip hitSound;
	private float hitVolumeAmt = 1.0f;

	private ParticleSystem partSys;

	private ParticleSystem tracerParticles;
	private ParticleSystem.Particle[] activeParticles;
	private int numparticles;
	private int numParticlesAlive;
	private bool rotateParticle;

	private float randvel;

	public void Start(){
		weaponObj = transform.gameObject;
		PlayerWeaponsComponent = weaponObj.GetComponentInChildren<PlayerWeapons>();
		WeaponBehaviorComponent = PlayerWeaponsComponent.CurrentWeaponBehaviorComponent;
		FPSWalkerComponent = Camera.main.transform.GetComponent<CameraControl>().playerObj.GetComponent<FPSRigidBodyWalker>();
		activeParticles = new ParticleSystem.Particle[256]; 
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Draw Impact Effects
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public bool ImpactEffects ( Collider hitcol, Vector3 hitPoint, bool NpcAttack, bool meleeAttack, Vector3 rayNormal = default(Vector3)){
		WeaponBehaviorComponent = PlayerWeaponsComponent.CurrentWeaponBehaviorComponent;
		//find the tag name of the hit game object and emit the particle effects for the surface type
		switch(hitcol.gameObject.tag){
		case "Dirt":
			impactObj = dirtImpact;//set impactObj to the particle effect game object group for this surface type
			rotateParticle = true;
			if(!meleeAttack){
				if(defaultImpactSounds[0]){
					hitSound = defaultImpactSounds[Random.Range(0, defaultImpactSounds.Length)];//select random impact sound for this surface type
				}
			}else{
				if(defaultImpactSoundsMelee[0]){
					hitSound = defaultImpactSoundsMelee[Random.Range(0, defaultImpactSoundsMelee.Length)];//select random impact sound for this surface type
				}
			}
			break;
		case "Metal":
			impactObj = metalImpact;
			rotateParticle = true;
			if(!meleeAttack){
				if(metalImpactSounds[0]){
					hitSound = metalImpactSounds[Random.Range(0, metalImpactSounds.Length)];//select random impact sound for this surface type
				}
			}else{
				if(metalImpactSoundsMelee[0]){
					hitSound = metalImpactSoundsMelee[Random.Range(0, metalImpactSoundsMelee.Length)];//select random impact sound for this surface type
				}
			}
			break;
		case "Wood":
			impactObj = woodImpact;
			rotateParticle = true;
			if(!meleeAttack){
				if(woodImpactSounds[0]){
					hitSound = woodImpactSounds[Random.Range(0, woodImpactSounds.Length)];//select random impact sound for this surface type
				}
			}else{
				if(woodImpactSoundsMelee[0]){
					hitSound = woodImpactSoundsMelee[Random.Range(0, woodImpactSoundsMelee.Length)];//select random impact sound for this surface type
				}
			}
			break;
		case "Water":
			impactObj = waterImpact;
			rotateParticle = false;
			if(waterImpactSounds[0]){
				hitSound = waterImpactSounds[Random.Range(0, waterImpactSounds.Length)];
			}
			break;
		case "Glass":
			impactObj = glassImpact;
			rotateParticle = false;
			if(glassImpactSounds[0]){
				hitSound = glassImpactSounds[Random.Range(0, glassImpactSounds.Length)];
			}
			break;
		case "Flesh":
			case "HitBox":
			case "Player":
				impactObj = fleshImpact;
			rotateParticle = false;
			if(!meleeAttack){
				if(fleshImpactSounds[0]){
					hitSound = fleshImpactSounds[Random.Range(0, fleshImpactSounds.Length)];//select random impact sound for this surface type
				}
			}else{
				if(fleshImpactSoundsMelee[0]){
					hitSound = fleshImpactSoundsMelee[Random.Range(0, fleshImpactSoundsMelee.Length)];//select random impact sound for this surface type
				}
			}
			break;
		case "Stone":
			impactObj = stoneImpact;
			rotateParticle = true;
			if(!meleeAttack){
				if(stoneImpactSounds[0]){
					hitSound = stoneImpactSounds[Random.Range(0, stoneImpactSounds.Length)];//select random impact sound for this surface type
				}
			}else{
				if(stoneImpactSoundsMelee[0]){
					hitSound = stoneImpactSoundsMelee[Random.Range(0, stoneImpactSoundsMelee.Length)];//select random impact sound for this surface type
				}
			}
			break;
		default:
			impactObj = metalImpact;
			rotateParticle = false;
			if(!meleeAttack){
				if(defaultImpactSounds[0]){
					hitSound = defaultImpactSounds[Random.Range(0, defaultImpactSounds.Length)];//select random impact sound for this surface type
				}
			}else{
				if(defaultImpactSoundsMelee[0]){
					hitSound = defaultImpactSoundsMelee[Random.Range(0, defaultImpactSoundsMelee.Length)];//select random impact sound for this surface type
				}
			}
			break;
		}
			
		impactObj.SetActive(true);
		impactObj.transform.position = hitPoint;

		foreach (Transform child in impactObj.transform){//emit all particles in the particle effect game object group stored in impactObj var
				partSys = child.GetComponent<ParticleSystem>();
				EmitRotatedParticle(partSys, rayNormal);
		}

		//modify the weapon impact sounds based on the weapon type, so the multiple shotgun impacts and automatic weapons aren't so loud
		if(!NpcAttack && !WeaponBehaviorComponent.meleeActive){
			if(WeaponBehaviorComponent.projectileCount > 1){
				hitVolumeAmt = 0.2f;	
			}else if(!WeaponBehaviorComponent.semiAuto){
				hitVolumeAmt = 0.8f;	
			}else{
				hitVolumeAmt = 1.0f;
			}
		}else{
			hitVolumeAmt = 1.0f;
		}

		//play sounds of bullets hitting surface/ricocheting
		PlayAudioAtPos.PlayClipAt(hitSound, hitPoint, hitVolumeAmt, 1.0f, 1.0f, 3.0f);
		return true;
	}

	//emit the particles and also rotate their velocities to be perpendicular to surface angle hit
	//by doing this, only one particle system needs to be used for each projectile impact type to save memory
	public void EmitRotatedParticle (ParticleSystem partSysToRotate, Vector3 direction){
		partSysToRotate.Emit(Mathf.RoundToInt(partSysToRotate.emission.rateOverTime.constant));//emit the particle(s)
		if(rotateParticle){
			numParticlesAlive = partSysToRotate.GetParticles(activeParticles);//get array of all active particles in this system (send to activeParticles array)
			for (int i = numParticlesAlive-1; i > numParticlesAlive-1 - partSysToRotate.emission.rateOverTime.constant; i--){
				if(Random.value > 0.5f){//calculate random vertical velocity
					randvel = partSysToRotate.main.startSpeed.constant * Mathf.Clamp(Random.value, 0.1f, 1.0f); 
				}else{
					randvel = partSysToRotate.main.startSpeed.constant + (partSysToRotate.main.startSpeed.constant * (Mathf.Clamp(Random.value, 0.25f, 0.75f)));
				}
				activeParticles[i].velocity = direction * randvel;//manually apply random vertical velocity perpendicular to hit surface
			}
			partSysToRotate.SetParticles(activeParticles, numParticlesAlive);//apply velocity changes to particles for this impact emission
		}
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Draw Bullet Marks
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void BulletMarks ( RaycastHit hit, bool meleeAttack){

		if(!meleeAttack){//not a melee weapon
			//find the tag name of the hit game object and select an impact mark for the surface type
			switch(hit.collider.gameObject.tag){
			case "Dirt":
				markObjID = dirtMarksID[Random.Range(0, dirtMarksID.Length)];
				break;
			case "Metal":
				markObjID = metalMarksID[Random.Range(0, metalMarksID.Length)];
				break;
			case "Wood":
				markObjID = woodMarksID[Random.Range(0, woodMarksID.Length)];
				break;
			case "Glass":
				markObjID = glassMarksID[Random.Range(0, glassMarksID.Length)];
				break;
			case "Stone":
				markObjID = stoneMarksID[Random.Range(0, stoneMarksID.Length)];
				break;
			default:
				markObjID = dirtMarksID[Random.Range(0, dirtMarksID.Length)];
				break;
			}
		}else{
			switch(hit.collider.gameObject.tag){//select a melee weapon impact mark
			case "Dirt":
				markObjID = dirtMarksMeleeID[Random.Range(0, dirtMarksMeleeID.Length)];
				break;
			case "Metal":
				markObjID = metalMarksMeleeID[Random.Range(0, metalMarksMeleeID.Length)];
				break;
			case "Wood":
				markObjID = woodMarksMeleeID[Random.Range(0, woodMarksMeleeID.Length)];
				break;
			case "Glass":
				markObjID = glassMarksID[Random.Range(0, glassMarksID.Length)];
				break;
			default:
				markObjID = dirtMarksMeleeID[Random.Range(0, dirtMarksMeleeID.Length)];
				break;
			}
		}

		if(hit.collider//check only objects with colliders attatched to prevent null reference error
			&& hit.collider.gameObject.layer != 9//don't leave marks on ragdolls
			&& hit.collider.gameObject.layer != 13//don't leave marks on NPCs
			&& hit.collider.gameObject.tag != "NoHitMark"//don't leave marks on active public List<AI> public List<AI> Npcs = new List<AI>();  = new List<AI>();  or objects with NoHitMark or PickUp tag
			&& hit.collider.gameObject.tag != "PickUp"
			&& hit.collider.gameObject.tag != "Flesh"
			&& hit.collider.gameObject.tag != "Usable"
			&& hit.collider.gameObject.tag != "Water"){
			//create an instance of the bullet mark and place it parallel and slightly above the hit surface to prevent z buffer fighting
			//GameObject clone = Instantiate(markObjID, hit.point + (hit.normal * 0.025f), Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject; 
			GameObject cloneParent = AzuObjectPool.instance.SpawnPooledObj(markObjID, hit.point + (hit.normal * 0.025f), Quaternion.identity) as GameObject;
			GameObject cloneDecal = cloneParent.transform.GetChild(0).gameObject;
			FadeOutDecals FadeOutDecalsComponent = cloneDecal.transform.GetComponent<FadeOutDecals>();
			Transform tempObjTransform = FadeOutDecalsComponent.parentObjTransform;
			Transform cloneTransform = FadeOutDecalsComponent.myTransform;
			FadeOutDecalsComponent.InitializeDecal();

			cloneTransform.parent = null;
			cloneDecal.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

			//save initial scaling of bullet mark prefab object
			Vector3 scale = cloneTransform.localScale;		
			//set parent of empty game object to hit object's transform
			tempObjTransform.parent = hit.transform;

			//set scale of empty game object to (1,1,1) to prepare it for applying the inverse scale of the object that was hit
			tempObjTransform.localScale = Vector3.one;
			//sync empty game object's rotation quaternion with hit object's quaternion for correct scaling of euler angles (use the same orientation of axes)
			Quaternion tempQuat = hit.transform.rotation;
			tempObjTransform.rotation = tempQuat;
			Vector3 tempScale1 = Vector3.one;
			//detect if hit collider is scaled parent object, or if we need to check one parent higher for unevenly scaled object
			if(tempObjTransform.parent.transform.localScale != Vector3.one || tempObjTransform.parent.transform == hit.transform.root){
				//calculate inverse of hit object's scale to compensate for objects that have been unevenly scaled in editor
				tempScale1 = new Vector3(1.0f / tempObjTransform.parent.transform.localScale.x, 
					1.0f / tempObjTransform.parent.transform.localScale.y, 
					1.0f / tempObjTransform.parent.transform.localScale.z);
			}else if(tempObjTransform.parent.transform.parent.transform.localScale != Vector3.one){
				//calculate inverse of hit object's scale to compensate for objects that have been unevenly scaled in editor
				tempScale1 = new Vector3(1.0f / tempObjTransform.parent.transform.parent.transform.localScale.x, 
					1.0f / tempObjTransform.parent.transform.parent.transform.localScale.y, 
					1.0f / tempObjTransform.parent.transform.parent.transform.localScale.z);
			}
			//apply inverse scale of the collider that was hit to empty game object's transform
			tempObjTransform.localScale = tempScale1;
			//set parent of bullet mark object to empy game object and set localScale to (1,1,1)
			cloneTransform.parent = tempObjTransform;
			//apply hit mark's initial scale to hit mark instance
			cloneTransform.localScale = scale;
			//randomly scale bullet marks slightly for more natural visual effect
			if(!meleeAttack){//not a melee weapon
				float tempScale = Random.Range (-0.25f, 0.25f);//find random scale amount
				cloneTransform.localScale = scale + new Vector3(tempScale, 0, tempScale);//apply random scale to bullet mark object's localScale
				cloneTransform.RotateAround(hit.point, hit.normal, Random.Range (-50, 50));
			}else{
				//rotate hit mark randomly for more variation
				cloneTransform.RotateAround(hit.point, hit.normal, Random.Range (-70, 70));
			}
		}

	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Draw Bullet Tracers
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void BulletTracers ( Vector3 direction, Vector3 position, float tracerDist = 0.0f, float tracerDistSwim = 0.0f, bool isPlayer = true){

		if(isPlayer && !WeaponBehaviorComponent.MouseLookComponent.thirdPerson) {
			tracerParticles = null; //playerTracerParticles;
		}else{
			tracerParticles = npcTracerParticles;
		}

		//Draw Bullet Tracers
		if (!FPSWalkerComponent.holdingBreath)
		{
			if (WeaponBehaviorComponent.muzzleParticles && isPlayer)
			{
				WeaponBehaviorComponent.muzzleParticles.SetActive(false);
				WeaponBehaviorComponent.muzzleParticles.SetActive(true);
			}
			//if (tracerParticles == null)
			//{
			//	AzuObjectPool.instance.SpawnPooledObj(18, position + direction * tracerDist, WeaponBehaviorComponent.gameObject.transform.rotation);
			//}
			if (tracerParticles) {
				//Set tracer origin to a small amount forward of the end of gun barrel (muzzle flash position)
				tracerParticles.transform.position = position + direction * tracerDist;
				tracerParticles.Emit(Mathf.RoundToInt(tracerParticles.emission.rateOverTime.constant));

				numParticlesAlive = tracerParticles.GetParticles(activeParticles);
				activeParticles[numParticlesAlive - 1].velocity = direction * tracerParticles.main.startSpeed.constant;
				tracerParticles.SetParticles(activeParticles, numParticlesAlive);
			}
		}else{
			if (bubbleParticles) {
				bubbleParticles.transform.position = position - direction * tracerDistSwim;
				bubbleParticles.transform.rotation = Quaternion.FromToRotation(Vector3.forward, direction);
				bubbleParticles.Emit(Mathf.RoundToInt(bubbleParticles.emission.rateOverTime.constant));
			}	
		}

	}
	public void PlayImpactSountRandomy(Vector3 hitPoint)
	{
		if (defaultImpactSounds[0] && Random.Range(0, 100) > 75)
		{
			hitSound = defaultImpactSounds[Random.Range(0, defaultImpactSounds.Length)];//select random impact sound for this surface type
		}
		else
		{
			if (fleshImpactSounds[0])
			{
				hitSound = fleshImpactSounds[Random.Range(0, fleshImpactSounds.Length)];//select random impact sound for this surface type
			}
		}
		PlayAudioAtPos.PlayClipAt(hitSound, hitPoint, hitVolumeAmt, 1.0f, 1, 3.0f);

	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//Draw Explosions
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public void ExplosionEffect ( Vector3 position){
		explosionObj = explosion;
		explosionObj.transform.position = position;
		foreach (Transform child in explosionObj.transform){//emit all particles in the particle effect game object group stored in impactObj var
			if(child.GetComponent<ParticleSystem>()){
				partSys = child.GetComponent<ParticleSystem>();
				partSys.Emit(Mathf.RoundToInt(partSys.emission.rateOverTime.constant));//emit the particle(s)
			}
		}
	}

	public void VehicleExplosion(Vector3 position)
	{
		explosionObj = CarExplosion;
		explosionObj.transform.position = position;
		foreach (Transform child in explosionObj.transform)
		{//emit all particles in the particle effect game object group stored in impactObj var
			if (child.GetComponent<ParticleSystem>())
			{
				partSys = child.GetComponent<ParticleSystem>();
				partSys.Emit(Mathf.RoundToInt(partSys.emission.rateOverTime.constant));//emit the particle(s)
			}
		}
	}


}
