using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;

namespace HenryTool
{
    public interface IMenuRoot
    {
        void ShowMenu(int _index);

        void HideAllMenus();

        void AddMenu(MenuManager _menu);


    }

}


