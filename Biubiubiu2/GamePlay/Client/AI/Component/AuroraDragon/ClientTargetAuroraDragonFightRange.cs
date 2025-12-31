using System;
using Sofunny.BiuBiuBiu2.ClientGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientTargetAuroraDragonFightRange : ComponentBase {
        private bool initBgm;
        private uint fightBGMPlayingId;
        private bool beGivenUp;

        protected override void OnAwake() {
            MsgRegister.Register<CM_Sausage.LocalPlayerChangeBossFightBGM>(OnLocalPlayerGiveUpBossFight);
            mySystem.Register<CE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Register<CE_AI.Event_GiantDaDaStartSwitchPerform>(OnStartSwitchCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            initBgm = false;
            AddUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            if (!initBgm) {
                mySystem.Dispatcher(new CE_AI.Event_GiantDaDaGetFightRangeData() {
                    CallBack = SetFightRangeData
                });
            }
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_AI.Rpc_GoldDashRemoveFightRange.ID, OnRemoveFightRange);
        }

        protected override void OnClear() {
            base.OnClear();
            StopFightBGM();
            RemoveProtoCallBack(Proto_AI.Rpc_GoldDashRemoveFightRange.ID, OnRemoveFightRange);
            mySystem.Unregister<CE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Unregister<CE_AI.Event_GiantDaDaStartSwitchPerform>(OnStartSwitchCallBack);
        }

        private void OnLocalPlayerGiveUpBossFight(CM_Sausage.LocalPlayerChangeBossFightBGM ent) {
            beGivenUp = true;
            // 本地玩家放弃 Boss 战，停止 BGM
            StopFightBGM();
        }

        private void OnRemoveFightRange(INetwork network, IProto_Doc docData) {
            var data = (Proto_AI.Rpc_GoldDashRemoveFightRange)docData;
            if (data.BelongGPOId == GpoID) {
                // 战斗区域被移除，针对的是 Boss 战失败的情形
                StopFightBGM();
                RemoveAbility();
            }
        }

        private void OnSetIsDeadCallBack(ISystemMsg body, CE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead) {
                // Boss 死亡
                StopFightBGM();
            }
        }

        private void RemoveAbility() {
            var abSystem = (C_Ability_Base)mySystem;
            abSystem.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = abSystem.GetAbilityId()
            });
        }

        private void OnStartSwitchCallBack(ISystemMsg body, CE_AI.Event_GiantDaDaStartSwitchPerform ent) {
            StopFightBGM();
        }

        private void SetFightRangeData(Vector3 center, float radius) {
            if (radius > 0f) {
                PlayBossFightBGM();
                initBgm = true;
            }
        }

        private void PlayBossFightBGM() {
            if (beGivenUp) {
                return;
            }
            var audioKey = iGPO.GetGpoMID() == GPOM_AuroraDragonSet.Id_AuroraDragon
                ? WwiseAudioSet.Id_GoldDashBossMusicAuroraDragon
                : WwiseAudioSet.Id_GoldDashBossMusicAncientDragon;
            MsgRegister.Dispatcher(new CM_Sausage.TryToStartBossFightBGM() {
                BossGPO = iGPO,
                BossFightBGMKey = AudioData.GetWwiseEventAudioById(audioKey),
                CallBack = SetFightBGMPlayingIdCallBack
            });
        }

        private void SetFightBGMPlayingIdCallBack(uint playingId) {
            fightBGMPlayingId = playingId;
        }

        private void StopFightBGM() {
            if (fightBGMPlayingId != 0) {
                MsgRegister.Dispatcher(new CM_Sausage.StopBossFightBGM() {
                    BossGPO = iGPO,
                    FightBGMPlayingId = fightBGMPlayingId,
                });
                fightBGMPlayingId = 0;
            }
        }
    }
}