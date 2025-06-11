using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleCTRL : MonoBehaviour
{
    public MeshRenderer muzzle;
    public GameObject Gun;
    public void EnableMuzzli()
    {
        muzzle.enabled = true;
    }
    public void DisableMuzzli()
    {
        muzzle.enabled = false;
    }

    public void DetachWeapon()
    {
        muzzle.enabled = false;
        Gun.transform.parent = null;
        Gun.AddComponent<Rigidbody>();
        Gun.gameObject.GetComponent<BoxCollider>().enabled=true;
    }
    public void GunIdlePosition()
    {
        Gun.transform.localPosition = new Vector3(-0.037f, 0.157f, 0.033f);
        Gun.transform.localRotation = Quaternion.Euler(-62.816f, -57.867f, 115.783f);
      
    }
    
         public void GunShootPosition()
    {
        Gun.transform.localPosition = new Vector3(-0.019f, 0.168f, 0.05f);
        Gun.transform.localRotation = Quaternion.Euler(-63.272f, -96.739f, 174.416f);

    }
}
