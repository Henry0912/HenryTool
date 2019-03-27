using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.Networking;

public class UNetLiteTest : MonoBehaviour
{
    public UNetServerLite uServer;
    public UNetClientLite uClient;

    public float aRandomFloat
    {
        get {
            return Random.Range(1, 50);
        }
    }

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }


    public void StartServer()
    {
        uServer = gameObject.AddComponent<UNetServerLite>();

        Dictionary<short, NetworkMessageDelegate> handlers = new Dictionary<short, NetworkMessageDelegate>();

        handlers.Add((short)MessageCommand.CMD_TO_SERVER, ServerMsg);
        handlers.Add((short)MessageCommand.TEST_MSG, OnReceiveClientMsg);

        uServer.StartServer(handlers);
        UNetTestSataic.isReady = true;
        UNetTestSataic.isServer = true;

        uServer.uLog("" + IPManager.GetIPv4());
        uServer.uLog("" + IPManager.GetIPv6());


    }

    public void StartClient(string _serverIp)
    {
        uClient = gameObject.AddComponent<UNetClientLite>();
        uClient.networkAddress = _serverIp;

        Dictionary<short, NetworkMessageDelegate> handlers = new Dictionary<short, NetworkMessageDelegate>();

        handlers.Add((short)MessageCommand.CMD_TO_CLIENT, ClientMsg);
        handlers.Add((short)MessageCommand.TEST_MSG, OnReceiveServerMsg);

        uClient.StartClient(uClient.networkAddress, handlers);
        UNetTestSataic.isReady = true;
        UNetTestSataic.isServer = false;


    }

    void ServerMsg(NetworkMessage _msg)
    {
        var msg = _msg.ReadMessage<UNetMessage>();

        int count = msg.msgs.Length;
        for (int i = 0; i < count; i++) {
            uServer.uLog("ID: " + msg.msgs[i].id + ", Name: " + msg.msgs[i].name + ", POS: " + msg.msgs[i].position);
        }

        uServer.uLog("=====================");
    }


    void ClientMsg(NetworkMessage _msg)
    {
        var msg = _msg.ReadMessage<MsgTest>();
        uClient.uLog("Send to Client: " + msg.msg);


    }

    void OnReceiveServerMsg(NetworkMessage _msg)
    {
        var msg = _msg.ReadMessage<MsgLite>();

        int rValue = (int)aRandomFloat;

        if (rValue < msg.value) {
            uClient.uLog("" + rValue.ToString() + " vs " + msg.value + ", Lose");
            msg.value = rValue;
            uClient.SendMessageToServer((short)MessageCommand.TEST_MSG, msg, UNetBase.RELIABLE);
        }
        else {
            uClient.uLog("" + rValue.ToString() + " vs " + msg.value + ", WIN");

        }

    }

    void OnReceiveClientMsg(NetworkMessage _msg)
    {
        var msg = _msg.ReadMessage<MsgLite>();

        int rValue = (int)aRandomFloat;
        uServer.uLog("" + rValue.ToString());

        //if (rValue < msg.value) 
        {
            msg.value = rValue;
            uServer.SendMessageToClient(_msg.conn, (short)MessageCommand.TEST_MSG, msg, UNetBase.RELIABLE);

        }

    }


    private void OnGUI()
    {
        if (UNetTestSataic.isReady) {
            if (UNetTestSataic.isServer) {
                if (GUI.Button(new Rect(100, 100, 150, 100), "Server test")) {
                    MsgTest msg = new MsgTest();
                    msg.msg = "test msg";

                    uServer.SendMessageToAllClients((short)MessageCommand.CMD_TO_CLIENT, msg, UNetBase.RELIABLE);
                }

                if (GUI.Button(new Rect(300, 100, 150, 100), "Show Server IP address.")) {
                    //System.Diagnostics.Process.Start("E:/Projects/HenryTool/Build/Network/PC/PC.exe");
                    uServer.uLog(IPManager.GetIPv4());
                }

                if (GUI.Button(new Rect(500, 100, 150, 100), "Clear")) {
                    DebugLogMain.logManager.ClearLog();

                }


            }
            else {
                if (GUI.Button(new Rect(100, 100, 150, 100), "Client test")) {
                    UNetMessage msg = new UNetMessage();
                    //msg.msg = "test server msg";
                    int length = Random.Range(1, 5);
                    MsgStrct[] al = new MsgStrct[length];
                    for (int i = 0; i < length; i++) {
                        MsgStrct us;
                        us.id = (uint)i;
                        us.name = "N" + i.ToString();
                        //us.position = new Vector3(Random.Range(1, 50), Random.Range(1, 50), Random.Range(1, 50));
                        us.position = new Vector3(aRandomFloat, aRandomFloat, aRandomFloat);
                        al[i] = us;
                    }

                    msg.msgs = al;

                    uClient.SendMessageToServer((short)MessageCommand.CMD_TO_SERVER, msg, UNetBase.RELIABLE);


                }

                if (GUI.Button(new Rect(300, 100, 150, 100), "Client value")) {
                    MsgLite msg = new MsgLite();
                    msg.value = (int)aRandomFloat;
                    uClient.SendMessageToServer((short)MessageCommand.TEST_MSG, msg, UNetBase.RELIABLE);

                }

                if (GUI.Button(new Rect(500, 100, 150, 100), "Clear")) {
                    DebugLogMain.logManager.ClearLog();
                    
                }

            }


        }



    }






}




public enum MessageLite : short
{
    CMD_TO_CLIENT = 1000,
    CMD_TO_SERVER = 2000
}



public struct MsgStrctLite
{
    public uint id;
    public string name;
    public Vector3 position;

}

public class UNetLiteMessage : MessageBase
{
    public MsgStrct[] msgs;

}

public class MsgTestLite : MessageBase
{
    public string msg;
}

public class MsgLite : MessageBase
{
    public int value;

}


