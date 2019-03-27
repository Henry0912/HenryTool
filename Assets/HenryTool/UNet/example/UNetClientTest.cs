using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.Networking;
using System;

public class UNetClientTest : MonoBehaviour {

    NetworkClient _myClient;
    public NetworkClient myClient
    {
        get {
            if (_myClient == null) {
                _myClient = new NetworkClient();
            }
            return _myClient;
        }

        set {
            _myClient = value;
        }
    }

    public void OnConnected(NetworkMessageDelegate reader)
    {
        Debug.Log("Connected to server");
    }

    public void OnDisconnected(NetworkConnection conn, NetworkReader reader)
    {
        Debug.Log("Disconnected from server");
    }

    public void OnError(NetworkConnection conn, NetworkReader reader)
    {
        //SystemErrorMessage errorMsg = reader.SmartRead<SystemErrorMessage>();
        //Debug.Log("Error connecting with code " + errorMsg.errorCode);
    }

    public void Start()
    {
        //myClient = NetworkClient.Instance;

        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
        myClient.RegisterHandler(MsgType.Error, OnError);

        myClient.Connect("192.168.6.34", 7777);
    }

    private void OnError(NetworkMessage netMsg)
    {
        Debug.Log("OnError");
    }

    private void OnDisconnected(NetworkMessage netMsg)
    {
        Debug.Log("OnDisconnected");
    }

    private void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("OnConnected");
    }
}
