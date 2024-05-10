using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    public GameObject AxisObject;
    private GameObject selectedGameObejct;

    void Awake()
    {
        Instance = this;
    }

    public void SetSelectedGameObject(GameObject selectedGameObejct)
    {
        this.selectedGameObejct = selectedGameObejct;
        AxisObject.transform.position = selectedGameObejct.transform.position;
        AxisObject.transform.rotation = selectedGameObejct.transform.rotation;
    }

    public void SetAxisEnabled(bool value)
    {
        AxisObject.SetActive(value);
    }

    private float previousXValuse = 0;

    public void SetXValue(float value, TransformType transformType)
    {
        if (selectedGameObejct != null)
        {
            Vector3 temp;

            if (transformType == TransformType.Position)
            {
                temp = selectedGameObejct.transform.localPosition;
                temp.x = value;
                selectedGameObejct.transform.localPosition = temp;
                AxisObject.transform.position = selectedGameObejct.transform.position;
            }
            else if (transformType == TransformType.Scale)
            {
                temp = selectedGameObejct.transform.localScale;
                temp.x = value;
                selectedGameObejct.transform.localScale = temp;
            }
            else
            {
                if (value != 0)
                {
                    selectedGameObejct.transform.localRotation = selectedGameObejct.transform.localRotation * Quaternion.AngleAxis(value - previousXValuse, Vector3.right);
                    AxisObject.transform.rotation = selectedGameObejct.transform.rotation;
                }
                previousXValuse = value;
            }
        }
    }

    private float previousYValuse = 0;

    public void SetYValue(float value, TransformType transformType)
    {
        if (selectedGameObejct != null)
        {
            Vector3 temp;

            if (transformType == TransformType.Position)
            {
                temp = selectedGameObejct.transform.localPosition;
                temp.y = value;
                selectedGameObejct.transform.localPosition = temp;
                AxisObject.transform.position = selectedGameObejct.transform.position;
            }
            else if (transformType == TransformType.Scale)
            {
                temp = selectedGameObejct.transform.localScale;
                temp.y = value;
                selectedGameObejct.transform.localScale = temp;
            }
            else
            {
                if (value != 0)
                {
                    selectedGameObejct.transform.localRotation = selectedGameObejct.transform.localRotation * Quaternion.AngleAxis(value - previousYValuse, Vector3.up);
                    AxisObject.transform.rotation = selectedGameObejct.transform.rotation;
                }
                previousYValuse = value;
            }
        }
    }

    private float previousZValuse = 0;

    public void SetZValue(float value, TransformType transformType)
    {
        if (selectedGameObejct != null)
        {
            Vector3 temp;

            if (transformType == TransformType.Position)
            {
                temp = selectedGameObejct.transform.localPosition;
                temp.z = value;
                selectedGameObejct.transform.localPosition = temp;
                AxisObject.transform.position = selectedGameObejct.transform.position;
            }
            else if (transformType == TransformType.Scale)
            {
                temp = selectedGameObejct.transform.localScale;
                temp.z = value;
                selectedGameObejct.transform.localScale = temp;
            }
            else
            {
                if (value != 0)
                {
                    selectedGameObejct.transform.localRotation = selectedGameObejct.transform.localRotation * Quaternion.AngleAxis(value - previousZValuse, Vector3.forward);
                    AxisObject.transform.rotation = selectedGameObejct.transform.rotation;
                }
                previousZValuse = value;
            }
        }
    }
}
