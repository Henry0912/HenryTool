using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;

public class ObjectPoolExample : MonoBehaviour
{
    public ObjectPool objPool;

    public GameObject poolRoot;

    public List<PoolObject> objList;

    // Use this for initialization
    void Start() {
        objList = new List<PoolObject>();

        //objPool.maxObjCount = 6;
    }

    // Update is called once per frame
    void Update() {

    }

    void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Add")) {
            PoolObject obj = (PoolObject)objPool.GetOneObject();

            obj.transform.position = transform.position + new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
            obj.transform.SetParent(transform);

            objList.Add(obj);
            
        }

        if (GUI.Button(new Rect(200, 10, 150, 100), "delete")) {
            if (objList.Count > 0) {
                int index = Random.Range(0, objList.Count);
                Debug.Log("" + index);
                PoolObject obj = objList[index];
                objList.RemoveAt(index);
                obj.RecycleSelf();
            }
            
        }

        if (GUI.Button(new Rect(400, 10, 150, 100), "Recycle All")) {
            objPool.RecycleAll();
            
        }

        if (GUI.Button(new Rect(600, 10, 150, 100), "Initialize Pool")) {
            objPool.InitPool(5);

        }

    }

}
