//ExplosiveObject.cs by Azuline Studios© All Rights Reserved
//set up and detonation of explosive objects
using UnityEngine;
using System.Collections;

public class ExplosiveObject : MonoBehaviour {
	ExplosiveObject objj;
	protected  WeaponEffects WeaponEffectsComponent;
	[Tooltip("When hit points of object are depleted, object will explode.")]
	public float hitPoints = 100;
	protected float initialHitPoints = 100;//when hit points of object are depleted, object will explode
	[Tooltip("Maximum damage dealt at center of explosion (damage decreases from center).")]
	public float explosionDamage = 200.0f;
	[Tooltip("Delay before this object applies explosion force and damage to other objects;.")]
	public float damageDelay = 0.2f;
	private float explosionDamageAmt;//actual explosion damage amount
	[Tooltip("Explosive physics force applied to objects in blast radius.")]
	public float blastForce = 15.0f;
	[Tooltip("Radius of explosion.")]
	public float radius = 7.0f;
	[Tooltip("Layers that will be hit by explosion.")]
	public LayerMask blastMask;
	[Tooltip("Layers that will block explosion blast.")]
	public LayerMask obstructionMask;//should be layers 9,10,11,12,13
	protected Transform myTransform;
	private bool detonated;
	private bool audioPlayed;
	public int objectPoolIndex = 0;//used for grenades
    public bool Isbarrel;
	public bool IsNotDestroyable;
	public bool IsCameraMission;
	//[HideInInspector]
	public GameObject ObjectToActive;
    protected AudioSource aSource;
	private Collider hitCollider;
	private Rigidbody hitRigidbody;
	//Camera mainCamera;

	void Start (){
        if (Isbarrel || IsNotDestroyable)
        {
           // GetComponent<Rigidbody>().isKinematic = true;
            objectPoolIndex = 0;
        }
		WeaponEffectsComponent = HudMenuManager.instance.player.WeaponEffectsComponent;
		myTransform = transform;
		initialHitPoints = hitPoints;
		aSource = GetComponent<AudioSource>();
	

	}

	IEnumerator DetectDestroyed(){
		while(true){
			if(audioPlayed && !aSource.isPlaying){//destroy object after sound has finished playing
				if(objectPoolIndex == 0){
					//prevent attached hitmarks from being destroyed with game object
					FadeOutDecals[] decals = gameObject.GetComponentsInChildren<FadeOutDecals>(true);
					foreach (FadeOutDecals dec in decals) {
						dec.parentObjTransform.parent = AzuObjectPool.instance.transform;
						dec.parentObj.SetActive(false);
						dec.gameObject.SetActive(false);
					}
					//drop arrows if object is destroyed
					ArrowObject[] arrows = gameObject.GetComponentsInChildren<ArrowObject>(true);
					foreach (ArrowObject arr in arrows) {
						arr.transform.parent = null;
						arr.myRigidbody.isKinematic = false;
						arr.myBoxCol.isTrigger = false;
						arr.gameObject.tag = "Usable";
						arr.falling = true;
					}
					Destroy(myTransform.gameObject);
                  
				}else{
					ResetExplosiveObject();
					AzuObjectPool.instance.RecyclePooledObj(objectPoolIndex, myTransform.gameObject);
				}
				yield break;
			}
			yield return new WaitForSeconds(0.2f);
		}

	}

	void ResetExplosiveObject(){
		detonated = false;
		audioPlayed = false;
		hitPoints = initialHitPoints;
		if (myTransform.GetComponent<MeshRenderer>())
		{
			myTransform.GetComponent<MeshRenderer>().enabled = true;
		}
	}
	
