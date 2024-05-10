using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TravelBallShadow
{
    public TravelingBall TravelBall;
    public Transform Barrier;
    public Transform ShadowObject;
    public LineSegment LineSegment;

    public bool isForceFrontFalse;

    private Vector3 barrierForward;
    private float dot, barrierDot, barrierRadius, shadowBarrierDist;
    private float shadowDistance = 0, currentShadowDistance, shadowScale;

    private bool isOnBarrier;

    public TravelBallShadow(TravelingBall TravelBall, Transform Barrier, Transform parent)
    {
        this.TravelBall = TravelBall;
        this.Barrier = Barrier;
        barrierRadius = Barrier.localScale.x / 2;

        ShadowObject = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        ShadowObject.parent = parent;
        ShadowObject.position = TravelBall.transform.position;
        ShadowObject.GetComponent<Renderer>().material.color = Color.black;
        ShadowObject.transform.localScale = new Vector3(1, 1, 0.01f);

        GameObject lineSegment = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        lineSegment.transform.parent = parent;
        LineSegment = lineSegment.AddComponent<LineSegment>();
        LineSegment.Init(TravelBall.gameObject, ShadowObject.gameObject, 0.01f, Color.black);
        LineSegment.gameObject.SetActive(false);

        Update();
        LineSegment.gameObject.SetActive(true);
    }

    public void Update()
    {
        if (Barrier != null)
        {
            barrierForward = (-Barrier.forward).normalized;
            barrierDot = Vector3.Dot(Barrier.position, -Barrier.forward);

            dot = Vector3.Dot(TravelBall.transform.position, barrierForward);

            ShadowObject.position = TravelBall.transform.position - ((dot - barrierDot - 0.03f) * barrierForward);
            ShadowObject.transform.forward = barrierForward;

            shadowBarrierDist = Distance(ShadowObject.position, Barrier.position);

            isOnBarrier = shadowBarrierDist < barrierRadius && (dot - barrierDot) > 0;

            ShadowObject.gameObject.SetActive(isOnBarrier);
            LineSegment.gameObject.SetActive(isOnBarrier);

            SimulateShadowBallScalling();
        }
        else
        {
            TravelBall.RemoveTravelBallShadow(this);
        }
    }

    private void SimulateShadowBallScalling()
    {
        if (shadowDistance == 0)
        {
            shadowDistance = Distance(TravelBall.transform.position, ShadowObject.position);
        }
        currentShadowDistance = Distance(TravelBall.transform.position, ShadowObject.position);
        shadowScale = 1 - (currentShadowDistance / shadowDistance);
        shadowScale = Mathf.Clamp(shadowScale, 0.1f, 1);
        ShadowObject.localScale = new Vector3(shadowScale, shadowScale, 0.01f);
    }

    public bool CanReflect()
    {
        return (dot - barrierDot) > 0 && !isForceFrontFalse && shadowBarrierDist < barrierRadius;
    }

    private float Distance(Vector3 a, Vector3 b)
    {
        Vector3 vector = a - b;
        return vector.magnitude;
    }

    public IEnumerator IForceReflectFalse()
    {
        isForceFrontFalse = true;
        yield return new WaitForSeconds(0.5f);
        isForceFrontFalse = false;
    }
}
