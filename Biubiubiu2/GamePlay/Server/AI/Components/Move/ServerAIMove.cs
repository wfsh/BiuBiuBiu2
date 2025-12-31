using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine.AI;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIMove : ServerNetworkComponentBase {
        protected const float gravitey = 1f; // 影响下落速度和跳跃高度 默认 1 倍重力
        protected RaycastHit[] raycastHit;
        protected CharacterController controller;
        protected NavMeshAgent navMeshAgent;
        protected EntityBase entity;
        private float delayGetNavMeshPointTime = 0.0f;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Entity.SyncPointAndRota>(SetSyncMovePoint, 1);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Entity.SyncPointAndRota>(SetSyncMovePoint);
            controller = null;
            navMeshAgent = null;
            raycastHit = null;
            entity = null;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            entity = (EntityBase)iEntity;
            raycastHit = new RaycastHit[10];
            GetCharacterController();
            GetNavMeshAgent();
        }

        private void GetCharacterController() {
            controller = entity.GetComponent<CharacterController>();
            if (controller == null) {
                Debug.LogError("缺少移动组件 CharacterController");
            }
        }

        private void GetNavMeshAgent() {
            navMeshAgent = entity.GetComponent<NavMeshAgent>();
            if (navMeshAgent != null) {
                navMeshAgent.enabled = false;
                navMeshAgent.updatePosition = false;
                navMeshAgent.updateRotation = false;
            } else {
                Debug.LogError("缺少移动组件 NavMeshAgent");
            }
        }

        private void SetSyncMovePoint(ISystemMsg body, SE_Entity.SyncPointAndRota ent) {
            if ( IsTestCanMove() == false) {
                return;
            }
            iEntity.SetPoint(ent.Point);
        }

        protected void ControllerMove(Vector3 moveVector) {
            if (controller == null || controller.enabled == false || IsTestCanMove() == false) {
                return;
            }
            controller.Move(moveVector);
        }

        protected void DelayGetNavMeshPoint() {
            if (IsTestCanMove() == false || navMeshAgent.isOnNavMesh) {
                return;
            }
            if (Time.realtimeSinceStartup - delayGetNavMeshPointTime < 3f) {
                return;
            }
            delayGetNavMeshPointTime = Time.realtimeSinceStartup;
            if (navMeshAgent.isOnNavMesh) {
                return;
            }
            var navMeshPoint = GetNavMeshPoint(iEntity.GetPoint());
            iEntity.SetPoint(navMeshPoint);
            navMeshAgent.nextPosition = iEntity.GetPoint();
        }

        private bool IsTestCanMove() {
            return WarData.TestIsAIMove;
        }

        private Vector3 GetNavMeshPoint(Vector3 point) {
            var filter = new NavMeshQueryFilter() {
                agentTypeID = navMeshAgent.agentTypeID,
                areaMask = NavMesh.AllAreas
            };
            if (NavMesh.SamplePosition(point, out NavMeshHit hit, 1, filter)) {
                point = hit.position;
            } else {
                Debug.Log($"{iGPO.GetName()} - {point} 1m 范围内找不到可寻路点");
            }
            return point;
        }
    }
}