using HenryTool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HenryTool;
using UnityEngine.UI;

public class UNetLiteMenu : MenuRootBehavior
{
    public MenuBehavior menuMain;
    public MenuBehavior menuServer;
    public MenuBehavior menuClient;

    public InputField ipInputField;

    public UNetLiteTest uNet;

    // Use this for initialization
    void Start()
    {
        AddMenuWithButtons(menuMain, new UnityAction[] {
            ShowServer,
            ShowClient
        });

        AddMenuWithButtons(menuServer, new UnityAction[] {
            StartServer,
            ShowMainMenu
        });

        AddMenuWithButtons(menuClient, new UnityAction[] {
            StartClient,
            ShowMainMenu
        });

        ShowMainMenu();

    }


    void OnInputIp(string _ip) {

    }

    public void ShowMainMenu()
    {
        HideAllMenus();
        menuMain.ShowMenu();
    }

    public void ShowServer()
    {
        HideAllMenus();
        menuServer.ShowMenu();
    }

    public void ShowClient()
    {
        HideAllMenus();
        menuClient.ShowMenu();
    }

    void StartServer()
    {
        uNet.StartServer();
        HideAllMenus();
    }

    void StartClient()
    {
        uNet.StartClient(ipInputField.text);
        HideAllMenus();
    }

}

