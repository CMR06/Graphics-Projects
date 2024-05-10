using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexturePlacement : MonoBehaviour
{
    public Vector2 Offset = Vector2.zero;
    public Vector2 Scale = Vector2.one;
    public float RotationAngle = 0f; // Rotation angle in degrees
    Vector2[] mInitUV = null; // initial values

    public void GetSliderValues(Vector3 v, string type)
    {
        // Debug.Log(v);
        switch (type)
        {
            case "translate":
                Offset = new Vector2(v.x, v.y);
                break;
            case "scale":
                Scale = new Vector2(v.x, v.y);
                break;
            case "rotate":
                RotationAngle = v.z;
                break;
            default:
                break;
        }
    }

    public void SaveInitUV(Vector2[] uv)
    {
        mInitUV = new Vector2[uv.Length];
        for (int i = 0; i < uv.Length; i++)
            mInitUV[i] = uv[i];
    }

    // Update is called once per frame
    void Update()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector2[] uv = theMesh.uv;

        if (mInitUV == null || uv.Length != mInitUV.Length)
        {
            return;
        }

        Matrix4x4 trsMatrix = Matrix4x4.TRS(new Vector3(Offset.x, Offset.y, 0), Quaternion.identity, new Vector3(Scale.x, Scale.y, 1));

        // Rotation around z-axis
        float radAngle = RotationAngle * Mathf.Deg2Rad;
        float cosAngle = Mathf.Cos(radAngle);
        float sinAngle = Mathf.Sin(radAngle);
        Matrix4x4 rotationMatrix = new Matrix4x4(
            new Vector4(cosAngle, -sinAngle, 0, 0),
            new Vector4(sinAngle, cosAngle, 0, 0),
            new Vector4(0, 0, 1, 0),
            new Vector4(0, 0, 0, 1)
        );
        trsMatrix *= rotationMatrix;

        for (int i = 0; i < uv.Length; i++)
        {
            if (i >= mInitUV.Length)
            {
                break;
            }

            Vector3 vertex = new Vector3(mInitUV[i].x, mInitUV[i].y, 0);
            vertex = trsMatrix.MultiplyPoint(vertex);

            if (i >= uv.Length)
            {
                break;
            }

            uv[i] = new Vector2(vertex.x, vertex.y);
        }

        theMesh.uv = uv;
    }
}
