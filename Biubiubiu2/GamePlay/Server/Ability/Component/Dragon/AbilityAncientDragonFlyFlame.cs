using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityAncientDragonFlyFlame : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityM_AncientDragonFlyFlame Param;
        }
        
        private SAB_AncientDragonFlyFlameSystem abSystem;
        private AbilityM_AncientDragonFlyFlame config;
        private ServerGPO fireGPO;
        private AIEntity myAIEntity;
        
        private Vector3 fightRangeCenter;
        private float fightRangeRadius;
        
        private float flyDisMax; // 冲撞最远距离
        private float flyRangeDis; // 移动不能超过战斗场地边缘距离

        private Vector3 endPos;
        private float flyUpTime;
        private float flyDownTime;
        
        private Vector3 startPos;
        private Vector3 moveDir;
        private float flyTime;
        private float flyDis; // 实际冲撞距离
        
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_AncientDragonFlyFlameSystem)mySystem;
            var initData = (InitData)initDataBase;
            config = initData.Param;
            SetDefaultValue();
        }

        protected override void OnStart() {
            base.OnStart();
            #region 埋点
            MsgRegister.Dispatcher(new SM_Sausage.BossReleaseAbility() {
                SourceAbilityType = config.GetTypeID(),
            });
            #endregion
            myAIEntity = (AIEntity)fireGPO.GetEntity();
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData {
                CallBack = (Vector3 center, float radius, float endTime, bool blockDamage) => {
                    fightRangeCenter = center;
                    fightRangeCenter.y = 0f;
                    fightRangeRadius = radius;
                }
            });
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_FlyFlameStartAnim {});
            CalcDragonCarEndPos();
            PlayWarringEffect();
            AddUpdate(OnUpdate);
        }
        
        protected override void OnClear() {
            base.OnClear();
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_FlyFlameEndAnim {
                IsTrue = false
            });
            RemoveUpdate(OnUpdate);
            fireGPO = null;
        }

        private void SetDefaultValue() {
            abSystem = (SAB_AncientDragonFlyFlameSystem)mySystem;
            fireGPO = abSystem.FireGPO;
            flyDisMax = config.M_FlyDisMax;
            flyRangeDis = config.M_RangeDis;
            flyUpTime = config.M_FlyUpTime;
            flyTime = config.M_FlyTime;
            flyDownTime = config.M_FlyDownTime;
        }

        private void OnUpdate(float deltaTime) {
            if (flyUpTime > 0f) {
                flyUpTime -= deltaTime;
                if (flyUpTime <= 0f) {
                    // 进入飞行攻击阶段
                    StartAttack();
                }
            } else if (flyTime > 0f) {
                flyTime -= deltaTime;
                if (flyTime <= 0f) {
                    // 进入降落阶段
                    myAIEntity.SetPoint(endPos);
                    fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_FlyFlameEndAnim {
                        IsTrue = true
                    });
                } else {
                    AttackTimeEvent(deltaTime);
                }
            }
        }
        
        // 计算冲刺结束位置
        private void CalcDragonCarEndPos() {
            startPos = fireGPO.GetPoint();
            startPos.y = 0;
            moveDir = fireGPO.GetForward();
            moveDir.y = 0;
            moveDir.Normalize();
            endPos = startPos + moveDir * flyDisMax;
            var disMax = fightRangeRadius - flyRangeDis;
            if ((endPos - fightRangeCenter).sqrMagnitude > disMax * disMax) {
                endPos = GetRangeMaxDisPos(disMax);
            }
            flyDis = (startPos - endPos).magnitude;
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

        private void PlayWarringEffect() {
            var forward = myAIEntity.transform.eulerAngles;
            forward.x = forward.z = 0f;
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(AbilityM_PlayEffectWithFullDimensionScale.ID_RectangleWarring),
                InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                    In_StartPoint = fireGPO.GetPoint() + new Vector3(0f, 0.2f, 0f),
                    In_StartRota = Quaternion.Euler(forward),
                    In_StartScale = new Vector3(config.M_DamageRadius / 4.5f, 1f, flyDis / 5f),
                    In_LifeTime = config.M_FlyUpTime + config.M_FlyTime,
                },
            });
        }

        private void StartAttack() {
            startPos.y = endPos.y = myAIEntity.GetPoint().y;
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_FlyFlameAnim {});
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_AuroraDragonDragonCarFlame.CreateForKey(config.K_BossType),
                InData = new AbilityIn_AuroraDragonDragonCarFlame() {
                    In_StartPos = startPos,
                    In_EndPos = endPos,
                    In_Speed = config.M_FlySpeed,
                    In_DamageRadius = config.M_DamageRadius,
                    In_DamageInterval = config.M_DamageInterval,
                    In_DamageHeight = config.M_DamageHeight,
                    In_Damage = config.M_ATK,
                    In_LifeTime = config.M_FlyTime + config.M_FlameTime,
                },
            });
        }
        
        private void AttackTimeEvent(float deltaTime) {
            if (myAIEntity.IsClear()) {
                return;
            }

            var newPos = myAIEntity.GetPoint() + moveDir * config.M_FlySpeed * deltaTime;
            if ((newPos - startPos).sqrMagnitude > flyDis * flyDis) {
                newPos = endPos;
            }
            myAIEntity.SetPoint(newPos);
        }
    }
}