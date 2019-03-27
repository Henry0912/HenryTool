using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;

namespace HenryTool {
    public interface IMissile : IPoolObject
    {
        void MissileBehavior();
        void FireMissileBehavior();

        void FireMissile(Vector3 _startPos, Vector3 _startDir, GameObject _target);


    }

    public struct MissileInfo
    {
        public Vector3 startPosition;
        public Vector3 startDirection;
        public Transform target;

        public float initSpeed;
        public float finalSpeed;

    }


}


