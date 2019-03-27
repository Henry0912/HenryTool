using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace HenryTool
{
    public class UNetClientLite : UNetBase
    {
        // 創建NetworkClient對象
        protected NetworkClient networkClient = new NetworkClient();

        public string networkAddress = "127.0.0.1";
        public int networkPort = 9527;

        public bool autoReconnect = true;

        public NetworkMessageDelegate OnConnected = new NetworkMessageDelegate(DummyEmpty);
        public NetworkMessageDelegate OnDisconnected = new NetworkMessageDelegate(DummyEmpty);


        public void StartClient()
        {
            if (OnConnected == DummyEmpty)
                OnConnected = OnConnectedHandler;

            if (OnDisconnected == DummyEmpty)
                OnDisconnected = OnDisconnectedHandler;

            if (OnError == DummyEmpty)
                OnError = OnErrorHandler;

            RegisterHandler(MsgType.Connect, OnConnected);
            RegisterHandler(MsgType.Disconnect, OnDisconnected);
            RegisterHandler(MsgType.Error, OnError);
            
            // 連接服務器
            networkClient.Connect(networkAddress, networkPort); // 服務器IP 服務器端口
            
        }

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
                if (!networkClient.handlers.ContainsKey(sn.Key)) {
                    networkClient.RegisterHandler(sn.Key, sn.Value);
                }
            }

        }

        public void StartClient(string _serverIp, int _serverPort, Dictionary<short, NetworkMessageDelegate> _handlers)
        {
            StartClient(_serverIp, _serverPort);
            foreach (KeyValuePair<short, NetworkMessageDelegate> sn in _handlers) {
                if (!networkClient.handlers.ContainsKey(sn.Key)) {
                    networkClient.RegisterHandler(sn.Key, sn.Value);
                }
            }

        }

        public bool SendMessageToServer(short _cmd, MessageBase _msg, bool reliable)
        {
            bool result = false;
            if (networkClient.isConnected) {
                if (reliable)
                    result = networkClient.connection.Send(_cmd, _msg);
                else
                    result = networkClient.connection.SendUnreliable(_cmd, _msg);

            }

            return result;
        }

        public bool RegisterHandler(short _msgType, NetworkMessageDelegate handler)
        {
            if (!networkClient.handlers.ContainsKey(_msgType)) {
                networkClient.RegisterHandler(_msgType, handler);
                return true;
            }
            else {
                uLog("" + _msgType + " has already registered for Client.");
                return false;
            }

        }

        public void RegisterHandlerOverride(short _msgType, NetworkMessageDelegate handler)
        {
            if (networkClient.handlers.ContainsKey(_msgType)) {
                networkClient.handlers.Remove(_msgType);
            }

            networkClient.RegisterHandler(_msgType, handler);

        }


        #region Event Handlers
        
        void OnConnectedHandler(NetworkMessage _msg)
        {
            uLog("Connected successfully to server, now to set up other stuff for the client...");
            uLog("host id:"+_msg.conn.hostId);
        }

        void OnDisconnectedHandler(NetworkMessage _msg)
        {
            //uLog(""+networkClient.GetConnectionStats());
            networkClient.Disconnect();
            //networkClient.Shutdown();            
            
            if (_msg.conn.lastError != NetworkError.Ok) {
                if (LogFilter.logError) { uLog("ClientDisconnected due to error: " + _msg.conn.lastError); }
            }

            uLog("Client disconnected from server: " + _msg.conn + ", isConnected: " + networkClient.isConnected);

            if (autoReconnect) {
                uLog("Auto reconnecting to server...");
                networkClient.Connect(networkAddress, networkPort);
                //networkClient.ReconnectToNewHost(networkAddress, networkPort);
            }

        }
        
        #endregion



    }


}


