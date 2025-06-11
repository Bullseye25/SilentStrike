using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedLineRenderer : MonoBehaviour
{
    public float drawSpeed;
    public bool animated;

    private Transform _origin, _destination;
    private bool _startAnimation;
    private LineRenderer _lineRenderer;
    private float _dist, _counter, _orgOffset, _destOffset;
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetPosition(0, _origin.position);
        _dist = Vector3.Distance(_origin.position, _destination.position);}

    public void SetLinePoints(Transform A, float orgOffset, Transform B, float destOffset)
    {
        _origin = A;
        _destination = B;
        _orgOffset = orgOffset;
        _destOffset = destOffset;

        if (animated)
            _startAnimation = true;
        else
            GetComponent<LineRenderer>().SetPosition(1, _destination.position + (Vector3.up * _destOffset));
    }

    void Update()
    {
        if (!_startAnimation) return;
        
        if (_counter < _dist && (_origin && _destination))
        {
            _counter += 0.1f / drawSpeed;

            float x = Mathf.Lerp(0, _dist, _counter);

            Vector3 pointA = _origin.position + (Vector3.up * _orgOffset);
            Vector3 pointB = _destination.position + (Vector3.up * _destOffset);
            
            // Get the unit direction in desired direction, multiply by the desired length and add the starting point
            Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

            _lineRenderer.SetPosition(1, pointAlongLine);
        }
    }
}
