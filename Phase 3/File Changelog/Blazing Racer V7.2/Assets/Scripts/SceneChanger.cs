using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public void GoToDevRoomScenes()
    {
        SceneManager.LoadScene("DevRoom");
    }
    
    public void GoToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToMainTrackScene()
    {
        SceneManager.LoadScene("MainTrack");
    }

    public void GoToSignInScreenScene()
    {
        SceneManager.LoadScene("SignInScreen");
    }

}
