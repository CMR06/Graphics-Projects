using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private bool hasHitBlock;

    private void Update()
    {
        if (!hasHitBlock && Controller.Instance.IsIndivisualIlluminationOn)
        {
            Shader.SetGlobalVector("LightPosition", transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            if (!hasHitBlock)
            {
                Controller.Instance.AddScore();
            }
            hasHitBlock = true;
        }


        if (collision.gameObject.tag == "Floor")
        {
            if (Controller.Instance.GetBlockID(gameObject) != 0)
            {
                Controller.Instance.GameOver();
                hasHitBlock = true;
            }
            else
            {
                if (!hasHitBlock)
                {
                    Controller.Instance.AddScore();
                }
                hasHitBlock = true;
            }
        }
    }

}
