using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPositions : MonoBehaviour
{
    public bool isCrouchPosition;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, 1);
    }
}
