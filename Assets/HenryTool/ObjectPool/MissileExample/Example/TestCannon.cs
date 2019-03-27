using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;

public class TestCannon : MonoBehaviour
{
    public MissilePool missilePool;

    public float nextFireTime;

    public GameObject target;

    [Range(0.01f, 1.0f)]
    public float firePeriod = 0.05f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform);

        if (Time.time > nextFireTime)
        {
            missilePool.FireOneMissile(transform, target);
            nextFireTime = Time.time + firePeriod;
        }
    }
}
