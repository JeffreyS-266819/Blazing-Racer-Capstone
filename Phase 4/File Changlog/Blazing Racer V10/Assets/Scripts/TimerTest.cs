using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float elapsedTime = 0f;
    public bool isRunning = true;    
    // Exposed read-only property
    public float FinalTime => elapsedTime;


    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000f) % 1000f);

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}",
            minutes, seconds, milliseconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}