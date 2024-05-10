using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : MonoBehaviour
{
    public GameObject BlastParticlePrefab;

    private bool hasHitSomeThing;

    private void Update()
    {
        if (!hasHitSomeThing && Controller.Instance.IsIndivisualIlluminationOn)
        {
            Shader.SetGlobalVector("LightPosition", transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHitSomeThing)
        {
            if (collision.gameObject.tag == "Block")
            {
                Controller.Instance.RemoveHeart();
            }
            Instantiate(BlastParticlePrefab).transform.position = transform.position;
            hasHitSomeThing = true;
            if (transform.GetChild(transform.childCount - 1).GetComponent<Camera>() != null)
            {
                transform.GetChild(transform.childCount - 1).parent = null;
            }
            Destroy(gameObject);
        }
    }
}
