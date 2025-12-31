using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIFootRotation : ComponentBase {
        [Tooltip("四足最大坡度")]
        private float maxAngleSlope = 40f;
        
        [Tooltip("四足最大高低差")]
        private float footMaxStep = 0.8f;

        private Transform myTransform;
        private Transform footRotaTransform;
        private RaycastHit[] raycastHit;
        private Vector3 terrainNormal = Vector3.zero;
        private Vector3 groundPoint = Vector3.zero;
        private Vector3 groundNormal = Vector3.zero;
        private float alignRotLerpDelta;
        private bool isEnabledDriveMoveIng = true;
        private float delayRaycastCheck;

        protected override void OnAwake() {
            mySystem.Register<CE_AI.Event_EnabledDriveMove>(OnEnabledDriveMoveCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            raycastHit = new RaycastHit[10];
        }

        protected override void OnClear() {
            base.OnClear();
            raycastHit = null;
            myTransform = null;
            footRotaTransform = null;
            mySystem.Unregister<CE_AI.Event_EnabledDriveMove>(OnEnabledDriveMoveCallBack);
            RemoveUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var entity = (EntityBase)iEntity;
            myTransform = entity.transform;
            footRotaTransform = entity.GetBodyTran(GPOData.PartEnum.FootRotaCheck);
            AddUpdate(OnUpdate);
        }

        private void OnEnabledDriveMoveCallBack(ISystemMsg body, CE_AI.Event_EnabledDriveMove ent) {
            isEnabledDriveMoveIng = ent.IsTrue;
        }

        private void OnUpdate(float delta) {
            if (isEnabledDriveMoveIng == false) {
                return;
            }

            if (delayRaycastCheck <= 0) {
                RaycastCheckGround();
                delayRaycastCheck = 0.1f;
            } else {
                delayRaycastCheck -= delta;
            }

            if (groundPoint == Vector3.zero) {
                this.terrainNormal = Vector3.up;
            } else {
                AdjustPetOrientation();
            }
            AlignRotation();
        }

        private void RaycastCheckGround() {
            groundPoint = Vector3.zero;
            groundNormal = Vector3.zero;
            var headPoint = footRotaTransform.position;
            var distance = headPoint.y - iEntity.GetPoint().y + Mathf.Abs(Vector3.Dot(myTransform.up, Vector3.up)) * 2f;
            var count = Physics.RaycastNonAlloc(headPoint, Vector3.down, raycastHit, distance,
                (1 << LayerMask.NameToLayer("Default")));
            if (count <= 0) {
                return;
            }
            var checkDistance = -1f;
            for (int i = 0; i < count; i++) {
                var ray = raycastHit[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                var dis = Vector3.Distance(headPoint, ray.point);
                if (checkDistance < 0f || checkDistance > dis) {
                    checkDistance = dis;
                    groundPoint = ray.point;
                    groundNormal = ray.normal;
                }
            }
        }

        // 根据头部位置调整朝向
        void AdjustPetOrientation() {
            // 计算新的方向向量：由中心点到前方地面点
            var direction = (groundPoint - iEntity.GetPoint()).normalized;
            var side = Vector3.Cross(Vector3.up, direction).normalized;
            var terrainNormal = Vector3.Cross(direction, side).normalized;
            var terrainSlope = Vector3.Angle(terrainNormal, Vector3.up);
            var groundSlope = Vector3.Angle(groundNormal, Vector3.up);
            if (terrainSlope > maxAngleSlope || groundSlope > maxAngleSlope || groundNormal.y <= 0) {
                return;
            }
            var exAngle = Vector3.Angle(groundNormal, Vector3.up);
            if (Math.Abs(groundPoint.y - iEntity.GetPoint().y) > 60 + footMaxStep * (1 - Math.Max(0, Math.Min(1, (terrainSlope - exAngle) / 15)))) {
                return;
            }
            this.terrainNormal = terrainNormal;
        }

        private void AlignRotation() {
            var tranRota = myTransform.rotation;
            var headPoint = footRotaTransform.position;
            Debug.DrawLine(headPoint, groundPoint, Color.red);
            Debug.DrawLine(iEntity.GetPoint(), groundPoint, Color.red);
            var alignRot = Quaternion.FromToRotation(myTransform.up, this.terrainNormal) * tranRota;
            myTransform.rotation = Quaternion.Slerp(tranRota, alignRot, Mathf.Clamp(6 * Time.deltaTime, 0.1f, 1f));
        }
    }
}