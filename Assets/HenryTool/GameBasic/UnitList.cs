using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace HenryTool
{
    [CreateAssetMenu]
    public class UnitList : ScriptableObject
    {
        [SerializeField]
        private List<UnitBasic> unitList = new List<UnitBasic>();

        public List<UnitListEvent> addUnitEventListeners = new List<UnitListEvent>();
        public List<UnitListEvent> removeUnitEventListeners = new List<UnitListEvent>();


        public void Add(UnitBasic _unit) {
            if (!unitList.Contains(_unit)) {
                unitList.Add(_unit);
                InvokeListEvent(addUnitEventListeners, _unit);

            }
        }

        public void Remove(UnitBasic _unit, bool _recycle) {
            if (unitList.Contains(_unit)) {
                unitList.Remove(_unit);
                InvokeListEvent(removeUnitEventListeners, _unit);

            }

            if (_recycle)
                _unit.RecycleSelf();
            else
                Destroy(_unit.gameObject);
        }

        private void InvokeListEvent(List<UnitListEvent> _eventListeners, UnitBasic _unit) {
            for (int i = (_eventListeners.Count - 1); i >= 0; i--) {
                if (!_eventListeners[i].Equals(null)) {
                    _eventListeners[i](_unit);
                }
                else {
                    _eventListeners.RemoveAt(i);
                }
            }
        }

        public UnitBasic GetRandomUnit() {
            UnitBasic instance = null;
            if (unitList.Count > 0) {
                int index = Random.Range(0, unitList.Count);
                instance = unitList[index];
            }

            return instance;
        }


        public void ClearAndResetList() {
            unitList.Clear();
            addUnitEventListeners.Clear();
            removeUnitEventListeners.Clear();
        }

        private void OnEnable() {
            ClearAndResetList();
        }

    }


}

