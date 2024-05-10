using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI IntervalValueText;
    public TextMeshProUGUI SpeedValueText;
    public TextMeshProUGUI AliveValueText;

    private void Awake()
    {
        Instance = this;
    }

    public void OnIntervalSliderChange(float value)
    {
        Controller.Instance.SpawnManager.Interval = value;
        IntervalValueText.text = value.ToString();
    }

    public void OnSpeedSliderChange(float value)
    {
        Controller.Instance.SpawnManager.Speed = value;
        SpeedValueText.text = value.ToString();
    }

    public void OnAliveSliderChange(float value)
    {
        Controller.Instance.SpawnManager.AliveSeconds = value;
        AliveValueText.text = value.ToString();
    }

}
