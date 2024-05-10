using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject MainMenuPanel;
    public GameObject GamePlayPanel;
    public GameObject GameCompeletePanel;

    public Transform HeartsParent;

    public Crane Crane;
    public TextMeshProUGUI YSliderValue;
    public TextMeshProUGUI SwingSpeedSliderValue;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI GameOverScoreText;
    public TextMeshProUGUI HighestScoreText;

    private const string HIGHESTSCORE = "HIGHESTSCORE";

    private void Awake()
    {
        Instance = this;
        ShowPanel(MainMenuPanel);
    }

    private void ShowPanel(GameObject panel)
    {
        MainMenuPanel.SetActive(false);
        GamePlayPanel.SetActive(false);
        GameCompeletePanel.SetActive(false);

        panel.SetActive(true);
    }

    public void ShowGameOver(int score)
    {
        ShowPanel(GameCompeletePanel);
        int HighestScore = PlayerPrefs.GetInt(HIGHESTSCORE, 0);
        HighestScore = HighestScore < score ? score : HighestScore;
        PlayerPrefs.SetInt(HIGHESTSCORE, HighestScore);
        GameOverScoreText.text = score.ToString();
        HighestScoreText.text = "Highest Score - " + HighestScore.ToString();
    }

    public void OnPlayButtonPressed()
    {
        ShowPanel(GamePlayPanel);
        Controller.Instance.StartGame();
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnYSliderChanged(float value)
    {
        Crane.SetYValue(value);
        YSliderValue.text = value.ToString();
    }

    public void OnSwingSpeedSliderChanged(float value)
    {
        Crane.SetSwingSpeed(value);
        SwingSpeedSliderValue.text = value.ToString();
    }

    public void ToggleAnimation(bool value)
    {
        Crane.SetCanPlayAnimation(value);
    }

    public void ToggleGlobalIllumination(bool value)
    {
        Controller.Instance.ToggleLights(value);
    }

    public void ToggleTexture(bool value)
    {
        Controller.Instance.ToggleTextureAnimation(value);
    }

    public void ToggleBlockIllumination(bool value)
    {
        Controller.Instance.ToggleIndivisualLight(value);
    }

    public void ToggleLampLights(bool value)
    {
        Controller.Instance.ToggleLampLights(value);
    }

    public void OnSwitchCameraPressed()
    {
        Controller.Instance.ToggleCamera();
    }

    public void OnResetCameraPressed()
    {
        Controller.Instance.ResetMainCamera();
    }

    public void SetScoreText(int score)
    {
        ScoreText.text = "Score - " + score.ToString();
    }

    public void UpdateHeats(int hearts)
    {
        for (int i = 2; i >= hearts; i--)
        {
            HeartsParent.GetChild(i).gameObject.SetActive(false);
        }
    }
}
