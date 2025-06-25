using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;
using TMPro;

public class LoadingManager : MonoBehaviourPunCallbacks
{
    public Slider loadingBar; 
    public TMP_Text statusText;   

    private string homeSceneName = "HomeScene"; 

    void Start()
    {
        StartCoroutine(LoadHomeScene());
    }

    IEnumerator LoadHomeScene()
    {
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(homeSceneName);
        asyncLoad.allowSceneActivation = false; 

        
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); 
            loadingBar.value = progress;
            statusText.text = $"Loading... {Mathf.RoundToInt(progress * 100)}%";

            if (progress >= 1f && PhotonNetwork.IsConnectedAndReady)
            {
                statusText.text = "Connected! Entering the world...";
                yield return new WaitForSeconds(1f);
                asyncLoad.allowSceneActivation = true; 
            }

            yield return null;
        }
    }

    
    public override void OnConnectedToMaster()
    {
        
    }

    
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        statusText.text = "Connection failed. Retrying...";
        PhotonNetwork.ConnectUsingSettings();
    }
}
