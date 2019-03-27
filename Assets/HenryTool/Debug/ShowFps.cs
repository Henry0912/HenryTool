using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFps : MonoBehaviour
{
    float fps;
    float fpsMax;
    float fpsMin;

    GUIStyle uiStyle;

    // Use this for initialization
    void Start()
    {
        fpsMax = 0.0f;
        fpsMin = float.MaxValue;
        uiStyle = new GUIStyle();
        uiStyle.fontSize = (int)(Screen.height / 50.0f);

    }

    // Update is called once per frame
    void Update() {
        fps = ((int)((1.0f / Time.deltaTime) * 10.0f)) / 10.0f;
        if (fps > fpsMax)
            fpsMax = fps;

        if (fps < fpsMin)
            fpsMin = fps;

    }

    void OnGUI() {
        if (GUI.Button(new Rect(10.0f, 20.0f, 200.0f, 150.0f), "FPS: " + fps.ToString(), uiStyle))
        {
            Debug.Log("targetFrameRate::: " + Application.targetFrameRate);
        }

        if (GUI.Button(new Rect(400.0f, 20.0f, 200.0f, 150.0f), "FPS Min: " + fpsMin.ToString(), uiStyle))
        {
            fpsMin = float.MaxValue;
        }

        if (GUI.Button(new Rect(800.0f, 20.0f, 200.0f, 150.0f), "FPS Max: " + fpsMax.ToString(), uiStyle))
        {
            fpsMax = 0.0f;
        }

        /*
        if (GUI.Button(new Rect(10, 110, 150, 100), "Screen Size: " + Screen.width + " x " + Screen.height, uiStyle)) {
            Debug.Log("targetFrameRate::: " + Application.targetFrameRate);
        }
        */

    }


}
