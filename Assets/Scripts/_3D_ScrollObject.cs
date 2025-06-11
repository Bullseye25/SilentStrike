using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _3D_ScrollObject : MonoBehaviour
{
    RectTransform RectTransformThis;
    Camera mainCam;
    void Start()
    {
        RectTransformThis = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(RectTransformThis,Vector2.zero))
        {
            Debug.Log("Here It Is");
        }
    }
}
