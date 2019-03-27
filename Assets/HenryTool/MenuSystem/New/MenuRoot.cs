using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HenryTool;
using UnityEngine.Events;
using RotaryHeart.Lib.SerializableDictionary;

namespace HenryTool
{
    [RequireComponent(typeof(RectTransform))]
    public class MenuRoot : MonoBehaviour, IMenuRoot
    {
        public List<MenuManager> allMenus = new List<MenuManager>();


        public void HideAllMenus()
        {
            foreach (MenuManager menu in allMenus)
            {
                menu.HideMenu();
            }
        }

        public void ShowMenu(int _index)
        {
            if (_index < allMenus.Count)
            {
                if (allMenus[_index].menuType != MenuType.PopUp)
                    HideAllMenus();

                allMenus[_index].ShowMenu();
            }
        }


        public void AddMenu(MenuManager _menu)
        {
            if (!allMenus.Contains(_menu))
                allMenus.Add(_menu);

        }

      
    }



}



