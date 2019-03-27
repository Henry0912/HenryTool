using HenryTool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UiRootMng : MenuRootBehavior
{
    public MenuBehavior menuMain;
    public MenuBehavior menu1;
    public MenuBehavior menu2;

    // Use this for initialization
    void Start()
    {
        AddMenuWithButtons(menuMain, new UnityAction[] {
                ShowMenu1,
                ShowMenu2
            });

        AddMenuWithButtons(menu1, new UnityAction[] {
                ShowMainMenu
            });
        AddMenuWithButtons(menu2, new UnityAction[] {
                ShowMainMenu
            });

        ShowMainMenu();

    }

    // Update is called once per frame
    //void Update() { }

    public void ShowMainMenu()
    {
        HideAllMenus();
        menuMain.ShowMenu();
    }

    public void ShowMenu1()
    {
        HideAllMenus();
        menu1.ShowMenu();
    }

    public void ShowMenu2()
    {
        HideAllMenus();
        menu2.ShowMenu();
    }



}
