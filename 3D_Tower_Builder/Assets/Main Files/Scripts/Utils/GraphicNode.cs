using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicNode : MonoBehaviour
{
    public Transform Graphic;
    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);
    public Vector3 NodeOrigin = Vector3.zero;
    public Vector3 Pivot;

    public void LoadShaderMatrix(ref Matrix4x4 nodeMatrix)
    {
        if (Graphic == null)
        {
            Graphic = transform;
        }
        Matrix4x4 subChild;
        Matrix4x4 m;
        Matrix4x4 p = Matrix4x4.TRS(Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 invp = Matrix4x4.TRS(-Pivot, Quaternion.identity, Vector3.one);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
        if (Graphic != transform)
        {
            subChild = Matrix4x4.TRS(Graphic.localPosition, Graphic.localRotation, Graphic.localScale);
            m = nodeMatrix * p * trs * invp * subChild;
        }
        else
        {
            m = nodeMatrix * p * trs * invp;
        }

        Graphic.GetComponent<Renderer>().material.SetMatrix("MyTRSMatrix", m);
        Graphic.GetComponent<Renderer>().material.SetColor("MyColor", MyColor);
    }
}