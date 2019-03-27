//#define USE_ONGUI
using UnityEngine;
using HenryTool;
using UnityEngine.UI;

namespace HenryTool
{
    public class DebugLogMain
    {
        public static DebugLogManager logManager;
        public static LogStringVariable gameLog;
        public static DelegateVoidOfString hLog = new DelegateVoidOfString(Debug.Log);

        static bool isErrorLogHandlerAdded = false;
        static void ErrorLogHandler(string condition, string stackTrace, LogType type)
        {
            if (type.Equals(LogType.Error) || type.Equals(LogType.Exception)) {
                hLog("condition: " + type.ToString() + ", " + condition + ", \n" + stackTrace);

            }
        }

        public static void InitDebugLog(LogStringVariable _log)
        {
            if (!isErrorLogHandlerAdded) {
                isErrorLogHandlerAdded = true;
                Application.logMessageReceived += ErrorLogHandler;
            }
                        
            if (_log == null)
                return;

            gameLog = _log;
            hLog = gameLog.Log;

            if (logManager == null) {
                logManager = new GameObject("LogManager").AddComponent<DebugLogManager>();
                logManager.debugLog = gameLog;
                logManager.isShownOnGui = true;
                logManager.BuildUiCanvas();
            }

            logManager.ClearLog();


        }

        
    }



    public class DebugLogManager : MonoBehaviour
    {
        const float ZOOM_OFFSET = 0.1f;
        const float ZOOM_TIME_INTERVAL = 0.2f;

        public RectTransform logRootRect;
        public bool isShownOnGui;
        public LogStringVariable debugLog;

        GameObject debugUIManager, backgroundImage, logTextGo;
        public Text logText;

        float nextZoomTime;

        void Awake()
        {
            uiStyle = new GUIStyle();
            uiStyle.fontSize = 25;
            //uiStyle.fontSize = 100;
            //debugLog.value = "";

            nextZoomTime = Time.time;

            if (isShownOnGui) {
                BuildUiCanvas();

            }

        }

        // Use this for initialization
        //public void InitStart() { }


        // Update is called once per frame
        void Update()
        {
            if (logText != null) {
                logText.text = debugLog.value;
            }

            if (logRootRect != null) {
                if (Input.GetKeyDown(KeyCode.P)) {
                    logRootRect.gameObject.SetActive(!logRootRect.gameObject.activeSelf);
                }

                //if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad, OVRInput.Controller.RTrackedRemote)) { logRootRect.gameObject.SetActive(!logRootRect.gameObject.activeSelf); }

                if (logRootRect.gameObject.activeSelf) {
                    if (Time.time > nextZoomTime) {
                        if (Input.GetKey(KeyCode.I)) {
                            logRootRect.localPosition -= new Vector3(0.0f, 0.0f, ZOOM_OFFSET);
                            nextZoomTime += ZOOM_TIME_INTERVAL;
                        }
                        else if (Input.GetKey(KeyCode.O)) {
                            logRootRect.localPosition += new Vector3(0.0f, 0.0f, ZOOM_OFFSET);
                            nextZoomTime += ZOOM_TIME_INTERVAL;
                        }
                    }
                }

            }

        }

        public void ClearLog()
        {
            debugLog.ClearLog();
        }

        public void Log(string _log)
        {
            debugLog.AddLogLine(_log);
        }

        public void LogError(string _log)
        {
            debugLog.AddLogLine("E: " + _log);
        }

        public void BuildUiCanvas()
        {
            isShownOnGui = true;
            debugUIManager = SetLogCanvas(debugUIManager);

            backgroundImage = SetBackground(backgroundImage);

            logTextGo = SetUiText(logTextGo);
            logText = logTextGo.GetComponent<Text>();
        }

        GameObject SetLogCanvas(GameObject _logManager)
        {
            _logManager = new GameObject();
            _logManager.name = "LogManager";

            if (logRootRect == null) {
                logRootRect = new GameObject("Log Root").AddComponent<RectTransform>();
                logRootRect.transform.SetParent(Camera.main.transform);
                logRootRect.sizeDelta = new Vector2(2.0f, 2.0f);
                logRootRect.localScale = Vector3.one;
                logRootRect.transform.localPosition = new Vector3(0.0f, 0.0f, 2.0f);
                logRootRect.localEulerAngles = Vector3.zero;

            }

            _logManager.transform.parent = logRootRect.transform;
            RectTransform rectTransform = _logManager.AddComponent<RectTransform>();
            rectTransform.sizeDelta = logRootRect.sizeDelta;
            rectTransform.localScale = Vector3.one;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localEulerAngles = Vector3.zero;

            Canvas canvas = _logManager.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.pixelPerfect = false;

            _logManager.SetActive(true);

            return _logManager;
        }

        GameObject SetBackground(GameObject _Go)
        {
            _Go = new GameObject("Background");
            _Go.transform.SetParent(debugUIManager.transform);

            RectTransform rect = _Go.AddComponent<RectTransform>();
            _Go.AddComponent<CanvasRenderer>();
            Image img = _Go.AddComponent<Image>();

            Vector2 size = debugUIManager.GetComponent<RectTransform>().sizeDelta;
            size.x *= 4.0f;
            //rect.sizeDelta = debugUIManager.GetComponent<RectTransform>().sizeDelta;
            rect.sizeDelta = size;
            rect.localScale = Vector3.one;
            rect.localPosition = Vector3.zero;
            rect.localEulerAngles = Vector3.zero;

            img.color = new Color(7f / 255f, 45f / 255f, 71f / 255f, 200f / 255f);

            return _Go;
        }

        GameObject SetUiText(GameObject _Go)
        {
            _Go = new GameObject("LogText");
            _Go.transform.SetParent(backgroundImage.transform);

            Text lText = _Go.AddComponent<Text>();

            RectTransform rectTransform = lText.GetComponent<RectTransform>();
            //rectTransform.sizeDelta = backgroundImage.GetComponent<RectTransform>().sizeDelta * 100.0f;
            Vector2 size = backgroundImage.GetComponent<RectTransform>().sizeDelta;
            rectTransform.sizeDelta = new Vector2(size.x * 300.0f, size.y * 100.0f);
            rectTransform.localPosition = Vector3.zero;

            lText.text = "";
            lText.fontSize = 10;
            lText.transform.localEulerAngles = Vector3.zero;

            lText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            lText.alignment = TextAnchor.MiddleCenter;

            rectTransform.localScale = Vector3.one / 100.0f;
            return _Go;
        }


        GUIStyle uiStyle;
#if USE_ONGUI
        void OnGUI()
        {
            if (!isShownOnGui) {
                GUI.Label(new Rect(10, 10, Screen.width * 0.9f, Screen.height * 0.9f), debugLog.value, uiStyle);

            }

        }

#endif

    }


}

