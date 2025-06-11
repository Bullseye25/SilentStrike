using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageRelease : MonoBehaviour
{
    public Animator anim;

    public void Release()
    {
        anim.SetBool("release", true);
    }
}
