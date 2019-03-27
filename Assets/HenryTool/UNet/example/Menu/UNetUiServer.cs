using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.UI;
using UnityEngine.Events;

public class UNetUiServer : MenuWithButtons
{
    public Text serverIp;

    // Use this for initialization
    void Start()
    {
        //serverIp.text = Network.ip
        serverIp.text = "IP: " + IPManager.GetIPv4();

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Start Server")) {
            if (onClickStart != null) {
                onClickStart();
            }
        }

        if (GUI.Button(new Rect(10, 110, 150, 100), "back")) {
            if (onClickBack != null) {
                onClickBack();
            }
        }
    }
    */

}
