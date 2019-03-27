using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

namespace HenryTool
{
    public class UNetBase : MonoBehaviour
    {
        public const bool RELIABLE = true;
        public const bool UNRELIABLE = false;
        //public delegate void UNetMessageHandler(NetworkMessage _msg);

        public DelegateVoidOfString uLog = new DelegateVoidOfString(DebugLogMain.hLog);
        public NetworkMessageDelegate OnError = new NetworkMessageDelegate(DummyEmpty);

        protected void OnErrorHandler(NetworkMessage _msg)
        {
            var msg = _msg.ReadMessage<ErrorMessage>();
            uLog("Network error occurred: " + (NetworkError)msg.errorCode);

        }

        protected static void DummyEmpty(NetworkMessage _dummy) { }


    }

}

