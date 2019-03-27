using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HenryTool;
using UnityEngine.Events;

namespace HenryTool
{
    [RequireComponent(typeof(RectTransform))]
    //public abstract class MenuRootBehavior : MonoBehaviour, IMenuRoot
    public class MenuRootBehavior : MonoBehaviour, IMenuRoot
    {
        protected List<IMenuBehavior> allMenus = new List<IMenuBehavior>();

        public static void InitButton(Button _btn, UnityAction _callBack)
        {
            _btn.onClick.RemoveAllListeners();
            _btn.onClick.AddListener(_callBack);
        }


        public void HideAllMenus()
        {
            foreach (MenuBehavior mb in allMenus) {
                mb.HideMenu();
            }
        }

        public void ShowMenu(IMenuBehavior _menu) {
            if (allMenus.Contains(_menu)) {
                HideAllMenus();
                _menu.ShowMenu();

            }
        }


        public void AddMenu(IMenuBehavior _menu) {
            if (!allMenus.Contains(_menu)) {
                allMenus.Add(_menu);

            }

        }

        public void AddMenuWithButtons(MenuBehavior _menu, params UnityAction[] _buttonActions)
        {
            if (!allMenus.Contains(_menu)) {
                allMenus.Add(_menu);
                ((MenuWithButtons)_menu).InitMenu(_buttonActions);

            }

        }

        public void AddMenu(string _name, IMenuBehavior _menu)
        {
            throw new System.NotImplementedException();
        }

        public void ShowMenu(string _menuName)
        {
            throw new System.NotImplementedException();
        }

        public void ShowMenu(int _index)
        {
            throw new System.NotImplementedException();
        }

        public void AddMenu(MenuManager _menu)
        {
            throw new System.NotImplementedException();
        }
    }


}


