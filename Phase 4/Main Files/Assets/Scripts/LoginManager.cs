using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text statusText;

    string serverURL = "http://localhost:3000/login";

    public void SignIn()
    {
        StartCoroutine(LoginRequest());
    }

    IEnumerator LoginRequest()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (username == "" || password == "")
        {
            statusText.text = "Enter username and password";
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(serverURL, form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            statusText.text = "Server connection error";
        }
        else
        {
            string response = www.downloadHandler.text;

            statusText.text = response;

            if (response == "Login successful")
            {
                SceneManager.LoadScene("GameScene");
            }
        }
    }
}