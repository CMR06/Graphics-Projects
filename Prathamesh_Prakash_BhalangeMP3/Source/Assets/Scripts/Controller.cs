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

    private GameObject selectedEndPoint;
    private Camera myCamera;

    private RaycastHit hit;
    private Ray ray;

    public const int WALL_LAYER = 6;
    public const int LINE_ENDPOINT_LAYER = 7;
    public const int LINE_SEGMENT_LAYER = 8;

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
                        selectedEndPoint = hit.collider.gameObject;
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
                    }
                    else if (hitObject.layer == LINE_SEGMENT_LAYER)
                    {
                        LineSegment lineSegment = hit.collider.GetComponent<LineSegment>();
                        if (SpawnManager.LineSegments.Contains(lineSegment))
                        {
                            SpawnManager.LineSegments.Remove(lineSegment);
                        }
                        lineSegment.DestroyLineSegment();
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
        Vector3 pos1 = LeftWall.transform.position + (Vector3.up * 10);
        Vector3 pos2 = RightWall.transform.position + (Vector3.up * 10);
        SpawnManager.SpawnPointLine(pos1, pos2, 0.2f, Color.gray, true);

        pos1 = pos2 = BackWall.transform.position + (Vector3.up * 6);
        pos2.y = 0;
        pos2.z = -1f;
        SpawnManager.SpawnPointLine(pos1, pos2, 1.9f, Color.magenta);
    }
}