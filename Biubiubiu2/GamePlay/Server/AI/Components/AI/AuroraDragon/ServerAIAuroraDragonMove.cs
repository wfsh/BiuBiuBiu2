using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine.AI;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIAuroraDragonMove : ServerNetworkComponentBase {
        private Vector3 moveDir = Vector3.zero;
        private Vector3 movePoint = Vector3.zero;
        private bool isMovePoint = false;
        private Vector3 fightCenter;
        private float sqrFightRadius;
        private float sqrMinFollowDis;
        private AIDragonConfig config;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Behaviour.Event_StopMove>(SetStopMoveForAction);
            mySystem.Register<SE_Behaviour.Event_MovePoint>(SetMovePointForAction);
            mySystem.Register<SE_Entity.Event_IsShowEntity>(SetHideEntity);

            var monsterBase = (S_AI_Base)mySystem;
            AssetManager.LoadAISO(monsterBase.AttributeData.Sign+"Config", LoadConfigComplete);
        }

        private void LoadConfigComplete(ScriptableObject so) {
            config = (AIDragonConfig)so;
            sqrMinFollowDis = config.MinFollowDistance * config.MinFollowDistance;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_Behaviour.Event_StopMove>(SetStopMoveForAction);
            mySystem.Unregister<SE_Behaviour.Event_MovePoint>(SetMovePointForAction);
            mySystem.Unregister<SE_Entity.Event_IsShowEntity>(SetHideEntity);
        }

        private void SetFightRangeDataCallBack(Vector3 center, float radius, float endTime, bool isInFight) {
            fightCenter = center;
            var fightRadius = radius;
            sqrFightRadius = fightRadius * fightRadius;
        }

        private void OnUpdate(float delta) {
            if (ModeData.PlayGameState == ModeData.GameStateEnum.ModeOver) {
                StopMove();
                RemoveUpdate(OnUpdate);
                return;
            }

            if (iGPO.IsDead()) {
                return;
            }

            Move(delta);
        }

        private void StopMove() {
            if (isMovePoint == true) {
                Dispatcher(new SE_AI.Event_IsMovePoint {
                    IsTrue = false
                });
                Dispatcher(new SE_GPO.Event_MovePointEnd());
            }

            movePoint = iEntity.GetPoint();
            isMovePoint = false;
        }

        private void SetStopMoveForAction(ISystemMsg body, SE_Behaviour.Event_StopMove ent) {
            StopMove();
        }

        private void SetMovePointForAction(ISystemMsg body, SE_Behaviour.Event_MovePoint entData) {
            SetMovePoint(entData.movePoint, entData.MoveType);
        }

        private void SetHideEntity(ISystemMsg body, SE_Entity.Event_IsShowEntity ent) {
            StopMove();
        }

        private void SetMovePoint(Vector3 movePoint, AIData.MoveType moveType) {
            if (this.movePoint == movePoint) {
                return;
            }

            if (sqrFightRadius <= 0) {
                Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData() {
                    CallBack = SetFightRangeDataCallBack
                });
            }
            
            if (Vector3.SqrMagnitude(movePoint - fightCenter) >= sqrFightRadius) {
                return;
            }

            this.movePoint = movePoint;
            isMovePoint = true;
            moveDir = movePoint - iEntity.GetPoint();
            moveDir.y = 0;
            moveDir.Normalize();
            Dispatcher(new SE_AI.Event_IsMovePoint {
                IsTrue = true, MoveType = moveType
            });
            Dispatcher(new SE_GPO.Event_MoveDir {
                MoveDir = moveDir
            });
        }

        private void Move(float deltaTime) {
            if (!isMovePoint) {
                return;
            }

            if (Vector3.SqrMagnitude(iEntity.GetPoint() - movePoint) < sqrMinFollowDis) {
                StopMove();
                return;
            }

            iEntity.SetPoint(iEntity.GetPoint() + moveDir * deltaTime * config.MoveSpeed);
            var targetForward = Vector3.RotateTowards(iEntity.GetForward(), moveDir, deltaTime * config.AngleSpeed, 0.1f);
            iEntity.SetRota(Quaternion.LookRotation(targetForward));
        }
    }
}
