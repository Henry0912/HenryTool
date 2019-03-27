using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HenryTool;
using UnityEngine.Events;

namespace HenryTool
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class MenuBuilder : MenuRootBehavior
    {
        public MenuType CreateMenu<MenuType>(string _menuName) where MenuType : MenuBehavior
        {
            MenuType[] menus = GetComponentsInChildren<MenuType>(true);
            MenuType menuBehavior = null;
            foreach (MenuType mt in menus) {
                if (mt.name == _menuName) {
                    menuBehavior = mt;
                    if (!allMenus.Contains(menuBehavior)) {
                        allMenus.Add(menuBehavior);
                    }
                    return menuBehavior;
                }
            }

            GameObject menuObj = new GameObject(_menuName);

            RectTransform menuRect = menuObj.AddComponent<RectTransform>();
            menuRect.transform.SetParent(transform);
            menuRect.anchorMin = Vector2.zero;
            menuRect.anchorMax = Vector2.one;
            menuRect.sizeDelta = Vector2.zero;
            menuRect.localScale = Vector3.one;
            menuRect.transform.localPosition = Vector3.zero;
            menuRect.localEulerAngles = Vector3.zero;

            menuObj.AddComponent<CanvasRenderer>();

            menuBehavior = menuObj.AddComponent<MenuType>();
            if (!allMenus.Contains(menuBehavior)) {
                allMenus.Add(menuBehavior);
            }
            return menuBehavior;

        }

        public RectTransform MenuCreateRect(MenuBehavior _menu, string _name)
        {
            GameObject uiObj = new GameObject(_name);

            RectTransform uiRect = uiObj.AddComponent<RectTransform>();
            uiObj.transform.SetParent(_menu.transform);

            uiObj.AddComponent<CanvasRenderer>();

            return uiRect;
        }
        public RectTransform MenuCreateRect(MenuBehavior _menu, string _name, Vector2 _position)
        {
            RectTransform uiRect = MenuCreateRect(_menu, _name);

            SetUiRect(uiRect, _position);

            return uiRect;
        }

        public Image MenuForceAddImage(MenuBehavior _menu, string _imageName, Sprite _imageSprite, Vector2 _position)
        {
            RectTransform uiRect = MenuCreateRect(_menu, _imageName);

            Image buttonImage = uiRect.gameObject.AddComponent<Image>();
            if (_imageSprite != null) {
                SetUiRect(uiRect, _position);

                buttonImage.sprite = _imageSprite;
                buttonImage.SetNativeSize();
            }
            else {
                SetExpandedUiRect(uiRect, Vector3.zero);
                buttonImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            }

            return buttonImage;
        }
        public Image MenuForceAddImage(MenuBehavior _menu, string _imageName, Vector2 _position)
        {
            return MenuForceAddImage(_menu, _imageName, GetSprite(_imageName), _position);

        }
        public Image MenuAddImage(MenuBehavior _menu, string _imageName, Sprite _imageSprite, Vector2 _position)
        {
            Image[] images = _menu.GetComponentsInChildren<Image>(true);

            foreach (Image img in images) {
                if (img.name == _imageName) {
                    return img;
                }
            }

            return MenuForceAddImage(_menu, _imageName, _imageSprite, _position);

        }
        public Image MenuAddImage(MenuBehavior _menu, string _imageName, Vector2 _position)
        {
            return MenuAddImage(_menu, _imageName, GetSprite(_imageName), _position);

        }

        public Button MenuForceAddButton(MenuBehavior _menu, string _buttonName, Sprite _buttonSprite, Vector2 _position, UnityAction _actionFunction)
        {
            Image buttonImage = MenuAddImage(_menu, _buttonName, _buttonSprite, _position);

            Button button = buttonImage.gameObject.AddComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(_actionFunction);

            return button;
        }
        public Button MenuForceAddButton(MenuBehavior _menu, string _buttonName, Vector2 _position, UnityAction _actionFunction)
        {
            return MenuForceAddButton(_menu, _buttonName, GetSprite(_buttonName), _position, _actionFunction);

        }
        public Button MenuForceAddButton(MenuBehavior _menu, UnityAction _actionFunction)
        {
            return MenuForceAddButton(_menu, "BUTTON", null, Vector3.zero, _actionFunction);

        }
        public Button MenuAddButton(MenuBehavior _menu, string _name, Sprite _sprite, Vector2 _position, UnityAction _action)
        {
            Button[] buttons = _menu.GetComponentsInChildren<Button>(true);

            foreach (Button btn in buttons) {
                if (btn.name == _name) {
                    return btn;
                }
            }

            return MenuForceAddButton(_menu, _name, _sprite, _position, _action);

        }
        public Button MenuAddButton(MenuBehavior _menu, string _buttonName, Vector2 _position, UnityAction _actionFunction)
        {
            return MenuAddButton(_menu, _buttonName, GetSprite(_buttonName), _position, _actionFunction);

        }
        public Button MenuAddButton(MenuBehavior _menu, UnityAction _actionFunction)
        {
            return MenuAddButton(_menu, "BUTTON", null, Vector3.zero, _actionFunction);

        }

        public Text MenuForceAddText(MenuBehavior _menu, string _name, Vector2 _position, string _context)
        {
            RectTransform uiRect = MenuCreateRect(_menu, _name, _position);

            Text uiText = uiRect.gameObject.AddComponent<Text>();
            uiText.text = _context;

            uiText.alignment = TextAnchor.MiddleCenter;
            uiText.fontSize = 100;
            uiText.color = Color.black;
            uiText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            uiText.raycastTarget = false;

            return uiText;

        }
        public Text MenuAddText(MenuBehavior _menu, string _name, Vector2 _position, string _context)
        {
            Text[] texts = _menu.GetComponentsInChildren<Text>(true);

            foreach (Text txt in texts) {
                if (txt.name == _name) {
                    return txt;
                }
            }

            return MenuForceAddText(_menu, _name, _position, _context);

        }

        public InputField MenuForceAddInputField(MenuBehavior _menu, string _name, Vector2 _position)
        {
            RectTransform uiRect = MenuCreateRect(_menu, _name, _position);

            InputField input = uiRect.gameObject.AddComponent<InputField>();

            return input;

        }

        public InputField MenuAddInputField(MenuBehavior _menu, string _name, Vector2 _position)
        {
            InputField[] inputFields = _menu.GetComponentsInChildren<InputField>(true);

            foreach (InputField inf in inputFields) {
                if (inf.name == _name) {
                    return inf;
                }
            }
            return MenuForceAddInputField(_menu, _name, _position);

        }


        public void SetUiRect(RectTransform _rect, Vector2 _position)
        {
            _rect.anchorMin = _rect.anchorMax = new Vector2(0.5f, 0.5f);

            _rect.localScale = Vector3.one;
            _rect.localPosition = (Vector3)_position;
            _rect.localEulerAngles = Vector3.zero;

        }
        public void SetUiRect(RectTransform _rect, Vector2 _position, float _width, float _height)
        {
            SetUiRect(_rect, _position);

            if ((_width > 0) && (_height > 0)) {
                _rect.sizeDelta = new Vector2(_width, _height);
            }

        }
        public void SetExpandedUiRect(RectTransform _rect, Vector2 _position)
        {
            _rect.anchorMin = Vector2.zero;
            _rect.anchorMax = Vector2.one;

            _rect.localScale = Vector3.one;
            _rect.localPosition = _position;
            _rect.localEulerAngles = Vector3.zero;


        }
        
        public Sprite GetSprite(string _name)
        {
            Sprite imageSprite = Resources.Load<Sprite>("ButtonSprites/" + _name);

            return imageSprite;

        }

    }


}



