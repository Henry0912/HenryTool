using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;

public class AiUnitController : UnitBasic
{
    //public UnitList unitList;
    public UnitBasic targetUnit;

    float actionTime;

    // Use this for initialization
    void Start() {
        actionTime = Time.time + 1.0f;
    }

    Vector3 tempVec3 = Vector3.zero;

    // Update is called once per frame
    void Update() {
        if (targetUnit == this) {
            if (Time.time > actionTime) {
                tempVec3 = transform.position + (new Vector3(Random.Range(0.0f, 5.0f), Random.Range(0.0f, 5.0f), Random.Range(0.0f, 5.0f)));
                actionTime += 3.0f;
                hp = maxHp;
            }
            else {
                if (Random.Range(0, 8) > 2) {
                    targetUnit = unitList.GetRandomUnit();
                }
            }

            Vector3 tempV3 = tempVec3 - transform.position;
            transform.Translate(tempV3 * Time.deltaTime, Space.World);

        }
        else if (targetUnit == null) {
            targetUnit = unitList.GetRandomUnit();
        }
        else if (!targetUnit.gameObject.activeSelf) {
            targetUnit = unitList.GetRandomUnit();
        }
        else {
            Vector3 tempV3 = targetUnit.transform.position - transform.position;
            if (tempV3.sqrMagnitude < 5.0f) {
                if (Time.time > actionTime) {
                    AttackUnit((AiUnitController)targetUnit);
                    Debug.DrawLine(transform.position, targetUnit.transform.position, Color.red);
                    actionTime += 0.6f;
                }

            }
            else {

                transform.Translate(tempV3 * Time.deltaTime, Space.World);
            }
        }




    }

    void AttackUnit(AiUnitController _target) {
        _target.GetDemage(Random.Range(5, 25));
    }

    public void GetDemage(int _demage) {
        hp -= _demage;
        if (hp <= 0) {
            unitList.Remove(this, true);
            //RecycleSelf();
        }
    }

}
