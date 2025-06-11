using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowmotion : MonoBehaviour
{
    public float time;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Time.timeScale = time;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
