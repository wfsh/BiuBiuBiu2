using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine.AI;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    // 直线匀速移动
    public class ServerAIGoldJokerMove : ComponentBase {
        private S_AI_Base aiBase;
        private AIGoldJokerConfig config;
        private Vector3 movePoint = Vector3.zero;
        private bool isHideEntity;
        private bool isStopMove;
        private float moveSpeed;
        private float angleSpeed;

        protected override void OnAwake() {
            mySystem.Register<SE_Behaviour.Event_MovePoint>(SetMovePointForAction);
            mySystem.Register<SE_Behaviour.Event_StopMove>(BehaviourStopMove);
            mySystem.Register<SE_Entity.Event_IsShowEntity>(SetHideEntity);
            mySystem.Register<SE_Entity.SyncPointAndRota>(SetSyncMovePoint);
            mySystem.Register<SE_AI_Joker.Event_SetGoldJokerFlashTeleport>(OnSetGoldJokerFlashTeleport);
            aiBase = (S_AI_Base)mySystem;
            AssetManager.LoadAISO(aiBase.AttributeData.Sign+"Config", LoadConfigComplete);
        }
        
        private void LoadConfigComplete(ScriptableObject so) {
            config = (AIGoldJokerConfig)so;
            moveSpeed = config.moveSpeed;
            angleSpeed = config.angleSpeed;
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
            StopMove();
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_AI_Joker.Event_SetGoldJokerFlashTeleport>(OnSetGoldJokerFlashTeleport);
            mySystem.Unregister<SE_Behaviour.Event_MovePoint>(SetMovePointForAction);
            mySystem.Unregister<SE_Behaviour.Event_StopMove>(BehaviourStopMove);
            mySystem.Unregister<SE_Entity.Event_IsShowEntity>(SetHideEntity);
            mySystem.Unregister<SE_Entity.SyncPointAndRota>(SetSyncMovePoint);
        }

        private void OnUpdate(float delta) {
            if (ModeData.PlayGameState == ModeData.GameStateEnum.ModeOver ||
                iEntity == null || iEntity.IsClear() ||
                iGPO == null || iGPO.IsClear() || iGPO.IsDead()) {
                StopMove();
                return;
            }

            UpdateMove(delta);
        }

        private void UpdateMove(float delta) {
            if (isStopMove) {
                return;
            }

            var deltaDis = moveSpeed * delta;
            var vector = movePoint - iEntity.GetPoint();
            if (Vector3.SqrMagnitude(vector) > deltaDis * deltaDis) {
                iEntity.SetPoint(iEntity.GetPoint() + vector.normalized * deltaDis);
                iEntity.SetRota(Quaternion.RotateTowards(iEntity.GetRota(), Quaternion.LookRotation(vector), delta * angleSpeed));
            } else {
                iEntity.SetPoint(movePoint);
                StopMove();
            }
        }
        
        private void OnSetGoldJokerFlashTeleport(ISystemMsg body, SE_AI_Joker.Event_SetGoldJokerFlashTeleport ent) {
            var vec = ent.Point - iEntity.GetPoint();
            iEntity.SetPoint(ent.Point);
            iEntity.SetRota(Quaternion.LookRotation(vec));
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
            Dispatcher(new SE_AI.Event_IsMovePoint {
                IsTrue = true, MoveType = moveType
            });
        }
    }
}
