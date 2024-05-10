using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public Node RootNode;

    public GameObject SelectedGameobject;
    public TextMeshProUGUI SelectedText;

    public Slider XSlider;
    public TextMeshProUGUI XSliderValue;
    public Slider YSlider;
    public TextMeshProUGUI YSliderValue;
    public Slider ZSlider;
    public TextMeshProUGUI ZSliderValue;

    public TMP_Dropdown NodeDropDown;

    public GameObject AxisCheckbox;

    private TransformType currentTransformType = TransformType.Rotation;

    private List<TMP_Dropdown.OptionData> dropDownOptionData = new List<TMP_Dropdown.OptionData>();
    private List<Transform> nodeList = new List<Transform>();

    private Dictionary<TransformType, TransformTypeData> transformTypeDatas = new Dictionary<TransformType, TransformTypeData>();

    private TransformTypeData bigLimits = new TransformTypeData(-180, 180, 0);

    private void Start()
    {
        transformTypeDatas.Add(TransformType.Position, new TransformTypeData(-10, 10, 0));
        transformTypeDatas.Add(TransformType.Rotation, new TransformTypeData(-180, 180, 0));
        transformTypeDatas.Add(TransformType.Scale, new TransformTypeData(0.1f, 5, 1));

        InitDropDownData();
        SetSelectedGameObject(RootNode.gameObject);
    }

    public void SetSelectedGameObject(GameObject selectedObject)
    {
        SelectedGameobject = selectedObject;

        if (selectedObject != null)
        {
            SelectedText.text = "Selected: " + selectedObject.name;
        }
        else
        {
            SelectedText.text = "Selected: None";
        }
        Controller.Instance.SetSelectedGameObject(selectedObject);
        initTransformData();
    }

    public void ShowAxis()
    {
        AxisCheckbox.SetActive(!AxisCheckbox.activeInHierarchy);
        Controller.Instance.SetAxisEnabled(AxisCheckbox.activeInHierarchy);
    }

    public void InitDropDownData()
    {
        dropDownOptionData.Add(new TMP_Dropdown.OptionData(RootNode.transform.name));
        nodeList.Add(RootNode.transform);
        AddChildrenData("", RootNode);
        NodeDropDown.AddOptions(dropDownOptionData);
    }

    private void AddChildrenData(string prefix, Node node)
    {
        string space = prefix + ".";
        List<Node> children = node.ChildrenList;
        for (int i = 0; i < children.Count; i++)
        {
            TMP_Dropdown.OptionData d = new TMP_Dropdown.OptionData(space + children[i].transform.name);
            dropDownOptionData.Add(d);
            nodeList.Add(children[i].transform);
            AddChildrenData(space, children[i]);
        }
    }

    public void OnDropDownValueChanged(int value)
    {
        SetSelectedGameObject(nodeList[value].gameObject);
    }

    private void initTransformData()
    {
        TransformTypeData transformData = transformTypeDatas[currentTransformType];

        XSlider.minValue = YSlider.minValue = ZSlider.minValue = bigLimits.MinLimit;
        XSlider.maxValue = YSlider.maxValue = ZSlider.maxValue = bigLimits.MaxLimit;

        if (SelectedGameobject != null)
        {
            Vector3 vectorData = getVectorData();

            if (currentTransformType == TransformType.Rotation)
            {
                XSliderValue.text = (XSlider.value = 0).ToString();
                YSliderValue.text = (YSlider.value = 0).ToString();
                ZSliderValue.text = (ZSlider.value = 0).ToString();
            }
            else
            {
                XSliderValue.text = (XSlider.value = vectorData.x).ToString();
                YSliderValue.text = (YSlider.value = vectorData.y).ToString();
                ZSliderValue.text = (ZSlider.value = vectorData.z).ToString();
            }
        }
        else
        {
            XSliderValue.text = (XSlider.value = transformData.DefaultValue).ToString();
            YSliderValue.text = (YSlider.value = transformData.DefaultValue).ToString();
            ZSliderValue.text = (ZSlider.value = transformData.DefaultValue).ToString();
        }

        XSlider.minValue = YSlider.minValue = ZSlider.minValue = transformData.MinLimit;
        XSlider.maxValue = YSlider.maxValue = ZSlider.maxValue = transformData.MaxLimit;
    }

    private Vector3 getVectorData()
    {
        if (currentTransformType == TransformType.Position)
        {
            return SelectedGameobject.transform.localPosition;
        }
        else if (currentTransformType == TransformType.Scale)
        {
            return SelectedGameobject.transform.localScale;
        }
        else
        {
            return SelectedGameobject.transform.localRotation.eulerAngles;
        }
    }

    public void OnPositionBtnPressed()
    {
        currentTransformType = TransformType.Position;
        initTransformData();
    }

    public void OnScaleBtnPressed()
    {
        currentTransformType = TransformType.Scale;
        initTransformData();
    }

    public void OnRotationBtnPressed()
    {
        currentTransformType = TransformType.Rotation;
        initTransformData();
    }


    public void OnXSliderChanged(float value)
    {
        XSliderValue.text = value.ToString();
        Controller.Instance.SetXValue(value, currentTransformType);
    }

    public void OnYSliderChanged(float value)
    {
        YSliderValue.text = value.ToString();
        Controller.Instance.SetYValue(value, currentTransformType);
    }

    public void OnZSliderChanged(float value)
    {
        ZSliderValue.text = value.ToString();
        Controller.Instance.SetZValue(value, currentTransformType);
    }

    public void OnResetButtonPresed()
    {
        SceneManager.LoadScene(0);
    }
}

public enum TransformType
{
    Position,
    Scale,
    Rotation
}

public class TransformTypeData
{
    public float MinLimit;
    public float MaxLimit;
    public float DefaultValue;

    public TransformTypeData(float minLimit, float maxLimit, float defaultValue)
    {
        MinLimit = minLimit;
        MaxLimit = maxLimit;
        DefaultValue = defaultValue;
    }
}