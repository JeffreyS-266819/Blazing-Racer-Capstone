using UnityEngine;
using UnityEngine.InputSystem;

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
		
		#Currently only works with controller.
		if (Gamepad.current.buttonNorth.isPressed)
		{
        var raceManager = FindObjectOfType<RaceManager>();
        if (raceManager != null)
        {
            raceManager.RespawnPlayer();
        }
    }
	}
}