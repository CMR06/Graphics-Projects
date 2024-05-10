// #define OurOwnRotation
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Assume hanging on a cylinder
public class LineSegment : MonoBehaviour
{
    protected Vector3 mP1 = Vector3.zero;
    protected Vector3 mP2 = Vector3.one;

    protected Vector3 mV;  // direction of the line, normalized
    protected float mL;   // Len of the line segment

    // Use this for initialization
    void Start()
    {
        ComputeLineDetails();
    }

    public virtual void SetEndPoints(Vector3 p1, Vector3 P2)
    {
        mP1 = p1;
        mP2 = P2;
        ComputeLineDetails();
    }

    public void SetWidth(float w)
    {
        Vector3 s = transform.localScale;
        s.x = s.z = w;
        transform.localScale = s;
    }

    // Getters
    public float GetLineLength() { return mL; }
    public Vector3 GetLineDir() { return mV; }
    public Vector3 GetStartPos() { return mP1; }
    public Vector3 GetEndPos() { return mP2; }

    // Return: negative when there is no valid projection
    //         Only projections within the line segment are valid
    public float DistantToPoint(Vector3 p, out Vector3 ptOnLine)
    {
        Vector3 va = p - mP1;
        float h = Vector3.Dot(va, mV);

        float d = 0f;
        ptOnLine = Vector3.zero;

        if ((h < 0) || (h > mL))
        {
            d = -1; // not valid
        }
        else
        {
            d = Mathf.Sqrt(va.sqrMagnitude - h * h);
            ptOnLine = mP1 + h * mV;
        }
        return d;
    }

    // Compute the line direction/length and move the cylinder to the proper place
    protected void ComputeLineDetails()
    {
        mV = mP2 - mP1;
        mL = mV.magnitude;
        mV = mV / mL;

        // Calculate the new position by moving along the Y-axis
        Vector3 newPosition = mP1 - mV * mL;

        // Scale the length of the cylinder
        Vector3 s = transform.localScale;
        s.y = mL;
        transform.localScale = s;

        // Set the new position and rotation
        transform.localPosition = newPosition;

        // Compute the rotation
#if OurOwnRotation
        // Rotation logic remains unchanged
#else
        Quaternion q = Quaternion.FromToRotation(Vector3.up, mV);
        transform.localRotation = q;
#endif
    }
}