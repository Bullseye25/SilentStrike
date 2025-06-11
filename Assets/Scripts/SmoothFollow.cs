using UnityEngine;
public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 3.0f;
    public float height = 3.0f;
    public float damping = 5.0f;
    private void Start()
    {
        target = FPSPlayer.instance.gameObject.transform;

    }
    void Update()
    {
        if (target!=null)
        {
            Vector3 wantedPosition = target.TransformPoint(0, height, -distance);
            transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);
        }
        
    }
}