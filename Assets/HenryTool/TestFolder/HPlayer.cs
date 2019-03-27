using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using HenryTool;

public class HPlayer : NetworkBehaviour
{
    public enum Process
    {
        start,
        action,
        wait,
        end
    }

    public Image ImgPlayer1;
    public Image ImgPlayer2;
    GM gm;

    public LogStringVariable errorLog;

    [SyncVar]
    public Process process = Process.start;

    [SyncVar]
    public string SysMsg;

    public Button btnAddOne;
    public Button btnAddTwo;
    public Text txtMunber;
    public Text txtSysMsg;

    [SyncVar]
    public int Munber;

    void Start() {
        if (isServer) {
            gm = GameObject.Find("GM").GetComponent<GM>();
            gm.Login(this);
        }
        if (isLocalPlayer) {
            txtSysMsg = GameObject.Find("txtSysMsg").GetComponent<Text>();
            txtMunber = GameObject.Find("txtMunber").GetComponent<Text>();
            btnAddOne = GameObject.Find("btn1").GetComponent<Button>();
            btnAddTwo = GameObject.Find("btn2").GetComponent<Button>();
            btnAddOne.onClick.AddListener(() => CmdAddMunbers(1));
            btnAddTwo.onClick.AddListener(() => CmdAddMunbers(2));
        }
    }

    void Update() {
        if (isServer)
            Munber = gm.Number;
        if (isLocalPlayer) {
            txtSysMsg.text = SysMsg;
            txtMunber.text = Munber.ToString();
        }
    }

    [ClientRpc]
    public void RpcSetPlayer(int id) {
        if (isLocalPlayer)//hasAuthority
        {
            switch (id) {
                case 1:
                    ImgPlayer1.gameObject.SetActive(true);
                    break;
                case 2:
                    ImgPlayer2.gameObject.SetActive(true);
                    break;
            }
        }
    }


    [Command]
    public void CmdAddMunbers(int addMun) {
        if (process == Process.action)
            gm.AddNunber(addMun);
    }

    public void SetProcess(Process process) {
        this.process = process;
    }
}
