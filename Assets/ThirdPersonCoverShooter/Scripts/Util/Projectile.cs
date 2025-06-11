using UnityEngine;

namespace CoverShooter
{
    /// <summary>
    /// An object that flies a distance and then destroys itself.
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// Speed of the projectile in meters per second.
        /// </summary>
        [Tooltip("Speed of the projectile in meters per second.")]
        public float Speed = 10;
        public TrailRenderer trail;
        //[HideInInspector]
        public float Distance = 500;

        [HideInInspector]
        public Vector3 Direction;

        [HideInInspector]
        public GameObject Target;

        [HideInInspector]
      //  public Hit Hit;

        private float _path = 0;

        private void OnEnable()
        {
           
            _path = 0;
          //  trail.Clear();
           // Invoke("ResetTrail",0.1f);
        }

        void ResetTrail()
        {
            trail.Clear();
        }
        private void Update()
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
            _path += Speed * Time.deltaTime;
            if (Physics.Raycast(this.transform.position,transform.forward, out RaycastHit hit, 0.3f))
            {

                if (hit.collider!=null)
                {
                    AzuObjectPool.instance.RecyclePooledObj(18, this.gameObject);
                }


            }
            if (_path >= Distance)
            {
                AzuObjectPool.instance.RecyclePooledObj(18, this.gameObject);


            }
        }
       
    }
}