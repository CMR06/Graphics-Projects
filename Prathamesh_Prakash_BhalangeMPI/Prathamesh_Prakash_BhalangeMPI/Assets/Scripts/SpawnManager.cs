using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public static SpawnManager instance;

    public Controller controllerObj;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void SpawnObject(PrimitiveType shape)
    {
        GameObject shapeObject = GameObject.CreatePrimitive(shape);
        shapeObject.transform.position = controllerObj.spawnPosition;

        PrimitiveShape primitiveShapeObj = shapeObject.AddComponent<PrimitiveShape>();
        if (shape == PrimitiveType.Sphere)
        {
            shapeObject.transform.localScale = new Vector3(0.5f, 1, 1);
            primitiveShapeObj.Init(Color.magenta, new Vector3(0, 1, 0), 1, 45);
        }
        else if (shape == PrimitiveType.Cylinder)
        {
            shapeObject.transform.localScale = new Vector3(0.5f, 1, 1);
            primitiveShapeObj.Init(Color.cyan, new Vector3(0, 0, 1), 1.5f, 90);
        }
        else
        {
            primitiveShapeObj.Init(Color.yellow, new Vector3(1, 0, 0), 2f, 135);
        }
        shapeObject.transform.position += new Vector3(0, .5f, 0);
        shapeObject.tag = "Primitive";
        shapeObject.layer = 7;
    }
}