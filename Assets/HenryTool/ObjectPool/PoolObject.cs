using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;


namespace HenryTool
{
    public class PoolObject : MonoBehaviour, IPoolObject
    {
        protected ObjectPool objPool;

        public void InitObject(ObjectPoolBase _thePool) {
            objPool = (ObjectPool)_thePool;
        }

        public void RecycleSelf() {
            objPool.Recycle(this);
        }

        public GameObject GetGameObject()
        {
            return gameObject;

        }
    }





}


