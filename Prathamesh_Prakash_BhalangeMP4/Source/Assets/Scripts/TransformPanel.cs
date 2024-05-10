using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransformPanel : MonoBehaviour
{

    public GameObject SelectedGameobject;
    public TextMeshProUGUI SelectedText;

    public Slider XSlider;
    public TextMeshProUGUI XSliderValue;
    public Slider YSlider;
    public TextMeshProUGUI YSliderValue;
    public Slider ZSlider;
    public TextMeshProUGUI ZSliderValue;

    private TransformType currentTransformType;

    private Dictionary<TransformType, TransformTypeData> transformTypeDatas = new Dictionary<TransformType, TransformTypeData>();

    private TransformTypeData bigLimits = new TransformTypeData(-180, 180, 0);

    private void Awake()
    {
        transformTypeDatas.Add(TransformType.Position, new TransformTypeData(-20, 20, 0));
        transformTypeDatas.Add(TransformType.Rotation, new TransformTypeData(-180, 180, 0));

        initTransformData();
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
        initTransformData();
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