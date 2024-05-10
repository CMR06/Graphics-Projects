using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimitiveShape : MonoBehaviour
{

    public Color color;

    public Vector3 direction;
    public Vector3 rotationDirection = new Vector3(0,1,0);
    public float speed;
    public float rotationSpeed;

    public Vector3 initialPosition;
    public Vector3 targetPosition;

    private Vector3 currentInitialPosition;
    private Vector3 currentTargetPosition;

    private float elapsedTime;
    private float totalTime;

    public void Init(Color color, Vector3 direction, float speed, float rotationSpeed)
    {
        this.color = color;
        this.direction = direction;
        this.speed = speed;
        this.rotationSpeed = rotationSpeed;
    }

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = getTargetPosition(transform.position);

        startMovement(targetPosition);
        initialPosition = getInitialPosition(transform.position);
    }

    private Vector3 getInitialPosition(Vector3 position)
    {
        Vector3 initialPos = Vector3.one - direction;
        initialPos = new Vector3(initialPos.x * position.x, initialPos.y * position.y, initialPos.z * position.z);
        return initialPos;
    }

    private Vector3 getTargetPosition(Vector3 position)
    {
        Vector3 targetpos = Vector3.one - direction;
        targetpos = new Vector3(targetpos.x * position.x, targetpos.y * position.y, targetpos.z * position.z);
        targetpos += direction * 5;
        return targetpos;
    }

    private void startMovement(Vector3 targetPosition)
    {
        elapsedTime = 0;
        currentInitialPosition = transform.position;
        currentTargetPosition = targetPosition;
        totalTime = (1 / speed) * Vector3.Distance(currentInitialPosition, currentTargetPosition);
    }

    void Update()
    {
        if (elapsedTime < totalTime)
        {
            transform.position = Vector3.Lerp(currentInitialPosition, currentTargetPosition, elapsedTime / totalTime);
            elapsedTime += Time.deltaTime;
        }
        else
        {
            if (currentTargetPosition == targetPosition)
            {
                startMovement(initialPosition);
                GetComponent<Renderer>().material.color = color;
            }
            else
            {
                startMovement(targetPosition);
                GetComponent<Renderer>().material.color = Color.white;
            }
        }

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
