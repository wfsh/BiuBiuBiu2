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
    public class ServerAIGoldJokerAnim : ServerNetworkComponentBase {
        private S_AI_Base aiBase;
        private EntityAnimConfig config;
        private SausagePlayable playable;
        private EntityBase entity = null;
        private int playAnim = 0;
        private string groupSign = "";
        private bool isPlayWarReportAnim = false;
        private bool isPlayDead = false;

        protected override void OnAwake() {
            Register<SE_GPO.Event_PlayWarReportAnimIdStart>(OnPlayWarReportAnimIdStartCallBack);
            Register<SE_AI.Event_PlayBossAnim>(OnPlayBossAnim);
            Register<SE_AI.Event_IsMovePoint>(OnPlayMoveAnim);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            aiBase = (S_AI_Base)mySystem;
            config = AIAnimConfig.Get(AIAnimConfig.GoldDash_BOSSAceJoker);
            InitPlayableGraph();
            PlayAnim(AnimConfig_GoldDash_BOSSAceJoker.Anim_BOSSAceJoker_Idle);
            AddUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            playable?.OnUpdate(deltaTime);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            Unregister<SE_GPO.Event_PlayWarReportAnimIdStart>(OnPlayWarReportAnimIdStartCallBack);
            Unregister<SE_AI.Event_PlayBossAnim>(OnPlayBossAnim);
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
            playable.Init(entity.transform, animator, config, $"S_GPOID:{iGPO.GetGpoID()}_{iGPO.GetSign()}");
            playable.CloseLoadEffect();
            SetGroupSign(groupSign);
            PlayAnim(playAnim);
        }
        
        private void OnPlayBossAnim(ISystemMsg body, SE_AI.Event_PlayBossAnim ent) {
            PlayAnim(ent.Id, true);
        }
        
        private void OnPlayMoveAnim(ISystemMsg body, SE_AI.Event_IsMovePoint ent) {
            if (ent.IsTrue) {
                PlayAnim(AnimConfig_GoldDash_BOSSAceJoker.Anim_BOSSAceJoker_Walk);
            } else {
                PlayAnim(AnimConfig_GoldDash_BOSSAceJoker.Anim_BOSSAceJoker_Idle);
            }
        }

        public void OnPlayWarReportAnimIdStartCallBack(ISystemMsg body, SE_GPO.Event_PlayWarReportAnimIdStart ent) {
            isPlayWarReportAnim = true;
            var animId = ent.AnimId;
            playAnim = animId;
            playable?.PlayAnimId(animId);
            PlayAnimEnd();
        }

        private void PlayAnim(int animId, bool isForce = false) {
            if (playAnim == animId && !isForce || isPlayWarReportAnim || isPlayDead) {
                return;
            }
            //boss 播放死亡动画后，其他动画都不能进行播放
            if (animId == AnimConfig_GoldDash_BOSSAceJoker.Anim_BOSSAceJoker_Leave) {
                isPlayDead = true;
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