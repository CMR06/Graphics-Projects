using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XfromControl : MonoBehaviour
{
    public MyMesh myMesh;
    public TexturePlacement myTexture;

    public Toggle T, R, S;
    public SliderWithEcho X, Y, Z;
    public SliderWithEcho CylinderRotSlider, CylinderResXSlider, CylinderResYSlider, ResXSlider, ResYSlider;
    public TMP_Dropdown dropdown;

    private Vector3 translationValues = Vector3.zero;
    private Vector3 rotationValues = Vector3.zero;
    private Vector3 scalingValues = Vector3.one;

    // Use this for initialization
    void Start()
    {
        dropdown.onValueChanged.AddListener(HandleDropdownClick);

        T.onValueChanged.AddListener(SetToTranslation);
        R.onValueChanged.AddListener(SetToRotation);
        S.onValueChanged.AddListener(SetToScaling);

        X.SetSliderListener(XValueChanged);
        Y.SetSliderListener(YValueChanged);
        Z.SetSliderListener(ZValueChanged);

        CylinderRotSlider.SetSliderListener(HandleCylinderRot);
        CylinderResXSlider.SetSliderListener(HandleCylinderResX);
        CylinderResYSlider.SetSliderListener(HandleCylinderResY);
        ResXSlider.SetSliderListener(HandleResX);
        ResYSlider.SetSliderListener(HandleResY);

        CylinderRotSlider.InitSliderRange(10, 360, 275);
        CylinderResXSlider.InitSliderRange(4, 20, 10);
        CylinderResYSlider.InitSliderRange(4, 20, 10);
        ResXSlider.InitSliderRange(2, 20, 2);
        ResYSlider.InitSliderRange(2, 20, 2);

        translationValues = new Vector3(0, 0, 0);
        rotationValues = new Vector3(0, 0, 0);
        scalingValues = new Vector3(1f, 1f, 1f);

        T.isOn = true;
        R.isOn = false;
        S.isOn = false;
        SetToTranslation(true);
    }

    void HandleDropdownClick(int val)
    {
        string selectedOption = dropdown.options[val].text;
        myMesh?.GetDropdownOption(selectedOption);
    }
    //---------------------------------------------------------------------------------

    // resopond to sldier bar value changes
    void HandleCylinderRot(float v)
    {
        myMesh?.GetCylinderRotation(v);
    }

    void HandleCylinderResX(float v)
    {
        myMesh?.GetCylinderResolutionX(v);
    }
    void HandleCylinderResY(float v)
    {
        myMesh?.GetCylinderResolutionY(v);
    }

    void HandleResX(float v)
    {
        myMesh?.GetRowSize(v);
    }
    void HandleResY(float v)
    {
        myMesh?.GetColSize(v);
    }

    void SetToTranslation(bool v)
    {
        if (!v) return;
        T.isOn = true; R.isOn = false; S.isOn = false;

        X.InitSliderRange(-4, 4, translationValues.x);
        Y.InitSliderRange(-4, 4, translationValues.y);
        Z.InitSliderRange(-4, 4, translationValues.z);

        X.EnableSlider(); Y.EnableSlider(); Z.DisableSlider();
    }

    void SetToScaling(bool v)
    {
        if (!v) return;
        T.isOn = false; R.isOn = false; S.isOn = true;

        X.InitSliderRange(0.1f, 10, scalingValues.x);
        Y.InitSliderRange(0.1f, 10, scalingValues.y);
        Z.InitSliderRange(0.1f, 10, scalingValues.z);

        X.EnableSlider(); Y.EnableSlider(); Z.DisableSlider();
    }

    void SetToRotation(bool v)
    {
        if (!v) return;
        T.isOn = false; R.isOn = true; S.isOn = false;
        Z.InitSliderRange(-180, 180, rotationValues.z);

        X.DisableSlider(); Y.DisableSlider(); Z.EnableSlider();
    }

    void XValueChanged(float v)
    {
        if (T.isOn)
        {
            translationValues.x = v;
            UISetObjectXform();
        }
        else if (S.isOn)
        {
            scalingValues.x = v;
            UISetObjectXform();
        }
    }

    void YValueChanged(float v)
    {
        if (T.isOn)
        {
            translationValues.y = v;
            UISetObjectXform();
        }
        else if (S.isOn)
        {
            scalingValues.y = v;
            UISetObjectXform();
        }
    }

    void ZValueChanged(float v)
    {
        if (R.isOn)
        {
            rotationValues.z = v;
            UISetObjectXform();
        }
    }

    private void UISetObjectXform()
    {
        if (T.isOn)
        {
            myTexture.GetSliderValues(translationValues, "translate");
        }
        else if (S.isOn)
        {
            myTexture.GetSliderValues(scalingValues, "scale");
        }
        else
        {
            myTexture.GetSliderValues(rotationValues, "rotate");
        }
    }

}