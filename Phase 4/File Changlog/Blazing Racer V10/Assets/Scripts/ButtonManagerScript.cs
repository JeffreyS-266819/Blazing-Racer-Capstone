using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManagerScript : MonoBehaviour
{

    public GameObject TutorialCanvas;

    //Give buttons destinations for when they are pressed | -------------------------------------------------------------
    public void GoToDevRoomScenes()
    {
        SceneManager.LoadScene("DevRoom");
    }
    
    public void GoToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void GoToMainTrackScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainTrack");
    }

    public void GoToSignInScreenScene()
    {
        SceneManager.LoadScene("SignInScreen");
    }

    public void ReloadCurrentScene() 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //This give quit button functionallity and also make quit button work will previewing | -------------------------
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game has been eneded");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    //Controls the tutroial canvas buttons | -----------------------------------------------------------------------
    public void OpenTutorial()
    {
        TutorialCanvas.SetActive(true);
    }

    public void ExitTutorial()
    {
        TutorialCanvas.SetActive(false);
    }

}
