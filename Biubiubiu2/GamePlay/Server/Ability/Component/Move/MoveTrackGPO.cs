using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class MoveTrackGPO : ServerNetworkComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public int TargetGPOId;
            public float TrackSpeed;
            public float LockSpeed;
            public float StopTrackDistance;
        }
        private float trackSpeed;
        private float lockSpeed;
        private float stopTrackDistance;
        private Vector3 forward = Vector3.zero;
        private Vector3 startPoint = Vector3.zero;
        private IGPO targetGPO;
        private int targetGPOId;
        private bool isStopTracking = false;
        private Transform targetBody;
        private bool isGetBody = false;
        
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.TargetGPOId, initData.TrackSpeed, initData.LockSpeed, initData.StopTrackDistance);
        }

        protected override void OnStart() {
            base.OnStart();
            startPoint = iEntity.GetPoint();
            forward = iEntity.GetRota() * Vector3.forward;
            MsgRegister.Dispatcher(new SM_GPO.GetGPO {
                GpoId = targetGPOId,
                CallBack = gpo => {
                    targetGPO = gpo;
                    targetBody = gpo.GetTargetTransform();
                    if (targetBody != null) {
                        isGetBody = true;
                    }
                }
            });
            AddUpdate(OnUpdate);
        }


        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
        }
        
        public void SetData(int targetGPOId,float trackSpeed,float lockSpeed, float stopTrackDistance) {
            this.targetGPOId = targetGPOId;
            this.trackSpeed = trackSpeed;
            this.lockSpeed = lockSpeed;
            this.stopTrackDistance = stopTrackDistance;
        }

        private void Tracking(float delta) {
            if (isStopTracking) {
                return;
            }
            float distance = Vector3.Distance(iEntity.GetPoint(), targetGPO.GetPoint());
            if (targetGPO.IsGodMode() || distance <= stopTrackDistance) {
                isStopTracking = true;
            } else {
                var point = iEntity.GetPoint();
                if (targetGPO.IsClear() == false && targetBody != null) {
                    forward = (targetBody.position - point).normalized;
                }
                iEntity.SetRota(Quaternion.LookRotation(forward));
                point += forward * this.trackSpeed * delta;
                iEntity.SetPoint(point);
#if UNITY_EDITOR
                Debug.DrawLine(startPoint, point, Color.red);
#endif
            }
        }

        private void Locking(float delta) {
            if (isStopTracking) {
                var point = iEntity.GetPoint();
                point += forward * this.lockSpeed * delta;
                iEntity.SetPoint(point);
#if UNITY_EDITOR
                Debug.DrawLine(startPoint, point, Color.red);
#endif
            }
        }

        private void OnUpdate(float delta) {
            Tracking(delta);
            Locking(delta);
        }
    }
}