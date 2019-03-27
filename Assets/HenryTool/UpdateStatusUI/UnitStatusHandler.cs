using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;
using UnityEngine.UI;

public class UnitStatusHandler : MonoBehaviour
{
    public ObjectPool statusUiPool;
    public UnitList theUnitList;
    //public Queue<UnitBasic> tempUnitQueue;
    public Dictionary<UnitBasic, UnitStatus> unitDict;

    public GameObject StatusUiPrefab;

    public Transform activeUi;
    public Transform inactiveUi;

    private void Awake() {
        theUnitList.addUnitEventListeners.Add(OnAddUnit);
        theUnitList.removeUnitEventListeners.Add(OnRemoveUnit);
        //tempUnitQueue = new Queue<UnitBasic>();
        unitDict = new Dictionary<UnitBasic, UnitStatus>();
    }

    // Use this for initialization
    public void InitStart(Canvas _canvas) {
        //uiCanvas = _canvas;
    }

    // Update is called once per frame
    void Update() {
        //updateBehavior();
        UpdateStatusUIs();
    }



    void OnAddUnit(UnitBasic _unit) {
        UnitStatus obj;

        if (activeUi != null) {
            obj = (UnitStatus)(statusUiPool.GetOneObject());
            obj.transform.SetParent(activeUi);
            obj.InitStart(_unit);
        }
        else {
            //tempUnitQueue.Enqueue(_unit);
            obj = null;
        }

        unitDict.Add(_unit, obj);

    }

    void OnRemoveUnit(UnitBasic _unit) {
        if (unitDict.ContainsKey(_unit)) {
            unitDict[_unit].GetComponent<PoolObject>().RecycleSelf();
            unitDict[_unit].transform.SetParent(inactiveUi);
            unitDict.Remove(_unit);

        }

    }

    void UpdateStatusUIs() {
        foreach (KeyValuePair<UnitBasic, UnitStatus> statusUi in unitDict) {
            statusUi.Value.UpdateStatus();
        }



    }

    void ShowUnitStatus(UnitBasic _unit) {

    }



}
