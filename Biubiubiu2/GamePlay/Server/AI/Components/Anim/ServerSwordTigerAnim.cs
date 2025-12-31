using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Playable.Config;
using UnityEngine;
using UnityEngine.AI;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSwordTigerAnim : ServerNetworkComponentBase {
        private const float runThresholdAngle = 20f; // 转向多少角度内开始跑步
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private S_AI_Base aiBase;
        private Vector3 moveDir = Vector3.zero;
        private GPOData.GPOType driveGpoType = GPOData.GPOType.NULL;
        private string monsterSign = "";
        private bool isMovePoint = false;
        private int playAnim = 0;
        private int attackAnim = 0;
        private SausagePlayable playable;

        protected override void OnAwake() {
            mySystem.Register<SE_AI.Event_IsMovePoint>(OnIsMovePointCallBack);
            mySystem.Register<SE_GPO.Event_MovePointEnd>(OnMovePoineEndCallBack);
            mySystem.Register<SE_GPO.Event_MoveDir>(SetMoveDirCallBack);
            mySystem.Register<SE_AI.Event_DriveState>(OnDriveStateCallBack);
            mySystem.Register<SE_AI.Event_PlayAttackAnim>(OnAttackCallBack);
            mySystem.Register<SE_AI.Event_PlayBellowAnim>(OnBellowAttactCallBack);
            mySystem.Register<SE_AI.Event_DragonCarAnim_Run>(OnDragonCarRunCallBack);
            mySystem.Register<SE_AI.Event_DragonCarAnim>(OnDragonCarCallBack);
            aiBase = (S_AI_Base)mySystem;
            monsterSign = aiBase.AttributeData.Sign;
        }
        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            monsterSign = "";
            mySystem.Unregister<SE_AI.Event_IsMovePoint>(OnIsMovePointCallBack);
            mySystem.Unregister<SE_GPO.Event_MovePointEnd>(OnMovePoineEndCallBack);
            mySystem.Unregister<SE_GPO.Event_MoveDir>(SetMoveDirCallBack);
            mySystem.Unregister<SE_AI.Event_DriveState>(OnDriveStateCallBack);
            mySystem.Unregister<SE_AI.Event_PlayAttackAnim>(OnAttackCallBack);
            mySystem.Unregister<SE_AI.Event_PlayBellowAnim>(OnBellowAttactCallBack);
            mySystem.Unregister<SE_AI.Event_DragonCarAnim_Run>(OnDragonCarRunCallBack);
            mySystem.Unregister<SE_AI.Event_DragonCarAnim>(OnDragonCarCallBack);
            playable.Dispose();
            playable = null;
        }
        
        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            InitPlayableGraph();
            AddUpdate(OnUpdate);
        }


        private void OnUpdate(float deltaTime) {
            playable?.OnUpdate(deltaTime);
        }
        
        protected override ITargetRpc SyncData() {
            return new Proto_AI.TargetRpc_Anim {
                animId = (ushort)playAnim
            };
        }
        
        private void OnAttackCallBack(ISystemMsg body, SE_AI.Event_PlayAttackAnim ent) {
            if (ent.IsTrue == false) {
                PlayAttackAnim(0);
                return;
            }
            PlayAnim(AnimConfig_SwordTiger.Anim_fall);
        }
        
        private void OnBellowAttactCallBack(ISystemMsg body, SE_AI.Event_PlayBellowAnim ent) {
            if (ent.IsTrue == false) {
                PlayAttackAnim(0);
                return;
            }
            PlayAnim(AnimConfig_SwordTiger.Anim_death);
        }
        
        private void OnDragonCarCallBack(ISystemMsg body, SE_AI.Event_DragonCarAnim ent) {
            PlayAttackAnim(AnimConfig_SwordTiger.Anim_death);
        }
        
        private void OnDragonCarRunCallBack(ISystemMsg body, SE_AI.Event_DragonCarAnim_Run ent) {
            if (ent.IsTrue == false) {
                PlayAttackAnim(0);
                return;
            }
            PlayAttackAnim(AnimConfig_SwordTiger.Anim_run_forward);
        }

        private void OnDriveStateCallBack(ISystemMsg body, SE_AI.Event_DriveState ent) {
            driveGpoType = ent.DriveGpoType;
        }


        private void InitPlayableGraph() {
            var entity = (EntityBase)iEntity;
            var config = AIAnimConfig.Get(AIAnimConfig.SwordTiger);
            var animator = entity.GetComponentInChildren<Animator>(true);
            playable = new SausagePlayable();
            playable.Init(entity.transform, animator, config, $"Server_{monsterSign}");
            playable.CloseLoadEffect();
            PlayAnim(AnimConfig_SwordTiger.Anim_idle);
        }

        private void OnMovePoineEndCallBack(ISystemMsg body, SE_GPO.Event_MovePointEnd ent) {
            PlayAnim(AnimConfig_SwordTiger.Anim_idle);
        }

        private void SetMoveDirCallBack(ISystemMsg body, SE_GPO.Event_MoveDir ent) {
            if (attackAnim != 0 || isMovePoint == false) {
                return;
            }
            moveDir = ent.MoveDir;
            var angle = Vector3.Angle(iEntity.GetForward(), moveDir);
            if (angle > runThresholdAngle) {
                var crossProduct = Vector3.Cross(iEntity.GetForward(), moveDir);
                if (crossProduct.y > 0) {
                    if (this.moveType == AIData.MoveType.Run) {
                        PlayAnim(AnimConfig_SwordTiger.Anim_run_right);
                    } else if (this.moveType == AIData.MoveType.BackMove) {
                        PlayAnim(AnimConfig_SwordTiger.Anim_back_right);
                    } else {
                        PlayAnim(AnimConfig_SwordTiger.Anim_walk_right);
                    }
                } else {
                    if (this.moveType == AIData.MoveType.Run) {
                        PlayAnim(AnimConfig_SwordTiger.Anim_run_left);
                    } else if (this.moveType == AIData.MoveType.BackMove) {
                        PlayAnim(AnimConfig_SwordTiger.Anim_back_left);
                    } else {
                        PlayAnim(AnimConfig_SwordTiger.Anim_walk_left);
                    }
                }
            } else {
                if (this.moveType == AIData.MoveType.Run) {
                    PlayAnim(AnimConfig_SwordTiger.Anim_run_forward);
                } else if (this.moveType == AIData.MoveType.BackMove) {
                    PlayAnim(AnimConfig_SwordTiger.Anim_back_forward);
                } else {
                    PlayAnim(AnimConfig_SwordTiger.Anim_walk_forward);
                }
            }
        }
        
        public void OnIsMovePointCallBack(ISystemMsg body, SE_AI.Event_IsMovePoint ent) {
            this.isMovePoint = ent.IsTrue;
            if (this.isMovePoint == false) {
                PlayAnim(AnimConfig_SwordTiger.Anim_idle);
                return;
            }
            this.moveType = ent.MoveType;
        }
        
        private void PlayAttackAnim(int anim) {
            attackAnim = anim;
            if (attackAnim == 0) {
                PlayAnim(AnimConfig_SwordTiger.Anim_idle);
            } else {
                PlayAnim(anim);
            }
        }
        
        private void PlayAnim(int anim) {
            if (driveGpoType == GPOData.GPOType.Role) {
                return;
            }
            if (playAnim == anim) {
                return;
            }
            playAnim = anim;
            playable?.PlayAnimId(anim);
            Rpc(new Proto_AI.Rpc_Anim {
                animId = (ushort)anim,
            });
        }
    }
}