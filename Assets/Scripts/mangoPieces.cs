using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mangoPieces : MonoBehaviour
{
    public GameObject[] pieces;

    private void OnEnable()
    {
        foreach (GameObject child in pieces)
        {
            if (child.GetComponent<Rigidbody>())
            {
                child.GetComponent<Rigidbody>().isKinematic = false;
                child.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-300,400), Random.Range(-300, 400), Random.Range(-300, 400)));
            }
            
        }
    }
}