	//find objects in blast radius, apply damage and physics force, and remove explosive object
    IEnumerator Detonate() {

		//play explosion effects and apply explosion damage and force after damageDelay
		yield return new WaitForSeconds(damageDelay);

		//play explosion effects
		// for vehicle Explosion
		if (IsNotDestroyable)
		{
			WeaponEffectsComponent.VehicleExplosion(myTransform.position);
			if (myTransform.GetComponent<MeshRenderer>())
				myTransform.GetComponent<MeshRenderer>().enabled = false;
			StartCoroutine(DestroyObject());
		}
		// for Simple Explosion
		else
		{
			WeaponEffectsComponent.ExplosionEffect(myTransform.position);
			if (myTransform.GetComponent<MeshRenderer>())
				myTransform.GetComponent<MeshRenderer>().enabled = false;
		}
		aSource.pitch = Random.Range(0.75f * Time.timeScale, 1.0f * Time.timeScale);//add slight random value to sound pitch for variety
		aSource.Play();
		audioPlayed = true;
        if (Isbarrel)
        {
            if (GetComponent<MeshCollider>())
            {
				GetComponent<MeshCollider>().enabled = false;
            }
        }

		//find surrounding objects to be damaged by explosion
		Collider[] hitColliders = Physics.OverlapSphere(myTransform.position, radius, blastMask, QueryTriggerInteraction.Ignore);
		//apply damage and force to surrounding objects
		for(int i = 0; i < hitColliders.Length; i++){
			RaycastHit hit;
			if(hitColliders[i].transform != myTransform
			//don't damage or apply force to object if it is shielded/hidden from blast by other object
			&& 	Physics.Raycast(myTransform.position, ( hitColliders[i].transform.position - myTransform.position).normalized, out hit, radius*2f)){
				hitCollider = hitColliders[i].GetComponent<Collider>();
				explosionDamageAmt = explosionDamage * Mathf.Clamp01((1.0f - (myTransform.position - hitColliders[i].transform.position).magnitude / radius));//make damage decrease by distance from center
				if(explosionDamageAmt >= 1 && hit.collider ==  hitCollider){
					//call ApplyDamage() function of objects in radius	
					switch(hitCollider.gameObject.layer){
						case 13://hit object is an NPC
							Debug.Log("hitCollider.gameObject" + hitCollider.gameObject);
							 if (hitCollider.gameObject.GetComponent<TacticalAI.HitBox>())
							{
								hitCollider.gameObject.GetComponent<TacticalAI.HitBox>().Damage(explosionDamageAmt, Vector3.zero);
							}else
							if (hitCollider.gameObject.GetComponent<CharacterDamage>()){
								hitCollider.gameObject.GetComponent<CharacterDamage>().ApplyDamage(explosionDamageAmt, Vector3.zero, myTransform.position, myTransform, false, true);
							}
							else if(hitCollider.gameObject.GetComponent<LocationDamage>()){
								hitCollider.gameObject.GetComponent<LocationDamage>().ApplyDamage(explosionDamageAmt, Vector3.zero, myTransform.position, myTransform, false, true);
							}
							if (hitCollider.gameObject.GetComponent<SocideBoomber>())
                            {
                                hit.collider.gameObject.GetComponent<SocideBoomber>().DamageApplie(explosionDamageAmt);

                                //hitCollider.gameObject.GetComponent<LocationDamage>().ApplyDamage(explosionDamageAmt, Vector3.zero, myTransform.position, myTransform, false, true);
                            }else if (hitCollider.gameObject.GetComponentInParent<SocideBoomber>())
							{
								hit.collider.gameObject.GetComponentInParent<SocideBoomber>().DamageApplie(explosionDamageAmt);

								//hitCollider.gameObject.GetComponent<LocationDamage>().ApplyDamage(explosionDamageAmt, Vector3.zero, myTransform.position, myTransform, false, true);
							}
							if (hit.collider.gameObject.GetComponent<Guncontroller>())
							{
								if (hit.collider.gameObject.GetComponent<Guncontroller>().isjeep)
								{
									hit.collider.gameObject.GetComponent<Guncontroller>().OnHit(explosionDamageAmt);
								}
								else
								{
									hit.collider.gameObject.GetComponent<Guncontroller>().OnHit(explosionDamageAmt / 2.5f);
								}
								//FPSPlayerComponent.UpdateHitTime();//used for hitmarker
							}
						if (hit.collider.gameObject.GetComponentInParent<MachineGunCtrl>())
							{
								hit.collider.gameObject.GetComponentInParent<MachineGunCtrl>().ApplyDamage(explosionDamageAmt);
							}


							break;
                        case 9:
                            if (hitCollider.gameObject.GetComponent<JeepCTRL>())
                            {
                                hitCollider.gameObject.GetComponent<JeepCTRL>().JeepExplod();
                            }
                            break;
						case 0://hit object
							if(hitCollider.gameObject.GetComponent<BreakableObject>()){
								hitCollider.gameObject.GetComponent<BreakableObject>().ApplyDamage(explosionDamageAmt);
							}else if(hitCollider.gameObject.GetComponent<AppleFall>()){
								hitCollider.gameObject.GetComponent<AppleFall>().ApplyDamage(explosionDamageAmt);
							}else if(hitCollider.gameObject.GetComponent<ExplosiveObject>()){
								hitCollider.gameObject.GetComponent<ExplosiveObject>().ApplyDamage(explosionDamageAmt);
							}else if(hitCollider.gameObject.GetComponent<MineExplosion>()){
								hitCollider.gameObject.GetComponent<MineExplosion>().ApplyDamage(explosionDamageAmt);
							}
							else if (hitCollider.gameObject.GetComponent<TacticalAI.HitBox>())
							{
								hitCollider.gameObject.GetComponent<TacticalAI.HitBox>().Damage(explosionDamageAmt,Vector3.zero);
							}
							break;
						//case 11://hit object is player
						//	//if(hitCollider.gameObject.GetComponent<FPSPlayer>()){
						//	//	hitCollider.gameObject.GetComponent<FPSPlayer>().ApplyDamage(explosionDamageAmt);
						//	//}	
						//	//break;
						default:
							break;	
					}
					//apply explosion force
					if (hitColliders[i].transform.GetComponent<Rigidbody>()){
						hitRigidbody = hitColliders[i].transform.GetComponent<Rigidbody>();
						hitRigidbody.AddExplosionForce(blastForce * hitRigidbody.mass, myTransform.position, radius, 3.0f, ForceMode.Impulse);
					}
				}
			}
			
			if(i < hitColliders.Length - 1){
				continue;
			}else{//if all objects have been damaged by blast, disable collider (it was needed for the line cast above)
				if(objectPoolIndex == 0){
                    if (myTransform.GetComponent<MeshCollider>())
                    {
                        myTransform.GetComponent<MeshCollider>().enabled = false;
                    }
				}
			}
		}
		StartCoroutine(DetectDestroyed());
    }

	 protected IEnumerator DestroyObject()
    {

		if (GetComponent<BoxCollider>())
		{
			//GetComponent<MeshCollider>().enabled = false;
			GetComponent<BoxCollider>().enabled = false;

		}
		yield return new WaitForSeconds(0.5f);
		ObjectToActive.SetActive(true);
	}

	
	//if explosive object is shot or damaged by explosion, subtract hitpoints or detonate
	public virtual void ApplyDamage( float damage ){
      
        hitPoints -= damage;
		//if(hitPoints <= 0.0f && IsCameraMission)
  //      {
		//	StartCoroutine(CameraDetector());
  //      }
		if(!detonated && hitPoints <= 0.0f && !IsCameraMission){
			detonated = true;//this line must be before call to Detonate() otherwise stack overflow occurs
			StartCoroutine(Detonate());
		}
	}
	
}