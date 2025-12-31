using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityGoldJokerSurpriseBoom : ComponentBase {
        private IGPO fireGPO;
        private AbilityM_GoldJokerSurpriseBoom useMData;
        private Vector3 startPoint;
        private Vector3 startDir;
        private float timer;
        private bool isPlayEffect;

        protected override void OnAwake() {
            base.OnAwake();
            var abSystem = (SAB_GoldJokerSurpriseBoomSystem)mySystem;
            useMData = abSystem.useMData;
            fireGPO = abSystem.FireGPO;
            startPoint = fireGPO.GetPoint() + Vector3.up * useMData.M_OffsetY;
            startDir = fireGPO.GetForward();
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
            timer = useMData.M_WarningTime;
            if (useMData.M_BossType == 1) {
                fireGPO.Dispatcher(new SE_AI.Event_PlayBossAnim() {
                    Id = AnimConfig_GoldDash_BOSSAceJoker.Anim_BOSSAceJoker_SurpriseBomb
                });
            } else {
                fireGPO.Dispatcher(new SE_AI.Event_PlayBossAnim() {
                    Id = AnimConfig_GoldDash_BOSSAceJoker.Anim_BOSSSupremeJoker_SurpriseBomb
                });
            }

            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld() {
                FireGPO = fireGPO,
                AbilityMData = new AbilityData.PlayAbility_PlayWarningEffect() {
                    ConfigId = AbilityConfig.PlayCircleWarningEffect,
                    In_StartPoint = startPoint,
                    In_StartLookAt = startDir,
                    In_StartScale = new Vector3(2 * useMData.M_MaxDistance, 1, 2 * useMData.M_MaxDistance),
                    In_Angle = 0,
                    In_FillCircle = true,
                    In_LifeTime = useMData.M_WarningTime,
                }
            });

            if (useMData.M_StartEffectId > 0) {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = fireGPO,
                    MData = AbilityM_PlayEffect.CreateForID((byte)useMData.M_StartEffectId),
                    InData = new AbilityIn_PlayEffect() {
                        In_StartPoint = fireGPO.GetPoint(),
                        In_StartRota = fireGPO.GetRota(),
                        In_LifeTime = useMData.M_LifeTime
                    },
                });
            }
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            fireGPO = null;
        }

        private void OnUpdate(float delta) {
            if (timer > 0) {
                timer -= delta;
                return;
            }
            
            if (!isPlayEffect) {
                isPlayEffect = true;
                PlayEffect();
            }
        }

        private void PlayEffect() {
            var id = useMData.M_BossType == 1 ? AbilityM_ExpandBoom.ID_AceJokerSurpriseBoom : AbilityM_ExpandBoom.ID_GoldJokerSurpriseBoom;
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility() {
                FireGPO = fireGPO,
                MData = AbilityM_ExpandBoom.CreateForID(id),
                InData = new AbilityIn_ExpandBoom() {
                    In_StartPoint = startPoint,
                    In_ExpandSpeed = useMData.M_ExpandSpeed,
                    In_MaxDistance = useMData.M_MaxDistance,
                    In_CheckHight = useMData.M_AttackHeight,
                    In_ATK = useMData.M_ATK,
                    In_LifeTime = useMData.M_LifeTime - useMData.M_WarningTime
                }
            });
        }
    }
}
