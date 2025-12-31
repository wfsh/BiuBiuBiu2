using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    // 直升机移动
    public class ServerMachineGunSetToGround : ServerNetworkComponentBase {
        private IGPO mMasterGPO;
        private float groundDis = 0f;
        private Vector3 groundPoint = Vector3.zero;
        private Vector3 terrainNormal = Vector3.zero;
        private Transform upperBody; // 上半身的Transform
        private Transform myTransform;
        private Transform footRotaTransform;
        private RaycastHit[] hitBuffer = new RaycastHit[10]; // 预分配 10 个碰撞体存储
        private Vector3 startPoint;

        protected override void OnAwake() {
            base.OnAwake();
            var monsterSystem = (S_AI_Base)mySystem;
            mMasterGPO = monsterSystem.MasterGPO;
            mySystem.Register<SE_AI.Event_SetStartPoint>(OnSetStartPointCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            hitBuffer = null;
            myTransform = null;
            footRotaTransform = null;
            this.upperBody = null;
            mySystem.Unregister<SE_AI.Event_SetStartPoint>(OnSetStartPointCallBack);
        }
        private void OnSetStartPointCallBack(ISystemMsg body, SE_AI.Event_SetStartPoint ent) {
            startPoint = ent.StartPoint;
            iEntity.SetPoint(ent.StartPoint + mMasterGPO.GetForward() * 1.8f);
           
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            var entity = (EntityBase)iEntity;
            myTransform = entity.transform;
            upperBody = entity.GetBodyTran(GPOData.PartEnum.Head);
            footRotaTransform = entity.GetBodyTran(GPOData.PartEnum.FootRotaCheck);
            myTransform.position = startPoint + mMasterGPO.GetForward() * 1.8f;
            CheckGrounded();
            
            RaycastCheckGround( new RaycastHit[10]);
            AdjustRota();
        }

        private void CheckGrounded() {
            var point = this.iEntity.GetPoint() + Vector3.up * 1.6f;
            var count = Physics.RaycastNonAlloc(point, Vector3.down, hitBuffer, 50,LayerData.DefaultLayerMask);
            this.groundDis = -1f;
            GetGoundData(count, point, hitBuffer);
        }

        private void GetGoundData(int count, Vector3 point, RaycastHit[] list) {
            int index = 0;
            for (int i = 0; i < count; i++) {
                var ray = list[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                var dis = Vector3.Distance(point, ray.point);
                if (groundDis < 0f || groundDis > dis) {
                    groundDis = dis;
                    index = i;
                }
            }
            
            iEntity.SetPoint(list[index].point);

            
        }

        private void RaycastCheckGround(RaycastHit[] raycastHit) {
            groundPoint = Vector3.zero;
            var headPoint = footRotaTransform.position;
            var distance = headPoint.y - iEntity.GetPoint().y + Mathf.Abs(Vector3.Dot(upperBody.up, Vector3.up)) * 2f;
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
                }
            }
        }

        private void AdjustRota() {
            if (groundPoint == Vector3.zero) {
                this.terrainNormal = Vector3.up;
            } else {
                AdjustPetOrientation();
            }
            AlignRotation();
        }
        // 根据头部位置调整朝向
        void AdjustPetOrientation() {
            // 计算新的方向向量：由中心点到前方地面点
            var direction = (groundPoint - iEntity.GetPoint()).normalized;
            var side = Vector3.Cross(Vector3.up, direction).normalized;
            var terrainNormal = Vector3.Cross(direction, side).normalized;
            this.terrainNormal = terrainNormal;
        }

        private void AlignRotation() {
            var tranRota = upperBody.rotation;
            var terrainSlope = Vector3.Angle(terrainNormal, Vector3.up);
            if (terrainSlope < 3) {
                return;
            }
            var alignRot = Quaternion.FromToRotation(upperBody.up, this.terrainNormal) * tranRota;
            myTransform.rotation = alignRot;
        }
    }
}