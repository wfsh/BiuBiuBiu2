using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerEffectRotateByDegree : ComponentBase {
        public Vector3 AroundPoint;
        public float CurDeg;
        public float Radius;
        public float MoveAngularSpeed;
        private SAB_PlayRotatingEffectSystem abSystem;
        private const float DEAD_CHECK_INTERVAL = 0.1f;
        private AbilityData.PlayAbility_PlayRotatingEffect inData;
        private float deadCheckCountDown = DEAD_CHECK_INTERVAL;
        private IGPO fireGPO;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Ability.GetServerEffectRotateByDegreeData>(OnGetServerEffectRotateByDegreeDataCallBack);
            abSystem = (SAB_PlayRotatingEffectSystem)mySystem;
            inData = abSystem.InData;
            InitData();
        }

        protected override void OnStart() {
            base.OnStart();

            UpdateRot();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Ability.GetServerEffectRotateByDegreeData>(OnGetServerEffectRotateByDegreeDataCallBack);
            RemoveUpdate(OnUpdate);
        }
        
        private void OnGetServerEffectRotateByDegreeDataCallBack(ISystemMsg body, SE_Ability.GetServerEffectRotateByDegreeData ent) {
            ent.CallBack.Invoke(CurDeg, Radius, AroundPoint, MoveAngularSpeed);
        }

        public void InitData() {
            AroundPoint = inData.In_RotateAroundPoint;
            CurDeg = inData.In_StartDeg;
            Radius = inData.In_StartRadius;
            MoveAngularSpeed = inData.In_MoveAngularSpeed;
        }

        private void OnUpdate(float delta) {
            if (iEntity == null) {
                return;
            }

            CurDeg += MoveAngularSpeed * delta;
            UpdateRot();

            FireGPODeadCheck(delta);
        }

        private void UpdateRot() {
            Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * CurDeg), 0, Mathf.Sin(Mathf.Deg2Rad * CurDeg)).normalized;
            var rot = Quaternion.LookRotation(dir, Vector3.up);
            iEntity.SetRota(rot);

            Vector3 point = inData.In_RotateAroundPoint + dir * Radius;
            iEntity.SetPoint(point);
        }

        private void FireGPODeadCheck(float dt) {
            if (inData.In_EndWhenFireGPODead) {
                deadCheckCountDown -= dt;
                if (deadCheckCountDown <= 0) {
                    if (fireGPO == null) {
                        MsgRegister.Dispatcher(new SM_GPO.GetGPO() {
                            GpoId = abSystem.FireGPO.GetGpoID(),
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
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = abSystem.GetAbilityId()
            });
        }
    }
}