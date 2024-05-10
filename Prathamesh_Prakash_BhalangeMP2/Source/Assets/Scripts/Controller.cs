using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    public Spawner Spawner;

    public GameObject AxisObject;

    public GameObject selectedGameObejct;

    public Material HighlightMaterial;

    private Camera myCamera;

    private RaycastHit hit;
    private Ray ray;

    private Material selectedObjectPreviousMat;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        myCamera = Camera.main;
        AxisObject.SetActive(false);

        Spawner = GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ray = myCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Primitive")
                {
                    if (selectedGameObejct != null)
                    {
                        selectedGameObejct.GetComponent<Renderer>().material = selectedObjectPreviousMat;
                    }

                    selectedGameObejct = hit.collider.gameObject;
                    selectedObjectPreviousMat = selectedGameObejct.GetComponent<Renderer>().material;
                    selectedGameObejct.GetComponent<Renderer>().material = HighlightMaterial;

                    UIManager.Instance.TransformPanel.SetSelectedGameObject(selectedGameObejct);

                    AxisObject.SetActive(true);
                    AxisObject.transform.position = selectedGameObejct.transform.position;
                    AxisObject.transform.rotation = selectedGameObejct.transform.rotation;
                }
                else if (!EventSystem.current.IsPointerOverGameObject())
                {
                    unSelectGameObject();
                }
            }
            else if (!EventSystem.current.IsPointerOverGameObject())
            {
                unSelectGameObject();
            }
        }
    }

    private void unSelectGameObject()
    {
        if (selectedGameObejct != null)
        {
            selectedGameObejct.GetComponent<Renderer>().material = selectedObjectPreviousMat;
            selectedGameObejct = null;

            UIManager.Instance.TransformPanel.SetSelectedGameObject(selectedGameObejct);

            AxisObject.SetActive(false);
        }
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
