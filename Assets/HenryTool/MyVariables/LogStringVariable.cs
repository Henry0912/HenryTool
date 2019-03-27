using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



namespace HenryTool
{
    [CreateAssetMenu]
    public class LogStringVariable : ScriptableObject
    {
        public int maxLines = 30;

        [Multiline]
        public string value;

        //public File logFile;

        public void LogError(string _errorLog) {
            AddLogLine("E: "+_errorLog);
        }

        public void Log(string _log) {
            AddLogLine(_log);
        }


        public void AddLogLine(string _log) {
            #region Editor
#if UNITY_EDITOR
            Debug.Log(_log);
#endif
            #endregion

            value += _log + "\n";

            if (maxLines > 0) {
                char[] delimiterChars = { '\n' };
                string[] logs = value.Split(delimiterChars);

                if (logs.Length > maxLines) {
                    value = "";
                    for (int i = (logs.Length - maxLines); i < maxLines; i++) {
                        value += logs[i] + "\n";
                    }
                }

            }

        }


        public void ClearLog() {
            value = "";
        }

#if UNITY_EDITOR
        [Multiline]
        public string objectDescription;
#endif

    }


}





