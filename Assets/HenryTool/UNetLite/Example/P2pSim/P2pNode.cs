using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;

public class P2pNode : PoolObject
{
    public int nodeId;
    
    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }


    public void InitP2pObject(int _id)
    {
        nodeId = _id;
    }

    public void SetPosition(int _maxId) {

    }


}

