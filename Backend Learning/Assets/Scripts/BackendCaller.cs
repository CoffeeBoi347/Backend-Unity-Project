using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class WebMessage
{
    public string message;
    public int score;
    public string status;
}

public class BackendCaller : MonoBehaviour
{
    public TMP_Text messageText;
    public BackendConfig config;

    public WebMessage webMessage;

    [Header("Debug")] 
    public string message;
    public int score;
    public string status;

    private void Start()
    {
        StartCoroutine(CallBackend(config.urlPath, config.requestMethod));
    }

    private IEnumerator ReceiveFromBackend(string pathName, string requestMethod)
    {
        UnityWebRequest webRequest = new UnityWebRequest(pathName, requestMethod);
        byte[]
        webRequest.uploadHandler = new UploadHandlerRaw();
    }
    private IEnumerator CallBackend(string pathName, string requestMethod)
    {
        UnityWebRequest webRequest = new UnityWebRequest(pathName, requestMethod);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json"); // accepts only json data

        yield return webRequest.SendWebRequest();

        if(webRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Web Request SUCCESS.");
            webMessage = JsonUtility.FromJson<WebMessage>(webRequest.downloadHandler.text);
            message = webMessage.message;
            score = webMessage.score;
            status = webMessage.status;

            messageText.text = $"Message: {message}. Score: {score}. Status: {status}.";
        }

        else
        {
            messageText.text = webRequest.result.ToString();
        }
    }
}