using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Toggle sphereToggle;
    public Toggle cylinderToggle;
    public Toggle cubeToggle;

    public Toggle previousToggle;

    public void SphereOnToggle(bool toggle)
    {
        if (previousToggle != null && previousToggle != sphereToggle)
        {
            previousToggle.isOn = false;
        }
        if (toggle)
        {
            SpawnManager.instance.SpawnObject(PrimitiveType.Sphere);
            previousToggle = sphereToggle;
        }
    }

    public void CylinderOnToggle(bool toggle)
    {
        if (previousToggle != null && previousToggle != cylinderToggle)
        {
            previousToggle.isOn = false;
        }
        if (toggle)
        {
            SpawnManager.instance.SpawnObject(PrimitiveType.Cylinder);
            previousToggle = cylinderToggle;
        }
    }

    public void CubeOnToggle(bool toggle)
    {
        if (previousToggle != null && previousToggle != cubeToggle)
        {
            previousToggle.isOn = false;
        }
        if (toggle)
        {
            SpawnManager.instance.SpawnObject(PrimitiveType.Cube);
            previousToggle = cubeToggle;
        }
    }
}
