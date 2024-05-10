using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{

    public void Init()
    {
        SetColor(Color.yellow);
    }

    public void SetColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
}
