using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using HenryTool;
using System.Runtime.InteropServices;
using System;
using UnityEngine.Networking.NetworkSystem;

namespace HenryTool
{
    public abstract class UNetClient : NetworkManager
    {
        public const bool RELIABLE = true;
        public const bool UNRELIABLE = false;

        protected NetworkConnection connectionToServer;

        public bool isConnected = false;
        public bool autoReconnect = true;

        public DelegateVoidOfString uNetClientLog = new DelegateVoidOfString(DebugLogMain.hLog);


        public void StartClient(string _serverIp)
        {
            networkAddress = _serverIp;
            StartClient();

        }

        public void StartClient(string _serverIp, int _serverPort)
        {
            networkPort = _serverPort;
            StartClient(_serverIp);
        }

        public void StartClient(string _serverIp, Dictionary<short, NetworkMessageDelegate> _handlers)
        {
            StartClient(_serverIp);
            foreach (KeyValuePair<short, NetworkMessageDelegate> sn in _handlers) {
                if (!client.handlers.ContainsKey(sn.Key)) {
                    client.RegisterHandler(sn.Key, sn.Value);
                }
            }

        }

        public void StartClient(string _serverIp, int _serverPort, Dictionary<short, NetworkMessageDelegate> _handlers)
        {
            StartClient(_serverIp, _serverPort);
            foreach (KeyValuePair<short, NetworkMessageDelegate> sn in _handlers) {
                if (!client.handlers.ContainsKey(sn.Key)) {
                    client.RegisterHandler(sn.Key, sn.Value);
                }
            }

        }

        public bool SendMessageToServer(short _cmd, MessageBase _msg, bool reliable)
        {
            bool result = false;
            if (isConnected) {
                if (reliable)
                    result = connectionToServer.Send(_cmd, _msg);
                else
                    result = connectionToServer.SendUnreliable(_cmd, _msg);

            }

            return result;
        }


        #region Client callbacks
        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            isConnected = true;
            connectionToServer = conn;

            uNetClientLog("Connected successfully to server, now to set up other stuff for the client...");

        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            //StopClient();
            if (conn.lastError != NetworkError.Ok) {
                if (LogFilter.logError) { uNetClientLog("ClientDisconnected due to error: " + conn.lastError); }
            }

            uNetClientLog("Client disconnected from server: " + conn);

            isConnected = false;

            if (autoReconnect) {
                StartClient();

            }

        }


        public override void OnClientError(NetworkConnection conn, int errorCode)
        {
            uNetClientLog("Client network error occurred: " + (NetworkError)errorCode);

        }

        public override void OnClientNotReady(NetworkConnection conn)
        {
            uNetClientLog("Server has set client to be not-ready (stop getting state updates)");

        }

        public override void OnStartClient(NetworkClient client)
        {
            uNetClientLog("Client has started");

        }

        public override void OnStopClient()
        {
            uNetClientLog("Client has stopped");

        }

        public override void OnClientSceneChanged(NetworkConnection conn)
        {
            base.OnClientSceneChanged(conn);
            uNetClientLog("Server triggered scene change and we've done the same, do any extra work here for the client...");

        }

        #endregion



    }



}








