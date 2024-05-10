using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimation : MonoBehaviour
{
    public Transform Car;

    public Transform StartPoint;
    public Transform EndPoint;

    public float CarSpeed = 3;

    private bool isSpawned = true;

    void Update()
    {
        if (Controller.Instance.CanAnimateTexture && isSpawned)
        {
            Car.position = Vector3.MoveTowards(Car.position, EndPoint.position, CarSpeed * Time.deltaTime);
            if (Car.position == EndPoint.position)
            {
                isSpawned = false;
                Car.gameObject.SetActive(false);
                StartCoroutine(SpawnCarIn(1));
                Car.position = StartPoint.position;
            }
        }
    }

    IEnumerator SpawnCarIn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Car.gameObject.SetActive(true);
        isSpawned = true;
    }
}
