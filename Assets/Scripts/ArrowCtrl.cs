using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.Rotate(Vector3.up * 100 * Time.deltaTime);
    }
   


}
