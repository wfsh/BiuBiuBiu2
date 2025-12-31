using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIGoldJokerSkill : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public byte bossType;
        }
        private S_AI_Base aiBase;
        private byte bossType = 0;
        private string sceneTypeSign;
        
        protected override void OnAwake() {
            Register<SE_AI.Event_PlayGoldJokerSkill>(OnPlayBossSkill);
            Register<SE_AI.Event_PlayBossTeleport>(OnPlayBossTeleport);
            aiBase = (S_AI_Base)mySystem;
            var initData = (InitData)initDataBase;
            bossType = initData.bossType;
        }

        protected override void OnClear() {
            Unregister<SE_AI.Event_PlayGoldJokerSkill>(OnPlayBossSkill);
            Unregister<SE_AI.Event_PlayBossTeleport>(OnPlayBossTeleport);
            aiBase = null;
        }

        protected override void OnStart() {
            base.OnStart();
            sceneTypeSign = ExtractPart(SceneData.Get(ModeData.SceneId).ElementConfig);
        }

        private void OnPlayBossSkill(ISystemMsg body, SE_AI.Event_PlayGoldJokerSkill ent) {
            switch (ent.SkillType) {
                case GoldJokerSkillType.FloatingGun:
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO, 
                        MData = AbilityM_GoldJokerFloatingGunSpawner.CreateForKey(bossType, sceneTypeSign),
                    });
                    break;
                case GoldJokerSkillType.RocketBomb:
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO, 
                        MData = AbilityM_GoldJokerRocketBombSpawner.CreateForKey(bossType, sceneTypeSign),
                    });
                    break;
                case GoldJokerSkillType.SurpriseBoom:
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO, 
                        MData = AbilityM_GoldJokerSurpriseBoom.CreateForKey(bossType, sceneTypeSign),
                    });
                    break;
                case GoldJokerSkillType.DollBomb:
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO,
                        MData = AbilityM_GoldJokerDollBombSpawner.CreateForKey(bossType, sceneTypeSign),
                    });
                    break;
                case GoldJokerSkillType.CardTrick:
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO, 
                        MData = AbilityM_GoldJokerCardTrickSpawner.CreateForKey(bossType, sceneTypeSign),
                    });
                    break;
            }
        }

        private void OnPlayBossTeleport(ISystemMsg body, SE_AI.Event_PlayBossTeleport ent) {
            Dispatcher(new SE_Behaviour.Event_StopMove());
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO,
                MData = AbilityM_GoldJokerFlash.CreateForKey(bossType),
                InData = new AbilityIn_GoldJokerFlash() {
                    In_EndPoint = ent.EndPoint
                }
            });
        }
        
        private string ExtractPart(string input) {
            var parts = input.Split('_');
            if (parts.Length >= 2) {
                // 取最后两个部分组合
                return $"{parts[parts.Length - 2]}_{parts[parts.Length - 1]}";
            }
            return input; // 格式不符合时返回原始字符串
        }  
    }
}