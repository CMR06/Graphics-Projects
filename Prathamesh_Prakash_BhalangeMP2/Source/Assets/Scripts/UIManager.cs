using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TransformPanel TransformPanel;

    public Toggle SphereToggle;
    public Toggle CylinderToggle;
    public Toggle CubeToggle;

    void Awake()
    {
        Instance = this;
    }

    public void OnSpherePressed(bool toggle)
    {
        if (toggle)
        {
            Controller.Instance.Spawner.SpawnPrimitive(PrimitiveType.Sphere);
            SphereToggle.isOn = false;
        }
    }

    public void OnCylinderPressed(bool toggle)
    {
        if (toggle)
        {
            Controller.Instance.Spawner.SpawnPrimitive(PrimitiveType.Cylinder);
            CylinderToggle.isOn = false;
        }
    }

    public void OnCubePressed(bool toggle)
    {
        if (toggle)
        {
            Controller.Instance.Spawner.SpawnPrimitive(PrimitiveType.Cube);
            CubeToggle.isOn = false;
        }
    }
}
