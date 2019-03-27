using HenryTool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using HenryTool;
using UnityEngine.UI;

public class P2pMenu : MenuRootBehavior
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

        /*
        if (allMenus == null) {
            allMenus = new List<MenuBehavior>();
            allMenus.Add(menuMain);
            allMenus.Add(menuServer);
            allMenus.Add(menuClient);

            ((MenuWithButtons)menuMain).InitMenu(new UnityAction[] {
                ShowServer,
                ShowClient
            });


            ((MenuWithButtons)menuServer).InitMenu(new UnityAction[] {
                StartServer,
                ShowMainMenu
            });


            ((MenuWithButtons)menuClient).InitMenu(new UnityAction[] {
                StartClient,
                ShowMainMenu
            });

        }
        */

        ShowMainMenu();

    }


    void OnInputIp(string _ip)
    {

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


