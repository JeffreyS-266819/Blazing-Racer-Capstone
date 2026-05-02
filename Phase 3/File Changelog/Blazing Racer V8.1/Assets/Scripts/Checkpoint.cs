using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        RaceManager rm = FindObjectOfType<RaceManager>();

        if (rm != null)
        {
            rm.PassCheckpoint(checkpointIndex, transform);
        }
    }
}