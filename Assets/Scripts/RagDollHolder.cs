using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollHolder : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    public void DisableAnimator(Animator animator)
    {
        this.animator = animator;
        Invoke("Disbale",1.25f);
    }

    void Disbale()
    {
        Debug.Log("Animate");
        animator.enabled = false;
    }
}
