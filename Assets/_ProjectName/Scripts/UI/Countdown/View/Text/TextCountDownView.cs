using TMPro;
using UnityEngine;

public class TextCountDownView : UIViewBase, ICountDownView
{
    [SerializeField] private TMP_Text timerText;

    [Header("Settings")]
    [SerializeField] private bool isCountDownMode = false;
    [SerializeField] private bool showHours = false;
    [SerializeField] private bool showMinutes = true;
    [SerializeField] private bool showSeconds = true;
    [SerializeField] private bool showMilliseconds = false;

    private float maxTime;

    protected override void Awake()
    {
        id = UI.TextCountDown;    
    }

    public override void Initialize(params object[] parameters)
    {
        maxTime = (float)parameters[0];
        UpdateTimer(maxTime);
    }

    public void UpdateTimer(float timeRemaining)
    {
        if (isCountDownMode)
            HandleCountDownMode(timeRemaining);
        else
            HandleGeneralTimerMode(timeRemaining);
    }

    private void HandleCountDownMode(float timeRemaining)
    {
        if (timeRemaining > 0f)
        {
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            timerText.text = seconds.ToString();

            if (seconds == 0)
                timerText.text = "GO!"; // TODO: Localization System
        }
    }

    private void HandleGeneralTimerMode(float timeRemaining)
    {
        int hours = Mathf.FloorToInt(timeRemaining / 3600f);
        int minutes = Mathf.FloorToInt((timeRemaining % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        int milliseconds = Mathf.FloorToInt((timeRemaining * 1000f) % 1000f);

        string timeFormatted = "";

        if (showHours && hours > 0)
            timeFormatted += string.Format("{0:00}:", hours);

        if (showMinutes)
            timeFormatted += string.Format("{0:00}:", minutes);

        if (showSeconds)
            timeFormatted += string.Format("{0:00}", seconds);

        if (showMilliseconds)
        {
            if (timeFormatted.Length > 0)
                timeFormatted += string.Format(":{0:000}", milliseconds);
            else
                timeFormatted += string.Format("{0:000}", milliseconds);
        }

        timerText.text = timeFormatted.TrimEnd(':');
    }

    public override void Conclude()
    {
        timerText.text = "";
    }
}
