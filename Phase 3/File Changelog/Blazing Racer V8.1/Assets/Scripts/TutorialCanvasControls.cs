using UnityEngine;

public class TutorialCanvasControls : MonoBehaviour
{
    public GameObject movementControls;
    public GameObject turnControls;
    public GameObject otherControls;

    public GameObject objective;
    private int userLocation = 0;

    public void nextText()
    {
        userLocation++;

        if (userLocation == 1)
        {
            movementControls.SetActive(false);
            turnControls.SetActive(true);
        }

        if (userLocation == 2)
        {
            turnControls.SetActive(false);
            otherControls.SetActive(true);
        }

        if (userLocation == 3)
        {
            otherControls.SetActive(false);
            objective.SetActive(true);
        }

        if (userLocation == 4)
        {
            objective.SetActive(false);
            movementControls.SetActive(true);
            userLocation = 0;
        }
    }

    public void tutorialExit()
    {
        movementControls.SetActive(true);
        turnControls.SetActive(false);
        otherControls.SetActive(false);
        userLocation = 0;
    }
}
