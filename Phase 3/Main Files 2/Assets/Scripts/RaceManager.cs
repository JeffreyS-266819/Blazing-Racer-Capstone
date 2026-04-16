using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RaceManager : MonoBehaviour
{
    [Header("Setup")]
    public Transform player;
    public Transform startPoint;
    public int totalCheckpoints = 4;
    public int totalLaps = 3;

    [Header("State")]
    private int nextCheckpointIndex = 0;
    private int currentLap = 1;

    private Vector3 lastCheckpointPosition;
    private Quaternion lastCheckpointRotation;

    void Start()
    {
        lastCheckpointPosition = startPoint.position;
        lastCheckpointRotation = startPoint.rotation;
        Time.timeScale = 1f;
    }

    public void PassCheckpoint(int checkpointIndex, Transform checkpointTransform)
    {
        if (checkpointIndex == nextCheckpointIndex)
        {
            Debug.Log("checkpoint " + checkpointIndex + " passed.");

            lastCheckpointPosition = checkpointTransform.position;
            lastCheckpointRotation = checkpointTransform.rotation;

            nextCheckpointIndex++;

            if (nextCheckpointIndex >= totalCheckpoints)
            {
                Debug.Log("all checkpoints done. go for finish");
            }
        }
        else
        {
            Debug.Log("incorrect checkpoint. expected: " + nextCheckpointIndex);
        }
    }

    public void CrossFinishLine()
    {
        if (nextCheckpointIndex >= totalCheckpoints)
        {
            Debug.Log("Lap " + currentLap + " complete!");

            if (currentLap >= totalLaps)
            {
                Debug.Log("race finished");

                var timer = FindObjectOfType<TimerDisplay>();

                if (timer != null)
                {
                    timer.StopTimer();

                    float finalTime = timer.FinalTime;
                    Debug.Log("Final Time: " + finalTime);

                    // SAVE TO DATABASE
                    StartCoroutine(SaveRaceTime(finalTime));
                }
            }
            else
            {
                currentLap++;
                nextCheckpointIndex = 0;

                Debug.Log("starting lap " + currentLap);
            }
        }
        else
        {
            Debug.Log("finish line crossed while missing checkpoint");
        }
    }

    public void RespawnPlayer()
    {
        player.position = lastCheckpointPosition + Vector3.up * 1.5f;
        player.rotation = lastCheckpointRotation;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("Respawned at last checkpoint.");
    }

    IEnumerator SaveRaceTime(float time)
    {
        string url = "http://localhost:3000/saveRace";

        WWWForm form = new WWWForm();
        form.AddField("user_id", UserSession.userId);
        form.AddField("lap_time", time.ToString("F3"));
        form.AddField("date_played", System.DateTime.Now.ToString("yyyy-MM-dd"));

        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error saving time: " + www.error);
        }
        else
        {
            Debug.Log("Race time saved!");
        }
    }
}