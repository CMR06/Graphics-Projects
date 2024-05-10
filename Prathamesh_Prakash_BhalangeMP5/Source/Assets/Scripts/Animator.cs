using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator : MonoBehaviour
{
    public Vector3 Axis = Vector3.left;
    public Vector3 Direction = Vector3.forward;

    private Vector3 initialPos;

    public float TargetDist = 0.25f;
    public float DeltaMove = 0.25f;
    private float dist = 0;


    public float TargetDegree = 60f;
    public float DeltaDegree = -60f;
    private float degree = 0f;

    void Start()
    {
        initialPos = transform.localPosition;
    }

    void Update()
    {
        degree += DeltaDegree * Time.deltaTime;
        transform.localRotation = Quaternion.AngleAxis(degree, Axis);
        if ((degree > TargetDegree) || (degree < (-TargetDegree)))
        {
            DeltaDegree *= -1f;
            degree += DeltaDegree * Time.deltaTime; // ensure we don't get stuck
        }

        // translation
        dist += DeltaMove * Time.deltaTime;
        Vector3 p = initialPos + (Direction * dist);
        transform.localPosition = p;
        if ((dist > TargetDist) || (dist < (-TargetDist)))
        {
            DeltaMove *= -1f;
            dist += DeltaMove * Time.deltaTime;
        }
    }
}
