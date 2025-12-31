using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientRayEffectRotateByDegree : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float StartDeg;
            public float AngularSpeed;
            public float Length;
            public long PlayTimestamp;
        }
        private CAB_PlayRotatingRayEffectSystem abSystem;
        private const float DEAD_CHECK_INTERVAL = 0.1f;
        private float curDeg;
        private float angularSpeed;
        private float length;
        private List<ParticleSystem> particleList;
        private Transform rootTrans;
        private bool isLengthSet;
        private float deadCheckCountDown = DEAD_CHECK_INTERVAL;
        private IGPO fireGPO;

        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (CAB_PlayRotatingRayEffectSystem)mySystem;
            particleList = new List<ParticleSystem>();
            var initData = (InitData)initDataBase;
            SetData(initData.StartDeg, initData.AngularSpeed, initData.Length, initData.PlayTimestamp);
        }

        protected override void OnStart() {
            base.OnStart();

            UpdateRot();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            rootTrans = ((EntityBase)iEntity).GetGameObj().transform;
            TrySetParticleLength(length);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            particleList.Clear();
            particleList = null;
        }

        private void OnUpdate(float delta) {
            if (iEntity == null) {
                return;
            }

            FireGPODeadCheck(delta);
            TrySetParticleLength(length);

            curDeg += angularSpeed * delta;
            UpdateRot();
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

        private void UpdateRot() {
            Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * curDeg), 0, Mathf.Sin(Mathf.Deg2Rad * curDeg)).normalized;
            var rot = Quaternion.LookRotation(dir, Vector3.up);
            iEntity.SetRota(rot);
        }

        public void SetData(float startDeg, float angularSpeed, float length, long playTimestamp) {
            this.angularSpeed = angularSpeed;
            this.length = length;
            this.curDeg = startDeg;
            curDeg = startDeg + this.angularSpeed * 0.001f * (TimeUtil.GetCurUTCTimestamp() - playTimestamp);
        }

        private void EndAbility() {
            abSystem.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = abSystem.GetAbilityId()
            });
        }

        private void TrySetParticleLength(float scale) {
            if (rootTrans == null) {
                return;
            }

            if (rootTrans.childCount <= 0) {
                return;
            }

            if (isLengthSet) {
                return;
            }

            // 获取所有 particle
            rootTrans.GetComponentsInChildren<ParticleSystem>(true, particleList);
            for (int i = 0, count = particleList.Count; i < count; i++) {
                ParticleSystem particle = particleList[i];
                var main = particle.main;
                // 将其长度缩放后重新播放
                main.startSizeY = new ParticleSystem.MinMaxCurve(scale);
                particle.Clear();
                particle.Play();
            }

            // 标记长度设置完毕
            isLengthSet = true;
        }
    }
}