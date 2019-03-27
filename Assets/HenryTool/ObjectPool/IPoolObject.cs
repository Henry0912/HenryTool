using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;

namespace HenryTool
{
    public interface IPoolObject
    {
        GameObject GetGameObject();

        void InitObject(ObjectPoolBase _objPool);

        void RecycleSelf();

    }

}

