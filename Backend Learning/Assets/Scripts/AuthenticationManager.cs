using System.Collections;
using TMPro;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class UserEntry
{
    public string username;
    public string password;
    public string token;
}

[System.Serializable]
public class Credentials
{
    public string username;
    public string password;
}


public class AuthenticationManager : MonoBehaviour
{
    public BackendConfig config;
    public TMP_Text messageText;

    private void Start()
    {
        StartCoroutine(RegisterUser("Aayush", "Worldsarenice"));
    }

    private IEnumerator RegisterUser(string user, string pass)
    {
        string json = JsonUtility.ToJson( new Credentials{username = user, password = pass} );
        using UnityWebRequest request = new UnityWebRequest($"{config.urlPath}/register", "POST");

        request.downloadHandler = new DownloadHandlerBuffer();
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if(request.result == UnityWebRequest.Result.Success)
        {
            messageText.text = request.downloadHandler.text;
            StartCoroutine(LoginUser(user, pass));
        }

        else
        {
            messageText.text = request.error;
        }
    }

    private IEnumerator LoginUser(string user, string pass)
    {
        string json = JsonUtility.ToJson(new Credentials{ username = user, password = pass });
        using UnityWebRequest request = new UnityWebRequest($"{config.urlPath}/login", "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        UserEntry userEntry = JsonUtility.FromJson<UserEntry>(request.downloadHandler.text);
        Debug.Log($"Token: {userEntry.token}");

        if (request.result == UnityWebRequest.Result.Success)
        {
            messageText.text = request.downloadHandler.text;
        }

        else
        {
            messageText.text = request.error;
        }
    }
}