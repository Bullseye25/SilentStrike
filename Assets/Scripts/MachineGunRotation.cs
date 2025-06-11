using System;
using UnityEngine;

public class MachineGunRotation : MonoBehaviour
{
    //public Vector3andSpace moveUnitsPerSecond;
    //public Vector3andSpace rotateDegreesPerSecond;
    public float speedForRotation;
    public bool ignoreTimescale;
    private float m_LastRealTime;
    // Start is called before the first frame update
   public void Start()
    {
      //  moveUnitsPerSecond = (, 0, 0);
        m_LastRealTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
   public void Update()
    {
        //float deltaTime = Time.deltaTime;
        //if (ignoreTimescale)
        //{
        //    deltaTime = (Time.realtimeSinceStartup - m_LastRealTime);
        //    m_LastRealTime = Time.realtimeSinceStartup;
        //}

        //if (speedForRotation == 0.0f)
        //{
        //    transform.Rotate(rotateDegreesPerSecond.value * deltaTime, moveUnitsPerSecond.space);
        //}
        //else
        //{
        //    transform.Rotate(rotateDegreesPerSecond.value * deltaTime * speedForRotation, moveUnitsPerSecond.space);
        //}
    }

    //[Serializable]
    //public class Vector3andSpace
    //{
    //    public Vector3 value;
    //    public Space space = Space.Self;
    //}

}

