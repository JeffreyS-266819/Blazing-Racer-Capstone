using UnityEngine;

public class CarRespawn : MonoBehaviour
{
    public float fallThreshold = -100f;

    private RaceManager raceManager;

    void Start()
    {
        raceManager = FindObjectOfType<RaceManager>();
    }

    void Update()
    {
        if (transform.position.y < fallThreshold)
        {
			Debug.Log("Fall Out");
            raceManager.RespawnPlayer();
        }
    }
}