using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.Networking;


public class UNetMainTest : MonoBehaviour
{

    public GameObject uNetServerPrefab;
    public GameObject uNetClientPrefab;

    public MyUNetServer uNetServer
    {
        get {
            return (MyUNetServer)NetworkManager.singleton;
        }
    }

    public MyUNetClient uNetClient
    {
        get {
            return (MyUNetClient)NetworkManager.singleton;
        }

    }



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartServer()
    {
        var netMng = Instantiate(uNetServerPrefab).GetComponent<MyUNetServer>();
        netMng.MyStartServer();
        UNetTestSataic.isReady = true;
        UNetTestSataic.isServer = true;

    }

    public void StartClient(string _serverIp)
    {
        var netMng = Instantiate(uNetClientPrefab).GetComponent<MyUNetClient>();
        netMng.MyStartClient(_serverIp);

        UNetTestSataic.isReady = true;
        UNetTestSataic.isServer = false;

    }


    private void OnGUI()
    {
        if (UNetTestSataic.isReady) {
            if (UNetTestSataic.isServer) {
                if (GUI.Button(new Rect(100, 100, 150, 100), "Server test")) {
                    MsgTest msg = new MsgTest();
                    msg.msg = "test msg";

                    uNetServer.SendMessageToAllClients((short)MessageCommand.CMD_TO_CLIENT, msg, UNetBase.RELIABLE);

                    /*
                    int client = Random.Range(0, uNetMain.connectionsToClients.Count);
                    DebugLogMain.Log("Send to Client " + client.ToString());
                    uNetMain.SendMessageToClient((uint)client, (short)MessageCommand.CMD_TO_CLIENT, msg);
                    */
                }

                if (GUI.Button(new Rect(320, 100, 150, 100), "Run another Program.")) {
                    System.Diagnostics.Process.Start("E:/Projects/HenryTool/Build/Network/PC/PC.exe");

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
                        us.position = new Vector3(Random.Range(1, 50), Random.Range(1, 50), Random.Range(1, 50));
                        al[i] = us;
                    }

                    msg.msgs = al;

                    uNetClient.SendMessageToServer((short)MessageCommand.CMD_TO_SERVER, msg, UNetBase.RELIABLE);


                }

            }


        }



    }





}


public enum MessageCommand : short
{
    CMD_TO_CLIENT = 1000,
    TEST_MSG = 1001,
    CMD_TO_SERVER = 2000
}

public class UNetTestSataic
{
    public static bool isReady;
    public static bool isServer;
}

public struct MsgStrct
{
    public uint id;
    public string name;
    public Vector3 position;

}

public class UNetMessage : MessageBase
{
    public MsgStrct[] msgs;

    //public override void Serialize(NetworkWriter writer) { }

    //public override void Deserialize(NetworkReader reader) { }


}

public class MsgTest : MessageBase
{
    public string msg;
}

