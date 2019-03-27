using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;

public class UNetUiClient : MenuWithButtons
{
    public Text serverIp;

    // Use this for initialization
    void Start()
    {
        //192.168.68.228
        //serverIp.text = "192.168.68.228";
    }

    // Update is called once per frame
    void Update()
    {
    }

    /*
    public void OnClickStart()
    {
        ((MyUNetMain)NetworkManager.singleton).MyStartClient(serverIp.text);

    }
    */

    


}

