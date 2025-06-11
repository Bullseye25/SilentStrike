using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    private void Awake()
    {
        RagDoll(false);
    }

    public void RagDoll(bool On)
    {
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = !On;
        }
    }
}
