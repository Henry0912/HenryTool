using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;

public class P2pSim : MonoBehaviour
{
    public ObjectPool objPool;

    public GameObject poolRoot;

    //public List<PoolObject> objList;

    public Dictionary<int, PoolObject> nodeList = new Dictionary<int, PoolObject>();

    public Queue<int> nodeIdQueue = new Queue<int>();

    public int maxNodeId = 0;

    public int randomNodeId
    {
        get {
            if (nodeList.Count > 0) {
                int result = Random.Range(0, maxNodeId);
                bool flag = true;
                bool isFound = nodeList.ContainsKey(result);

                while (!isFound) {
                    result = Random.Range(0, maxNodeId);
                    if (flag) {
                        isFound = !nodeIdQueue.Contains(result);
                        flag = false;
                    }
                    else {
                        isFound = nodeList.ContainsKey(result);
                        flag = true;
                    }
                }

                return result;
            }
            else {
                return -1;
            }

        }


    }

    public float randomFloat
    {
        get {
            return Random.Range(-5.0f, 5.0f);
        }

    }
    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Add")) {
            P2pNode node = (P2pNode)objPool.GetOneObject();

            node.transform.position = transform.position + new Vector3(randomFloat, randomFloat, randomFloat);
            node.transform.SetParent(transform);

            int nodeId;
            if (nodeIdQueue.Count > 0) {
                nodeId = nodeIdQueue.Dequeue();
            }
            else {
                nodeId = maxNodeId;
                maxNodeId++;
            }

            node.InitP2pObject(nodeId);
            nodeList.Add(nodeId, node);


        }

        if (GUI.Button(new Rect(200, 10, 150, 100), "delete")) {
            if (nodeList.Count > 0) {
                int nodeId = randomNodeId;
                DebugLogMain.hLog(""+nodeId);
                PoolObject pObj = nodeList[nodeId];
                nodeList.Remove(nodeId);
                nodeIdQueue.Enqueue(nodeId);
                pObj.RecycleSelf();
                
            }

            /*
            if (nodeList.Count > 0) {
                int index = Random.Range(0, nodeList.Count);
                Debug.Log("" + index);
                PoolObject obj = nodeList[index];
                nodeList.Remove(index);
                //objList.RemoveAt(index);
                obj.RecycleSelf();
            }
            */

        }

        if (GUI.Button(new Rect(400, 10, 150, 100), "Recycle All")) {
            objPool.RecycleAll();

        }

        if (GUI.Button(new Rect(600, 10, 150, 100), "Initialize Pool")) {
            objPool.InitPool(5);

        }

    }

}
