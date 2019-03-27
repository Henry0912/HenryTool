using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HenryTool;

public class NetworkMain : MonoBehaviour
{
    public LogStringVariable errorLog;

    public List<MyNetworkPlayer> allPlayer = new List<MyNetworkPlayer>();

    public RemoteCtl remoteCtl;
    public ServerCtl serverCtl;

    public void Login(MyNetworkPlayer player) {
        allPlayer.Add(player);
        player.RpcSetPlayer(allPlayer.Count);
    }

    


}