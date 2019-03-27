using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HenryTool;


namespace HenryTool
{
    [CreateAssetMenu]
    public class GuiCanvas : ScriptableObject
    {
        public Canvas uiCanvas;

        //private bool _isVisible;
        public bool isVisible {
            get {
                return (uiCanvas != null);
            }

            //set { _isVisible = value; }
        }

        // Use this for initialization
        //void Start () {}

        // Update is called once per frame
        //void Update () {}


    }

}

