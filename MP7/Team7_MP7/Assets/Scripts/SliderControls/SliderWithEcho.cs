using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Text Mesh Pro

public class SliderWithEcho : MonoBehaviour
{

    public Slider TheSlider = null;
    public TextMeshProUGUI TheEcho = null;
    public TextMeshProUGUI TheLabel = null;

    public delegate void SliderCallbackDelegate(float v);      // defined a new data type
    private SliderCallbackDelegate mCallBack = null;           // private instance of the data type


    // Use this for initialization
    void Start()
    {
        Debug.Assert(TheSlider != null);
        Debug.Assert(TheEcho != null);
        Debug.Assert(TheLabel != null);

        if (IsDifferentSlider())
        {
            TheSlider.onValueChanged.AddListener(SliderValChange);
        }
        else TheSlider.onValueChanged.AddListener(SliderValueChange);
    }

    private bool IsDifferentSlider()
    {
        if (TheSlider.transform.name == "CylinderRotSlider" || TheSlider.transform.name == "CylinderResXSlider" || TheSlider.transform.name == "CylinderResYSlider" || TheSlider.transform.name == "ResXSlider" || TheSlider.transform.name == "ResYSlider")
            return true;
        else return false;
    }

    public void SetSliderListener(SliderCallbackDelegate listener)
    {
        mCallBack = listener;
    }

    public void DisableSlider()
    {
        TheSlider.interactable = false;
    }
    public void EnableSlider()
    {
        TheSlider.interactable = true;
    }

    // GUI element changes the object
    void SliderValueChange(float v)
    {
        TheEcho.text = v.ToString("F4");
        if (mCallBack != null)
            mCallBack(v);
    }

    void SliderValChange(float v)
    {
        TheEcho.text = v.ToString("F0");
        if (mCallBack != null)
            mCallBack(v);
    }

    public float GetSliderValue() { return TheSlider.value; }
    public void SetSliderLabel(string l) { TheLabel.text = l; }
    public void SetSliderValue(float v)
    {
        TheSlider.value = v;
        if (IsDifferentSlider()) SliderValChange(v);
        else SliderValueChange(v);
    }

    public void InitSliderRange(float min, float max, float v)
    {
        TheSlider.minValue = min;
        TheSlider.maxValue = max;
        SetSliderValue(v);
    }

}