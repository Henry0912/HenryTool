using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HenryTool;

namespace HenryTool
{
    [RequireComponent(typeof(Rigidbody))]
    public class MissileObject : MonoBehaviour, IMissile
    {
        MissilePool missilePool;

        Rigidbody mRigidbody;
        [SerializeField]
        MissileInfo mInfo;

        DelegateVoidOfVoid mFixedBehavior;

        float autoRecycleTime;

        int frameCount;
        Vector3 targetDir;
        float dirAmount;

        int nextActionFrame;

        public TrailRenderer theTrail;


        public void FireMissile()
        {
            if (mInfo.target != null)
            {
                dirAmount = 0.01f;

                nextActionFrame = missilePool.skipFrames;

                frameCount = 0;

                transform.position = mInfo.startPosition;
                mRigidbody.velocity = mInfo.startDirection * mInfo.initSpeed;

                if (theTrail != null)
                    theTrail.Clear();

                autoRecycleTime = Time.time + missilePool.recyclePeriod;
                mFixedBehavior = FireMissileBehavior;

            }


        }

        public void FireMissile(MissileInfo _missileInfo)
        {
            mInfo = _missileInfo;
            FireMissile();
        }

        public void FireMissile(Vector3 _startPos, Vector3 _startDir)
        {
            mInfo.startPosition = _startPos;
            mInfo.startDirection = _startDir;

            mInfo.initSpeed = missilePool.initSpeed;
            mInfo.finalSpeed = missilePool.finalSpeed;

            FireMissile();


        }

        public void FireMissile(Vector3 _startPos, Vector3 _startDir, GameObject _target)
        {
            mInfo.target = _target.transform;
            FireMissile(_startPos, _startDir);

        }

        private void Update()
        {
            MissileBehavior();
        }

        private void FixedUpdate()
        {
            if (frameCount >= nextActionFrame)
            {
                mFixedBehavior();
                nextActionFrame += missilePool.skipFrames;
            }
            frameCount++;


        }



        public void InitObject(ObjectPoolBase _objPool)
        {
            missilePool = (MissilePool)_objPool;
            mRigidbody = GetComponent<Rigidbody>();
            mFixedBehavior = FireMissileBehavior;

        }

        public void RecycleSelf()
        {
            nextActionFrame = int.MaxValue;
            missilePool.Recycle(this);
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void FireMissileBehavior()
        {
            mRigidbody.velocity *= 0.9f;

            if (frameCount >= 10)
                mFixedBehavior = TurnMissile;

        }

        void TurnMissile()
        {
            targetDir = mInfo.target.position - transform.position;
            dirAmount = Time.fixedDeltaTime * frameCount;
            mRigidbody.velocity = 0.9f * (Vector3.Slerp(mRigidbody.velocity, targetDir, dirAmount));

            if (frameCount >= 50)
                mFixedBehavior = FinalVelocity;

        }

        void FinalVelocity()
        {
            targetDir = mInfo.target.position - transform.position;
            mRigidbody.velocity = mInfo.finalSpeed * targetDir.normalized;

            mFixedBehavior = CheckRecycle;

        }

        void CheckRecycle()
        {
            if (Time.time >= autoRecycleTime)
                RecycleSelf();

        }


        public void MissileBehavior()
        {
            float dis = (mInfo.target.position - transform.position).sqrMagnitude;

            if (dis <= missilePool.hitDistance)
            {
                RecycleSelf();
                return;
            }

        }

       
    }


}