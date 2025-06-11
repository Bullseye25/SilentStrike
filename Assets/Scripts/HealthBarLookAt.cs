using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarLookAt : MonoBehaviour
{
    public Image fillBar;
    // public Transform target;
    void Update()
    {
        // if (gameObject.activeSelf && target != null)
        //     transform.rotation = Quaternion.LookRotation(transform.position - /*Camera.main.transform*/target.position);

        if (!MConstants.IslastBullet && gameObject.activeSelf)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}
