using UnityEngine;

//vibecoded garbage
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
        // Initialize spawn point
        lastCheckpointPosition = startPoint.position;
        lastCheckpointRotation = startPoint.rotation;
    }

    // Called when hitting a checkpoint
    public void PassCheckpoint(int checkpointIndex, Transform checkpointTransform)
    {
        if (checkpointIndex == nextCheckpointIndex)
        {
            Debug.Log("checkpoint " + checkpointIndex + " passed.");

            // Save respawn point
            lastCheckpointPosition = checkpointTransform.position;
            lastCheckpointRotation = checkpointTransform.rotation;

            nextCheckpointIndex++;

            if (nextCheckpointIndex >= totalCheckpoints)
            {
                Debug.Log("all checkpoints dont. go for finish");
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
}