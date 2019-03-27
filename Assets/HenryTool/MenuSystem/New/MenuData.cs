using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using System;
using UnityEngine.UI;
using UnityEditor;

namespace HenryTool
{
    public enum MenuType { Normal, MainMenu, PopUp }
    public enum UiType { Image, Button, InputField }
    public enum ImageType { Sprite, Color, Text }
    public enum ImageSizeType { SpriteSize, FullScreen, Other }

    [CreateAssetMenu(fileName = "NewMenu", menuName = "Menu/Menu Data", order = 2)]
    public class MenuData : ScriptableObject
    {
        public string menuName;
        public MenuType menuType;
        public List<UiData> buttons = new List<UiData>();
        public List<UiData> images = new List<UiData>();
        public List<UiData> allUis = new List<UiData>();


    }

    public enum ButtonAction { ShowMenu, LoadScene, Other }


    [Serializable]
    public class UiData
    {
        public string name;
        public UiType uiType;
        public int layer;
        public Vector2 position;
        public Color color = Color.white;

        public ImageType type;
        public ImageSizeType sizeType;
        public Sprite sprite;
        public int fontSize;
        public string text;

        public ButtonAction action;
        public MenuData goTo;


        public UiData(UiType _type)
        {
            uiType = _type;
        }

        public UiData()
        {
            //color = Color.white;
        }

        public UiData(UiData bd)
        {
            name = bd.name;
            uiType = bd.uiType;
            layer = bd.layer;
            position = bd.position;
            color = bd.color;
            type = bd.type;
            sizeType = bd.sizeType;
            sprite = bd.sprite;
            action = bd.action;
            goTo = bd.goTo;
            fontSize = bd.fontSize;
            text = bd.text;
        }


    }




}

