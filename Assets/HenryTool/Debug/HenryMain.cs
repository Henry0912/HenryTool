using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;


namespace HenryTool
{
    public class HenryMain : MonoBehaviour
    {
        public LogStringVariable dLog;

        // Use this for initialization
        void Start()
        {
            DebugLogMain.InitDebugLog(dLog);
        }

        // Update is called once per frame
        //void Update() { }

    }

}


