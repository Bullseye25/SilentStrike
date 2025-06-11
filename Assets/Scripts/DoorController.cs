using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }


    public void DoorMechanism(DoorStates State)
    {
        switch(State)
        {
            case DoorStates.DoorClose:
                StartCoroutine(DoorIsClose());
              //  myAnimator.Play("DoorClose");
                break;

            case DoorStates.DoorOpen:
                myAnimator.Play("DoorOpen");
                break;
            case DoorStates.DoorDestruction:
                myAnimator.Play("DoorDestruction");
                break;
        }
    }

    IEnumerator DoorIsClose()
    {
        yield return new WaitForSeconds(4f);
        myAnimator.Play("DoorClose");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public enum DoorStates
    {
        DoorOpen,
        DoorClose,
        DoorDestruction
    }
}
