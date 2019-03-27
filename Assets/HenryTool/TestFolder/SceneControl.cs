using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using HenryTool;

public class SceneControl : MonoBehaviour
{
    public NetworkManagerHUD hud;
    public NetworkManager nManager;
    public LogStringVariable errorLog;

    void Start() {
        nManager = gameObject.GetComponent<NetworkManager>();
        hud = gameObject.GetComponent<NetworkManagerHUD>();
        errorLog.AddLogLine("");

        //errorLog.AddLogLine("" + Network.player.ipAddress);
        errorLog.AddLogLine("" + IPManager.GetIPv4());
        errorLog.AddLogLine("" + Application.persistentDataPath);
        errorLog.AddLogLine("streamingAssetsPath: " + Application.streamingAssetsPath);


    }

    void Update() {
        if (SceneManager.GetActiveScene().name == "Lobby") {
            hud.offsetX = Screen.width / 2 - 100; ;
            hud.offsetY = Screen.height - 200;
        }
        else {
            hud.offsetX = Screen.width - 215; ;
            hud.offsetY = 50;
        }
    }


    private void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Host")) {
            if (nManager != null) {
                nManager.StartHost();
            }

        }

        if (GUI.Button(new Rect(200, 10, 150, 100), "Client")) {
            if (nManager != null) {
                nManager.StartClient();
            }
        }

        nManager.networkAddress = GUI.TextField(new Rect(200, 120, 250, 50), nManager.networkAddress);
        

    }


}