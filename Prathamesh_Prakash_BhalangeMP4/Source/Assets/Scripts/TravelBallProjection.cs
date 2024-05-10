using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelBallProjection
{
    public TravelingBall TravelBall;

    public Transform LineEndPoint1;
    public Transform LineEndPoint2;

    public GameObject LineSegment;
    public Transform ProjectionBall;

    private Vector3 endPointsDistanceVector, ballNEndPointDistanceVector, vector;

    private float endPointsDistanceVectorLength, dot, distance;

    private bool isOnLineSegment;

    public TravelBallProjection(TravelingBall TravelBall, LineSegment lineSegmentToProject, Transform parent)
    {
        this.TravelBall = TravelBall;

        LineEndPoint1 = lineSegmentToProject.LineEndPoint1;
        LineEndPoint2 = lineSegmentToProject.LineEndPoint2;

        ProjectionBall = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        ProjectionBall.parent = parent;
        ProjectionBall.localScale = Vector3.one * 0.5f;

        LineSegment = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        LineSegment.transform.parent = parent;
        LineSegment.AddComponent<LineSegment>().Init(TravelBall.gameObject, ProjectionBall.gameObject, 0.01f, Color.black);

        Update();
    }

    public void Update()
    {
        if (LineEndPoint1 != null)
        {
            endPointsDistanceVector = LineEndPoint2.transform.position - LineEndPoint1.transform.position;
            endPointsDistanceVectorLength = endPointsDistanceVector.magnitude;

            ballNEndPointDistanceVector = TravelBall.transform.position - LineEndPoint1.transform.position;
            vector = (1f / endPointsDistanceVectorLength) * endPointsDistanceVector;

            dot = Vector3.Dot(ballNEndPointDistanceVector, vector);
            distance = Distance(TravelBall.transform.position, ProjectionBall.position);

            isOnLineSegment = dot > 0 && dot < endPointsDistanceVectorLength;
            ProjectionBall.gameObject.SetActive(isOnLineSegment && distance < 10);
            LineSegment.SetActive(isOnLineSegment && distance < 10);

            if (isOnLineSegment)
            {
                ProjectionBall.position = LineEndPoint1.transform.position + dot * vector;
                ProjectionBall.position += (TravelBall.transform.position - ProjectionBall.position).normalized;
            }
        }
        else
        {
            TravelBall.RemoveTravelBallProjection(this);
        }
    }

    private float Distance(Vector3 a, Vector3 b)
    {
        Vector3 vector = a - b;
        return vector.magnitude;
    }
}
