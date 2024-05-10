using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public float Interval = 1;
    public float Speed = 6;
    public float AliveSeconds = 10;

    public Color TravelBallColor = Color.cyan;

    public List<LineSegment> LineSegments = new List<LineSegment>();

    private int pointLineSpawned = 0;

    private float previousSpawnedTime = 0;

    private void Update()
    {
        if (Time.time - previousSpawnedTime >= Interval)
        {
            foreach (LineSegment lineSegment in LineSegments)
            {
                lineSegment.SpawnTravelBall(Speed, AliveSeconds, TravelBallColor);
            }
            previousSpawnedTime = Time.time;
        }
    }

    public GameObject SpawnPointLine(Vector3 hitPoint, Vector3 anotherPoint, float cylinderSize, Color color, bool isSmall = false)
    {
        GameObject pointLineParent = new GameObject("LinePointParent : " + ++pointLineSpawned);
        pointLineParent.transform.parent = transform;
        GameObject[] lineEndPts = new GameObject[2];
        GameObject lineSegment = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        lineSegment.transform.parent = pointLineParent.transform;
        for (int i = 0; i < 2; i++)
        {
            lineEndPts[i] = SpawnEndPoint(hitPoint, pointLineParent.transform);
        }
        lineEndPts[1].transform.position = anotherPoint;
        lineSegment.AddComponent<LineSegment>().Init(lineEndPts[0], lineEndPts[1], cylinderSize, color);
        lineSegment.layer = Controller.LINE_SEGMENT_LAYER;
        if (isSmall)
        {
            LineSegments.Add(lineSegment.GetComponent<LineSegment>());
        }
        return lineEndPts[0];
    }

    private GameObject SpawnEndPoint(Vector3 hitPoint, Transform parent)
    {
        GameObject lineEndPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        lineEndPoint.transform.parent = parent;
        lineEndPoint.GetComponent<Renderer>().material.color = Color.red;
        lineEndPoint.transform.position = hitPoint;
        lineEndPoint.transform.localScale = Vector3.one * 2;
        lineEndPoint.layer = Controller.LINE_ENDPOINT_LAYER;

        return lineEndPoint;
    }
}
