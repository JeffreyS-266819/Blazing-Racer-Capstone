using TMPro;
using UnityEngine;

public class FinishCanvasScript : MonoBehaviour
{
    public TimerDisplay timer;
    public Canvas winCanvas;
    public TMP_Text TimerTime;
    public TMP_Text FinalDisplayTime;
    public Canvas TimerCanvas;

    void Start()
    {
        winCanvas.enabled = false;
        Debug.Log("Timer running: " + timer.isRunning);
    }

    void Update()
    {
        if (timer != null && timer.isRunning == false)
        {
            winCanvas.enabled = true;
            Time.timeScale = 0f;
            TimerCanvas.enabled = false;
        }

        if (FinalDisplayTime.text != TimerTime.text)
        {
            FinalDisplayTime.text = TimerTime.text;
        }
    }

}