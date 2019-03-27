using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using HenryTool;
using System;

namespace HenryTool
{
    public delegate void WebRequestCallBack(string _result);

    [CreateAssetMenu]
    public class WebServiceObject : ScriptableObject
    {
        public const string TIME_OUT = "ERROR_TIME_OUT";
        public const string NETWORK_ERROR = "NETWORK_ERROR";

        public string domainUrl;

        //public Dictionary<string, string> serviceUrl;
        public string[] serviceUrl;

        private WebMainObject coroutimeGameObj;
        public WebMainObject CoroutimeGameObj
        {
            get {
                if (coroutimeGameObj == null)
                    coroutimeGameObj = (new GameObject("WebMain")).AddComponent<WebMainObject>();

                return coroutimeGameObj;
            }

            set {
                coroutimeGameObj = value;
            }
        }

        public Coroutine Get(string _uri, WWWForm _data, int _timeOut, WebRequestCallBack _callBack)
        {
            if (CoroutimeGameObj == null) {
                CoroutimeGameObj = (new GameObject("WebMain")).AddComponent<WebMainObject>();

            }

            return CoroutimeGameObj.StartCoroutine(WebRequestPost(_uri, _data, _timeOut, _callBack));
        }

        public Coroutine Post(string _uri, WWWForm _data, int _timeOut, WebRequestCallBack _callBack)
        {
            if (CoroutimeGameObj == null) {
                CoroutimeGameObj = (new GameObject("WebMain")).AddComponent<WebMainObject>();

            }

            return CoroutimeGameObj.StartCoroutine(WebRequestPost(_uri, _data, _timeOut, _callBack));
        }


        public Coroutine Get<T>(string _uri, WebRequestResult _result, int _timeOut, WebRequestCallBack _timeOutCallBack) where T : WebRequestResult
        {
            _result = result;

            return CoroutimeGameObj.StartCoroutine(WebRequestGet<T>(_uri, _timeOut, _timeOutCallBack));
        }




        public WebRequestResult result;

        IEnumerator WebRequestGet<T>(string _uri, int _timeOut, WebRequestCallBack _timeOutCallBack) where T : WebRequestResult
        {
            _timeOutCallBack("");
            UnityWebRequest request = UnityWebRequest.Get(_uri);

            AsyncOperation res = request.SendWebRequest();

            int cnt = 0;
            while (!res.isDone) {
                cnt++;
                if (cnt > _timeOut) {
                    _timeOutCallBack(TIME_OUT);
                    yield break;
                }

                yield return new WaitForSecondsRealtime(0.2f);
            }

            if (!request.isNetworkError) {
                string html = request.downloadHandler.text;
                DebugLogMain.hLog(html);
                result = JsonUtility.FromJson<T>(html);
            }
            else {

            }


            _timeOutCallBack(request.downloadHandler.text);

        }

        IEnumerator WebRequestPost(string _uri, WWWForm _data, int _timeOut, WebRequestCallBack _wrCallBack)
        {
            _wrCallBack("");
            UnityWebRequest request = UnityWebRequest.Post(_uri, _data);

            int cnt = 0;
            AsyncOperation res = request.SendWebRequest();

            while (!res.isDone) {
                cnt++;

                if (cnt > _timeOut) {
                    _wrCallBack(TIME_OUT);
                    yield break;
                }

                yield return new WaitForSecondsRealtime(0.2f);
            }

            if (request.isNetworkError) {
                _wrCallBack(NETWORK_ERROR);
                yield break;
            }

            _wrCallBack(request.downloadHandler.text);

        }



    }

    public interface WebRequestResult
    {
    }

    public struct ResultTest : WebRequestResult
    {
    }

}





