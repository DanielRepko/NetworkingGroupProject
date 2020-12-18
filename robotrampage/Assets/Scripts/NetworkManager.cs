using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;
using System;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager _instance = null;

    public int TokenID;

    public static NetworkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<NetworkManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("NetworkManager");
                    _instance = go.AddComponent<NetworkManager>();
                }
            }
            return _instance;
        }
    }

    public IEnumerator LoginRequest(string uri, string name)
    {
        LoginRequestsData myData = new LoginRequestsData();
        myData.Name = name;

        UnityWebRequest req = new UnityWebRequest(uri);
        req.method = UnityWebRequest.kHttpVerbPOST;
        req.downloadHandler = new DownloadHandlerBuffer();

        string myObjectAsJSON = JsonUtility.ToJson(myData);
        byte[] bytes = Encoding.ASCII.GetBytes(myObjectAsJSON);

        UploadHandlerRaw uH = new UploadHandlerRaw(bytes);
        req.uploadHandler = uH;

        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        Debug.Log("Response Code:" + req.responseCode);
        if (req.isNetworkError)
        {
            Debug.LogError("Error: " + req.error);
        }
        else
        {
            Debug.Log("Response Recieved");
            LoginResponsesData myJSON = JsonUtility.FromJson<LoginResponsesData>(req.downloadHandler.text);
            TokenID = myJSON.TokenID;
        }
    }

    public IEnumerator SetScoreRequest(string uri, string name, int score, Action<bool> callback)
    {
        SetScoreRequestData myData = new SetScoreRequestData();
        myData.Score = score;

        UnityWebRequest req = new UnityWebRequest(uri);
        req.method = UnityWebRequest.kHttpVerbPOST;
        req.downloadHandler = new DownloadHandlerBuffer();

        string myObjectAsJSON = JsonUtility.ToJson(myData);
        byte[] bytes = Encoding.ASCII.GetBytes(myObjectAsJSON);

        UploadHandlerRaw uH = new UploadHandlerRaw(bytes);
        req.uploadHandler = uH;

        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("TokenID", TokenID+"");
        req.SetRequestHeader("Name", name);

        yield return req.SendWebRequest();

        Debug.Log("Response Code:" + req.responseCode);
        if (req.isNetworkError)
        {
            Debug.LogError("Error: " + req.error);
        }
        else
        {
            Debug.Log("Response Recieved");
            SetScoreResponseData myJSON = JsonUtility.FromJson<SetScoreResponseData>(req.downloadHandler.text);
            callback.Invoke(myJSON.IsHighScore);
        }
    }

    public IEnumerator GetHighScoreRequest(string uri, string name)
    {
        UnityWebRequest req = new UnityWebRequest(uri);
        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
        req.downloadHandler = dH;

        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("TokenID", TokenID + "");
        req.SetRequestHeader("Name", name);

        yield return req.SendWebRequest();

        Debug.Log("Response Code:" + req.responseCode);

        if (req.isNetworkError)
        {
            Debug.LogError("Error: " + req.error);
        }
        else
        {
            Debug.Log("Response Recieved");
            GetHighScoreResponseData myJSON = JsonUtility.FromJson<GetHighScoreResponseData>(req.downloadHandler.text);
            Game.instance.SetHighScoreText("So close! The highscore to beat is: " + myJSON.Highscore);
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
