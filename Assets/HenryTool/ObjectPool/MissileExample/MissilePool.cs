using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;

namespace HenryTool {
    [CreateAssetMenu(fileName = "MissilePool", menuName = "Pool/MissilePool", order = 3)]
    public class MissilePool : ObjectPoolBase
    {
        [Range(10.0f, 200.0f)]
        public float initSpeed;
        [Range(10.0f, 500.0f)]
        public float finalSpeed;

        [Range(2, 10)]
        public int skipFrames;


        [Range(1.0f, 100.0f)]
        public float hitDistance;
        [Range(0.1f, 100.0f)]
        public float recyclePeriod;
        [Range(0.0f, 0.99f)]
        public float directionAmount;



        public float RandomFloatY
        {
            get
            {
                return Random.Range(0.0f, 40.0f);
            }
        }

        public float RandomFloat
        {
            get
            {
                return Random.Range(-10.0f, 10.0f);
            }
        }

        public void InitStart() {
            //poolQueue = (Queue<MissileObject>)poolQueue;
        }


        public void FireOneMissile(Transform _cannon, GameObject _target) {
            ((MissileObject)GetOneObject()).FireMissile(_cannon.position, GetMissileVelocity(_cannon), _target);

        }

        Vector3 GetMissileVelocity(Transform _cannon)
        {
            return Vector3.Slerp(_cannon.forward, GetMissileVelocityUp(_cannon), directionAmount);

        }

        Vector3 GetMissileVelocityUp(Transform _cannon)
        {
            Vector3 fwd = _cannon.forward;
            float vx = RandomFloat;
            float vy = RandomFloatY;
            float vz = (-(fwd.x * vx) - (fwd.y * vy)) / fwd.z;

            Vector3 velocity = new Vector3(vx, vy, vz);

            return velocity.normalized;
        }



    }

}
