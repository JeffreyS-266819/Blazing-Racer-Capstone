using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManagerScript : MonoBehaviour
{

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
        SceneManager.LoadScene("MainTrack");
    }

    public void GoToSignInScreenScene()
    {
        SceneManager.LoadScene("SignInScreen");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game has been eneded");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
