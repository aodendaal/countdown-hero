using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI countdownText;

    [Header("Timer Settings")]
    [SerializeField] private float durationInSeconds = 60f;
    [SerializeField] private bool autoStart = true;

    private float currentTime;
    private bool isRunning;

    private void Start()
    {
        currentTime = durationInSeconds;

        if (autoStart)
        {
            StartTimer();
        }
        else
        {
            UpdateTimerUI();
        }
    }

    private void Update()
    {
        if (!isRunning) return;

        // Count down using unscaledDeltaTime if you want it to run even during pauses,
        // or regular Time.deltaTime to respect Time.timeScale.
        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            OnTimerFinished();
        }

        UpdateTimerUI();
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResetTimer(float newDuration = -1f)
    {
        durationInSeconds = newDuration > 0 ? newDuration : durationInSeconds;
        currentTime = durationInSeconds;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (countdownText == null) return;

        // Calculate whole seconds and remaining milliseconds
        int seconds = (int)currentTime;
        int milliseconds = (int)((currentTime - seconds) * 100);

        // Format: SS:MMM (e.g., "05:042" or "12:800")
        // Use "D2" for 2-digit seconds and "D3" for 3-digit milliseconds
        countdownText.text = $"{seconds:D2}'{milliseconds:D2}";
    }

    private void OnTimerFinished()
    {
        // Add game jam logic here (e.g., trigger game over, enable plug-and-play event)
        Debug.Log("Countdown Finished!");
    }
}