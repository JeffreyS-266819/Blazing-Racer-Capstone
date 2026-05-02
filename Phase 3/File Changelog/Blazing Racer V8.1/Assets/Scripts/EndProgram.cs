using UnityEngine;

public class EndProgram : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game has been eneded");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
