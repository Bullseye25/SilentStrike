using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEnemy : ExplosiveObject
{
	public PlayerDetection detector;
	private DoorController DoorState;
	
    public  void Start()
    {
		// Parent Class Working
		if (Isbarrel || IsNotDestroyable)
		{
			// GetComponent<Rigidbody>().isKinematic = true;
			objectPoolIndex = 0;
		}
		WeaponEffectsComponent = Camera.main.GetComponent<CameraControl>().playerObj.GetComponent<FPSPlayer>().WeaponEffectsComponent;
		myTransform = transform;
		initialHitPoints = hitPoints;
		aSource = GetComponent<AudioSource>();

		// Child Class Working
		DoorState = detector.doorState;
		
    }
 

	 IEnumerator CameraDetector()
	{
		yield return new WaitForSeconds(damageDelay);
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
			GetComponent<Renderer>().material.color = Color.black;
			GetComponent<Animation>().enabled = false;
			if (myTransform.GetComponent<MeshCollider>())
			{
				myTransform.GetComponent<MeshCollider>().enabled = false;
			}

		    detector.ObjectToDisable();
			aSource.Play();
			ObjectToActive.SetActive(true);
			//DoorState.DoorMechanism(DoorController.DoorStates.DoorOpen);

		}

	}

	public override void ApplyDamage(float damage)
	{
		if (!IsCameraMission)
		{
			base.ApplyDamage(damage);
		}
		else
        {
			hitPoints -= damage;
			if (hitPoints <= 0.0f)
            {
				StartCoroutine(CameraDetector());
			}
        }

    }
}
