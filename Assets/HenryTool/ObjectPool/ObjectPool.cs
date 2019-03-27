using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using System;

namespace HenryTool
{
    [CreateAssetMenu(fileName = "ObjectPool", menuName = "Pool/ObjectPool", order = 2)]
    public class ObjectPool : ObjectPoolBase
    {
        public void InitPool(GameObject _objectPrefab, uint _objectCount) {
            objectPrefab = _objectPrefab;
            InitPool(_objectCount);
        }

        public void InitPool(uint _objectCount)
        {
            if (_objectCount == 0) {
                ClearPool();
            }
            else {
                while (poolQueue.Count > _objectCount) {
                    Destroy(poolQueue.Dequeue().GetGameObject());
                }

                while (poolQueue.Count < _objectCount) {
                    #region UNITY_EDITOR
#if UNITY_EDITOR
                    if (poolRoot == null) {
                        poolRoot = new GameObject("" + objectPrefab.name + "Pool").transform;
                        poolRoot.position = Vector3.zero;
                        poolRoot.eulerAngles = Vector3.zero;
                    }
#endif
                    #endregion

                    IPoolObject instance = Instantiate(objectPrefab).GetComponent<IPoolObject>();
                    instance.InitObject(this);
                    Recycle(instance);


                }

            }
           

        }

        public void RecycleAll()
        {
            foreach (IPoolObject ipo in poolQueue)
            {
                ipo.RecycleSelf();
            }
        }

        public void ClearPool()
        {
            poolQueue.Clear();

        }



    }







}

