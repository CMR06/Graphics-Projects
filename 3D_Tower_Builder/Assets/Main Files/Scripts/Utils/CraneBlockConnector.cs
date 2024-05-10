using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CraneBlockConnector : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, StartPoint.position);
        lineRenderer.SetPosition(1, EndPoint.position);
    }

    public void ShowConnector()
    {
        lineRenderer.enabled = true;
    }

    public void HideConnector()
    {
        lineRenderer.enabled = false;
    }
}