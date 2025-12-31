using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Util;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityAuroraDragonDelayBlast : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityM_DragonDelayBlastSpawner Param;
        }
        private SAB_AuroraDragonDelayBlastSystem abSystem;
        private AbilityM_DragonDelayBlastSpawner param;
        private ServerGPO fireGPO;
        private float createHitEffectTime;
        private float boomTime;
        private int checkIndex;
        
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_AuroraDragonDelayBlastSystem)mySystem;
            var initData = (InitData)initDataBase;
            param = initData.Param;
            fireGPO = abSystem.FireGPO;
            createHitEffectTime = param.M_CreateTime;
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
            if (createHitEffectTime > 0f) {
                createHitEffectTime -= deltaTime;
                if (createHitEffectTime <= 0f) {
                    PlayBallffect();
                    boomTime = param.M_BoomTime;
                }
            }

            if (boomTime > 0) {
                boomTime -= deltaTime;
                if (boomTime < param.M_BoomTime - checkIndex * param.M_FallBallCheckTime) {
                    checkIndex++;
                    CheckDamage();
                }
                
                if (boomTime <= 0f) {
                    PlayBoomEffect();
                    CheckBoomDamage();
                }
            }
        }
        
        private void PlayWarringEffect() {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(AbilityM_PlayEffectWithFullDimensionScale.ID_RoundWarring),
                InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                    In_StartPoint = abSystem.InData.In_StartPos,
                    In_LifeTime = param.M_CreateTime,
                    In_StartScale = new Vector3(param.M_Radius * 2f, 1f, param.M_Radius * 2f),
                },
            });
        }

        private void PlayBallffect() {
            var rowId = abSystem.InData.IsAncientBoss
                ? AbilityM_PlayEffect.ID_AncientDragonDelayBlastFireBall
                : AbilityM_PlayEffect.ID_AuroraDragonDelayBlastFireBall;
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility() {
                FireGPO = iGPO,
                MData = AbilityM_PlayEffect.CreateForID(rowId),
                InData = new AbilityIn_PlayEffect {
                    In_StartPoint = abSystem.InData.In_StartPos + new Vector3(0,param.M_BallCreateHeight,0),
                    In_LifeTime = param.M_BoomTime,
                },
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = abSystem.InData.IsAncientBoss? WwiseAudioSet.Id_GoldDashBossAdragonDelayBlastLv2 : WwiseAudioSet.Id_GoldDashBossAdragonDelayBlastLv1,
                    In_StartPoint = abSystem.InData.In_StartPos + new Vector3(0,param.M_BallCreateHeight,0),
                    In_LifeTime = param.M_BoomTime + 2f,
                }
            });

            if (abSystem.InData.IsAncientBoss) {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = fireGPO,
                    MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(AbilityM_PlayEffectWithFullDimensionScale.ID_DelayBlastTip),
                    InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                        In_StartPoint = abSystem.InData.In_StartPos, 
                        In_LifeTime = param.M_BoomTime, 
                        In_StartScale = new Vector3(param.M_BoomWidth / 4f, 1f, param.M_BoomLong / 40f),
                    },
                });
            } else {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = fireGPO,
                    MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(AbilityM_PlayEffectWithFullDimensionScale.ID_RoundWarring),
                    InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                        In_StartPoint = abSystem.InData.In_StartPos, 
                        In_LifeTime = param.M_BoomTime, 
                        In_StartScale = new Vector3(param.M_BoomRadius * 2f, 1f, param.M_BoomRadius * 2f),
                    },
                });
            }
        }
        
        private void PlayBoomEffect() {
            var rowId = abSystem.InData.IsAncientBoss
                ? AbilityM_PlayEffect.ID_AncientDragonDelayBlastExplosion
                : AbilityM_PlayEffect.ID_AuroraDragonDelayBlastExplosion;
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility() {
                FireGPO = iGPO,
                MData = AbilityM_PlayEffect.CreateForID(rowId),
                InData = new AbilityIn_PlayEffect {
                    In_StartPoint = abSystem.InData.In_StartPos,
                    In_LifeTime = 3,
                },
            });
        }
        
        private void CheckDamage() {
            if (fireGPO == null || fireGPO.IsClear() || fireGPO.IsDead()) {
                return;
            }
            List<IGPO> targetGPOList = null;
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetAllTargetInFightRange() {
                CallBack = (targetList) => { targetGPOList = targetList; },
            });
            if (null == targetGPOList || targetGPOList.Count <= 0) {
                return;
            }

            var attackPos = abSystem.InData.In_StartPos + new Vector3(0, param.M_BallCreateHeight, 0);
            var attackRadiusSqr = param.M_Radius * param.M_Radius;
            for (int i = 0; i < targetGPOList.Count; ++i) {
                var target = targetGPOList[i];
                if (null != target &&
                    !target.IsDead() &&
                    (attackPos - target.GetPoint()).sqrMagnitude <= attackRadiusSqr) {
                    mySystem.Dispatcher(new SE_Ability.HitGPO {
                        hitGPO = target,
                        isHead = false,
                        hitPoint = Vector3.zero,
                        SourceAbilityType = param.GetTypeID(),
                    });
                }
            }
        }
        
        private void CheckBoomDamage() {
            if (fireGPO == null || fireGPO.IsClear() || fireGPO.IsDead()) {
                return;
            }
            List<IGPO> targetGPOList = null;
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetAllTargetInFightRange() {
                CallBack = (targetList) => { targetGPOList = targetList; },
            });
            if (null == targetGPOList || targetGPOList.Count <= 0) {
                return;
            }

            var attackPos = abSystem.InData.In_StartPos;
            for (int i = 0; i < targetGPOList.Count; ++i) {
                var target = targetGPOList[i];
                if (null != target &&
                    !target.IsDead() ) {
                    if (CheckBoomRange(attackPos, target.GetPoint())) {
                        mySystem.Dispatcher(new SE_Ability.HitGPO {
                            hitGPO = target,
                            isHead = false,
                            hitPoint = Vector3.zero,
                            HurtRatio = param.M_BoomAtkRatio,
                            SourceAbilityType = param.GetTypeID(), });
                    }
                }
            }
        }

        private bool CheckBoomRange(Vector3 attackPos, Vector3 targetPos) {
            if (abSystem.InData.IsAncientBoss) {
                var offset = attackPos - targetPos;
                if (Mathf.Abs(offset.y) < param.M_BoomHeight && (Mathf.Abs(offset.x) < param.M_BoomWidth / 2f && Mathf.Abs(offset.z) < param.M_BoomLong / 2f || Mathf.Abs(offset.z) < param.M_BoomWidth / 2f && Mathf.Abs(offset.x) < param.M_BoomLong / 2f)) {
                    return true;
                }
            } else {
                attackPos += new Vector3(0, param.M_BallCreateHeight, 0);
                var attackRadiusSqr = param.M_BoomRadius * param.M_BoomRadius;
                if ((attackPos - targetPos).sqrMagnitude <= attackRadiusSqr) {
                    return true;
                }
            }

            return false;
        }
    }
}