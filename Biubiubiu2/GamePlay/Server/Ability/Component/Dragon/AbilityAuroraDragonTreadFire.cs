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
    public class AbilityAuroraDragonTreadFire : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityM_AuroraDragonTreadFire Param;
        }
        private SAB_AuroraDragonTreadFireSystem abSystem;
        private AbilityM_AuroraDragonTreadFire config;
        private ServerGPO fireGPO;
        private float startDamegeTime;
        private float damegeTime;
        private Vector3 attackPoint;
        private float attackRota;
        private float checkDamageTime;
        private Transform attackBoxTran;
        
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_AuroraDragonTreadFireSystem)mySystem;
            var initData = (InitData)initDataBase;
            config = initData.Param;
            fireGPO = abSystem.FireGPO;
            startDamegeTime = config.M_CheckDamageTime;
            damegeTime = config.M_DamageTime;
            attackRota = config.M_AttackRotationStart;
        }
        
        protected override void OnStart() {
            base.OnStart();
            var entity = (AIEntity)fireGPO.GetEntity();
            attackBoxTran = entity.AttackTran;
            AddUpdate(OnUpdate);
            attackPoint = fireGPO.GetPoint() + fireGPO.GetForward() * config.M_AttackFixDis;
            PlayAttackStartAnim(true);
        }
        
        protected override void OnClear() {
            PlayAttackStartAnim(false);
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireGPO = null;
        }

        private void OnUpdate(float deltaTime) {
            UpdateFirePoint();
            if (startDamegeTime > 0f) {
                startDamegeTime -= deltaTime;
                if (startDamegeTime <= 0f) {
                    // 开始攻击
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = fireGPO,
                        MData = AbilityM_PlayWWiseAudio.Create(),
                        InData = new AbilityIn_PlayWWiseAudio() {
                            In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonFireCom,
                            In_StartPoint = fireGPO.GetPoint(),
                            In_LifeTime = damegeTime,
                        }
                    });
                }
            } else if (damegeTime > 0f) {
                damegeTime -= deltaTime;
                CheckDamage(deltaTime);
            }
        }

        private void UpdateFirePoint() {
            if (attackBoxTran == null) {
                return;
            }
            iEntity.SetPoint(attackBoxTran.position);
            iEntity.SetRota(attackBoxTran.rotation);
        }
        
        private void PlayAttackStartAnim(bool isTrue) {
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_TreadFireAnim {
                IsTrue = isTrue,
            });
            if (isTrue) {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = fireGPO,
                    MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(AbilityM_PlayEffectWithFullDimensionScale.ID_FanWarring),
                    InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                        In_StartPoint = attackPoint,
                        In_StartRota = fireGPO.GetRota(),
                        In_StartScale = new Vector3(config.M_WarringEffectWidth, 2f, config.M_AttackRadius / 0.5f),
                        In_LifeTime = config.M_WarringEffectTime,
                    },
                });
            }
        }

        private void CheckDamage(float deltaTime) {
            attackRota -= config.M_AttackRotationSpeed * deltaTime;
#if UNITY_EDITOR
            DrawWarningRange(attackPoint, attackRota, config.M_AttackRadius);
#endif
            checkDamageTime += deltaTime;
            if (checkDamageTime < config.M_DamageInterval) {
                return;
            }
            checkDamageTime = 0;

            if (fireGPO == null || fireGPO.IsDead() || fireGPO.IsClear()) {
                return;
            }
            List<IGPO> targetGPOList = null;
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetAllTargetInFightRange() {
                CallBack = (targetList) => { targetGPOList = targetList; },
            });
            if (null == targetGPOList || targetGPOList.Count <= 0) {
                return;
            }

            var attackDir = Quaternion.Euler(0f, attackRota, 0f) * fireGPO.GetForward();
            for (int i = 0; i < targetGPOList.Count; ++i) {
                var target = targetGPOList[i];
                if (target == null || target.IsDead() || target.IsClear()) {
                    continue;
                }
                
                var dest = target.GetPoint() - attackPoint;
                // 高度差检测
                if (Mathf.Abs(dest.y) > config.M_AttackHeight) {
                    continue;
                }

                dest.y = 0;
                float dist = dest.magnitude;

                if (dist > config.M_AttackRadius) {
                    continue;
                }

                dest.Normalize();
                if (dest.x * attackDir.x + dest.z * attackDir.z < Mathf.Cos(config.M_AttackAngle * Mathf.PI / 360)) {
                    continue;
                }

                mySystem.Dispatcher(new SE_Ability.HitGPO {
                    hitGPO = target,
                    isHead = false,
                    hitPoint = Vector3.zero,
                    SourceAbilityType = config.GetTypeID(),
                });
            }
        }
        
#if UNITY_EDITOR
        private void DrawWarningRange(Vector3 center, float rota, float radius) {
            var curRota = fireGPO.GetForward();
            var rotaMin = rota + config.M_AttackAngle / 2f;
            var rotaMax = rota - config.M_AttackAngle / 2f;
            var dirMin = Quaternion.Euler(0f, rotaMin, 0f) * curRota;
            dirMin.Normalize();
            var dirMax = Quaternion.Euler(0f, rotaMax, 0f) * curRota;
            dirMax.Normalize();
            var posMin = center + dirMin * radius;
            var posMax = center + dirMax * radius;
            Debug.DrawLine(center, posMin, Color.red);
            Debug.DrawLine(center, posMax, Color.blue);
        }
#endif
    }
}