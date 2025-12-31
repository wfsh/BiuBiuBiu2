using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIMachineGunHeadRota : ServerNetworkComponentBase {
        private Transform upperBody; // 上半身的Transform
        private Transform gunBody;
        private float rotationSpeed = 5f; // 上半身旋转速度
        private Vector3 playerUpperBodyRota; // 玩家控制下的上半身旋转
        private IGPO target; // AI模式下的目标
        private float syncTime = 0.1f;
        private float barrelElevationLimit = 45f; // 炮口的最大上抬角度
        private Vector3 lookDirection;
        private S_AI_Base mAI;

        protected override void OnAwake() {
            mySystem.Register<SE_AI.Event_SetInsightTarget>(OnSetInsightTarget);
        }

        protected override void OnStart() {
            base.OnStart();
            mAI = (S_AI_Base)mySystem;
            IGPO masterGPO = mAI.MasterGPO;
            lookDirection = masterGPO.GetForward();
            AddUpdate(OnUpdate);
        }
        
        private void OnUpdate(float delta) {
            if (isSetEntityObj == false) {
                return;
            }
            if (target != null) {
                // AI模式下有目标，朝向目标方向
                lookDirection = (target.GetPoint() - upperBody.position).normalized;
            }

            RotateUpperBody(lookDirection);
            AdjustUpperBodyElevation(target);
            playerUpperBodyRota = new Vector3(270+gunBody.localEulerAngles.x, upperBody.eulerAngles.y, 0);// upperBody.eulerAngles;
            SyncUpperBodyRota();
        }


        protected override void OnClear() {
            this.upperBody = null;
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_AI.Event_SetInsightTarget>(OnSetInsightTarget);
            base.OnClear();
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var entity = (AIEntity)iEntity;
            upperBody = entity.GetBodyTran(GPOData.PartEnum.Head);
            gunBody = entity.GetBodyTran(GPOData.PartEnum.RightHand);
            InitRotation(lookDirection);
        }

        private void OnSetInsightTarget(ISystemMsg body, SE_AI.Event_SetInsightTarget ent) {
            target = ent.TargetGPO;
        }
        private void SyncUpperBodyRota() {
            if (syncTime > 0) {
                syncTime -= Time.deltaTime;
                return;
            }
            syncTime = 0.1f;
            Rpc(new Proto_AI.Rpc_SyncMachineGunUpperBodyRota() {
                eulerAngles = playerUpperBodyRota
            });
        }

        /// 水平旋转
        private void RotateUpperBody(Vector3 direction) {
            if (direction != Vector3.zero) {
                var targetRotation = Quaternion.LookRotation(direction);
                var currentRotation = upperBody.rotation;
                var newRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);
                upperBody.rotation = Quaternion.Euler(currentRotation.eulerAngles.x, newRotation.eulerAngles.y, 0f);
            }
        }

        private void InitRotation(Vector3 direction) {
            var targetRotation = Quaternion.LookRotation(direction);
            var currentRotation = upperBody.rotation;
            upperBody.rotation = Quaternion.Euler(currentRotation.eulerAngles.x, targetRotation.eulerAngles.y, 0f);
        }

        /// 仰角调整
        private void AdjustUpperBodyElevation(IGPO target) {
            if (target == null || target.IsClear()) {
                return;
            }
            // 计算到目标的方向和仰角
            Transform targetBody = target.GetBodyTran(GPOData.PartEnum.Body);
            var targetPoint = target.GetPoint();
            if (targetBody != null) {
                targetPoint = targetBody.position;
            }
            var directionToTarget = targetPoint - gunBody.position ;
            var targetDistance = new Vector3(directionToTarget.x, 0, directionToTarget.z).magnitude;
            var elevationAngle = Mathf.Atan2(directionToTarget.y, targetDistance) * Mathf.Rad2Deg;
            // 限制仰角范围
            elevationAngle = Mathf.Clamp(elevationAngle, 0, barrelElevationLimit);
            // 设置局部 X 轴角度
            gunBody.localEulerAngles = new Vector3(-elevationAngle,0, 0);
        }
    }
}