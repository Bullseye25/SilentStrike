using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticleCollision : MonoBehaviour
{
    public ParticleSystem particleSystem;
    List<ParticleCollisionEvent> particleCollisionEvents = new List<ParticleCollisionEvent>();

    // Start is called before the first frame update
    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particleSystem.GetCollisionEvents(other, particleCollisionEvents);

        for (int i=0;i<numCollisionEvents;i++)
        {
            Debug.Log("Collision");
        }
    }

}
