using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMesh : MonoBehaviour
{
    private LineSegment[] mNormals;
    private GameObject[] mControllers;
    public GameObject axisFramePrefab;
    public Texture myTexture;
    private GameObject axisFrame;

    private int rows = 2, cols = 2, sphereIndex = -1, cyResX = 4, cyResY = 4, thetaDegrees = 275;
    bool showDetails = false, sphereSelected = false;
    private string dropdownOption;

    public void GetRowSize(float size)
    {
        rows = (int)size;
        if (dropdownOption == "Mesh")
            UpdateQuadMesh();
    }

    public void GetColSize(float size)
    {
        cols = (int)size;
        if (dropdownOption == "Mesh")
            UpdateQuadMesh();
    }

    public void GetCylinderResolutionX(float size)
    {
        cyResX = (int)size;
        if (dropdownOption == "Cylinder")
            UpdateCylinderMesh();
    }
    public void GetCylinderResolutionY(float size)
    {
        cyResY = (int)size;
        if (dropdownOption == "Cylinder")
            UpdateCylinderMesh();
    }

    public void GetCylinderRotation(float size)
    {
        thetaDegrees = (int)size;
        if (dropdownOption == "Cylinder")
            UpdateCylinderMesh();
    }

    public void GetDropdownOption(string opt)
    {
        dropdownOption = opt;
        if (opt == "Mesh")
        {
            Camera.main.transform.position = new Vector3(-2f, 8f, -10f);
            Camera.main.transform.rotation = Quaternion.Euler(35f, 0f, 0f);
            UpdateQuadMesh();
        }
        else
        {
            Camera.main.transform.position = new Vector3(15f, 15f, -25f);
            Camera.main.transform.rotation = Quaternion.Euler(25f, -40f, 0f);
            UpdateCylinderMesh();
        }
    }

    void InitNormals(Vector3[] v, Vector3[] n)
    {
        mNormals = new LineSegment[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            o.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mNormals[i] = o.AddComponent<LineSegment>();
            mNormals[i].SetWidth(0.05f);
            mNormals[i].transform.SetParent(this.transform);
        }
        UpdateNormals(v, n);
    }

    void UpdateNormals(Vector3[] v, Vector3[] n)
    {
        for (int i = 0; i < v.Length; i++)
        {
            mNormals[i].SetEndPoints(v[i], v[i] + 1.0f * n[i]);
        }
    }

    Vector3 FaceNormal(Vector3[] v, int i0, int i1, int i2)
    {
        Vector3 a = v[i1] - v[i0];
        Vector3 b = v[i2] - v[i0];
        return Vector3.Cross(a, b).normalized;
    }

    void ComputeNormals(Mesh mesh, Vector3[] v, Vector3[] n)
    {
        List<Vector3>[] vertexFaceNormals = new List<Vector3>[v.Length];
        for (int i = 0; i < vertexFaceNormals.Length; i++)
        {
            vertexFaceNormals[i] = new List<Vector3>();
        }

        for (int j = 0; j < mesh.triangles.Length; j += 3)
        {
            int i0 = mesh.triangles[j];
            int i1 = mesh.triangles[j + 1];
            int i2 = mesh.triangles[j + 2];

            Vector3 faceNormal = FaceNormal(v, i0, i1, i2);

            vertexFaceNormals[i0].Add(faceNormal);
            vertexFaceNormals[i1].Add(faceNormal);
            vertexFaceNormals[i2].Add(faceNormal);
        }

        for (int i = 0; i < v.Length; i++)
        {
            Vector3 normal = Vector3.zero;

            foreach (Vector3 faceNormal in vertexFaceNormals[i])
            {
                normal += faceNormal;
            }

            n[i] = normal.normalized;
        }
        UpdateNormals(v, n);
    }

    void Start()
    {
        GetDropdownOption("Mesh");
        UpdateQuadMesh();
    }

    void UpdateCylinderMesh()
    {
        GetComponent<Renderer>().material.mainTexture = null;
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        theMesh.Clear();

        int rows = Mathf.Max(4, cyResX);
        int cols = Mathf.Max(4, cyResY);

        float scale = 10.0f;
        int numVertices = rows * cols;
        Vector3[] v = new Vector3[numVertices];
        Vector3[] n = new Vector3[numVertices];

        float thetaRadians = Mathf.Deg2Rad * thetaDegrees;
        float angleIncrement = thetaRadians / (cols - 1);

        int index = 0;
        for (int i = 0; i < rows; i++)
        {
            float posY = ((float)i / (rows - 1) - 0.5f) * scale * 2; // Height of the cylinder

            for (int j = 0; j < cols; j++)
            {
                float angle = j * angleIncrement; // Angle around the cylinder

                float posX = Mathf.Cos(angle) * scale;
                float posZ = Mathf.Sin(angle) * scale;

                v[index] = new Vector3(posX, posY, posZ);
                n[index] = new Vector3(posX, 0, posZ).normalized; // Normals point outward from the surface
                index++;
            }
        }

        theMesh.vertices = v;
        theMesh.normals = n;

        if (mControllers != null)
        {
            foreach (var controller in mControllers)
            {
                Destroy(controller);
            }
        }

        if (mNormals != null)
        {
            foreach (var normal in mNormals)
            {
                Destroy(normal.gameObject);
            }
        }

        InitControllers(v);
        InitNormals(v, n);
        AssignTriangles(theMesh, rows, cols);
    }

    void UpdateQuadMesh()
    {
        GetComponent<Renderer>().material.mainTexture = myTexture;
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        theMesh.Clear();

        float scale = 10.0f;
        int numVertices = rows * cols;
        Vector3[] v = new Vector3[numVertices];
        Vector3[] n = new Vector3[numVertices];
        Vector2[] uv = new Vector2[numVertices];

        int index = 0;
        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < cols; x++)
            {
                float posX = ((float)x / (cols - 1) - 0.5f) * scale;
                float posZ = ((float)z / (rows - 1) - 0.5f) * scale;

                v[index] = new Vector3(posX, 0, posZ);
                n[index] = Vector3.up; // Normals pointing upwards

                float u = (float)x / (cols - 1); // UV Mapping
                float vCoord = (float)z / (rows - 1);
                uv[index] = new Vector2(u, vCoord);

                index++;
            }
        }

        theMesh.vertices = v;
        theMesh.normals = n;
        theMesh.uv = uv;

        if (mControllers != null)
        {
            foreach (var controller in mControllers)
            {
                Destroy(controller);
            }
        }

        if (mNormals != null)
        {
            foreach (var normal in mNormals)
            {
                Destroy(normal.gameObject);
            }
        }

        InitControllers(v);
        InitNormals(v, n);
        AssignTriangles(theMesh, rows, cols);
        GetComponent<TexturePlacement>().SaveInitUV(uv);
    }

    void AssignTriangles(Mesh mesh, int numRows, int numCols)
    {
        int numTriangles = (numRows - 1) * (numCols - 1) * 2;
        int[] t = new int[numTriangles * 3];
        int index = 0;

        for (int z = 0; z < numRows - 1; z++)
        {
            for (int x = 0; x < numCols - 1; x++)
            {
                int vertexIndex = x + z * numCols;

                t[index++] = vertexIndex;
                t[index++] = vertexIndex + 1;
                t[index++] = vertexIndex + numCols;

                t[index++] = vertexIndex + 1;
                t[index++] = vertexIndex + numCols + 1;
                t[index++] = vertexIndex + numCols;
            }
        }
        mesh.triangles = t;
    }

    void Update()
    {
        GameObject[] spheres = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject sphere in spheres)
        {
            if (sphere.name.StartsWith("Sphere") && sphere.layer != LayerMask.NameToLayer("selectLayer"))
                sphere.GetComponent<Renderer>().material.color = Color.black;
        }

        CheckAndDeleteRowParents();

        if (Input.GetKey(KeyCode.LeftControl))
        {
            showDetails = true;
            if (Input.GetMouseButtonDown(0)) // Left mouse click
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layerMask = 1 << LayerMask.NameToLayer("selectLayer");
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    for (int i = 0; i < mControllers.Length; i++)
                    {
                        if (mControllers != null && i < mControllers.Length && hitObject == mControllers[i])
                        {
                            mControllers[i].GetComponent<Renderer>().material.color = Color.yellow;
                            sphereSelected = true;

                            if (sphereIndex != -1 && sphereIndex != i && sphereIndex < mControllers.Length)
                                mControllers[sphereIndex].GetComponent<Renderer>().material.color = Color.white;

                            if (axisFrame != null) Destroy(axisFrame);
                            axisFrame = Instantiate(axisFramePrefab, hitObject.transform.position, Quaternion.identity, mControllers[i].transform);
                            axisFrame.GetComponent<AxisFrameController>().GetDropdownOption(dropdownOption);

                            sphereIndex = i;
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            if (!sphereSelected)
            {
                showDetails = false;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                for (int i = 0; mControllers != null && i < mControllers.Length; i++)
                {
                    if (mControllers[i] != null)
                        mControllers[i].GetComponent<Renderer>().material.color = Color.white;
                }

                if (axisFrame != null) Destroy(axisFrame);
                sphereSelected = false;
            }
        }

        ToggleDetailsVisibility(showDetails);

        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] v = theMesh.vertices;
        Vector3[] n = theMesh.normals;

        if (mControllers != null && v != null && mControllers.Length <= v.Length)
        {
            for (int i = 0; i < mControllers.Length && i < v.Length; i++)
            {
                if (mControllers[i] != null)
                    v[i] = mControllers[i].transform.localPosition;
            }

            ComputeNormals(theMesh, v, n);
            theMesh.vertices = v;
            theMesh.normals = n;
        }
    }

    void ToggleDetailsVisibility(bool visible)
    {
        if (mControllers != null)
        {
            foreach (var controller in mControllers)
            {
                controller.SetActive(visible);
            }
        }

        if (mNormals != null)
        {
            foreach (var normal in mNormals)
            {
                normal.gameObject.SetActive(visible);
            }
        }
    }

    void InitControllers(Vector3[] v)
    {
        if (mControllers != null)
        {
            foreach (var controller in mControllers)
            {
                Destroy(controller); // Clear existing spheres
            }
        }

        mControllers = new GameObject[v.Length];
        Dictionary<int, GameObject> rowParents = new Dictionary<int, GameObject>(); // Dictionary to store row parents

        int rowCount = (dropdownOption == "Mesh") ? rows : cyResX;
        int colCount = (dropdownOption == "Mesh") ? cols : cyResY;

        for (int i = 0; i < v.Length; i++)
        {
            mControllers[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            mControllers[i].GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            mControllers[i].transform.localScale = new Vector3(.5f, .5f, .5f);
            mControllers[i].transform.localPosition = v[i];
            mControllers[i].transform.parent = this.transform;

            // Group spheres by row
            int rowIndex = i / colCount; // Calculate the row index based on the column count

            if (!rowParents.ContainsKey(rowIndex))
            {
                GameObject rowParent = new GameObject("RowParent_" + rowIndex); // Create a new parent object
                rowParent.transform.SetParent(this.transform);
                rowParents[rowIndex] = rowParent;
            }

            mControllers[i].transform.SetParent(rowParents[rowIndex].transform); // Set the sphere as a child of its row's parent
            mControllers[i].name = "Sphere_" + i; // Assign a name to the sphere for identification

            if (dropdownOption == "Cylinder")
            {
                Transform firstChild = rowParents[rowIndex].transform.GetChild(0);
                if (firstChild != null)
                {
                    if (mControllers[i].transform != firstChild)
                        mControllers[i].layer = LayerMask.NameToLayer("Default");
                    else
                        mControllers[i].layer = LayerMask.NameToLayer("selectLayer");
                }
            }
            else // For dropdownOption other than "Cylinder"
            {
                mControllers[i].layer = LayerMask.NameToLayer("selectLayer");
            }
        }
    }


    void CheckAndDeleteRowParents()
    {
        Transform[] allObjects = GameObject.FindObjectsOfType<Transform>();

        foreach (Transform obj in allObjects)
        {
            if (obj.name.StartsWith("RowParent_"))
            {
                if (obj.childCount == 0)
                {
                    Destroy(obj.gameObject);
                }
            }
        }
    }
}
