using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrgansCameraController : MonoBehaviour
{
    public Camera mainCamera;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
           _camera.fieldOfView = mainCamera.fieldOfView;
        }
    }
}
