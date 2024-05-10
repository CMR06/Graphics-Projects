using System.Collections.Generic;
using UnityEngine;

public class AxisFrameController : MonoBehaviour
{
    public GameObject xAxis, yAxis, zAxis;
    private GameObject selectedAxis;
    private bool isDragging = false;
    private Plane plane;
    private Ray dragRay;
    private Vector3 offset;

    // Store original colors
    private Color xAxisOriginalColor;
    private Color yAxisOriginalColor;
    private Color zAxisOriginalColor;

    private string dropdownOption = "Mesh";

    public void GetDropdownOption(string opt)
    {
        dropdownOption = opt;
    }

    void Start()
    {
        selectedAxis = null;
        plane = new Plane(Vector3.forward, Vector3.zero);

        // Store original colors
        xAxisOriginalColor = xAxis.GetComponent<Renderer>().material.color;
        yAxisOriginalColor = yAxis.GetComponent<Renderer>().material.color;
        zAxisOriginalColor = zAxis.GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == xAxis || hit.collider.gameObject == yAxis || hit.collider.gameObject == zAxis)
                {
                    selectedAxis = hit.collider.gameObject;
                    plane = new Plane(selectedAxis.transform.forward, hit.point);

                    Ray offsetRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (plane.Raycast(offsetRay, out float enter))
                    {
                        Vector3 hitPoint = offsetRay.GetPoint(enter);
                        offset = transform.parent.position - hitPoint;
                    }

                    isDragging = true;

                    // Change color to yellow while dragging
                    ChangeAxisColor(selectedAxis, Color.yellow);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            selectedAxis = null;

            // Revert colors back to original when dragging stops
            ChangeAxisColor(xAxis, xAxisOriginalColor);
            ChangeAxisColor(yAxis, yAxisOriginalColor);
            ChangeAxisColor(zAxis, zAxisOriginalColor);
        }

        // Perform object dragging along the selected axis
        if (isDragging && selectedAxis != null)
        {
            dragRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(dragRay, out float distance))
            {
                Vector3 hitPoint = dragRay.GetPoint(distance);
                Vector3 newPos = transform.parent.position;

                if (selectedAxis == xAxis)
                {
                    newPos.x = hitPoint.x + offset.x;
                }
                else if (selectedAxis == yAxis)
                {
                    newPos.y = hitPoint.y + offset.y;
                }
                else if (selectedAxis == zAxis)
                {
                    newPos.z = hitPoint.z + offset.z;
                }

                if (dropdownOption == "Mesh")
                    transform.parent.position = newPos;
                else UpdateCylinderMeshPosition(newPos);
            }
        }
    }

    void UpdateCylinderMeshPosition(Vector3 newPos)
    {
        if (transform.parent != null && transform.parent.parent != null)
        {
            Transform grandparent = transform.parent.parent;

            List<Transform> grandparentChildren = new List<Transform>();

            for (int i = 0; i < grandparent.childCount; i++)
            {
                Transform child = grandparent.GetChild(i);
                grandparentChildren.Add(child);
            }

            Vector3 center = Vector3.zero;
            foreach (Transform child in grandparentChildren)
            {
                center += child.position;
            }
            center /= grandparentChildren.Count;
            float radiusDifference = Vector3.Distance(center, newPos) - Vector3.Distance(center, transform.parent.position);

            Vector3 difference = newPos - transform.parent.position;

            for (int i = 0; i < grandparentChildren.Count; i++)
            {
                Transform child = grandparentChildren[i];
                Vector3 direction, updatedChildPos;

                if (selectedAxis == xAxis)
                {
                    direction = (child.position - center).normalized;
                    updatedChildPos = center + direction * (Vector3.Distance(center, child.position) + radiusDifference);
                    child.position = updatedChildPos;
                }
                else if (selectedAxis == yAxis)
                {
                    direction = child.TransformDirection(Vector3.up);
                    updatedChildPos = child.position + Vector3.Scale(direction, difference);
                    child.position = updatedChildPos;
                }
            }
        }
    }

    // Method to change axis color
    void ChangeAxisColor(GameObject axis, Color color)
    {
        axis.GetComponent<Renderer>().material.color = color;
    }
}
