using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float tumbleSpeed = 5f, trackSpeed = 5f, zoomSpeed = 2.5f;
    public GameObject cameraFramePrefab;
    [HideInInspector] public GameObject CameraFrame;

    void Awake()
    {
        CameraFrame = Instantiate(cameraFramePrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }

    void Update()
    {
        Vector3 lookAtPosition = CameraFrame.transform.position;

        // Tumble: Alt + Left Mouse Button
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * tumbleSpeed;
            float mouseY = Input.GetAxisRaw("Mouse Y") * tumbleSpeed;

            Vector3 pivotAxis = Vector3.Cross(transform.forward, Vector3.up);

            // Rotate around the pivot point
            Quaternion horizontalRotation = Quaternion.AngleAxis(mouseX, Vector3.up);
            Quaternion verticalRotation = Quaternion.AngleAxis(mouseY, pivotAxis);
            this.transform.forward = horizontalRotation * verticalRotation * transform.forward;

            // Maintain distance from lookAtPosition
            this.transform.position = lookAtPosition - this.transform.forward * (lookAtPosition - this.transform.position).magnitude;
        }

        // Track: Alt + Right Mouse Button
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(1))
        {
            CameraFrame.transform.position = this.transform.position + this.transform.forward * (lookAtPosition - this.transform.position).magnitude;

            float mouseX = Input.GetAxisRaw("Mouse X") * trackSpeed;
            float mouseY = Input.GetAxisRaw("Mouse Y") * trackSpeed;

            this.transform.position += transform.right * -mouseX + transform.up * -mouseY;
        }

        // Dolly: Alt + Scroll Wheel
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetAxisRaw("Mouse ScrollWheel") != 0f)
        {
            float scrollWheel = Input.mouseScrollDelta.y * zoomSpeed;

            Vector3 dollyDirection = lookAtPosition - transform.position;
            float currentDistance = dollyDirection.magnitude;

            float updatedDistance = currentDistance - scrollWheel;

            if (updatedDistance < 5f) updatedDistance = 5f;
            transform.position = lookAtPosition - dollyDirection.normalized * updatedDistance;
        }
    }
}


