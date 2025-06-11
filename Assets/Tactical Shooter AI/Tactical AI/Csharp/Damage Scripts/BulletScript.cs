using UnityEngine;
using System.Collections;

/*
 * Moves an object until it hits a target, at which time it calls the Damage(float) method on all scripts on the hit object
 * Can also home in on targets and "detonate" when in close proximity.
 * */

namespace TacticalAI
{
    public class BulletScript : MonoBehaviour
    {

		public string damageMethodName = "Damage";

        public float speed = 1000;
        public LayerMask layerMask;
        public float maxLifeTime = 3;

        //Bullet Stuff
        public float damage = 16;

        //use for shotguns
        public float bulletForce = 100;

        //Hit Effects
        public GameObject hitEffect;
        public float hitEffectDestroyTime = 1;
        public string hitEffectTag = "HitBox";
        public GameObject missEffect;
        public float missEffectDestroyTime = 1;
        public float timeToDestroyAfterHitting = 0.5f;
        private RaycastHit hit;
        private Transform myTransform;
        [HideInInspector] public Transform Sender = null;

        //Rotation
        private Quaternion targetRotation;

        private Transform target = null;
        public float homingTrackingSpeed = 3;

        public float minDistToDetonate = 0.5f;
        private float minDistToDetonateSqr;

        public string friendlyTag;

        float startTime;
        void Awake()
        {
            myTransform = transform;
            //Move();
            //StartCoroutine(SetTimeToDestroy());
        }

        private void OnEnable()
        {
            startTime = Time.time;
        }
        //Automatically destroy the bullet after a certain amount of time
        //Especially useful for missiles, which may end up flying endless circles around their target,
        //long after the appropriate sound effects have ended.
        IEnumerator SetTimeToDestroy()
        {
            yield return new WaitForSeconds(maxLifeTime);

            if (target)
            {
                //Instantiate(hitEffect, myTransform.position, myTransform.rotation);
            }

            AzuObjectPool.instance.RecyclePooledObj(20, gameObject);
            //Destroy(gameObject);
        }

        IEnumerator ApplyDamage()
        {
            //Reduce the enemy's health
            //Does NOT travel up the heirarchy.  
            if (hit.transform.tag != friendlyTag)
            {
                //Uncomment for RFPS

                if (hit.collider.gameObject.GetComponent<FPSPlayer>())
                {
                    hit.collider.gameObject.GetComponent<FPSPlayer>().ApplyDamage(damage,Sender);
                }


                hit.collider.SendMessage(damageMethodName, damage, SendMessageOptions.DontRequireReceiver);
            }
           // Debug.Log("hit.collider.gameObject" + hit.collider.gameObject.tag + "    lkl "+ hit.collider.gameObject.CompareTag(hitEffectTag) + " "+ hitEffectTag);

            //Produce the appropriate special effect
            if (hit.collider.gameObject.CompareTag( hitEffectTag) && hitEffect)
            {
                FPSPlayer.instance.WeaponEffectsComponent.PlayImpactSountRandomy(hit.point);
                //GameObject currentHitEffect = (GameObject)(Instantiate(hitEffect, hit.point, myTransform.rotation));
                //  GameObject.Destroy(currentHitEffect, hitEffectDestroyTime);
            }
            else if (missEffect)
            {
                FPSPlayer.instance.WeaponEffectsComponent.ImpactEffects(hit.collider, hit.point, true, false, hit.normal);
               // SoundManager.Instance.PlayMisHitEffect();
                 //GameObject currentMissEffect = (GameObject)(Instantiate(missEffect, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal)));
                // GameObject.Destroy(currentMissEffect, missEffectDestroyTime);
            }


            yield return null;

            //Wait a fram to apply forces as we need to make sure the thing is dead
            if (hit.rigidbody)
                hit.rigidbody.AddForceAtPosition(myTransform.forward * bulletForce, hit.point, ForceMode.Impulse);

            //Linger around for a while to let the trail renderer dissipate (if the bullet has one.)
            // Destroy(gameObject, timeToDestroyAfterHitting);
            AzuObjectPool.instance.RecyclePooledObj(21, gameObject);

        }

        // Update is called once per frame
        void Update()
        {
            Move();
            if (Time.time>startTime+maxLifeTime)
            {
                AzuObjectPool.instance.RecyclePooledObj(21, gameObject);
            }
        }

        //Move is a seperate method as the bullet must move IMMEDIATELY.
        //If it does not, the shooter may literally outrun the bullet-momentarily.
        //If the shooter passes the bullet, the bullet will then start moving and hit the shooter in the back.
        //That would not be good.
        void Move()
        {
            //Check to see if we're going to hit anything.  If so, move right to it and deal damage
            if (Physics.Raycast(myTransform.position, myTransform.forward, out hit, speed * Time.deltaTime, layerMask.value))
            {
                myTransform.position = hit.point;
                StartCoroutine(ApplyDamage());
            }
            else
            {
                //Move the bullet forwards
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }

            //Home in on the target
            if (target != null)
            {
                //Firgure out the rotation required to move directly towards the target
                targetRotation = Quaternion.LookRotation(target.position - transform.position);
                //Smoothly rotate to face the target over several frames.  The slower the rotation, the easier it is to dodg.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, homingTrackingSpeed * Time.deltaTime);

                Debug.DrawRay(transform.position, (target.position - transform.position).normalized * minDistToDetonate,Color.red);
                //Projectile will "detonate" upon getting close enough to the target..
                if (Vector3.SqrMagnitude(target.position - transform.position) < minDistToDetonateSqr)
                {
                    AzuObjectPool.instance.RecyclePooledObj(21, gameObject);

                    //The hitEffect should be your explosion.
                    //  Instantiate(hitEffect, myTransform.position, myTransform.rotation);
                    // GameObject.Destroy(gameObject);
                }
            }
        }

        //To avoid costly SqrRoot in Vector3.Distance
        public void SetDistToDetonate(float x)
        {
            minDistToDetonateSqr = x * x;
        }

        //Call this method upon instantating the bullet in order to make it home in on a target.
        public void SetAsHoming(Transform t)
        {
            target = t;
            SetDistToDetonate(minDistToDetonate);  
        }
    }
}
