using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelingBall : MonoBehaviour
{
    private List<TravelBallShadow> shadowList = new List<TravelBallShadow>();
    private List<TravelBallProjection> projectionList = new List<TravelBallProjection>();
    private Vector3 direction;

    private float speed;
    private float distance;

    public void Init(List<GameObject> barriers, List<LineSegment> bigLineSegments, Transform target, float speed, float aliveSeconds, Color color)
    {
        this.speed = speed;
        direction = (target.position - transform.position).normalized;
        GetComponent<Renderer>().material.color = color;

        foreach (GameObject barrier in barriers)
        {
            CreateTravelBallShadow(barrier.transform);
        }

        foreach (LineSegment lineSegment in bigLineSegments)
        {
            CreateTravelBallProjection(lineSegment);
        }

        StartCoroutine(DestroyTravelBall(aliveSeconds));
    }

    public void CreateTravelBallShadow(Transform Barrier)
    {
        TravelBallShadow shadowData = new TravelBallShadow(this, Barrier, transform.parent);
        Destroy(shadowData.LineSegment.GetComponent<Collider>());
        shadowList.Add(shadowData);
    }

    public void RemoveTravelBallShadow(TravelBallShadow shadowData)
    {
        Destroy(shadowData.ShadowObject.gameObject);
        shadowData.LineSegment.DestroyLineSegment();
        shadowList.Remove(shadowData);
    }

    public void CreateTravelBallProjection(LineSegment lineSegment)
    {
        TravelBallProjection projectionData = new TravelBallProjection(this, lineSegment, transform.parent);
        Destroy(projectionData.LineSegment.GetComponent<Collider>());
        projectionList.Add(projectionData);
    }

    public void RemoveTravelBallProjection(TravelBallProjection projectionData)
    {
        Destroy(projectionData.ProjectionBall.gameObject);
        projectionData.LineSegment.GetComponent<LineSegment>().DestroyLineSegment();
        projectionList.Remove(projectionData);
    }

    void Update()
    {
        CheckBarrierInteraction();

        transform.position += direction * (speed * Time.deltaTime);

        for (int i = 0; i < shadowList.Count; i++)
        {
            shadowList[i].Update();
        }

        for (int i = 0; i < projectionList.Count; i++)
        {
            projectionList[i].Update();
        }
    }

    private void CheckBarrierInteraction()
    {
        foreach (TravelBallShadow shadowData in shadowList)
        {
            distance = Distance(transform.position, shadowData.ShadowObject.position);
            if (distance <= 0.5f)
            {
                if (shadowData.CanReflect())
                {
                    direction = GetReflectionDirection(direction, shadowData.ShadowObject.forward);
                    break;
                }
                else if (!shadowData.isForceFrontFalse)
                {
                    StartCoroutine(shadowData.IForceReflectFalse());
                }
            }
        }
    }

    private Vector3 GetReflectionDirection(Vector3 inDirection, Vector3 inNormal)
    {
        return -2F * Vector3.Dot(inNormal, inDirection) * inNormal + inDirection;
    }

    IEnumerator DestroyTravelBall(float aliveSeconds)
    {
        yield return new WaitForSeconds(aliveSeconds);
        for (int i = shadowList.Count - 1; i >= 0; i--)
        {
            RemoveTravelBallShadow(shadowList[i]);
        }
        for (int i = projectionList.Count - 1; i >= 0; i--)
        {
            RemoveTravelBallProjection(projectionList[i]);
        }
        SpawnManager.Instance.RemoveTravelingBall(this);
        Destroy(gameObject);
    }

    private float Distance(Vector3 a, Vector3 b)
    {
        Vector3 vector = a - b;
        return vector.magnitude;
    }
}