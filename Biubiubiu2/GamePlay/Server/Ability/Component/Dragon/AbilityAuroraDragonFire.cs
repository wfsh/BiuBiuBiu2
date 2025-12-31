using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityAuroraDragonFire : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityM_AuroraDragonFire Param;
        }
        private SAB_AuroraDragonFireSystem abSystem;
        private AbilityM_AuroraDragonFire config;
        private ServerGPO fireGPO;
        
        private AIEntity myAIEntity;
        private Transform attackBoxTran;
        private float chargeTime;
        private float attackTime;
        private float waitTime;
        private IGPO attackGpo;
        
        private Vector3 fightRangeCenterPoint;
        private float fightRangeRadius;
        
        private float createChargeEffectTime = 0.2f; // 延迟创建蓄力特效时间
        private float chargeEffectTime;
        
        private float createRayEffectTime; // 延迟创建激光特效时间
        private float endRayEffectTime; // 提前结束激光特效时间（攻击结束动作时间）
        private float rayEffectTime; // 激光特效显示时间
        private float rayEffectLength = 26f; // 激光特效长度
        private float rayEffectCurLength; // 激光特效当前长度

        private Vector3 attackStartPos;
        private Vector3 attackEndPos;
        private Vector3 attackDir;

        private float checkDamageTime;
        private float checkDamageInterval;
        private List<IGPO> targetGPOList;
        private Dictionary<int, float> attackGPOTimeList;
        
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_AuroraDragonFireSystem)mySystem;
            var initData = (InitData)initDataBase;
            config = initData.Param;
            fireGPO = abSystem.FireGPO;
            
            chargeTime = config.M_ChargeTime;
            chargeEffectTime = chargeTime - createChargeEffectTime;
            attackTime = config.M_AttackTime;
            createRayEffectTime = config.M_CreateRayEffectTime;
            endRayEffectTime = config.M_EndRayEffectTime;
            rayEffectTime = config.M_AttackTime - createRayEffectTime - endRayEffectTime;
            waitTime = config.M_WaitTime;
        }
        
        protected override void OnStart() {
            base.OnStart();
            #region 埋点
            MsgRegister.Dispatcher(new SM_Sausage.BossReleaseAbility() {
                SourceAbilityType = config.GetTypeID(),
            });
            #endregion
            myAIEntity = (AIEntity)fireGPO.GetEntity();
            attackBoxTran = myAIEntity.AttackTran;
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetAllTargetInFightRange() {
                CallBack = (targetList) => { targetGPOList = targetList; },
            });
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData {
                CallBack = (Vector3 center, float radius, float endTime, bool blockDamage) => {
                    fightRangeCenterPoint = center;
                    fightRangeRadius = radius;
                }
            });
            // 进入准备阶段
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_FireStartAnim {});
            AddUpdate(OnUpdate);
        }
        
        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireGPO = null;
        }
        
        private void OnUpdate(float deltaTime) {
            if (chargeTime >= 0f) {
                if (createChargeEffectTime >= 0f) {
                    createChargeEffectTime -= deltaTime;
                    if (createChargeEffectTime < 0f) {
                        StartCharge();
                    }
                }
                chargeTime -= deltaTime;
                if (chargeTime < 0f) {
                    // 进入攻击阶段
                    fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_FireForwardAnim {
                        IsTrue = true
                    });
                }
            } else if (attackTime >= 0f) {
                if (createRayEffectTime >= 0f) {
                    createRayEffectTime -= deltaTime;
                    if (createRayEffectTime < 0f) {
                        CreateFireEffect();
                    }
                }
                attackTime -= deltaTime;
                if (attackTime < 0f) {
                    // 退出攻击阶段
                    fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_FireForwardAnim {
                        IsTrue = false
                    });
                } else {
                    AttackTimeEvent(deltaTime);
                }
            }
        }
        
        
        private void StartCharge() {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_AuroraDragonFireEffect.CreateForKey(config.K_BossType, "FireEffect"),
                InData = new AbilityIn_AuroraDragonFireEffect() {
                    In_AttackBoxTran = attackBoxTran,
                    In_LifeTime = chargeEffectTime,
                    In_StartRota = Quaternion.identity,
                    In_StartScale = Vector3.one,
                    In_IsUpdate = false,
                },
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonSkill2Charge,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = config.M_ChargeTime,
                }
            });
        }

        private void CreateFireEffect() {
            // 激光特效
            UpdateRayEffectCurLength();
            Debug.Log( config.M_EffectPos + " -- " +  config.M_EffectRota + " -- " + config.M_EffectScale.x);
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_AuroraDragonFireEffect.CreateForKey(config.K_BossType, "FireEffectRay"),
                InData = new AbilityIn_AuroraDragonFireEffect() {
                    In_AttackBoxTran = attackBoxTran,
                    In_LifeTime = rayEffectTime,
                    In_IsUpdate = true,
                    In_IsFollowHead = false,
                    In_StartPos = config.M_EffectPos,
                    In_StartRota = Quaternion.Euler(config.M_EffectRota),
                    In_StartScale = new Vector3(config.M_EffectScale.x, rayEffectCurLength / rayEffectLength, config.M_EffectScale.z),
                },
            });
            // 嘴部特效
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_AuroraDragonFireEffect.CreateForKey(config.K_BossType, "FireRayStart"),
                InData = new AbilityIn_AuroraDragonFireEffect() {
                    In_AttackBoxTran = attackBoxTran,
                    In_LifeTime = rayEffectTime,
                    In_IsUpdate = false,
                    In_IsFollowHead = true,
                    In_StartPos = new Vector3(0f, -0.43f, 0f),
                    In_StartRota = Quaternion.identity,
                    In_StartScale = Vector3.one,
                },
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonEruption,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = rayEffectTime
                }
            });
        }

        private void AttackTimeEvent(float deltaTime) {
            if (attackBoxTran == null ||
                fireGPO == null) {
                return;
            }

            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_GetAttackTarget {
                CallBack = (gpo) => {
                    attackGpo = gpo;
                }
            });

            if (attackGpo != null) {
                // 朝攻击目标转向
                var targetPos = attackGpo.GetPoint();
                var dest = targetPos - fireGPO.GetPoint();
                var newForward = fireGPO.GetForward();
                MsgRegister.Dispatcher(new SM_Sausage.GetLerpForward {
                    NowForward = newForward,
                    TargetForward = dest.normalized,
                    LerpValue = deltaTime * config.M_AttackRotationSpeed,
                    CallBack = (pos) => { newForward = pos; }
                });
                myAIEntity.LookAT(myAIEntity.GetPoint() + newForward);
            }

            var newPos = myAIEntity.GetPoint() + myAIEntity.GetForward() * deltaTime * config.M_AttackMoveSpeed;
            var disMax = fightRangeRadius - config.M_MoveRangeDis;
            if ((newPos - fightRangeCenterPoint).sqrMagnitude < disMax * disMax) {
                // 移动不能超过场地边缘距离
                myAIEntity.SetPoint(newPos);
            }

            UpdateRayEffectCurLength();
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_AuroraDragonFireEffectScale {
                Scale = new Vector3(config.M_EffectScale.x, rayEffectCurLength / rayEffectLength, config.M_EffectScale.z)
            });

            checkDamageTime += deltaTime;
            if (checkDamageTime >= checkDamageInterval) {
                checkDamageTime = 0;
                if (null == targetGPOList || targetGPOList.Count <= 0) {
                    return;
                }
                // 修正伤害起点，覆盖头下方
                var fixStartPos = attackStartPos - attackDir * config.M_DamageFixLength;
                attackEndPos.y = attackStartPos.y;
                var dirNormal = Vector3.Cross(attackDir, Vector3.up).normalized;
                Vector3[] corners = new Vector3[4];
                corners[0] = fixStartPos - dirNormal * config.M_DamageRadius;
                corners[1] = fixStartPos + dirNormal * config.M_DamageRadius;
                corners[2] = attackEndPos + dirNormal * config.M_DamageRadius;
                corners[3] = attackEndPos - dirNormal * config.M_DamageRadius;
                for (int i = 0; i < targetGPOList.Count; ++i) {
                    var target = targetGPOList[i];
                    if (null != target &&
                        !target.IsDead() &&
                        CheckDamageInterval(target)) {
                        // 高度差检测
                        var targetPos = target.GetBodyTran(GPOData.PartEnum.Head).position;
                        if (Mathf.Abs(targetPos.y - attackStartPos.y) > config.M_DamageHeight) {
                            continue;
                        }

                        MsgRegister.Dispatcher(new SM_Sausage.GetIsPointInRectangleIgnoreY {
                            Pos = targetPos,
                            Corners = corners,
                            CallBack = (isHit) => {
                                if (isHit) {
                                    Dispatcher(new SE_Ability.HitGPO {
                                        hitGPO = target,
                                        isHead = false,
                                        hitPoint = Vector3.zero,
                                        SourceAbilityType = config.GetTypeID(),
                                    });
                                    attackGPOTimeList[target.GetGpoID()] = Time.realtimeSinceStartup;
                                }
                            }
                        });
                    }
                }
            }
        }

        // 检测是否达到伤害间隔
        private bool CheckDamageInterval(IGPO target) {
            if (attackGPOTimeList == null) {
                attackGPOTimeList = new Dictionary<int, float>();
            }

            var checkId = target.GetGpoID();
            if (!attackGPOTimeList.ContainsKey(checkId)) {
                attackGPOTimeList.Add(checkId, 0f);
                return true;
            }

            if (Time.realtimeSinceStartup - attackGPOTimeList[checkId] >= config.M_DamageInterval) {
                return true;
            }
            return false;
        }

        private void UpdateRayEffectCurLength() {
            attackStartPos = attackBoxTran.position + fireGPO.GetRota() * config.M_DamageFixPos;
            attackDir = myAIEntity.GetForward();
            attackDir.y = 0;
            attackDir.Normalize();
            attackEndPos = attackStartPos;
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_GetBlockFireEndPoint {
                FirePos = attackStartPos,
                FireDir = attackDir,
                AttackHeight = 0f,
                AttackLength = config.M_DamageLength,
                CallBack = (pos) => { attackEndPos = pos; }
            });
            rayEffectCurLength = (attackEndPos - attackStartPos).magnitude;
        }
    }
}