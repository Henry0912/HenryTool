using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoteCtl : MonoBehaviour {
    public MyNetworkPlayer nPlayer;

    public Button button1;
    public Button button2;
    public Button button3;


    // Use this for initialization
    public void InitStart (MyNetworkPlayer _nPlayer) {
        if (nPlayer == null) {
            nPlayer = _nPlayer;

            button1.onClick.RemoveAllListeners();
            button1.onClick.AddListener(OnClickButton1);

            button2.onClick.RemoveAllListeners();
            button2.onClick.AddListener(OnClickButton2);

            button3.onClick.RemoveAllListeners();
            button3.onClick.AddListener(OnClickButton3);
        }


    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnClickButton1() {
        nPlayer.CmdPlayVideo("neorest_01.mp4");
    }

    public void OnClickButton2() {
        //nPlayer.errorLog.AddLogLine("btn 2");
        nPlayer.CmdPlayVideo("neorest_02.mp4");
    }

    public void OnClickButton3() {
        //nPlayer.errorLog.AddLogLine("btn 3");
        nPlayer.CmdPlayVideo("neorest_03.mp4");

    }


}
