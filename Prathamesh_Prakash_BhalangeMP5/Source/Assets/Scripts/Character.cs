using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Character : MonoBehaviour
{
    private Node node;

    private void Start()
    {
        node = GetComponent<Node>();
    }

    private void Update()
    {
        if (node == null)
            node = GetComponent<Node>();

        Matrix4x4 i = Matrix4x4.identity;
        node.CompositeXform(ref i);
    }
}
