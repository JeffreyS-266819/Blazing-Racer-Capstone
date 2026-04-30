using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class FinishCanvasScript : MonoBehaviour
{
    public TimerDisplay timer;
    public Canvas winCanvas;
    public TMP_Text TimerTime;
    public TMP_Text FinalDisplayTime;
    public Canvas TimerCanvas;

    public AudioSource RaceMusic;
    public AudioSource WinMusic;
    public AudioSource WinChime;

    private bool hasFinished = false;

    void Start()
    {
        winCanvas.enabled = false;
    }

    void Update()
    {
        if (!hasFinished && timer != null && timer.isRunning == false)
        {
            hasFinished = true;

            winCanvas.enabled = true;
            TimerCanvas.enabled = false;

            RaceMusic.Pause();

            WinMusic.ignoreListenerPause = true;
            WinMusic.Play();

            WinChime.ignoreListenerPause = true;
            WinChime.Play();
            

            Time.timeScale = 0f;
        }

        if (FinalDisplayTime.text != TimerTime.text)
        {
            FinalDisplayTime.text = TimerTime.text;
        }
    }
}