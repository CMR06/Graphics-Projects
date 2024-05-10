using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    public Crane Crane;

    public GameObject BlockPrefab;
    public Texture[] BlockTextureList;
    public GameObject TNTPrefab;
    public GameObject[] Lights;
    public GameObject[] LampLights;

    public Camera MainCamera;
    public Camera BlockCamera;

    public Transform BlockParent;

    public bool IsIndivisualIlluminationOn { get { return isIndivisualIlluminationOn; } }

    public bool CanAnimateTexture { get { return canAnimateTexture; } }

    public int Score { get { return score; } }

    private GameObject currentBlock;
    private List<GameObject> blockList = new List<GameObject>();

    private Vector3 mainCameraInitialPosition;
    private Quaternion mainCameraInitialRotation;

    private int currentCamera = 0;
    private int score, hearts = 3;

    private bool isGameOver = false, hasGameStarted = false, isIndivisualIlluminationOn = true, canAnimateTexture = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpawnBlock();
        mainCameraInitialPosition = MainCamera.transform.position;
        mainCameraInitialRotation = MainCamera.transform.rotation;
    }

    void Update()
    {
        if (!isGameOver && hasGameStarted)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && currentBlock != null)
                {
                    currentBlock.transform.parent = transform;
                    currentBlock.GetComponent<Rigidbody>().isKinematic = false;
                    currentBlock = null;
                    Crane.HideCraneBlockConnectors();
                    DestroyLast();
                    StartCoroutine(SpawnBlockAfter(1.5f));
                }
            }
        }
    }

    private void SpawnBlock()
    {
        currentBlock = Instantiate(BlockPrefab, BlockParent);
        Material mat = new Material(currentBlock.GetComponent<Renderer>().material);
        mat.SetTexture("_MainTex", BlockTextureList[Random.Range(0, BlockTextureList.Length)]);
        currentBlock.GetComponent<Renderer>().material = mat;
        //BlockCamera.transform.parent = currentBlock.transform;
        //BlockCamera.transform.localPosition = Vector3.down * 2;
        blockList.Add(currentBlock);
    }

    private void SpawnTNT()
    {
        currentBlock = Instantiate(TNTPrefab, BlockParent);
        //BlockCamera.transform.parent = currentBlock.transform;
        //BlockCamera.transform.localPosition = Vector3.down * 1.5f;
    }

    private void DestroyLast()
    {
        if (blockList.Count > 4)
        {
            GameObject temp = blockList[0];
            blockList.Remove(temp);
            Destroy(temp);
        }
    }

    public void StartGame()
    {
        hasGameStarted = true;
    }

    public void ToggleLights(bool value)
    {
        for (int i = 0; i < Lights.Length; i++)
        {
            Lights[i].SetActive(value);
        }
    }

    public void ToggleCamera()
    {
        MainCamera.depth = currentCamera == 0 ? 1 : 0;
        Rect mainCameraRect = new Rect
        {
            width = currentCamera == 0 ? 0.3f : 1,
            height = currentCamera == 0 ? 0.3f : 1
        };
        MainCamera.rect = mainCameraRect;

        BlockCamera.depth = currentCamera == 0 ? 0 : 1;
        Rect blockCameraRect = new Rect
        {
            width = currentCamera != 0 ? 0.3f : 1,
            height = currentCamera != 0 ? 0.3f : 1
        };
        BlockCamera.rect = blockCameraRect;

        currentCamera = currentCamera == 0 ? 1 : 0;
    }

    public void ResetMainCamera()
    {
        MainCamera.transform.position = mainCameraInitialPosition;
        MainCamera.transform.rotation = mainCameraInitialRotation;
        MainCamera.GetComponent<CameraController>().ResetCameraFrame();
    }

    public void AddScore()
    {
        if (!isGameOver)
        {
            score++;
            UIManager.Instance.SetScoreText(score);
        }
    }

    public void RemoveHeart()
    {
        if (!isGameOver)
        {
            hearts--;
            UIManager.Instance.UpdateHeats(hearts);
            if (hearts <= 0)
            {
                GameOver();
            }
        }
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            UIManager.Instance.ShowGameOver(score);
            isGameOver = true;
        }
    }

    public int GetBlockID(GameObject gameObject)
    {
        return blockList.IndexOf(gameObject);
    }

    public void ToggleIndivisualLight(bool value)
    {
        isIndivisualIlluminationOn = value;
    }

    public void ToggleTextureAnimation(bool value)
    {
        canAnimateTexture = value;
    }

    public void ToggleLampLights(bool value)
    {
        for (int i = 0; i < LampLights.Length; i++)
        {
            LampLights[i].SetActive(value);
        }
    }

    private IEnumerator SpawnBlockAfter(float seconds = 1)
    {
        yield return new WaitForSeconds(seconds);
        Crane.ShowCraneBlockConnectors();
        int rand = Random.Range(0, 4);
        if (rand == 2)
        {
            SpawnTNT();
        }
        else
        {
            SpawnBlock();
        }
    }
}
