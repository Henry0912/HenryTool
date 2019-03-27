using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using HenryTool;
using System;
using System.IO;

public class MyNetworkPlayer : NetworkBehaviour
{
    //public Image ImgPlayer1;
    //public Image ImgPlayer2;

    public GameObject serverUnit;
    public GameObject remoteUnit;

    public RemoteCtl remoteCtl;
    public ServerCtl serverCtl;

    public GameObject playerUnit;

    NetworkMain gm;
    public LogStringVariable errorLog;

    void Start() {
        errorLog.AddLogLine("is server: " + isServer.ToString());
        gm = GameObject.Find("NetworkMain").GetComponent<NetworkMain>();

        if (isLocalPlayer) {
            if (isServer) {
                //gm = GameObject.Find("GM").GetComponent<NetworkMain>();
                //gm.Login(this);
                InitialServerPlayer();

            }
            else {
                InitialRemotePlayer();

            }
        }
        else {
            serverCtl = gm.serverCtl;
            remoteCtl = gm.remoteCtl;
        }


    }

    [ClientRpc]
    public void RpcSetPlayer(int id) {
        errorLog.AddLogLine("rpc test");
        if (isLocalPlayer) {
            InitialServerPlayer();
        }
        else {
            InitialRemotePlayer();
        }


        if (isLocalPlayer) {
            switch (id) {
                case 1:
                    errorLog.AddLogLine("P1");
                    //ImgPlayer1.gameObject.SetActive(true);
                    break;
                case 2:
                    errorLog.AddLogLine("P2");
                    //ImgPlayer2.gameObject.SetActive(true);
                    break;

                default: {
                        errorLog.AddLogLine("P:" + id.ToString());
                        break;
                    }
            }
        }
    }

    [Command]
    public void CmdTestF() {

    }

    [Command]
    public void CmdPlayVideo(string _fileName) {
        string filePath = Application.streamingAssetsPath +"/"+ _fileName;

        errorLog.AddLogLine("isClient: " + isClient);

        serverCtl.PlayVideo(filePath);

        /*
        if (File.Exists(filePath)) {
            serverCtl.PlayVideo(filePath);
        }
        else {
            errorLog.AddLogLine("file not exits."+filePath);
        }
        */


        /*
        try {
            serverCtl.PlayVideo(Application.dataPath + "/HenryTool/TestFolder/" + _fileName);
        }
        catch (Exception e) {
            errorLog.AddLogLine(""+e.ToString());
        }
        */

    }

    void InitialServerPlayer() {
        errorLog.AddLogLine("Initial Server Player: " );
        serverCtl = (Instantiate(serverUnit) as GameObject).GetComponent<ServerCtl>();
        //playerUnit.transform.SetParent(this.transform);

        serverCtl.InitStart();
        gm.serverCtl = serverCtl;

    }

    void InitialRemotePlayer() {
        errorLog.AddLogLine("Initial Remote Player");
        remoteCtl = (Instantiate(remoteUnit) as GameObject).GetComponent<RemoteCtl>();
        //playerUnit.transform.SetParent(this.transform);

        remoteCtl.InitStart(this);
        gm.remoteCtl = remoteCtl;

    }



}