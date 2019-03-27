using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HenryTool {

    public class UnitBasic : PoolObject
    {
        public UnitList unitList;

        //public UnitStatusData unitStatus;
        public float maxHp;
        public float maxMp;
        public float hp;
        public float mp;

        private int updateFrameCount;
        //public GameObject statusUiPrefab;

        public void InitUnit() {
            maxHp = 100.0f;
            maxMp = 100.0f;
            hp = 100.0f;
            mp = 100.0f;
            //Debug.Log("wp:" + transform.position);
            //Debug.Log("sp:" + Camera.main.WorldToScreenPoint(transform.position));
        }

        public void UpdateUnitData() {
            updateFrameCount = Time.frameCount;

        }


        //public Vector3 pos;



        /*
        // Use this for initialization
        void Start () {}

        // Update is called once per frame
        void Update () {}

        */

    }


}



