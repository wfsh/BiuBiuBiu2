using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine.AI;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    // 直线匀速移动
    public class ServerJokerUAVMove : ComponentBase {
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private NavMeshAgent navMeshAgent;
        private EntityBase entity;
        private S_AI_Base aiSystem;
        private Vector3 movePoint = Vector3.zero;
        private Vector3 prevPoint = Vector3.zero;
        private bool isHideEntity;
        private bool isStopMove;
        private bool hasBehaviorInit;
        private bool canMoveToTarget;
        private float navMeshSampleTolerance = 2; // NavMesh 采样长度
        private bool isLookTarget;
        private IGPO lookTarget;

        protected override void OnAwake() {
            hasBehaviorInit = false;
            mySystem.Register<SE_Behaviour.Event_MovePoint>(SetMovePointForAction);
            mySystem.Register<SE_Behaviour.Event_StopMove>(BehaviourStopMove);
            mySystem.Register<SE_Entity.Event_IsShowEntity>(SetHideEntity);
            mySystem.Register<SE_Entity.SyncPointAndRota>(SetSyncMovePoint);
            mySystem.Register<SE_Behaviour.Event_AfterBehaviorConfigInit>(AfterBehaviorConfigInitCallBack);
            mySystem.Register<SE_Behaviour.Event_CanMoveToTarget>(CanMoveToTarget);
            mySystem.Register<SE_Behaviour.Event_LookTarget>(OnLookTargetCallBack);
            MsgRegister.Register<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            aiSystem = (S_AI_Base)mySystem;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            entity = (EntityBase)iEntity;
            GetNavMeshAgent();
            AddUpdate(OnUpdate);
            StopMove();
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_Behaviour.Event_MovePoint>(SetMovePointForAction);
            mySystem.Unregister<SE_Behaviour.Event_StopMove>(BehaviourStopMove);
            mySystem.Unregister<SE_Entity.Event_IsShowEntity>(SetHideEntity);
            mySystem.Unregister<SE_Entity.SyncPointAndRota>(SetSyncMovePoint);
            mySystem.Unregister<SE_Behaviour.Event_AfterBehaviorConfigInit>(AfterBehaviorConfigInitCallBack);
            mySystem.Unregister<SE_Behaviour.Event_CanMoveToTarget>(CanMoveToTarget);
            mySystem.Unregister<SE_Behaviour.Event_LookTarget>(OnLookTargetCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
            aiSystem = null;
        }

        private void OnUpdate(float delta) {
            if (!hasBehaviorInit) {
                return;
            }

            if (ModeData.PlayGameState == ModeData.GameStateEnum.ModeOver ||
                iEntity == null || iEntity.IsClear() ||
                iGPO == null || iGPO.IsClear() || iGPO.IsDead()) {
                StopMove();
                return;
            }

            UpdateMove();
            UpdateRotate(delta);
        }

        private void UpdateMove() {
            if (isStopMove) {
                return;
            }

            if (!navMeshAgent.enabled || Vector3.SqrMagnitude(prevPoint - movePoint) > 0.025f) {
                prevPoint = movePoint;
                var filter = new NavMeshQueryFilter() {
                    agentTypeID = navMeshAgent.agentTypeID,
                    areaMask = NavMesh.AllAreas
                };

                if (NavMesh.SamplePosition(movePoint, out NavMeshHit hit, navMeshSampleTolerance, filter)) {
                    movePoint = hit.position;
                }

                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(movePoint);
                Dispatcher(new SE_AI.Event_IsMovePoint {
                    IsTrue = true, MoveType = moveType
                });
            }

            if (navMeshAgent.enabled && !navMeshAgent.pathPending) {
                canMoveToTarget = navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;
            }
        }
        
        private void UpdateRotate(float delta) {
            if (!isLookTarget) {
                return;
            }

            var lookDir = lookTarget.GetPoint() - navMeshAgent.transform.position;
            if (lookDir.sqrMagnitude == 0) {
                return;
            }

            lookDir.y = 0;
            lookDir.Normalize();
            Quaternion rotation = Quaternion.LookRotation(lookDir);
            navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, rotation, delta * 10f);
        }

        private void GetNavMeshAgent() {
            navMeshAgent = entity.GetComponent<NavMeshAgent>();
            if (navMeshAgent != null) {
                navMeshAgent.enabled = false;
                navMeshAgent.stoppingDistance = 0.5f;
            } else {
                Debug.LogError("缺少移动组件 NavMeshAgent");
            }
        }

        private void AfterBehaviorConfigInitCallBack(ISystemMsg body, SE_Behaviour.Event_AfterBehaviorConfigInit ent) {
            hasBehaviorInit = true;
        }
        
        private void CanMoveToTarget(ISystemMsg body, SE_Behaviour.Event_CanMoveToTarget ent) {
            ent.CallBack(canMoveToTarget);
        }

        private void BehaviourStopMove(ISystemMsg body, SE_Behaviour.Event_StopMove ent) {
            if (isHideEntity) {
                return;
            }

            StopMove();
        }

        private void StopMove() {
            if (!isStopMove) {
                Dispatcher(new SE_AI.Event_IsMovePoint {
                    IsTrue = false
                });
                Dispatcher(new SE_GPO.Event_MovePointEnd());
            }

            movePoint = iEntity.GetPoint();
            isStopMove = true;
            navMeshAgent.enabled = false;
        }

        private void SetSyncMovePoint(ISystemMsg body, SE_Entity.SyncPointAndRota ent) {
            iEntity.SetPoint(ent.Point);
        }

        private void SetMovePointForAction(ISystemMsg body, SE_Behaviour.Event_MovePoint entData) {
            SetMovePoint(entData.movePoint, entData.MoveType);
        }

        private void SetHideEntity(ISystemMsg body, SE_Entity.Event_IsShowEntity ent) {
            isHideEntity = !ent.IsShow;
            StopMove();
        }

        private void SetMovePoint(Vector3 movePoint, AIData.MoveType moveType) {
            if (this.movePoint == movePoint && !isStopMove) {
                return;
            }

            isStopMove = false;
            this.movePoint = movePoint;
            this.moveType = moveType;
            canMoveToTarget = true;
            Dispatcher(new SE_AI.Event_IsMovePoint {
                IsTrue = true, MoveType = moveType
            });
            
            Dispatcher(new SE_AI.Event_GetActivateStatus() {
                CallBack = SetActivateStatus
            });
        }

        private void OnSwitchAllBehaviorCallBack(SM_Sausage.SausageSwitchAllBehavior ent) {
            StopMove();
        }

        private void SetActivateStatus(bool isInAlertStatus, bool isInFightStatus) {
            var cfg = MonsterBehaviorSet.GetMonsterBehaviorByMonsterSign(iGPO.GetSign());
            var runSpeed = cfg.PatrolWalkSpeed;
            if (isInFightStatus) {
                runSpeed = cfg.FightRunSpeed;
            } else if (isInAlertStatus) {
                runSpeed = cfg.AlertWalkSpeed;
            }

            navMeshAgent.speed = runSpeed;
        }

        private void OnLookTargetCallBack(ISystemMsg body, SE_Behaviour.Event_LookTarget ent) {
            isLookTarget = ent.isLookTarget;
            lookTarget = ent.lookTarget;
        }
    }
}
