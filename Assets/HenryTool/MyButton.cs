using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using HenryTool;

namespace HenryTool
{
    public enum ButtonState { DOWN, UP }

    [RequireComponent(typeof(Button))]
    public class MyButton : EventTrigger
    {
        public LogStringVariable debugLog;

        public ButtonState state;
        private Button _nativeButton;
        public Button button
        {
            get {
                if (_nativeButton == null) {
                    button = GetComponent<Button>();
                }

                return _nativeButton;
            }

            set {
                _nativeButton = value;
            }
        }

        public bool isDown
        {
            get {
                return (state == ButtonState.DOWN);
            }
        }

        DelegateVoidOfVoid _initDelegate;
        private DelegateVoidOfVoid ButtonInitDelegate
        {
            get {
                if (_initDelegate == null) {
                    _initDelegate = InitMyButton;
                }

                return _initDelegate;
            }

            set {
                _initDelegate = value;
            }
        }

        // Use this for initialization
        public void InitStart()
        {
            ButtonInitDelegate();

        }


        public void InitStart(UnityEngine.Events.UnityAction _onClickCallBack)
        {
            InitStart();
            button.interactable = true;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(_onClickCallBack);
            //isInited = true;

        }



        private void DummyFun()
        {
            debugLog.AddLogLine("DummyFun: " + gameObject.name);
        }

        private void InitMyButton()
        {
            button = GetComponent<Button>();
            state = ButtonState.UP;

            ButtonInitDelegate = DummyFun;

        }


        public override void OnPointerDown(PointerEventData data)
        {
            state = ButtonState.DOWN;
        }

        public override void OnPointerEnter(PointerEventData data)
        {

        }

        public override void OnPointerExit(PointerEventData data)
        {
            state = ButtonState.UP;
        }

        public override void OnPointerUp(PointerEventData data)
        {
            state = ButtonState.UP;
        }




    }

}



