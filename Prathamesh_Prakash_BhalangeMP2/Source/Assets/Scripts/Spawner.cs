using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Material CyanMaterial;
    public Material BlackMaterial;

    private Controller controller;

    private int numberOfPrimitiveSpawned = 0;

    void Start()
    {
        controller = GetComponent<Controller>();
    }

    public void SpawnPrimitive(PrimitiveType primitiveType)
    {
        GameObject primitive = GameObject.CreatePrimitive(primitiveType);
        primitive.name = primitive.name + ":" + ++numberOfPrimitiveSpawned;

        if (controller.selectedGameObejct != null)
        {
            Transform selectedGOTransform = controller.selectedGameObejct.transform;
            primitive.transform.parent = selectedGOTransform;
            if (selectedGOTransform.childCount > 1)
            {
                primitive.transform.localPosition = selectedGOTransform.GetChild(selectedGOTransform.childCount - 2).localPosition + (Vector3.one * 0.5f);
                primitive.transform.localRotation = selectedGOTransform.GetChild(selectedGOTransform.childCount - 2).localRotation;
                primitive.transform.localScale = selectedGOTransform.GetChild(selectedGOTransform.childCount - 2).localScale;
                primitive.GetComponent<Renderer>().material = selectedGOTransform.GetChild(selectedGOTransform.childCount - 2).GetComponent<Renderer>().material;
            }
            else
            {
                primitive.transform.localPosition = Vector3.one * 0.5f;
                primitive.transform.localScale = Vector3.one;
                primitive.GetComponent<Renderer>().material = CyanMaterial;
            }
        }
        else
        {
            primitive.transform.localPosition = Vector3.one * 0.5f;
            primitive.GetComponent<Renderer>().material = BlackMaterial;
        }
        primitive.tag = "Primitive";
    }
}
