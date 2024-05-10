using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegment : MonoBehaviour
{
    public Transform LineEndPoint1;
    public Transform LineEndPoint2;

    public float cylinderSize;

    private List<LineSegmentReflection> lineSegmentReflectionList = new List<LineSegmentReflection>();


    public void Init(GameObject lineEndPt1, GameObject lineEndPt2, float cylinderSize, Color color)
    {
        LineEndPoint1 = lineEndPt1.transform;
        LineEndPoint2 = lineEndPt2.transform;
        this.cylinderSize = cylinderSize;

        float distance = Distance(LineEndPoint1.position, LineEndPoint2.position) / 2;
        transform.localScale = new Vector3(cylinderSize, distance, cylinderSize);
        GetComponent<Renderer>().material.color = color;
        UpdatePositionAndRotation();
    }

    private void Update()
    {
        UpdatePositionAndRotation();

        for (int i = 0; i < lineSegmentReflectionList.Count; i++)
        {
            lineSegmentReflectionList[i].Update();
        }
    }

    public TravelingBall SpawnTravelBall(List<GameObject> barriers, List<LineSegment> bigLineSegments, float speed, float aliveSeconds, Color color)
    {
        TravelingBall travelingBall = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<TravelingBall>();
        travelingBall.transform.position = LineEndPoint1.position;
        travelingBall.transform.parent = transform.parent.parent;
        travelingBall.Init(barriers, bigLineSegments, LineEndPoint2, speed, aliveSeconds, color);
        travelingBall.gameObject.layer = 11;

        return travelingBall;
    }

    public void CreateLineSegmentReflection(Transform barrier)
    {
        LineSegmentReflection lineSegmentReflection = new LineSegmentReflection(this, barrier, SpawnManager.Instance.transform);
        Destroy(lineSegmentReflection.ReflectionObject.GetComponent<Collider>());
        lineSegmentReflectionList.Add(lineSegmentReflection);
    }

    public void RemoveLineSegmentReflection(LineSegmentReflection lineSegmentReflection)
    {
        Destroy(lineSegmentReflection.ReflectionObject.gameObject);
        lineSegmentReflectionList.Remove(lineSegmentReflection);
    }

    public void DestroyLineSegmentAndPoints()
    {
        for (int i = lineSegmentReflectionList.Count - 1; i >= 0; i--)
        {
            RemoveLineSegmentReflection(lineSegmentReflectionList[i]);
        }
        Destroy(transform.parent.gameObject);
    }

    public void DestroyLineSegment()
    {
        Destroy(gameObject);
    }

    private void UpdatePositionAndRotation()
    {
        float distance = Distance(LineEndPoint1.position, LineEndPoint2.position) / 2;
        transform.localScale = new Vector3(cylinderSize, distance, cylinderSize);

        transform.position = (LineEndPoint1.position + LineEndPoint2.position) / 2;

        LookAt(LineEndPoint1);
    }

    private void LookAt(Transform target)
    {
        Vector3 forward = (target.position - transform.position).normalized;
        transform.up = forward;
    }

    private float Distance(Vector3 a, Vector3 b)
    {
        Vector3 vector = a - b;
        return vector.magnitude;
    }
}
