using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.UI;

namespace HenryTool
{
    public class UnitStatus : PoolObject
    {
        public const float STATUS_SIZE = 4.0f;
        //public const float STATUS_UI_POSITION = 100.0f;

        public Image hpBar;

        private UnitBasic theUnit;

        private int frameCount;
        private float _depth;
        public float depth {
            get {
                if (frameCount != Time.frameCount) {
                    _depth = Camera.main.WorldToScreenPoint(theUnit.transform.position).z;
                    frameCount = Time.frameCount;
                }

                return _depth;
            }
        }

        public RectTransform uiRootRect;

        public void InitStart(UnitBasic _unit) {
            theUnit = _unit;

            uiRootRect = GetComponent<RectTransform>();
            //Debug.Log(transform.position);
        }

        void Update() { }

        public void UpdateStatus() {
            if (theUnit != null) {
                UpdatePosition();

                hpBar.fillAmount = theUnit.hp / theUnit.maxHp;

                ShowStatusUi();
            }
            else {
                HideStatusUi();
            }


        }


        void UpdatePosition() {
            Vector3 camPos = Camera.main.transform.position;
            Vector3 unitPos = theUnit.transform.position;

            RaycastHit hit;

            if (Physics.Raycast(camPos, (unitPos - camPos), out hit)) {
                if (hit.transform != theUnit.transform) {
                    Debug.DrawLine(Camera.main.transform.position, hit.point, Color.yellow);
                    HideStatusUi();

                }
                else {
                    //Debug.DrawLine(Camera.main.transform.position, hit.point, Color.blue);
                    Vector3 unitScreenPos = Camera.main.WorldToScreenPoint(unitPos);

                    Vector3 tempV3 = Vector3.zero;
                    tempV3.y = (Screen.height * 0.2f) / unitScreenPos.z;
                    transform.position = unitScreenPos + tempV3;

                    tempV3.x = STATUS_SIZE / unitScreenPos.z;
                    tempV3.y = STATUS_SIZE / unitScreenPos.z;
                    tempV3.z = STATUS_SIZE / unitScreenPos.z;

                    uiRootRect.localScale = tempV3;
                }
            }



        }

        void ShowStatusUi() {
            //hpBar.color = new Color(hpBar.color.r, hpBar.color.g, hpBar.color.b, 1.0f);
        }

        void HideStatusUi() {
            transform.position = new Vector3(0.0f, 0.0f, -100.0f);

            //hpBar.color = new Color(hpBar.color.r, hpBar.color.g, hpBar.color.b, 0.0f);
        }

    }

}