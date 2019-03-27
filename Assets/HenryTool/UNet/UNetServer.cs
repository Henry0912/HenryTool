using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using HenryTool;
using System.Runtime.InteropServices;
using System;

namespace HenryTool
{
    public abstract class UNetServer : NetworkManager
    {
        public const bool RELIABLE = true;
        public const bool UNRELIABLE = false;

        public Dictionary<uint, NetworkConnection> connectionsOfClients = new Dictionary<uint, NetworkConnection>();
        public DelegateVoidOfString uNetServerLog = new DelegateVoidOfString(DebugLogMain.hLog);


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
            //if (_clientConn.isConnected && _clientConn.isReady) {
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
            return SendMessageToClient(connectionsOfClients[_clientConnId], _cmd, _msg, reliable);

        }

        public bool SendMessageToAllClients(short _cmd, MessageBase _msg, bool reliable)
        {
            if (reliable)
                return NetworkServer.SendToAll(_cmd, _msg);
            else
                return NetworkServer.SendUnreliableToAll(_cmd, _msg);
        }


        #region Server callbacks      
        public override void OnServerConnect(NetworkConnection conn)
        {
            uNetServerLog("A client connected to the server: " + conn);

            if (connectionsOfClients.ContainsKey((uint)conn.connectionId)) {
                connectionsOfClients.Remove((uint)conn.connectionId);
            }

            connectionsOfClients.Add((uint)conn.connectionId, conn);

            //connectionsToClients.Add(nextClientConnectionId, conn);
            //nextClientConnectionId++;
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            NetworkServer.DestroyPlayersForConnection(conn);
            if (conn.lastError != NetworkError.Ok) {
                if (LogFilter.logError) { uNetServerLog("ServerDisconnected due to error: " + conn.lastError); }

            }

            uNetServerLog("A client disconnected from the server: " + conn);

        }

        public override void OnServerReady(NetworkConnection conn)
        {
            NetworkServer.SetClientReady(conn);
            uNetServerLog("Client is set to the ready state (ready to receive state updates): " + conn);

        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            uNetServerLog("Client has requested to get his player added to the game");

        }

        public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
        {
            if (player.gameObject != null)
                NetworkServer.Destroy(player.gameObject);

        }

        public override void OnServerError(NetworkConnection conn, int errorCode)
        {
            uNetServerLog("Server network error occurred: " + (NetworkError)errorCode);

        }

        public override void OnStartHost()
        {
            uNetServerLog("Host has started");
        }

        public override void OnStartServer()
        {
            uNetServerLog("Server has started.(UNetManager)");
        }

        public override void OnStopServer()
        {
            uNetServerLog("Server has stopped");
        }

        public override void OnStopHost()
        {
            uNetServerLog("Host has stopped");
        }

        #endregion


    }



}








