using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.Networking;

/*
public enum MessageCommand : short
{
    CMD_TO_CLIENT = 1000,
    CMD_TO_SERVER = 2000
}
*/

public class MyUNetServer : UNetServer
{

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() { }

    public void MyStartServer()
    {
        Dictionary<short, NetworkMessageDelegate> handlers = new Dictionary<short, NetworkMessageDelegate>();

        handlers.Add((short)MessageCommand.CMD_TO_SERVER, ServerMsg);

        StartServer(handlers);

    }

    void ServerMsg(NetworkMessage _msg)
    {
        var msg = _msg.ReadMessage<UNetMessage>();

        int count = msg.msgs.Length;
        for (int i = 0; i < count; i++) {
            DebugLogMain.hLog("ID: " + msg.msgs[i].id + ", Name: " + msg.msgs[i].name + ", POS: " + msg.msgs[i].position);
        }

        DebugLogMain.hLog("=====================");
    }



}





