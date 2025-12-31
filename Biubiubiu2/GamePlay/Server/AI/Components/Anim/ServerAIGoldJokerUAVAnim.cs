using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Playable.Config;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIGoldJokerUAVAnim : ServerNetworkComponentBase {
        private S_AI_Base aiBase;
        private EntityAnimConfig config;
        private SausagePlayable playable;
        private EntityBase entity = null;
        private string monsterSign = "";
        private int playAnim = 0;
        private string groupSign = "";
        private bool isPlayWarReportAnim = false;
        private bool isStartAutoFire;
        private float waitCloseFireTiem = 1f;

        protected override void OnAwake() {
            Register<SE_GPO.Event_PlayWarReportAnimIdStart>(OnPlayWarReportAnimIdStartCallBack);
            Register<SE_AI.Event_PlayJokerDroneAttackAnim>(OnPlayJokerDroneAttackAnim);
            Register<SE_AI.Event_SetJokerDroneAttackState>(OnEnabledAutoFireCallBack);
            Register<SE_AI.Event_PlayBossAnim>(OnPlayBossAnim);
            Register<SE_AI.Event_IsMovePoint>(OnPlayMoveAnim);
            Register<SE_GPO.Event_SetIsDead>(OnSetIsDead);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            aiBase = (S_AI_Base)mySystem;
            monsterSign = aiBase.AttributeData.Sign;
            config = AIAnimConfig.Get(AIAnimConfig.GoldDash_JokerDrone);
            InitPlayableGraph();
            PlayAnim(AnimConfig_GoldDash_JokerDrone.Anim_GoldDash_JokerDrone_Idle);
            AddUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            waitCloseFireTiem-= deltaTime;
            if (waitCloseFireTiem <= 0) {
                isStartAutoFire = false;
            }
            playable?.OnUpdate(deltaTime);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            Unregister<SE_GPO.Event_PlayWarReportAnimIdStart>(OnPlayWarReportAnimIdStartCallBack);
            Unregister<SE_AI.Event_PlayJokerDroneAttackAnim>(OnPlayJokerDroneAttackAnim);
            Unregister<SE_AI.Event_SetJokerDroneAttackState>(OnEnabledAutoFireCallBack);
            Unregister<SE_AI.Event_PlayBossAnim>(OnPlayBossAnim);
            Unregister<SE_AI.Event_IsMovePoint>(OnPlayMoveAnim);
            Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDead);
            playable?.Dispose();
            playable = null;
        }

        protected override ITargetRpc SyncData() {
            return new Proto_AI.TargetRpc_Anim {
                animId = (ushort)playAnim
            };
        }

        private void SetGroupSign(string sign) {
            if (sign == "") {
                return;
            }
            groupSign = sign;
            playable?.SetGroupSign(groupSign);
        }

        private void InitPlayableGraph() {
            entity = (EntityBase)iEntity;
            var animator = entity.GetComponentInChildren<Animator>(true);
            playable = new SausagePlayable();
            playable.Init(entity.transform, animator, config, $"S_GPOID:{iGPO.GetGpoID()}_{monsterSign}");
            SetGroupSign(groupSign);
            PlayAnim(playAnim);
        }
        
        private void OnPlayBossAnim(ISystemMsg body, SE_AI.Event_PlayBossAnim ent) {
            PlayAnim(ent.Id);
        }
        
        private void OnPlayMoveAnim(ISystemMsg body, SE_AI.Event_IsMovePoint ent) {
            if (isStartAutoFire) {
                return;
            }
            if (ent.IsTrue) {
                isStartAutoFire = false;
                if (ent.MoveType == AIData.MoveType.Walk) {
                    PlayAnim(AnimConfig_GoldDash_JokerDrone.Anim_JokerDrone_Walk);
                } else {
                    PlayAnim(AnimConfig_GoldDash_JokerDrone.Anim_GoldDash_JokerDrone_Run);
                }
            }
        }
        
        private void OnEnabledAutoFireCallBack(ISystemMsg body, SE_AI.Event_SetJokerDroneAttackState ent) {
            waitCloseFireTiem = 1f;
            isStartAutoFire = ent.isAttack;
        }
        
        private void OnSetIsDead(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead) {
                PlayAnim(AnimConfig_GoldDash_JokerDrone.Anim_JokerDrone_Dead);
            }
        }

        
        public void OnPlayWarReportAnimIdStartCallBack(ISystemMsg body, SE_GPO.Event_PlayWarReportAnimIdStart ent) {
            isPlayWarReportAnim = true;
            var animId = ent.AnimId;
            playAnim = animId;
            playable?.PlayAnimId(animId);
            PlayAnimEnd();
        }
        
        public void OnPlayJokerDroneAttackAnim(ISystemMsg body, SE_AI.Event_PlayJokerDroneAttackAnim ent) {
            if (ent.isRayAttack) {
                playable?.PlayAnimId(AnimConfig_GoldDash_JokerDrone.Anim_JokerDrone_Shoot02);
            } else {
                playable?.PlayAnimId(AnimConfig_GoldDash_JokerDrone.Anim_JokerDrone_Shoot01);
            }
        }

        private void PlayAnim(int animId) {
            if (playAnim == animId || isPlayWarReportAnim) {
                return;
            }
            playAnim = animId;
            playable?.PlayAnimId(animId);
            PlayAnimEnd();
        }

        private void PlayAnimEnd() {
            mySystem.Dispatcher(new SE_GPO.Event_PlayAnimIdEnd {
                AnimId = playAnim,
            });
            Rpc(new Proto_AI.Rpc_Anim {
                animId = (ushort)playAnim,
            });
        }
    }
}