using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class LeaderboardEntry
{
    public string name;
    public int score;
}

[System.Serializable]
public class LeaderboardResponse
{
    public string message;
    public LeaderboardEntry[] leaderboard;
}

public class LeaderboardManager : MonoBehaviour
{
    public BackendConfig config;
    public TMP_Text messageText;

    private void Start()
    {
        StartCoroutine(SendLeaderboardEntryRequest("Alia", 2600));
        StartCoroutine(GetLeaderboard());

    }

    private IEnumerator SendLeaderboardEntryRequest(string playerName, int playerScore)
    {
        string json = JsonUtility.ToJson(new LeaderboardEntry { name = playerName, score = playerScore });
        UnityWebRequest webRequest = new UnityWebRequest($"{config.urlPath}/submit", "POST");

        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if(webRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Submitted Score: {webRequest.downloadHandler.text}");
        }

        else
        {
            Debug.Log($"404 Error: {webRequest.downloadHandler.error}");
        }
    }

    private IEnumerator GetLeaderboard()
    {
        UnityWebRequest webRequest = new UnityWebRequest($"{config.urlPath}/leaderboard", "GET");
        webRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return webRequest.SendWebRequest();

        if(webRequest.result == UnityWebRequest.Result.Success)
        {
            LeaderboardResponse request = JsonUtility.FromJson<LeaderboardResponse>(webRequest.downloadHandler.text);
            string response = $"{request.message}";

            foreach(var entry in request.leaderboard)
            {
                response += $"{entry.name} : {entry.score}";
            }

            messageText.text = response;
        }

        else
        {
            messageText.text = webRequest.downloadHandler.error;
        }
    }
}