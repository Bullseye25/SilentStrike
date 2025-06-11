using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class JetGun : MonoBehaviour
{


	public bool isFiring;
	public float shotInterval = 0.175f;
	private float lastShotTime;
	public ParticleSystem muzzleFlash;

	public Transform projectileOrigin;      // Transform where projectiles will be fired from.
	Quaternion fireRotation;

	public Animator animator;
	private AudioSource audioSource;
	public AudioClip shotSound;


	public void StartFiring()
	{
		isFiring = true;
	}

	public void StopFiring()
	{
		isFiring = false;
	}

	private void FixedUpdate()
	{
		if (this.isFiring)
		{

			this.FireBullet();
			if ((this.audioSource != null) && (this.shotSound != null) && !this.audioSource.isPlaying) // && (!this.shotSoundLooped))
			{
				this.audioSource.loop = true;
				this.audioSource.clip = this.shotSound;
				this.audioSource.Play();
				//this.audioSource.volume = 0.1f;

				//this.audioSource.PlayOneShot(this.shotSound);
			}
		}
		else
		{
			if (animator)
				animator.SetBool("Shoot", false);
			if ((this.muzzleFlash != null))
			{
				this.muzzleFlash.gameObject.SetActive(false);
			}
			if ((this.audioSource != null) && (this.shotSound != null) && this.audioSource.isPlaying) // && (!this.shotSoundLooped))
			{
				this.audioSource.Stop();
			}
		}
	}
	string hitEffectTag = "Player";

	private void FireBullet()
	{
		if ((Time.time - this.lastShotTime) >= this.shotInterval)
		{
			this.lastShotTime = Time.time;


			// Shoot...


			
				// Emit particles...

				if ((this.muzzleFlash != null))
				{
					this.muzzleFlash.gameObject.SetActive(true);
				muzzleFlash.Play();

			}


			FPSPlayer.instance.WeaponEffectsComponent.BulletTracers(projectileOrigin.forward, projectileOrigin.position, -3.0f, 0.0f, false);
			
			RaycastHit hit;
			if (Physics.Raycast(projectileOrigin.position, projectileOrigin.transform.forward, out hit, 250))
			{
				if (hit.transform.CompareTag(hitEffectTag))
				{
					//Uncomment for RFPS

					if (hit.collider.gameObject.GetComponent<FPSPlayer>() )
					{
						hit.collider.gameObject.GetComponent<FPSPlayer>().ApplyDamage(0.1f, transform);
					}
					FPSPlayer.instance.WeaponEffectsComponent.PlayImpactSountRandomy(hit.point);

				}
				else
				{
					FPSPlayer.instance.WeaponEffectsComponent.ImpactEffects(hit.collider, hit.point, true, false, hit.normal);

				}
				

			}

			// Fire projectile.

			if ((this.projectileOrigin != null))
				{
				

					if (animator)
						animator.SetBool("Shoot", true);
					
				}

				
			}

			// No bullets left!!

			

		}
	}


