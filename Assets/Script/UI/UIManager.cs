using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text goldText;
    public Slider healthSlider;
    public Text waveText;
    public Text timerText;
    public Button pauseButton;
    public Button resumeButton;

    private int currentGold = 0;
    private int currentWave = 1;
    private float currentTime = 0f;

    void Start()
    {
        // Initialize UI elements
        UpdateGold(currentGold);
        UpdateWave(currentWave);
        UpdateTimer(currentTime);
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
    }

    public void UpdateGold(int goldAmount)
    {
        goldText.text = "Gold: " + goldAmount;
    }

    public void UpdateWave(int waveNumber)
    {
        waveText.text = "Wave: " + waveNumber;
    }

    public void UpdateHealth(float healthPercentage)
    {
        healthSlider.value = healthPercentage;
    }

    public void UpdateTimer(float timeRemaining)
    {
        timerText.text = "Time: " + Mathf.Max(0, Mathf.Round(timeRemaining));
    }

    public void OnPauseButtonClicked()
    {
        Time.timeScale = 0; // Pause game
    }

    public void OnResumeButtonClicked()
    {
        Time.timeScale = 1; // Resume game
    }

    void Update()
    {
        // Update the timer
        currentTime -= Time.deltaTime;
        UpdateTimer(currentTime);
    }
}
