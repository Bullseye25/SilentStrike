using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShot : MonoBehaviour
{
    private Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.Alpha1))
            m_Animator.Play("Gun_Firing_Rifle", 0, 0.0f);

     if (Input.GetKeyDown(KeyCode.Alpha2))
            m_Animator.Play("Gun_Firing_Shotgun", 0, 0.0f);

     if (Input.GetKeyDown(KeyCode.Alpha3))
            m_Animator.Play("Gun_Firing_GranageLaunsher", 0, 0.0f);

     if (Input.GetKeyDown(KeyCode.Alpha4))
            m_Animator.Play("Gun_Combined_SFX", 0, 0.0f);



    }
}