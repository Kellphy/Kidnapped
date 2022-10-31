using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    // store ref to own line renderer 
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void UpdateBeam(Vector3 start, Vector3 end, float laserBeamWidth)
    {
        lineRenderer.startWidth = laserBeamWidth;
        lineRenderer.endWidth = laserBeamWidth;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
