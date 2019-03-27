using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.Networking;
using UnityEngine.Events;

public class UNetUiManager : MenuRootBehavior
{
    //public UNetInit uNetInit;
    public UNetMainTest uNetMainTest;

    public MenuBehavior lobbyMenu;
    public MenuBehavior serverMenu;
    public MenuBehavior clientMenu;

    private void Start()
    {
        AddMenuWithButtons(lobbyMenu, new UnityAction[] {
            ShowServerUi,
            ShowClientUi
        });

        AddMenuWithButtons(serverMenu, new UnityAction[] {
            OnStartServer,
            ShowLobby
        });

        AddMenuWithButtons(clientMenu, new UnityAction[] {
            OnStartClient,
            ShowLobby
        });

        ShowLobby();
    }


    // Update is called once per frame
    void Update() { }

    void OnStartServer()
    {
        HideAllMenus();

        UNetTestSataic.isReady = true;
        UNetTestSataic.isServer = true;

        uNetMainTest.StartServer();
        //((MyUNetServer)NetworkManager.singleton).MyStartServer();
        //((MyUNetMain)NetworkManager.singleton).StartGameClient(serverIp.text);

    }

    void OnStartClient()
    {
        HideAllMenus();

        UNetTestSataic.isReady = true;
        UNetTestSataic.isServer = false;

        uNetMainTest.StartClient(((UNetUiClient)clientMenu).serverIp.text);
        //((UNetUiClient)clientMenu).OnClickStart();

    }

    void ShowLobby()
    {
        DebugLogMain.hLog("ShowLobby");

        HideAllMenus();
        lobbyMenu.ShowMenu();
    }

    void ShowServerUi()
    {
        HideAllMenus();
        serverMenu.ShowMenu();
    }

    void ShowClientUi()
    {
        HideAllMenus();
        clientMenu.ShowMenu();
    }


}

