using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        RaceManager rm = FindObjectOfType<RaceManager>();

        if (rm != null)
        {
            rm.CrossFinishLine();
        }
    }
}
/*
public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Finish line crossed!");

            // Example: stop timer
            FindObjectOfType<TimerDisplay>().enabled = false;

            // You could also trigger race end logic here
        }
    }
}*/

/* //Test
public class FinishLine : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("TRIGGER HIT by: " + other.name);
	}
}*/