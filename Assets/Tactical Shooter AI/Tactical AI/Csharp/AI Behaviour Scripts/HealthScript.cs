﻿using UnityEngine;
using System.Collections;
using System;

/*
 * Manages the agent's health. 
 * Will trigger the suppresion state on agents using cover if shields are down.
 * */

namespace TacticalAI{
public class HealthScript : MonoBehaviour {

	public float health = 100;
	public float shields = 25;	
	private float maxShields = 10;	
	public bool shieldsBlockDamage = false;	
	public float timeBeforeShieldRegen = 5;
	private float currentTimeBeforeShieldRegen;
	public float shieldRegenRate = 10;	
	public TacticalAI.TargetScript myTargetScript;
	public TacticalAI.BaseScript myAIBaseScript;	
	public TacticalAI.SoundScript soundScript;
	
	public Rigidbody[] rigidbodies;
	public Collider[] collidersToEnable;
	public TacticalAI.RotateToAimGunScript rotateToAimGunScript;
	public Animator animator;
	
	public TacticalAI.GunScript gunScript;
		
	private bool beenHitYetThisFrame = false;

	public HealthElement healthElement;

	public bool isdead;

		//Initiation stuff.
		void Awake()
		{
			soundScript = gameObject.GetComponent<TacticalAI.SoundScript>();
            if (shields < 0)
            {
                shields = 0.1f;
            }
			maxShields = shields;
			healthElement = animator.gameObject.GetComponentInChildren<HealthElement>();
            if (healthElement )
            {
				healthElement.currentHealth = health;

			}
			//healthBar = animator.gameObject.GetComponentInChildren<EnemyHealthBar>();
			//         if (healthBar)
			//         {
			//	healthBar.SetHealthBar(this);
			//         }
		}

      
        void Update()
	{
		currentTimeBeforeShieldRegen -= Time.deltaTime;
        timeTillNextStagger -= Time.deltaTime;

         //Only let us take explosion damage once per frame. (could also be used for weapons that would pass through an agent's body)
         //This will prevent the agent from taking the damage multiple times- once for each hitbox.
         beenHitYetThisFrame = false;
		

		if(currentTimeBeforeShieldRegen < 0  && shields < maxShields)
			{
				shields = Mathf.Clamp(shields + shieldRegenRate*Time.deltaTime, 0, maxShields);

                //When our shields are fully charged, stop being suppressed.
                if (shields == maxShields)
                { 
				    myAIBaseScript.ShouldFireFromCover(true);
                }
			}
	}
	
	public void Damage(float damage, Vector3 directionArg)
		{	
            //Look for the source of the damage.
			if(myTargetScript)
				myTargetScript.CheckForLOSAwareness(true);	
		
			ReduceHealthAndShields(damage);
			myAIBaseScript.CheckToSeeIfWeShouldDodge();
				
			if(health <= 0)
				{
				    isdead = true;
					DeathCheck( directionArg);
				}	
		}
	
	public IEnumerator SingleHitBoxDamage(float damage)
		{
            //Look for the source of the damage.
			if(myTargetScript)
				myTargetScript.CheckForLOSAwareness(true);

            //Only let us take explosion damage once per frame. (could also be used for weapons that would pass through an agent's body)
            //This will prevent the agent from taking the damage multiple times- once for each hitbox.
			if(!beenHitYetThisFrame)
				{
					ReduceHealthAndShields(damage);
									
					if(health <= 0)
						{
							DeathCheck(Vector3.zero);
						}
					beenHitYetThisFrame = true;	
				}
					
			yield return null;
				beenHitYetThisFrame = false;
		}
	
	
	public void ReduceHealthAndShields(float damage)
		{
            //Shields are mandatory for the suppressioon mechanic to work.
            //However, as you may not want the agent to have any sort of regenerating health, you can choose whether or not they will actually block damage or merely work as a recent damage counter.
			if(shieldsBlockDamage)
				{
					if(damage > shields)
						{
							if(soundScript && myAIBaseScript.HaveCover() && shields > 0)
								soundScript.PlaySuppressedAudio();				
						
                            //Eliminate shields and pass on remaining damage to health.
							damage -= shields;
							shields = 0;
							health -= damage;
							
	                        //If the agent's shields go down, become suppressed (ie: agent will stay in cover as much as possible, and will avoid standing up to fire)
							myAIBaseScript.ShouldFireFromCover(false);								
						}
					else
						{
							shields -= damage;
						}
				}
			else
				{
					if(damage > shields)
						{
							if(soundScript && myAIBaseScript.HaveCover() && shields > 0)
								soundScript.PlaySuppressedAudio();

                            //If the agent's shields go down, become suppressed (ie: agent will stay in cover as much as possible, and will avoid standing up to fire)
							myAIBaseScript.ShouldFireFromCover(false);
						}	
				    
	                //Do the same amount of damage to shields AND health.
					shields = Mathf.Max(shields-damage, 0);
					health -= damage;										
				}
					
			currentTimeBeforeShieldRegen = timeBeforeShieldRegen;

			if (healthElement != null)
			{
				healthElement.OnHealthChanged(health);
			}

		
			//Sound
			if(soundScript)
				soundScript.PlayDamagedAudio();

            if (health > 0 && damage > staggerThreshhold && canStagger && UnityEngine.Random.value < staggerOdds && timeTillNextStagger < 0)
            {
                myAIBaseScript.StaggerAgent();
                timeTillNextStagger = timeBetweenNextStaggers;
            }
		}

    public float staggerThreshhold = 1.0f;
    public bool canStagger = false;
    public float staggerOdds = 0.5f;
    private float timeTillNextStagger = 1.0f;
    public float timeBetweenNextStaggers = 1.0f;

    //Check to see if we are dead.
   void DeathCheck(Vector3 directionArg)
		{
            

			if (healthElement != null)
			{
				healthElement.OnDie();
			}
			KillAI( directionArg);
		
			if(myAIBaseScript)
				myAIBaseScript.KillAI();
			this.enabled = false;
		}
	
	void KillAI(Vector3 directionArg)
		{
			//Check if we've done this before
			if(this.enabled)
				{
                int i;

                //Enable the ragdoll
                for (i = 0; i < rigidbodies.Length; i++)
                {
                    rigidbodies[i].isKinematic = false;
                }

                Rigidbody hip = animator.GetBoneTransform(HumanBodyBones.Chest).GetComponent<Rigidbody>();
                directionArg.y = 1.5f;
                for (i = 0; i < collidersToEnable.Length; i++)
                {
                    collidersToEnable[i].enabled = true;
                }
				for (i = 0; i < rigidbodies.Length; i++)
				{
					rigidbodies[i].drag = 5;
				}
				if (myAIBaseScript.cWave && myAIBaseScript.cWave.IsSlomoTime())
                {
					RagDollHolder ragDollHolder = animator.transform.gameObject.AddComponent<RagDollHolder>();
					ragDollHolder.DisableAnimator(animator);
					
					//animator.SetTrigger("IsDead");
					animator.SetTrigger("DeadT");
                }
                else
                {
                    if (animator)
                        animator.enabled = false;
					hip.AddForce(directionArg * 5, ForceMode.VelocityChange);

				}



				//Disable scripts
				if (rotateToAimGunScript)
						rotateToAimGunScript.enabled = false;		
						
							
											
					if(gunScript)
						{
							gunScript.enabled = false;
						}
							
					this.enabled = false;
				}
		}
}
}
