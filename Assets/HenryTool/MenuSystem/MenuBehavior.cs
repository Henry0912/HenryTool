using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.Events;

namespace HenryTool
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class MenuBehavior : MonoBehaviour, IMenuBehavior
    {
        public virtual void ShowMenu()
        {
            gameObject.SetActive(true);
        }

        public void HideMenu()
        {
            gameObject.SetActive(false);
        }

        //public abstract void InitMenu();

        public abstract T InitMenu<T>(params UnityAction[] _actionFunctions) where T : MenuBehavior;

        public abstract void InitMenu(params UnityAction[] _actionFunctions);


    }


}




