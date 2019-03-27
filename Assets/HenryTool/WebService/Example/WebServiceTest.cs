using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;

public class WebServiceTest : MonoBehaviour {

    public WebServiceObject webServiceObj;

    public WebRequestCallBack testCallBack;

    // Use this for initialization
    void Start () {
        //http://chanel.ndm.tw/api/getid.ashx

        StartCoroutine(GetTest());

    }

    // Update is called once per frame
    //void Update () {}

    IEnumerator GetTest() {
        yield return null;

        WebRequestResult result = null;


        yield return webServiceObj.Get<IdFormat>("http://chanel.ndm.tw/api/getid.ashx", result, 1000, tempCallBack);

        if (result != null) {
            DebugLogMain.hLog(((IdFormat)result).KEY);
        }
        else {
            DebugLogMain.hLog("error: "+((IdFormat)webServiceObj.result).KEY);
        }

    }


    void tempCallBack(string _msg) {

    }

}


public struct IdFormat: WebRequestResult
{
    public string RS;
    public string KEY;
}
