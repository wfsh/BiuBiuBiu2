using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerRayEffectRotateByDegree : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float StartDeg;
            public float AngularSpeed;
            public float Length;
        }
        public float CurDeg;
        public float AngularSpeed;
        public float Length;
        private SAB_PlayRotatingRayEffectSystem abSystem;
        private const float DEAD_CHECK_INTERVAL = 0.1f;
        private List<ParticleSystem> particleList;
        private Transform rootTrans;
        private bool isLengthSet;
        private float deadCheckCountDown = DEAD_CHECK_INTERVAL;
        private IGPO fireGPO;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Ability.GetRayEffectRotateByDegree>(OnGetRayEffectRotateByDegree);
            abSystem = (SAB_PlayRotatingRayEffectSystem)mySystem;
            var initData = (InitData)initDataBase;
            SetData(initData.StartDeg, initData.AngularSpeed, initData.Length);
        }

        protected override void OnStart() {
            base.OnStart();
            particleList = new List<ParticleSystem>();
            fireGPO = abSystem.FireGPO;
            UpdateRot();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            rootTrans = ((EntityBase)iEntity).GetGameObj().transform;
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Ability.GetRayEffectRotateByDegree>(OnGetRayEffectRotateByDegree);
            RemoveUpdate(OnUpdate);
            particleList.Clear();
            particleList = null;
            fireGPO = null;
        }
        
        private void OnGetRayEffectRotateByDegree(ISystemMsg body, SE_Ability.GetRayEffectRotateByDegree ent) {
            ent.CallBack.Invoke(Length, CurDeg);
        }

        private void OnUpdate(float delta) {
            if (isClear) {
                return;
            }
            FireGPODeadCheck(delta);
            CurDeg += AngularSpeed * delta;
            UpdateRot();
        }

        private void FireGPODeadCheck(float dt) {
            if (abSystem.InData.In_EndWhenFireGPODead) {
                deadCheckCountDown -= dt;
                if (deadCheckCountDown <= 0) {
                    if (fireGPO == null) {
                        fireGPO = abSystem.FireGPO;
                    }
                    if (fireGPO != null && fireGPO.IsDead()) {
                        EndAbility();
                    }
                }
            }
        }

        private void UpdateRot() {
            Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * CurDeg), 0, Mathf.Sin(Mathf.Deg2Rad * CurDeg)).normalized;
            var rot = Quaternion.LookRotation(dir, Vector3.up);
            iEntity.SetRota(rot);
        }

        public void SetData(float startDeg, float angularSpeed, float length) {
            this.CurDeg = startDeg;
            this.AngularSpeed = angularSpeed;
            this.Length = length;
        }

        private void EndAbility() {
            MsgRegister.Dispatcher(new SM_Ability.BeforeRemoveAbility() {
                abSystem = abSystem,
            });
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = abSystem.GetAbilityId(),
            });
        }
    }
}