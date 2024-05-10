using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{
    public Transform Hook;
    public Transform BlockParent;

    public CraneBlockConnector[] CraneBlockConnectors;

    public Vector3 Axis = Vector3.left;

    public float TargetDegree = 60f;
    public float DeltaDegree = 180f;
    public float BlockDistance = 2.5f;

    private float degree = 0f;

    private float previousYValuse = 0;

    private bool canPlayAnimation = true;

    void Start()
    {
        BlockParent.parent = null;
    }

    public void Update()
    {
        if (canPlayAnimation)
        {
            degree += DeltaDegree * Time.deltaTime;
            Hook.localRotation = Quaternion.AngleAxis(degree, Axis);
            if ((degree > TargetDegree) || (degree < (-TargetDegree)))
            {
                DeltaDegree *= -1f;
                degree += DeltaDegree * Time.deltaTime;
            }

            BlockParent.position = (-Hook.up * BlockDistance) + Hook.position;
            BlockParent.rotation = transform.rotation;
        }
    }

    public void SetYValue(float value)
    {
        if (value != 0)
        {
            transform.localRotation = transform.localRotation * Quaternion.AngleAxis(value - previousYValuse, Vector3.up);
        }
        previousYValuse = value;
    }

    public void SetSwingSpeed(float speed)
    {
        if (DeltaDegree > 0)
        {
            DeltaDegree = speed;
        }
        else
        {
            DeltaDegree = speed * -1;
        }
    }

    public void ShowCraneBlockConnectors()
    {
        for (int i = 0; i < CraneBlockConnectors.Length; i++)
        {
            CraneBlockConnectors[i].ShowConnector();
        }
    }

    public void HideCraneBlockConnectors()
    {
        for (int i = 0; i < CraneBlockConnectors.Length; i++)
        {
            CraneBlockConnectors[i].HideConnector();
        }
    }

    public void SetCanPlayAnimation(bool value)
    {
        canPlayAnimation = value;
    }
}
