using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    [System.Serializable]
    public class LeaderboardEntry
    {
        public string username;
        public float lap_time;
    }

    [System.Serializable]
    public class LeaderboardList
    {
        public List<LeaderboardEntry> entries;
    }

    public TMP_Text leaderboardText;
    public string leaderboardUrl = "http://localhost:3000/leaderboard";

    void Start()
    {
        leaderboardText.text = "Global Leader Board\n\nLoading...";
        StartCoroutine(GetLeaderboard());
    }

    IEnumerator GetLeaderboard()
    {
        UnityWebRequest request = UnityWebRequest.Get(leaderboardUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            leaderboardText.text = "Global Leader Board\n\nFailed to load leaderboard.";
            Debug.LogError("Leaderboard error: " + request.error);
            yield break;
        }

        string json = request.downloadHandler.text;

        // Wrap the JSON array so JsonUtility can read it
        string wrappedJson = "{ \"entries\": " + json + " }";
        LeaderboardList leaderboard = JsonUtility.FromJson<LeaderboardList>(wrappedJson);

        leaderboardText.text = "Global Leader Board\n\n";

        if (leaderboard == null || leaderboard.entries == null || leaderboard.entries.Count == 0)
        {
            leaderboardText.text += "No scores yet.";
        }
        else
        {
            foreach (LeaderboardEntry entry in leaderboard.entries)
            {
                leaderboardText.text += entry.username + " - " + entry.lap_time.ToString("F2") + "\n";
            }
        }
    }
}