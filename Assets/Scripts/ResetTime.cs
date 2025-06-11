using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTime : MonoBehaviour
{
    public void ResetTimeScale()
    {
        Time.timeScale = 1.5f;
    }

    public void ResetTimeScale2()
    {
        Time.timeScale = 1f;
    }
}