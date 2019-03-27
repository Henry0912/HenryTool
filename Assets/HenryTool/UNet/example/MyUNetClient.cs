using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.Networking;


public class MyUNetClient : UNetClient
{
    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public void MyStartClient(string _ip)
    {
        Dictionary<short, NetworkMessageDelegate> handlers = new Dictionary<short, NetworkMessageDelegate>();

        handlers.Add((short)MessageCommand.CMD_TO_CLIENT, ClientMsg);

        StartClient(_ip, handlers);

    }

    void ClientMsg(NetworkMessage _msg)
    {
        var msg = _msg.ReadMessage<MsgTest>();
        DebugLogMain.hLog("Send to Client: " + msg.msg);

    }




}






