using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuWithButtons : MenuBehavior
{
    public List<Button> menuButtons = new List<Button>();

    public override T InitMenu<T>(params UnityAction[] _actionFunctions)
    {
        InitMenu(_actionFunctions);
        
        T menuWithButtons = gameObject.GetComponent<T>();

        return menuWithButtons;

    }

    public override void InitMenu(params UnityAction[] _actionFunctions)
    {
        int size = 0;
        if (menuButtons.Count < _actionFunctions.Length)
            size = menuButtons.Count;
        else
            size = _actionFunctions.Length;

        for (int i = 0; i < size; i++) {
            MenuRootBehavior.InitButton(menuButtons[i], _actionFunctions[i]);
        }
        
    }


}
