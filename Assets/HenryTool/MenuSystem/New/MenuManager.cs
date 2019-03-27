using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.UI;
using System;

namespace HenryTool
{
    [RequireComponent(typeof(RectTransform))]
    public class MenuManager : MonoBehaviour, IMenuBehavior
    {
        public MenuType menuType;
        //public List<MenuButtonStr> buttons;
        public List<Button> buttons;
        public List<Image> images;
        // Start is called before the first frame update
        //public void InitStart() { }

        // Update is called once per frame
        //void Update() { }



        public void HideMenu()
        {
            gameObject.SetActive(false);
        }

        public void ShowMenu()
        {
            gameObject.SetActive(true);
        }



    }




}
