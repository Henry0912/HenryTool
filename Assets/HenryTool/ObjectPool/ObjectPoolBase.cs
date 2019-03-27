using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using System;

namespace HenryTool
{
    [CreateAssetMenu(fileName = "PoolBase", menuName = "Pool/ObjectPoolBase", order = 1)]
    public class ObjectPoolBase : ScriptableObject
    {
        public Queue<IPoolObject> poolQueue = new Queue<IPoolObject>();

        /// <summary>
        /// The Prefab for instantiating the pool object.
        /// </summary>
        public GameObject objectPrefab;

        /// <summary>
        /// the maximum PoolObject count of the pool
        /// </summary>
        [Range(1, 300)]
        public int maxObjCount = 10;


        public IPoolObject GetOneObject()
        {
            IPoolObject instance = null;
            if (poolQueue.Count > 0)
            {
                instance = poolQueue.Dequeue();
            }
            else
            {
                instance = Instantiate(objectPrefab).GetComponent<IPoolObject>();
                instance.InitObject(this);

                #region UNITY_EDITOR
#if UNITY_EDITOR
                if (poolRoot == null)
                {
                    poolRoot = new GameObject("" + objectPrefab.name + "Pool").transform;
                    poolRoot.position = Vector3.zero;
                    poolRoot.eulerAngles = Vector3.zero;
                }
#endif
                #endregion

            }


            instance.GetGameObject().SetActive(true);

            #region UNITY_EDITOR
#if UNITY_EDITOR
            instance.GetGameObject().transform.SetParent(poolRoot);
#endif
            #endregion

            return instance;

        }

        public void Recycle(IPoolObject _object)
        {
            if (poolQueue.Count < maxObjCount)
            {
                _object.GetGameObject().SetActive(false);
                poolQueue.Enqueue(_object);

                #region UNITY_EDITOR
#if UNITY_EDITOR
                _object.GetGameObject().transform.SetParent(poolRoot);
#endif
                #endregion
            }
            else
            {
                Destroy(_object.GetGameObject());
            }

        }


        #region UNITY_EDITOR
#if UNITY_EDITOR
        public Transform poolRoot;
#endif
        #endregion



    }







}

