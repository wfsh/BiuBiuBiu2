using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityAuroraDragonFireBall : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityM_AuroraDragonFireBall Param;
        }
        private SAB_AuroraDragonFireBallSystem abSystem;
        private AbilityM_AuroraDragonFireBall useMData;
        private AbilityIn_AuroraDragonFireBall useInData;
        private ServerGPO fireGPO;
        private float createHitEffectTime;
        private Vector3 moveDir;
        private float hitTime;
        
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_AuroraDragonFireBallSystem)mySystem;
            useInData = (AbilityIn_AuroraDragonFireBall)abSystem.InData;
            var initData = (InitData)initDataBase;
            useMData = initData.Param;
            fireGPO = abSystem.FireGPO;
            createHitEffectTime = useMData.M_CreateHitEffectTime;
            hitTime = useMData.M_CheckDamageTime;
            SetDefaultValue();
        }

        protected override void OnStart() {
            base.OnStart();
            PlayWarringEffect();
            AddUpdate(OnUpdate);
        }
        
        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireGPO = null;
        }

        private void OnUpdate(float deltaTime) {
            iEntity.SetPoint(iEntity.GetPoint() + moveDir * deltaTime);
            if (createHitEffectTime > 0f) {
                createHitEffectTime -= deltaTime;
                if (createHitEffectTime <= 0f) {
                    PlayBoomEffect();
                }
            }
            if (hitTime > 0f) {
                hitTime -= deltaTime;
                if (hitTime <= 0f) {
                    CheckDamage();
                }
            }
        }
        
        private void SetDefaultValue() {
            moveDir = useInData.In_EndPos - useInData.In_StartPos;
            moveDir.y = 0f;
            moveDir.Normalize();
            moveDir.y = 20f;
        }
        
        private void PlayWarringEffect() {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(AbilityM_PlayEffectWithFullDimensionScale.ID_RoundWarring),
                InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                    In_StartPoint = useInData.In_EndPos,
                    In_LifeTime = 2f,
                    In_StartScale = new Vector3(useMData.M_Radius * 2f, 1f, useMData.M_Radius * 2f),
                },
            });
        }

        private void PlayBoomEffect() {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility() {
                FireGPO = iGPO,
                MData = AbilityM_PlayEffect.CreateForID(AbilityM_PlayEffect.ID_AuroraDragonFireBallBoom),
                InData = new AbilityIn_PlayEffect {
                    In_StartPoint = useInData.In_EndPos + new Vector3(0f, 0.1f, 0f),
                    In_LifeTime = 2f,
                },
            });
        }
        
        private void CheckDamage() {
            if (fireGPO == null || fireGPO.IsDead() || fireGPO.IsClear()) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonSkill3Explosion,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = 1f
                }
            });
            List<IGPO> targetGPOList = null;
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetAllTargetInFightRange() {
                CallBack = (targetList) => { targetGPOList = targetList; },
            });
            if (null == targetGPOList || targetGPOList.Count <= 0) {
                return;
            }

            var attackPos = useInData.In_EndPos;
            var attackRadiusSqr = useMData.M_Radius * useMData.M_Radius;
            for (int i = 0; i < targetGPOList.Count; ++i) {
                var target = targetGPOList[i];
                if (null != target &&
                    !target.IsDead() &&
                    !target.IsClear() &&
                    (attackPos - target.GetPoint()).sqrMagnitude <= attackRadiusSqr) {
                    mySystem.Dispatcher(new SE_Ability.HitGPO {
                        hitGPO = target,
                        isHead = false,
                        hitPoint = Vector3.zero,
                        SourceAbilityType = useMData.GetTypeID(),
                    });
                }
            }
        }
    }
}