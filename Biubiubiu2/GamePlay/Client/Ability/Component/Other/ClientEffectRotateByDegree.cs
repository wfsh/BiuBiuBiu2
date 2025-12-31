using System;
using Sofunny.BiuBiuBiu2.ClientGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientEffectRotateByDegree : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float StartDeg;
            public float StartRadius;
            public float MoveAngularSpeed;
            public long PlayTimestamp;
        }
        private CAB_PlayRotatingEffectSystem abSystem;
        private const float DEAD_CHECK_INTERVAL = 0.1f;
        private Proto_Ability.Rpc_PlayRotatingEffect abilityData;
        private Vector3 aroundPoint;
        private float curDeg;
        private float radius;
        private float moveAngularSpeed;
        private float deadCheckCountDown = DEAD_CHECK_INTERVAL;
        private IGPO fireGPO;

        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (CAB_PlayRotatingEffectSystem)mySystem;
            abilityData = abSystem.AbilityData;
            var initData = (InitData)initDataBase;
            SetData(initData.StartDeg, initData.StartRadius, initData.MoveAngularSpeed, initData.PlayTimestamp);
        }

        protected override void OnStart() {
            base.OnStart();

            UpdateRot();
            AddUpdate(OnUpdate);
        }

        public void SetData(float startDeg, float startRadius, float moveAngularSpeed, long playTimestamp) {
            radius = startRadius;
            this.moveAngularSpeed = moveAngularSpeed;
            curDeg = startDeg + moveAngularSpeed * 0.001f * (TimeUtil.GetCurUTCTimestamp() - playTimestamp);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float delta) {
            if (iEntity == null) {
                return;
            }

            curDeg += moveAngularSpeed * delta;
            UpdateRot();

            FireGPODeadCheck(delta);
        }

        private void UpdateRot() {
            Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * curDeg), 0, Mathf.Sin(Mathf.Deg2Rad * curDeg)).normalized;
            var rot = Quaternion.LookRotation(dir, Vector3.up);
            iEntity.SetRota(rot);

            Vector3 point = abilityData.rotateAroundPoint + dir * radius;
            iEntity.SetPoint(point);
        }

        private void FireGPODeadCheck(float dt) {
            if (abSystem.AbilityData.endWhenFireGPODead) {
                deadCheckCountDown -= dt;
                if (deadCheckCountDown <= 0) {
                    if (fireGPO == null) {
                        MsgRegister.Dispatcher(new CM_GPO.GetGPO() {
                            GpoId = abSystem.FireGpoId,
                            CallBack = SetFireGPO,
                        });
                    }

                    if (fireGPO != null && fireGPO.IsDead()) {
                        EndAbility();
                    }
                }
            }
        }

        private void SetFireGPO(IGPO gpo) {
            fireGPO = gpo;
        }

        private void EndAbility() {
            abSystem.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = abSystem.GetAbilityId()
            });
        }
    }
}