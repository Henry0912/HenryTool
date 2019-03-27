#define USE_ONGUI
using UnityEngine;
using HenryTool;


public class ErrorLogManager : MonoBehaviour
{
    public bool isShownOnGui;
    public LogStringVariable errorLog;

    GameObject debugUIManager;


    void Awake() {
        uiStyle = new GUIStyle();
        uiStyle.fontSize = 25;
        //uiStyle.fontSize = 100;
        errorLog.value = "";

        if (isShownOnGui) {
            debugUIManager = new GameObject();
            debugUIManager.name = "LogManager";

            debugUIManager.transform.parent = Camera.main.transform;

            RectTransform rectTransform = debugUIManager.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(100f, 100f);
            rectTransform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            rectTransform.localPosition = new Vector3(0.0f, 0.0f, 0.83f);
            rectTransform.localEulerAngles = Vector3.zero;

            Canvas canvas = debugUIManager.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.pixelPerfect = false;

            debugUIManager.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update() {

    }


    GUIStyle uiStyle;
#if USE_ONGUI
    void OnGUI() {
        /*
        if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button")) {
            errorLog.AddLogLine("log: " + Time.time);
        }
        */

        if (!isShownOnGui) {
            GUI.Label(new Rect(10, 10, Screen.width * 0.9f, Screen.height * 0.9f), errorLog.value, uiStyle);

        }

    }


#endif

}

