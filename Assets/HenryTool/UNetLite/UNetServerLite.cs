using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace HenryTool
{
    public class UNetServerLite : UNetBase
    {
        public Dictionary<uint, NetworkConnection> clientConnections = new Dictionary<uint, NetworkConnection>();

        public int networkPort = 9527;

        public NetworkMessageDelegate OnAClientConnected = new NetworkMessageDelegate(DummyEmpty);
        public NetworkMessageDelegate OnAClientDisconnected = new NetworkMessageDelegate(DummyEmpty);


        public bool StartServer()
        {
            // 註冊消息回調,當Server收到指定類型的消息時,會回調註冊好的方法
            if (OnAClientConnected == DummyEmpty)
                OnAClientConnected = OnAClientConnectedHandler;

            if (OnAClientDisconnected == DummyEmpty)
                OnAClientDisconnected = OnAClientDisconnectedHandler;

            if (OnError == DummyEmpty)
                OnError = OnErrorHandler;

            RegisterHandler(MsgType.Connect, OnAClientConnected);
            RegisterHandler(MsgType.Disconnect, OnAClientDisconnected);
            RegisterHandler(MsgType.Error, OnError);

            // 監聽9527端口
            bool succeed = NetworkServer.Listen(networkPort);
            if (succeed)
                uLog("服務器成功啟動!");
            else
                uLog("服務器無法啟動,端口為" + networkPort.ToString());

            return succeed;

        }

        public void StartServer(Dictionary<short, NetworkMessageDelegate> _handlers)
        {
            StartServer();

            foreach (KeyValuePair<short, NetworkMessageDelegate> sn in _handlers) {
                if (!NetworkServer.handlers.ContainsKey(sn.Key)) {
                    NetworkServer.RegisterHandler(sn.Key, sn.Value);
                }
            }

        }


        public bool SendMessageToClient(NetworkConnection _clientConn, short _cmd, MessageBase _msg, bool reliable)
        {
            bool result = false;

            //if (_clientConn.isConnected && _clientConn.isReady) 
            if (_clientConn.isConnected) {
                if (reliable)
                    result = _clientConn.Send(_cmd, _msg);
                else
                    result = _clientConn.SendUnreliable(_cmd, _msg);

            }

            return result;

        }

        public bool SendMessageToClient(uint _clientConnId, short _cmd, MessageBase _msg, bool reliable)
        {
            return SendMessageToClient(clientConnections[_clientConnId], _cmd, _msg, reliable);

        }

        public bool SendMessageToAllClients(short _cmd, MessageBase _msg, bool reliable)
        {
            if (reliable)
                return NetworkServer.SendToAll(_cmd, _msg);
            else
                return NetworkServer.SendUnreliableToAll(_cmd, _msg);
        }


        public bool RegisterHandler(short _msgType, NetworkMessageDelegate _handler)
        {
            if (!NetworkServer.handlers.ContainsKey(_msgType)) {
                NetworkServer.handlers.Add(_msgType, _handler);
                return true;
            }
            else {
                uLog("" + _msgType + " has already registered for Server.");
                return false;
            }
        }

        public void RegisterHandlerOverride(short _msgType, NetworkMessageDelegate handler)
        {
            if (NetworkServer.handlers.ContainsKey(_msgType)) {
                NetworkServer.handlers.Remove(_msgType);
            }

            NetworkServer.RegisterHandler(_msgType, handler);

        }


        #region Event Handlers

        void OnAClientConnectedHandler(NetworkMessage _msg)
        {
            uLog("Client Connection ID: " + _msg.conn.connectionId);
            uLog("Client Connected: " + _msg.conn);
            //connections.Add(_msg.conn); //保存NetworkConnection對象,代表C/S之間的連線
            clientConnections.Add((uint)_msg.conn.connectionId, _msg.conn);

        }

        void OnAClientDisconnectedHandler(NetworkMessage _msg)
        {
            uint connId = (uint)_msg.conn.connectionId;
            if (clientConnections.ContainsKey(connId)) {
                clientConnections.Remove(connId);
            }

            if (_msg.conn.lastError != NetworkError.Ok) {
                if (LogFilter.logError) { uLog("ServerDisconnected due to error: " + _msg.conn.lastError); }
            }

            uLog("A client disconnected from the server: " + _msg.conn);


        }

        #endregion



    }

}




