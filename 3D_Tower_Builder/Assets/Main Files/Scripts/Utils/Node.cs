using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{

    protected Matrix4x4 mCombinedParentXform;

    public Vector3 NodeOrigin = Vector3.zero;
    public List<GraphicNode> GraphicsList;
    public List<Node> ChildrenList;


    protected void Start()
    {
        InitializeSceneNode();
    }

    private void InitializeSceneNode()
    {
        mCombinedParentXform = Matrix4x4.identity;
    }

    public void CompositeXform(ref Matrix4x4 parentXform)
    {
        Matrix4x4 orgT = Matrix4x4.Translate(NodeOrigin);
        Matrix4x4 trs = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

        mCombinedParentXform = parentXform * orgT * trs;

        foreach (Node child in ChildrenList)
            child.CompositeXform(ref mCombinedParentXform);

        foreach (GraphicNode p in GraphicsList)
            p.LoadShaderMatrix(ref mCombinedParentXform);

    }
}