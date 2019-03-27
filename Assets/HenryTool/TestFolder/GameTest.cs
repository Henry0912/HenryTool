using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.UI;

public class GameTest : MonoBehaviour
{
    public WebCamTextureVariable theTexture;

    public LogStringVariable errorLog;

    // Use this for initialization
    void Start() {
        StartCoroutine(CheckWebCam());

    }

    // Update is called once per frame
    void Update() {

    }

    const int TIME_OUT = 10000;

    IEnumerator CheckWebCam() {
        int cnt = 0;

        while (!theTexture.isPlaying) {
            yield return null;

            cnt++;
            if (cnt >= TIME_OUT)
                yield break;

        }

        if (cnt < TIME_OUT)
            GetComponent<MeshRenderer>().material.mainTexture = theTexture.theWebCam;

    }


}
