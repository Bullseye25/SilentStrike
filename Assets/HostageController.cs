using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostageController : MonoBehaviour
{
    public int speed;
    [HideInInspector] public Transform _target;
    bool run;
    public void StartRunning(Transform target)
    {
        run = true;
        _target = target;
        GetComponent<Animator>().SetBool("Run", true);

    }

    // Update is called once per frame
    void Update()
    {
        if (run)
        {
            float step = speed * Time.deltaTime;
            Vector3 newpos = Vector3.MoveTowards(transform.position, _target.position, step);
            transform.position = newpos;
        }
    }
}
