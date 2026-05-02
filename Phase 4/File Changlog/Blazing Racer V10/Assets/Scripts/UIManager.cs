using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject createAccountPanel;

    [Header("Login Inputs")]
    public TMP_InputField loginUsernameInput;
    public TMP_InputField loginPasswordInput;
    public TMP_Text loginStatusText;

    [Header("Create Inputs")]
    public TMP_InputField createUsernameInput;
    public TMP_InputField createPasswordInput;
    public TMP_Text createStatusText;

    [Header("Server URLs")]
    public string loginURL = "http://localhost:3000/login";
    public string createURL = "http://localhost:3000/create";

    private void Start()
    {
        ShowLogin();
    }

    // ---------------- LOGIN ----------------
    public void SignIn()
    {
        StartCoroutine(LoginRequest());
    }

    IEnumerator LoginRequest()
    {
        if (string.IsNullOrEmpty(loginUsernameInput.text) || string.IsNullOrEmpty(loginPasswordInput.text))
        {
            loginStatusText.text = "Enter username and password";
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("username", loginUsernameInput.text);
        form.AddField("password", loginPasswordInput.text);

        UnityWebRequest www = UnityWebRequest.Post(loginURL, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            loginStatusText.text = "Server connection error";
        }
        else
        {
            string response = www.downloadHandler.text;
            loginStatusText.text = response;

            // EXPECT: SUCCESS|userId
            if (response.StartsWith("SUCCESS"))
            {
                string[] parts = response.Split('|');

                UserSession.userId = int.Parse(parts[1]);
                UserSession.username = loginUsernameInput.text;

                Debug.Log("Logged in user ID: " + UserSession.userId);

                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    // ---------------- CREATE ACCOUNT ----------------
    public void CreateAccount()
    {
        StartCoroutine(CreateRequest());
    }

    IEnumerator CreateRequest()
    {
        if (string.IsNullOrEmpty(createUsernameInput.text) || string.IsNullOrEmpty(createPasswordInput.text))
        {
            createStatusText.text = "Enter username and password";
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("username", createUsernameInput.text);
        form.AddField("password", createPasswordInput.text);

        UnityWebRequest www = UnityWebRequest.Post(createURL, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            createStatusText.text = "Server connection error";
        }
        else
        {
            createStatusText.text = www.downloadHandler.text;
        }
    }

    // ---------------- PANEL SWITCHING ----------------
    public void ShowLogin()
    {
        loginPanel.SetActive(true);
        createAccountPanel.SetActive(false);
        loginStatusText.text = "";
        loginUsernameInput.text = "";
        loginPasswordInput.text = "";
    }

    public void ShowCreateAccount()
    {
        loginPanel.SetActive(false);
        createAccountPanel.SetActive(true);
        createStatusText.text = "";
        createUsernameInput.text = "";
        createPasswordInput.text = "";
    }

    // ---------------- LOGOUT ----------------
    public void Logout()
    {
        UserSession.userId = 0;
        UserSession.username = "";

        ShowLogin();

        Debug.Log("User logged out");
    }
}