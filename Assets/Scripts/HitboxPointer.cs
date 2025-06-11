using UnityEngine;

public class HitboxPointer : MonoBehaviour
{
    void Update()
    {
        if (!(Camera.main is null))
            transform.LookAt(Camera.main.transform);
    }
}
