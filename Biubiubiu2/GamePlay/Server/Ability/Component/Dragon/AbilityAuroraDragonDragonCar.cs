using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityAuroraDragonDragonCar : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityM_AuroraDragonDragonCar Param;
        }
        private SAB_AuroraDragonDragonCarSystem abSystem;
        private AbilityM_AuroraDragonDragonCar config;
        private ServerGPO fireGPO;
        private AIEntity myAIEntity;
        private List<int> hitGPOs;
        
        private Vector3 fightRangeCenter;
        private float fightRangeRadius;

        private float rushDisMax; // 冲撞最远距离
        private float rushRangeDis; // 移动不能超过战斗场地边缘距离
        private float chargeTime;
        private float chargeTrackTime;
        private float rushTime;
        private float downTime;
        private float downDamageTime;
        
        private Vector3 startPos; // 冲撞起始位置
        private Vector3 moveDir; // 冲撞方向
        private Vector3 endPos; // 冲撞结束位置
        private float rushDis; // 实际冲撞距离
        
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_AuroraDragonDragonCarSystem)mySystem;
            var initData = (InitData)initDataBase;
            config = initData.Param;
            fireGPO = abSystem.FireGPO;
        }

        protected override void OnStart() {
            base.OnStart();
            #region 埋点
            MsgRegister.Dispatcher(new SM_Sausage.BossReleaseAbility() {
                SourceAbilityType = config.GetTypeID(),
            });
            #endregion
            myAIEntity = (AIEntity)fireGPO.GetEntity();
            hitGPOs = new List<int>();

            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData {
                CallBack = (Vector3 center, float radius, float endTime, bool blockDamage) => {
                    fightRangeCenter = center;
                    fightRangeCenter.y = 0f;
                    fightRangeRadius = radius;
                }
            });

            rushDisMax = config.M_DisMax;
            rushRangeDis = config.M_RangeDis;
            chargeTime = config.M_ChargeTime;
            chargeTrackTime = config.M_ChargeTrackTime;
            rushTime = config.M_RushTime;
            downTime = config.M_DownTime;
            downDamageTime = config.M_DownDamageTime;

            StartCharge();
            AddUpdate(OnUpdate);
        }
        
        protected override void OnClear() {
            base.OnClear();
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_DragonCarEndAnim {
                IsTrue = false
            });
            RemoveUpdate(OnUpdate);
            fireGPO = null;
        }

        private void OnUpdate(float deltaTime) {
            if (chargeTrackTime > 0f) {
                chargeTrackTime -= deltaTime;
                if (chargeTrackTime <= 0) {
                    // 停止追踪，计算冲撞终点
                    CalcDragonCarEndPos();
                    // 播放预警特效
                    PlayRushWarringEffect();
                } else {
                    // 朝目标转向
                    UpdateChargeRotation(deltaTime);
                }
            }

            if (chargeTime > 0f) {
                chargeTime -= deltaTime;
                if (chargeTime <= 0f) {
                    // 进入冲撞阶段
                    StartRush();
                }
            } else if (rushTime > 0f) {
                rushTime -= deltaTime;
                if (rushTime <= 0f) {
                    // 结束冲撞，撞地
                    StartDown();
                } else {
                    RushTimeEvent(deltaTime);
                }
            } else if (downTime > 0) {
                downTime -= deltaTime;
                if (downTime <= 0f) {
                    // 撞地动作结束
                    fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_DragonCarEndAnim {
                        IsTrue = true
                    });
                }

                if (downDamageTime > 0) {
                    downDamageTime -= deltaTime;
                    if (downDamageTime <= 0f) {
                        DownDamage();
                    }
                }
            }
        }

        private void StartCharge() {
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_DragonCarChargeAnim {});
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonSkill4Fly,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = config.M_ChargeTime,
                }
            });
        }

        private void PlayRushWarringEffect() {
            if(myAIEntity == null ||
                fireGPO == null) {
                return;
            }
            var forward = myAIEntity.transform.eulerAngles;
            forward.x = forward.z = 0f;
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(AbilityM_PlayEffectWithFullDimensionScale.ID_RectangleWarring),
                InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                    In_StartPoint = fireGPO.GetPoint() + new Vector3(0f, 0.2f, 0f),
                    In_StartRota = Quaternion.Euler(forward),
                    In_StartScale = new Vector3(config.M_RushDamageRadius / 4.5f, 1f, rushDis / 5f),
                    In_LifeTime = chargeTime - chargeTrackTime + rushTime,
                },
            });
        }
        
        // 朝目标转向
        private void UpdateChargeRotation(float deltaTime) {
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_GetAttackTarget() {
                CallBack = (attackGpo) => {
                    if (attackGpo == null) {
                        return;
                    }
                    
                    var dest = attackGpo.GetPoint() - myAIEntity.GetPoint();
                    var newForward = myAIEntity.GetForward();
                    MsgRegister.Dispatcher(new SM_Sausage.GetLerpForward {
                        NowForward = newForward,
                        TargetForward = dest.normalized,
                        LerpValue = deltaTime * config.M_ChargeTrackSpeed,
                        CallBack = (pos) => { newForward = pos; }
                    });
                    myAIEntity.LookAT(myAIEntity.GetPoint() + newForward);
                }
            });
        }
        
        // 计算冲刺结束位置
        private void CalcDragonCarEndPos() {
            startPos = fireGPO.GetPoint();
            startPos.y = 0;
            moveDir = fireGPO.GetForward();
            moveDir.y = 0;
            moveDir.Normalize();
            endPos = startPos + moveDir * rushDisMax;
            var disMax = fightRangeRadius - rushRangeDis;
            if ((endPos - fightRangeCenter).sqrMagnitude > disMax * disMax) {
                endPos = GetRangeMaxDisPos(disMax);
            }
            rushDis = (startPos - endPos).magnitude;
        }
        
        // 计算射线在圆上的交点
        private Vector3 GetRangeMaxDisPos(float disMax) {
            var dir = endPos - startPos;
            // 步骤1: 计算射线起点到圆心的向量
            var originToCenter = fightRangeCenter - startPos;
            // 步骤2: 计算二次方程系数
            float a = Vector3.Dot(dir, dir);
            float b = -2f * Vector3.Dot(dir, originToCenter);
            float c = Vector3.Dot(originToCenter, originToCenter) - disMax * disMax;
            // 步骤3: 计算判别式
            float discriminant = b * b - 4f * a * c;
            if (discriminant < 0) {
                return endPos;
            }
            // 步骤4: 计算可能的交点参数t
            float t = (-b + MathF.Sqrt(discriminant)) / (2f * a);
            // 步骤5: 筛选有效解（t >= 0 表示在射线方向上）
            if (t >= 0f) {
                return startPos + t * dir;
            }
            t = (-b - MathF.Sqrt(discriminant)) / (2f * a);
            if (t >= 0) {
                return startPos + t * dir;
            }
            return endPos;
        }

        // 开始冲撞
        private void StartRush() {
            if(myAIEntity == null ||
               fireGPO == null) {
                return;
            }
            startPos.y = endPos.y = myAIEntity.GetPoint().y;
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_DragonCarAttackAnim { });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_AuroraDragonDragonCarFlame.CreateForKey(config.K_BossType),
                InData = new AbilityIn_AuroraDragonDragonCarFlame() {
                    In_StartPos = startPos,
                    In_EndPos = endPos,
                    In_Speed = config.M_RushSpeed,
                    In_DamageRadius = config.M_RushDamageRadius,
                    In_DamageInterval = config.M_FlameDamageInterval,
                    In_DamageHeight = config.M_FlameDamageHeight,
                    In_Damage = config.M_FlameDamage,
                    In_LifeTime = config.M_RushTime + config.M_FlameTime,
                },
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonFlyStrike,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = config.M_RushTime
                }
            });
        }

        // 冲撞逻辑
        private void RushTimeEvent(float deltaTime) {
            if (myAIEntity == null ||
                myAIEntity.IsClear()) {
                return;
            }

            var newPos = myAIEntity.GetPoint() + moveDir * config.M_RushSpeed * deltaTime;
            if ((newPos - startPos).sqrMagnitude > rushDis * rushDis) {
                newPos = endPos;
            }
            myAIEntity.SetPoint(newPos);
            
            // 伤害检测
            List<IGPO> targetGPOList = null;
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetAllTargetInFightRange() {
                CallBack = (targetList) => { targetGPOList = targetList; },
            });
            if (null == targetGPOList || targetGPOList.Count <= 0) {
                return;
            }
            for (int i = 0; i < targetGPOList.Count; ++i) {
                var target = targetGPOList[i];
                if (target == null || target.IsDead() || target.IsClear() || hitGPOs.Contains(target.GetGpoID())) {
                    continue;
                }

                if ((target.GetPoint() - myAIEntity.GetPoint()).sqrMagnitude <=
                    config.M_RushDamageRadius * config.M_RushDamageRadius) {
                    MsgRegister.Dispatcher(new SM_Sausage.GetAttackBlock() {
                        AttackPos = myAIEntity.GetPoint(),
                        RolePos = target.GetPoint(),
                        AddOffsetY = 0.5f,
                        CallBack = (isHit) => {
                            if (isHit) {
                                hitGPOs.Add(target.GetGpoID());
                                mySystem.Dispatcher(new SE_Ability.HitGPO {
                                    hitGPO = target,
                                    isHead = false,
                                    hitPoint = Vector3.zero,
                                    SourceAbilityType = config.GetTypeID(),
                                });
                            }
                        }
                    });
                }
            }
        }

        // 开始撞地
        private void StartDown() {
            if(myAIEntity == null ||
               fireGPO == null) {
                return;
            }
            endPos.y = myAIEntity.GetPoint().y;
            myAIEntity.SetPoint(endPos);
            PlayDownWarringEffect();
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_DragonCarDownAnim { });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(AbilityM_PlayEffectWithFullDimensionScale.ID_AuroraDragonDragonCarDown),
                InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                    In_StartPoint = endPos + new Vector3(0f, 0.1f, 0f),
                    In_StartScale = new Vector3(config.M_DownDamageRadius / 3f, 1f, config.M_DownDamageRadius / 3f),
                    In_LifeTime = 2f,
                },
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonSkill4SprintUp,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = 2f,
                }
            });
        }

        private void PlayDownWarringEffect() {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(AbilityM_PlayEffectWithFullDimensionScale.ID_RoundWarring),
                InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                    In_StartPoint = endPos,
                    In_LifeTime = downDamageTime,
                    In_StartScale = new Vector3(config.M_DownDamageRadius * 2f, 1f, config.M_DownDamageRadius * 2f),
                },
            });
        }

        // 撞地伤害
        private void DownDamage() {
            if (fireGPO == null || fireGPO.IsClear()) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonSkill4Fall,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = 2f,
                }
            });
            List<IGPO> targetGPOList = null;
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetAllTargetInFightRange() {
                CallBack = (targetList) => { targetGPOList = targetList; },
            });
            if (null == targetGPOList || targetGPOList.Count <= 0) {
                return;
            }

            var attackRadiusSqr = config.M_DownDamageRadius * config.M_DownDamageRadius;
            for (int i = 0; i < targetGPOList.Count; ++i) {
                var target = targetGPOList[i];
                if (null != target &&
                    !target.IsDead() &&
                    (fireGPO.GetPoint() - target.GetPoint()).sqrMagnitude <= attackRadiusSqr &&
                    Mathf.Abs((fireGPO.GetPoint() - target.GetPoint()).y) < 1.5f) {
                    mySystem.Dispatcher(new SE_Ability.HitGPO {
                        hitGPO = target,
                        isHead = false,
                        hitPoint = Vector3.zero,
                        SourceAbilityType = config.GetTypeID(),
                    });
                }
            }
        }
    }
}