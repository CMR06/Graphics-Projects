using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegmentReflection
{
    public LineSegment LineSegment;

    public Transform Barrier;

    public Transform LineEndPoint1;
    public Transform LineEndPoint2;

    public Transform ReflectionObject;

    public float cylinderSize;

    private Vector3 barrierForward, endPointDistanceVector, pointOfInteract, endPointsDirection, reflectedDirection, reflectedEndPoint;

    private float barrierDot, denom = 0, d, interactPtNBarrierdistance, distance, barrierRadius;

    bool isLineNotParallelToPlane;

    public LineSegmentReflection(LineSegment lineSegment, Transform barrier, Transform parent)
    {
        LineSegment = lineSegment;
        Barrier = barrier;

        barrierRadius = barrier.localScale.x / 2;

        LineEndPoint1 = lineSegment.LineEndPoint1;
        LineEndPoint2 = lineSegment.LineEndPoint2;
        cylinderSize = lineSegment.cylinderSize;

        ReflectionObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder).transform;
        ReflectionObject.parent = parent;
        ReflectionObject.GetComponent<Renderer>().material.color = Color.green;
        ReflectionObject.localScale = new Vector3(cylinderSize, 1, cylinderSize);
        //ReflectionObject.gameObject.SetActive(false);

        Update();
    }

    public void Update()
    {
        if (Barrier != null)
        {
            barrierForward = (-Barrier.forward).normalized;
            barrierDot = Vector3.Dot(Barrier.position, -Barrier.forward);

            if (Vector3.Dot(LineEndPoint1.position, barrierForward) - barrierDot > 0)
            {
                SetReflectionOf(LineEndPoint1.position, LineEndPoint2.position);
            }
            else
            {
                SetReflectionOf(LineEndPoint2.position, LineEndPoint1.position);
            }
        }
        else
        {
            LineSegment.RemoveLineSegmentReflection(this);
        }
    }

    private void SetReflectionOf(Vector3 from, Vector3 to)
    {
        endPointDistanceVector = to - from;

        if (endPointDistanceVector.magnitude < float.Epsilon)
        {
            Debug.Log("Ill defined line (magnitude of zero). Not processed");
            return;
        }

        denom = Vector3.Dot(barrierForward, endPointDistanceVector);
        d = 0;

        d = (barrierDot - (Vector3.Dot(barrierForward, from))) / denom;
        pointOfInteract = from + d * endPointDistanceVector;

        interactPtNBarrierdistance = Distance(pointOfInteract, Barrier.position);

        isLineNotParallelToPlane = (Mathf.Abs(denom) > float.Epsilon) && interactPtNBarrierdistance < barrierRadius;

        if (isLineNotParallelToPlane)
        {

            distance = Distance(pointOfInteract, from);
            endPointsDirection = (to - from).normalized;
            reflectedDirection = GetReflectionDirection(endPointsDirection, barrierForward);

            ReflectionObject.up = reflectedDirection;
            ReflectionObject.position = pointOfInteract + reflectedDirection * (distance / 2);
            ReflectionObject.localScale = new Vector3(cylinderSize, distance / 2, cylinderSize);
        }
        ReflectionObject.gameObject.SetActive(isLineNotParallelToPlane);
    }

    private Vector3 GetReflectionDirection(Vector3 inDirection, Vector3 inNormal)
    {
        return -2F * Vector3.Dot(inNormal, inDirection) * inNormal + inDirection;
    }

    private float Distance(Vector3 a, Vector3 b)
    {
        Vector3 vector = a - b;
        return vector.magnitude;
    }

}
