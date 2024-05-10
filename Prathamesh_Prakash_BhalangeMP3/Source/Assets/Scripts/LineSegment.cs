using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegment : MonoBehaviour
{

    private Transform lineEndPoint1;
    private Transform lineEndPoint2;

    private float cylinderSize;


    public void Init(GameObject lineEndPt1, GameObject lineEndPt2, float cylinderSize, Color color)
    {
        lineEndPoint1 = lineEndPt1.transform;
        lineEndPoint2 = lineEndPt2.transform;
        this.cylinderSize = cylinderSize;

        float distance = Distance(lineEndPoint1.position, lineEndPoint2.position) / 2;
        transform.localScale = new Vector3(cylinderSize, distance, cylinderSize);
        GetComponent<Renderer>().material.color = color;
        UpdatePositionAndRotation();
    }

    private void Update()
    {
        UpdatePositionAndRotation();
    }

    public void SpawnTravelBall(float speed, float aliveSeconds, Color color)
    {
        TravelingBall travelingBall = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<TravelingBall>();
        travelingBall.transform.position = lineEndPoint1.position;
        travelingBall.Init(lineEndPoint2, speed, aliveSeconds, color);
        travelingBall.transform.parent = transform.parent.parent;
    }

    public void DestroyLineSegment()
    {
        Destroy(transform.parent.gameObject);
    }

    private void UpdatePositionAndRotation()
    {
        float distance = Distance(lineEndPoint1.position, lineEndPoint2.position) / 2;
        transform.localScale = new Vector3(cylinderSize, distance, cylinderSize);

        transform.position = (lineEndPoint1.position + lineEndPoint2.position) / 2;

        LookAt(lineEndPoint1);
    }

    private void LookAt(Transform target)
    {
        Vector3 forward = (target.position - transform.position).normalized;
        transform.up = forward;
    }

    private float Distance(Vector3 a, Vector3 b)
    {
        Vector3 vector = new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
    }
}
