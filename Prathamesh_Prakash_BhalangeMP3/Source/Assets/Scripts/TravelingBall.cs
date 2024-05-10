using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelingBall : MonoBehaviour
{
    private Vector3 direction;

    private float speed;

    public void Init(Transform target, float speed, float aliveSeconds, Color color)
    {
        this.speed = speed;
        direction = (target.position - transform.position).normalized;
        GetComponent<Renderer>().material.color = color;

        Destroy(gameObject, aliveSeconds);
    }

    void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
    }
}
