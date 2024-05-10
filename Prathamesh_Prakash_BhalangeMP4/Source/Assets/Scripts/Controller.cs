using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    public SpawnManager SpawnManager;

    public GameObject LeftWall;
    public GameObject RightWall;
    public GameObject BackWall;
    public GameObject Floor;

    public LayerMask WallLayer;
    public LayerMask LineEndPointLayer;

    public const int WALL_LAYER = 6;
    public const int LINE_ENDPOINT_LAYER = 7;
    public const int LINE_SEGMENT_LAYER = 8;
    public const int BARRIER_LAYER = 9;
    public const int BARRIER_SPHERE_LAYER = 10;

    private GameObject selectedEndPoint;
    private Camera myCamera;

    private RaycastHit hit;
    private Ray ray;

    private GameObject selectedBarrier;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        myCamera = Camera.main;
        SpawnInitialPointLines();
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ray = myCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (hitObject.layer == LINE_ENDPOINT_LAYER)
                    {
                        selectedEndPoint = hitObject;
                    }
                    else if (hitObject.layer == WALL_LAYER)
                    {
                        Vector3 hitPoint = hit.point;
                        Vector3 anotherHitPoint = hitPoint;
                        if (hitObject == LeftWall)
                        {
                            anotherHitPoint.x = RightWall.transform.position.x;
                            selectedEndPoint = SpawnManager.SpawnPointLine(hitPoint, anotherHitPoint, 0.2f, Color.gray, true);
                        }
                        else if (hitObject == BackWall)
                        {
                            anotherHitPoint.y = Floor.transform.position.y;
                            anotherHitPoint.z = -1f;
                            selectedEndPoint = SpawnManager.SpawnPointLine(hitPoint, anotherHitPoint, 1.9f, Color.magenta);
                        }
                        else if (hitObject == Floor)
                        {
                            if (selectedBarrier != null)
                            {
                                selectedBarrier.GetComponent<Barrier>().SetColor(Color.green);
                            }
                            selectedBarrier = SpawnManager.SpawnBarrier(hitPoint);
                            UIManager.Instance.TransformPanel.SetSelectedGameObject(selectedBarrier);
                        }
                    }
                    else if (hitObject.layer == LINE_SEGMENT_LAYER)
                    {
                        LineSegment lineSegment = hitObject.GetComponent<LineSegment>();
                        SpawnManager.RemoveLineSegment(lineSegment);
                        SpawnManager.RemoveSmallLineSegment(lineSegment);
                        SpawnManager.RemoveBigLineSegment(lineSegment);
                        lineSegment.DestroyLineSegmentAndPoints();
                    }
                    else if (hitObject.layer == BARRIER_LAYER)
                    {
                        SpawnManager.DestroyBarrier(hitObject);
                        if (selectedBarrier == hitObject)
                            selectedBarrier = null;
                        if (SpawnManager.barrierList.Count > 0)
                        {
                            selectedBarrier = SpawnManager.barrierList[0];
                            UIManager.Instance.TransformPanel.SetSelectedGameObject(selectedBarrier);
                        }
                        else
                        {
                            UIManager.Instance.TransformPanel.SetSelectedGameObject(selectedBarrier);
                        }
                    }
                    else if (hitObject.layer == BARRIER_SPHERE_LAYER)
                    {
                        if (selectedBarrier != null)
                        {
                            selectedBarrier.GetComponent<Barrier>().SetColor(Color.green);
                        }
                        selectedBarrier = hitObject.transform.parent.gameObject;
                        selectedBarrier.GetComponent<Barrier>().SetColor(Color.yellow);
                        UIManager.Instance.TransformPanel.SetSelectedGameObject(selectedBarrier);
                    }

                    if (selectedEndPoint != null)
                    {
                        selectedEndPoint.GetComponent<Renderer>().material.color = Color.black;
                    }
                }
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (selectedEndPoint != null)
                {
                    ray = myCamera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100, WallLayer))
                    {
                        selectedEndPoint.transform.position = hit.point;
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (selectedEndPoint != null)
                {
                    selectedEndPoint.GetComponent<Renderer>().material.color = Color.red;
                    selectedEndPoint = null;
                }
            }
        }
    }

    private void SpawnInitialPointLines()
    {
        Vector3 pos1 = LeftWall.transform.position + (Vector3.up * 5) + (Vector3.back * 5);
        Vector3 pos2 = RightWall.transform.position + (Vector3.up * 5) + (Vector3.back * 5);
        SpawnManager.SpawnPointLine(pos1, pos2, 0.2f, Color.gray, true);

        pos1 = pos2 = BackWall.transform.position + (Vector3.up * 6) + (Vector3.left * 5);
        pos2.y = 0;
        pos2.z = -1f;
        SpawnManager.SpawnPointLine(pos1, pos2, 1.9f, Color.magenta);

        Vector3 hitPoint = new Vector3(0, 6, 7);
        selectedBarrier = SpawnManager.SpawnBarrier(hitPoint);
        UIManager.Instance.TransformPanel.SetSelectedGameObject(selectedBarrier);
    }


    private float previousXValuse = 0;

    public void SetXValue(float value, TransformType transformType)
    {
        if (selectedBarrier != null)
        {
            Vector3 temp;

            if (transformType == TransformType.Position)
            {
                temp = selectedBarrier.transform.localPosition;
                temp.x = value;
                selectedBarrier.transform.localPosition = temp;
            }
            else
            {
                if (value != 0)
                {
                    selectedBarrier.transform.localRotation = selectedBarrier.transform.localRotation * Quaternion.AngleAxis(value - previousXValuse, Vector3.right);
                }
                previousXValuse = value;
            }
        }
    }

    private float previousYValuse = 0;

    public void SetYValue(float value, TransformType transformType)
    {
        if (selectedBarrier != null)
        {
            Vector3 temp;

            if (transformType == TransformType.Position)
            {
                temp = selectedBarrier.transform.localPosition;
                temp.y = value;
                selectedBarrier.transform.localPosition = temp;
            }
            else
            {
                if (value != 0)
                {
                    selectedBarrier.transform.localRotation = selectedBarrier.transform.localRotation * Quaternion.AngleAxis(value - previousYValuse, Vector3.up);
                }
                previousYValuse = value;
            }
        }
    }

    private float previousZValuse = 0;

    public void SetZValue(float value, TransformType transformType)
    {
        if (selectedBarrier != null)
        {
            Vector3 temp;

            if (transformType == TransformType.Position)
            {
                temp = selectedBarrier.transform.localPosition;
                temp.z = value;
                selectedBarrier.transform.localPosition = temp;
            }
            else
            {
                if (value != 0)
                {
                    selectedBarrier.transform.localRotation = selectedBarrier.transform.localRotation * Quaternion.AngleAxis(value - previousZValuse, Vector3.forward);
                }
                previousZValuse = value;
            }
        }
    }
}

public enum TransformType
{
    Position,
    Rotation
}
