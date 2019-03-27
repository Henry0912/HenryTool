using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;

public class StatusUiTest : MonoBehaviour
{
    public ObjectPool unitPool;
    public UnitList theUnitList;

    public Transform activeObjectsRoot;
    public Transform inactiveObjectsRoot;


    public float spawnTime;

    // Use this for initialization
    void Start() {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update() {
        if (Time.time > spawnTime) {
            SpawnUnit();
            spawnTime += 2.0f;
        }
    }

    void SpawnUnit() {
        UnitBasic unit = (UnitBasic)unitPool.GetOneObject();

        unit.transform.position = transform.position + new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
        unit.transform.SetParent(activeObjectsRoot);

        unit.InitUnit();

        theUnitList.Add(unit);
    }

    void OnGUI() {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Add")) {
            SpawnUnit();
            
        }

        if (GUI.Button(new Rect(200, 10, 150, 100), "delete")) {
            UnitBasic tempObj = theUnitList.GetRandomUnit();
            if (tempObj != null) {
                //tempObj.RecycleSelf();
                tempObj.transform.SetParent(inactiveObjectsRoot);
                theUnitList.Remove(tempObj, true);
            }

        }


    }

}
