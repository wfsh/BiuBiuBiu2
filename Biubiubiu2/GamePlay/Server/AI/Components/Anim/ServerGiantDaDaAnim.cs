using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Playable.Config;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGiantDaDaAnim : ServerNetworkComponentBase {
        private S_AI_Base aiBase;
        private EntityBase entity = null;
        private int base_layer = -1;
        private string monsterSign = "";
        private int playAnim = 0;
        private EntityAnimConfig config;
        private SausagePlayable playable;
        private int curStage = -1;
        private float bornAnimPlayDelayCountDown;
        private bool hasPlayedBornAnim;
        private bool hasShowEnt;

        protected override void OnAwake() {
            mySystem.Register<SE_AI_GiantDaDa.Event_GiantDaDaPlayIdleAnim>(OnPlayIdleAnimCallBack);
            mySystem.Register<SE_AI_GiantDaDa.Event_GiantDaDaPlayStageAnim>(OnPlayStageAnimCallBack);
            mySystem.Register<SE_AI_GiantDaDa.Event_GiantDaDaPlayAirBillowAnim>(OnPlayAirBillowAnimCallBack);
            mySystem.Register<SE_AI_GiantDaDa.Event_GiantDaDaPlayRollingStoneAnim>(OnPlayRollingStoneAnimCallBack);
            mySystem.Register<SE_AI_GiantDaDa.Event_GiantDaDaPlayElectricArcAnim>(OnPlayElectricArcAnimCallBack);
            mySystem.Register<SE_AI_GiantDaDa.Event_GiantDaDaPlayDropBugAnim>(OnPlayDropBugAnimCallBack);
            mySystem.Register<SE_AI_GiantDaDa.Event_GiantDaDaPlaySurgeIncomingAnim>(OnPlaySurgeIncomingAnimCallBack);
            mySystem.Register<SE_AI_GiantDaDa.Event_GiantDaDaPlayLightningWithBugAnim>(OnPlayLightningWithBugAnimCallBack);
            mySystem.Register<SE_AI_GiantDaDa.Event_SwitchStage>(OnSwitchStageCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_AI_GiantDaDa.Event_GiantDaDaPlayIdleAnim>(OnPlayIdleAnimCallBack);
            mySystem.Unregister<SE_AI_GiantDaDa.Event_GiantDaDaPlayStageAnim>(OnPlayStageAnimCallBack);
            mySystem.Unregister<SE_AI_GiantDaDa.Event_GiantDaDaPlayAirBillowAnim>(OnPlayAirBillowAnimCallBack);
            mySystem.Unregister<SE_AI_GiantDaDa.Event_GiantDaDaPlayRollingStoneAnim>(OnPlayRollingStoneAnimCallBack);
            mySystem.Unregister<SE_AI_GiantDaDa.Event_GiantDaDaPlayElectricArcAnim>(OnPlayElectricArcAnimCallBack);
            mySystem.Unregister<SE_AI_GiantDaDa.Event_GiantDaDaPlayDropBugAnim>(OnPlayDropBugAnimCallBack);
            mySystem.Unregister<SE_AI_GiantDaDa.Event_GiantDaDaPlaySurgeIncomingAnim>(OnPlaySurgeIncomingAnimCallBack);
            mySystem.Unregister<SE_AI_GiantDaDa.Event_GiantDaDaPlayLightningWithBugAnim>(OnPlayLightningWithBugAnimCallBack);
            mySystem.Unregister<SE_AI_GiantDaDa.Event_SwitchStage>(OnSwitchStageCallBack);
            playable.Dispose();
            playable = null;
        }

        protected override void OnStart() {
            base.OnStart();
            Dispatcher(new SE_Entity.Event_SetShowEntityForAnim() {
                IsShow = false,
            });
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            aiBase = (S_AI_Base)mySystem;
            monsterSign = aiBase.AttributeData.Sign;
            config = AIAnimConfig.Get(AIAnimConfig.GoldDashBossDaDa);
            InitPlayableGraph();
            PreLoadSwitchAnim();

            hasPlayedBornAnim = false;
            bornAnimPlayDelayCountDown = 1;

            AddUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            if (!hasPlayedBornAnim || !hasShowEnt) {
                bornAnimPlayDelayCountDown -= deltaTime;
                if (!hasPlayedBornAnim && bornAnimPlayDelayCountDown <= 0.3f) {
                    Dispatcher(new SE_AI_GiantDaDa.Event_GiantDaDaPlayStageAnim() {
                        Stage = SE_AI_GiantDaDa.Event_GiantDaDaPlayStageAnim.StageEnum.Born,
                    });
                    hasPlayedBornAnim = true;
                }

                if (!hasShowEnt && bornAnimPlayDelayCountDown <= 0) {
                    Dispatcher(new SE_Entity.Event_SetShowEntityForAnim() {
                        IsShow = true,
                    });
                    hasShowEnt = true;
                }
            }

            playable?.OnUpdate(deltaTime);
        }

        protected override ITargetRpc SyncData() {
            bool isShowEntity = true;
            Dispatcher(new SE_Entity.Event_GetShowEntity() {
                CallBack = result => isShowEntity = result
            });
            if (isShowEntity) {
                return new Proto_AI.TargetRpc_Anim {
                    animId = (ushort)playAnim
                };
            } else {
                return new Proto_AI.TargetRpc_Anim {
                    animId = AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_idle_02
                };
            }
        }

        private void InitPlayableGraph() {
            entity = (EntityBase)iEntity;
            var animator = entity.GetComponentInChildren<Animator>(true);
            playable = new SausagePlayable();
            playable.Init(entity.transform, animator, config, $"Server_{monsterSign}");
            playable.CloseLoadEffect();
        }

        // TODO: 临时做法，后续版本美术在标准制程下制作阶段切换特效后可以移除
        private void PreLoadSwitchAnim() {
            string path = config.GetAnimPath(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_switch);
            AssetManager.LoadAnimationClipForFullUrl(path, null);
        }

        private void OnPlayIdleAnimCallBack(ISystemMsg body, SE_AI_GiantDaDa.Event_GiantDaDaPlayIdleAnim ent) {
            PlayIdle();
        }

        private void OnPlayStageAnimCallBack(ISystemMsg body, SE_AI_GiantDaDa.Event_GiantDaDaPlayStageAnim ent) {
            switch (ent.Stage) {
                case SE_AI_GiantDaDa.Event_GiantDaDaPlayStageAnim.StageEnum.Born:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_appear);
                    break;
                case SE_AI_GiantDaDa.Event_GiantDaDaPlayStageAnim.StageEnum.Switch:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_switch);
                    break;
                case SE_AI_GiantDaDa.Event_GiantDaDaPlayStageAnim.StageEnum.Leave:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_leave);
                    break;
            }
        }

        private void OnPlayAirBillowAnimCallBack(ISystemMsg body, SE_AI_GiantDaDa.Event_GiantDaDaPlayAirBillowAnim ent) {
            switch (ent.Stage) {
                case 0:
                    PlayIdle();
                    break;
                case 1:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_01_attack01);
                    break;
                case 2:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_01_attack02);
                    break;
                case 3:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_01_attack03);
                    break;
            }
        }

        private void OnPlayRollingStoneAnimCallBack(ISystemMsg body, SE_AI_GiantDaDa.Event_GiantDaDaPlayRollingStoneAnim ent) {
            if (!ent.IsTrue) {
                PlayIdle();
                return;
            }

            PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_02);
        }

        private void OnPlayElectricArcAnimCallBack(ISystemMsg body, SE_AI_GiantDaDa.Event_GiantDaDaPlayElectricArcAnim ent) {
            switch (ent.Stage) {
                case 0:
                    PlayIdle();
                    break;
                case 1:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_04_attack01);
                    break;
                case 2:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_04_attack02);
                    break;
            }
        }

        private void OnPlayDropBugAnimCallBack(ISystemMsg body, SE_AI_GiantDaDa.Event_GiantDaDaPlayDropBugAnim ent) {
            switch (ent.Stage) {
                case SE_AI_GiantDaDa.Event_GiantDaDaPlayDropBugAnim.AnimStage.Undefined:
                    PlayIdle();
                    break;
                case SE_AI_GiantDaDa.Event_GiantDaDaPlayDropBugAnim.AnimStage.Start:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_03_Start);
                    break;
                case SE_AI_GiantDaDa.Event_GiantDaDaPlayDropBugAnim.AnimStage.Loop:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_03_Loop);
                    break;
                case SE_AI_GiantDaDa.Event_GiantDaDaPlayDropBugAnim.AnimStage.End:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_03_End);
                    break;
            }
        }

        private void OnPlaySurgeIncomingAnimCallBack(ISystemMsg body, SE_AI_GiantDaDa.Event_GiantDaDaPlaySurgeIncomingAnim ent) {
            if (!ent.IsTrue) {
                PlayIdle();
                return;
            }

            PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_05);
        }

        private void OnPlayLightningWithBugAnimCallBack(ISystemMsg body, SE_AI_GiantDaDa.Event_GiantDaDaPlayLightningWithBugAnim ent) {
            switch (ent.Stage) {
                case SE_AI_GiantDaDa.Event_GiantDaDaPlayLightningWithBugAnim.AnimStage.Undefined:
                    PlayIdle();
                    break;
                case SE_AI_GiantDaDa.Event_GiantDaDaPlayLightningWithBugAnim.AnimStage.LightningCast:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_07A);
                    break;
                case SE_AI_GiantDaDa.Event_GiantDaDaPlayLightningWithBugAnim.AnimStage.BugCast:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_08_Start);
                    break;
                case SE_AI_GiantDaDa.Event_GiantDaDaPlayLightningWithBugAnim.AnimStage.BugLoop:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_08_Loop);
                    break;
                case SE_AI_GiantDaDa.Event_GiantDaDaPlayLightningWithBugAnim.AnimStage.BugPost:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_08_End);
                    break;
                case SE_AI_GiantDaDa.Event_GiantDaDaPlayLightningWithBugAnim.AnimStage.LightningPost:
                    PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_skill_07B);
                    break;
            }
        }

        private void OnSwitchStageCallBack(ISystemMsg body, SE_AI_GiantDaDa.Event_SwitchStage ent) {
            curStage = ent.NextStage;
        }

        private void PlayIdle() {
            if (curStage == 0) {
                PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_idle_01);
            } else if (curStage == 1) {
                PlayAnim(AnimConfig_GoldDashBossDaDa.Anim_GoldDashBossDaDa_idle_02);
            }
        }

        private void PlayAnim(int animId) {
            if (playAnim == animId) {
                return;
            }
            playAnim = animId;
            playable?.PlayAnimId(animId);
            Rpc(new Proto_AI.Rpc_Anim {
                animId = (ushort)animId,
            });
        }
    }
}