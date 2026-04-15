using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialWarning : MonoBehaviour
{
    public GameObject WarningCanvas;
    public int OpenTutorial = 0;

    public void OpenedTutorial()
    {
        OpenTutorial = 1;
    }

    public void TutorialManager()
    {
        if (OpenTutorial != 1)
        {
            WarningCanvas.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("MainTrack");
        }
    }

    public void ExitWarning()
    {
        WarningCanvas.SetActive(false);
    }

    public void IgnoreWarning()
    {
        SceneManager.LoadScene("MainTrack");
    }

}
