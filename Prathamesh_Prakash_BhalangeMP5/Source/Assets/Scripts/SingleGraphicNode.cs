using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SingleGraphicNode : MonoBehaviour
{
    public Transform Graphic;

    public Color MyColor = new Color(0.1f, 0.1f, 0.2f, 1.0f);

    public Matrix4x4 mCombinedParentXform;
    public Vector3 NodeOrigin = Vector3.zero;
    public Vector3 Pivot;

    private void Update()
    {
        LoadShaderMatrix();
    }

    public void LoadShaderMatrix()
    {
        if (Graphic != null)
        {
            Matrix4x4 parent = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
            Matrix4x4 child = Matrix4x4.TRS(Graphic.localPosition, Graphic.localRotation, Graphic.localScale);

            mCombinedParentXform = parent * child;

            Graphic.GetComponent<Renderer>().material.SetMatrix("MyTRSMatrix", mCombinedParentXform);
            Graphic.GetComponent<Renderer>().material.SetColor("MyColor", MyColor);
        }
    }
}
