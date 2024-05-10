using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public UIController controller;

    public GameObject sphereObject;
    public LayerMask sphereLayerMask;

    public Vector3 spawnPosition;

    private Camera myCamera;
    private RaycastHit hit;


    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, sphereLayerMask))
            {
                if (hit.collider.tag == "Primitive")
                {
                    Destroy(hit.collider.gameObject);
                    if(controller.previousToggle != null) {
                        controller.previousToggle.isOn = false;
                        controller.previousToggle = null;
                    }
                    
                }
                else
                {
                    sphereObject.transform.position = hit.point + new Vector3(0,0.2f,0);
                    spawnPosition = hit.point;
                }
            }
        }
    }
}
