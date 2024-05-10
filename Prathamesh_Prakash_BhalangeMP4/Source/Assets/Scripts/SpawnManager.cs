using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public GameObject BarrierPrefab;

    public float Interval = 1;
    public float Speed = 6;
    public float AliveSeconds = 10;

    public Color TravelBallColor = Color.cyan;

    public List<LineSegment> LineSegmentList = new List<LineSegment>();
    public List<LineSegment> SmallLineSegments = new List<LineSegment>();
    public List<LineSegment> BigLineSegments = new List<LineSegment>();

    public List<GameObject> barrierList = new List<GameObject>();
    public List<TravelingBall> travelingBallList = new List<TravelingBall>();

    private int pointLineSpawned = 0;
    private int barrierSpawnCount = 0;

    private float previousSpawnedTime = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Time.time - previousSpawnedTime >= Interval)
        {
            foreach (LineSegment lineSegment in SmallLineSegments)
            {
                travelingBallList.Add(lineSegment.SpawnTravelBall(barrierList, BigLineSegments, Speed, AliveSeconds, TravelBallColor));
            }
            previousSpawnedTime = Time.time;
        }
    }

    public GameObject SpawnBarrier(Vector3 hitPoint)
    {
        GameObject barrier = Instantiate(BarrierPrefab, transform);
        barrier.GetComponent<Barrier>().Init();
        hitPoint.y = 6;
        barrier.transform.position = hitPoint;
        barrier.name = "Barrier:" + barrierSpawnCount++;
        barrierList.Add(barrier);

        foreach (TravelingBall travelingBall in travelingBallList)
        {
            travelingBall.CreateTravelBallShadow(barrier.transform);
        }

        foreach (LineSegment lineSegment in LineSegmentList)
        {
            lineSegment.CreateLineSegmentReflection(barrier.transform);
        }

        return barrier;
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

        LineSegment lineSegmentObj = lineSegment.GetComponent<LineSegment>();

        if (isSmall)
            SmallLineSegments.Add(lineSegmentObj);
        else
        {
            BigLineSegments.Add(lineSegmentObj);
            foreach (TravelingBall travelingBall in travelingBallList)
            {
                travelingBall.CreateTravelBallProjection(lineSegment.GetComponent<LineSegment>());
            }
        }
        foreach (GameObject barrier in barrierList)
        {
            lineSegmentObj.CreateLineSegmentReflection(barrier.transform);
        }
        LineSegmentList.Add(lineSegmentObj);
        

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

    public void RemoveLineSegment(LineSegment lineSegment)
    {
        if (LineSegmentList.Contains(lineSegment))
        {
            LineSegmentList.Remove(lineSegment);
        }
    }

    public void RemoveSmallLineSegment(LineSegment lineSegment)
    {
        if (SmallLineSegments.Contains(lineSegment))
        {
            SmallLineSegments.Remove(lineSegment);
        }
    }

    public void RemoveBigLineSegment(LineSegment lineSegment)
    {
        if (BigLineSegments.Contains(lineSegment))
        {
            BigLineSegments.Remove(lineSegment);
        }
    }

    public void DestroyBarrier(GameObject barrier)
    {
        if (barrierList.Contains(barrier))
        {
            barrierList.Remove(barrier);
            Destroy(barrier);
        }
    }

    public void RemoveTravelingBall(TravelingBall travelingBall)
    {
        if (travelingBallList.Contains(travelingBall))
        {
            travelingBallList.Remove(travelingBall);
        }
    }
}
