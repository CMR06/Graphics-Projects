using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondTextureAnim : MonoBehaviour
{
    public float animationSpeed = 10;

    private Material myMat;

    private Vector2 offset;

    private void Start()
    {
        myMat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (Controller.Instance.CanAnimateTexture)
        {
            myMat.SetTextureOffset("_SecTex", offset);
            offset += Vector2.left * Time.deltaTime * animationSpeed;
        }
    }
}
