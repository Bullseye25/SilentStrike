using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinWaveRotation : MonoBehaviour
{
    public float speed = 2f;
    public float maxRotation = 45f;
    public Vector3 enableVector;
    void Update()
    {
        transform.localRotation = Quaternion.Euler(maxRotation * Mathf.Sin(Time.time * speed) * enableVector.x, maxRotation * Mathf.Sin(Time.time * speed) * enableVector.y, maxRotation * Mathf.Sin(Time.time * speed)*enableVector.z);
       // transform.RotateAround(point.position, axiis,maxRotation);
    }
}
