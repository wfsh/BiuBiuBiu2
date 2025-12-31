using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Playable.Config;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityDragonFireTornado : ComponentBase {
        private enum AttackState {
            None,
            PreState,
            WarningState,
            AttackState,
            EndState,
        }
        
        private enum TornadoAnimState {
            None,
            Start = AnimConfig_AuroraDragon.Anim_AuroraDragon_FireTornadoStart,
            Loop = AnimConfig_AuroraDragon.Anim_AuroraDragon_FireTornadoLoop,
            Attack = AnimConfig_AuroraDragon.Anim_AuroraDragon_FireTornadoAttack,
            End = AnimConfig_AuroraDragon.Anim_AuroraDragon_FireTornadoEnd,
        }
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityM_DragonFireTornado Param;
        }
        private SAB_DragonFireTornadoSystem abSystem;
        private AbilityM_DragonFireTornado param;
        private ServerGPO fireGPO;
        private Collider[] hitResultArr;
        private float curTime;
        private float checkAttackTimer;
        private float checkDamageTimer = -1f;
        private Vector3 fightRangeCenter;
        private float fightRangeRadius;
        private float changeLoopAnimTime = -1f;
        private bool isStartChangeLoop = false;

        // 生成相关
        private int spawnRound = 0;
        private float attackStateTimeCounter = 0f;
        private List<Vector3> preCreatePoints = new List<Vector3>();
        private List<Vector3> createPoints = new List<Vector3>();
        private bool isEffectCreated = false;
        private TornadoAnimState animState;
        private TornadoAnimState lastAnimState = TornadoAnimState.None;
        private AttackState attackState = AttackState.None;
        private bool isCerateWarringEeffect = false;
        private float checkDamageLife;
        private Dictionary<Vector3, Dictionary<int, float>> pointHitCooldown = new Dictionary<Vector3, Dictionary<int, float>>();

        protected override void OnAwake() {
            base.OnAwake();
            animState = TornadoAnimState.None;
            attackState = AttackState.PreState;
            abSystem = (SAB_DragonFireTornadoSystem)mySystem;
            var initData = (InitData)initDataBase;
            param = initData.Param;
            fireGPO = abSystem.FireGPO;
            hitResultArr = new Collider[20];
            spawnRound = 1;
        }

        protected override void OnStart() {
            base.OnStart();
            #region 埋点
            MsgRegister.Dispatcher(new SM_Sausage.BossReleaseAbility() {
                SourceAbilityType = param.GetTypeID(),
            });
            #endregion
            attackState = AttackState.PreState;
            ChangeAnimState(TornadoAnimState.Start);
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData() {
                CallBack = SetFightRangeDataCallBack
            });
            AddUpdate(OnUpdate);
        }

        private void SetFightRangeDataCallBack(Vector3 center, float radius, float endTime, bool blockDamage) {
            fightRangeCenter = center;
            fightRangeRadius = radius - param.M_Radius;
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            createPoints.Clear();
            fireGPO = null;
            hitResultArr = null;
            abSystem = null;
            param = null;
        }

        private void OnUpdate(float deltaTime) {
            if (fireGPO == null || fireGPO.IsClear()) {
                return;
            }
            
            if (attackState == AttackState.PreState) {
                attackStateTimeCounter += deltaTime;
                if (attackStateTimeCounter >= param.M_PreTime) {
                    AddEndForce();
                    attackState = AttackState.AttackState;
                    attackStateTimeCounter = 0;
                }
                if (!isCerateWarringEeffect) {
                    isCerateWarringEeffect = true;
                    preCreatePoints.AddRange(GenerateFireTornadoPoints());
                    PlayWarningEffect();
                }
            }else if (attackState == AttackState.WarningState) {
                if (!isCerateWarringEeffect) {
                    isCerateWarringEeffect = true;
                    preCreatePoints.AddRange(GenerateFireTornadoPoints());
                    PlayWarningEffect();
                }

                attackStateTimeCounter += deltaTime;
                if (attackStateTimeCounter > param.M_WarmTime) {
                    AddEndForce();
                    attackState = AttackState.AttackState;
                    attackStateTimeCounter = 0;
                }
            }else if (attackState == AttackState.AttackState) {
                ChangeAnimState(TornadoAnimState.Attack);
                attackStateTimeCounter += deltaTime;
                if (attackStateTimeCounter > param.M_AttackEffecPlayTime) {
                    if (!isEffectCreated) {
                        createPoints.Clear();
                        createPoints.AddRange(preCreatePoints);
                        checkDamageLife = param.M_LifeTime;
                        PlayMainEffect();
                        isEffectCreated = true;
                    }
                }
                
                if (attackStateTimeCounter > param.M_AttackSpaceTime) {
                    if (spawnRound < param.M_SpawnCout) {
                        pointHitCooldown.Clear();
                        preCreatePoints.Clear();
                        ChangeAnimState(TornadoAnimState.Loop);
                        spawnRound++;
                        isEffectCreated = false;
                        isCerateWarringEeffect = false;
                        attackStateTimeCounter = 0;
                        attackState = AttackState.WarningState;
                    } else {
                        AddEndForce();
                        attackState = AttackState.EndState;
                        ChangeAnimState(TornadoAnimState.End);
                    }
                }
            }

            CheckDamage(deltaTime);
            PlayAnim();
        }
        
        private void CheckDamage(float deltaTime) {
            if (checkDamageLife > 0) {
                checkDamageLife -= deltaTime;
                checkDamageTimer -= deltaTime;
                if (checkDamageTimer <= 0) {
                    foreach (var point in createPoints) {
                        CheckDamage(point, deltaTime);
                    }
                    checkDamageTimer = 0.1f;
                }
            }
        }
        
        private void PlayAnim() {
            if (lastAnimState != animState) {
                lastAnimState = animState;
                fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_TornadoAnim() {
                    animState = (int)animState
                });
            }
        }
        
        private void ChangeAnimState(TornadoAnimState newState) {
            animState = newState;
        }

        private List<Vector3> GenerateFireTornadoPoints() {
            List<IGPO> playerGpo = new List<IGPO>();
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetAllTargetInFightRange() {
                CallBack = (targetList) => { playerGpo = targetList; },
            });
            if (null == playerGpo || playerGpo.Count <= 0) {
                return new List<Vector3>();
            }
            
            var playerPositions = new List<Vector3>();
            if (playerGpo != null) {
                foreach (var igpo in playerGpo) {
                    var isWeak = false;
                    igpo.Dispatcher(new SE_Character.GetSausageRoleIsWeak() {
                        Callback = v => isWeak = v
                    });
                    if (!igpo.IsDead() && !isWeak) {
                        var addPoint = igpo.GetPoint();
                        addPoint.y = fightRangeCenter.y;
                        playerPositions.Add(addPoint);
                    }
                }
            }
            
            return GenerateDiscretePoints(playerPositions, fightRangeCenter, fightRangeRadius, param.M_Count, param.M_Interval);
        }
        
        private List<Vector3> GenerateDiscretePoints(List<Vector3> playerPositions, Vector3 center, float radius, int targetCount = 10, float minInterval = 5f) {
            var basePoints = new List<Vector3>();
            float minIntervalSqr = minInterval * minInterval;

            // 1. 筛选基础坐标点，间隔大于5
            foreach (var pos in playerPositions) {
                bool canAdd = true;
                foreach (var exist in basePoints) {
                    if ((exist - pos).sqrMagnitude < minIntervalSqr) {
                        canAdd = false;
                        break;
                    }
                }
                if (canAdd) basePoints.Add(pos);
            }

            // 2. 以基础点为中心，围绕基础点随机生成，整体限制在大圆内
            var result = new List<Vector3>(basePoints);
            int tryLimit = (targetCount - result.Count) * 30;
            int tryCount = 0;
            System.Random rand = new System.Random();

            while (result.Count < targetCount && tryCount < tryLimit) {
                if (basePoints.Count < 1) break; // 防止异常
                // 随机选一个基础点为圆心
                var baseIdx = rand.Next(basePoints.Count - 1);
                var basePos = basePoints[baseIdx];

                // 以基础点为圆心，半径为radius/2（可调整），随机生成
                float angle = (float)(rand.NextDouble() * 2 * Math.PI);
                float dist = (float)(rand.NextDouble() * (radius * 0.5f));
                Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * dist;
                Vector3 candidate = basePos + offset;

                // 限制在大圆内
                if ((candidate - center).sqrMagnitude > radius * radius) {
                    tryCount++;
                    continue;
                }

                // 检查与所有已选点间隔
                bool valid = true;
                foreach (var p in result) {
                    if ((p - candidate).sqrMagnitude < minIntervalSqr) {
                        valid = false;
                        break;
                    }
                }
                if (valid) result.Add(candidate);
                tryCount++;
            }

            // 裁剪到目标数量
            if (result.Count > targetCount) result.RemoveRange(targetCount, result.Count - targetCount);

            return result;
        }
        
        private void PlayWarningEffect() {
            foreach (var point in preCreatePoints) {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = fireGPO,
                    MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(AbilityM_PlayEffectWithFullDimensionScale.ID_RoundWarring),
                    InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                        In_StartPoint = point,
                        In_LifeTime = param.M_WarmTime,
                        In_StartScale = new Vector3(param.M_Radius * 2, 1f, param.M_Radius * 2),
                    },
                });
            }
        }

        private void PlayMainEffect() {
            foreach (var point in createPoints) {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                    FireGPO = fireGPO,
                    AbilityMData = new AbilityData.PlayAbility_PlayFillScaleEffect() {
                        ConfigId = param.K_BossType == 1 ? AbilityConfig.AuroraDragonFireTornadoEffect : AbilityConfig.AncientDragonFireTornadoEffect,
                        In_StartPoint = point,
                        In_LifeTime = param.M_LifeTime,
                        In_StartScale = new Vector3(param.M_Radius * 0.5f, param.M_Radius * 0.5f, param.M_Radius * 0.5f),
                    }
                });
            }
        }

        private void CheckDamage(Vector3 centerPoint, float delta) {
            if (!pointHitCooldown.ContainsKey(centerPoint)) {
                pointHitCooldown[centerPoint] = new Dictionary<int, float>();
            }
            var hitCooldown = pointHitCooldown[centerPoint];

            var point1 = centerPoint;
            var point2 = centerPoint + Vector3.up * param.M_Height;
            int length = Physics.OverlapCapsuleNonAlloc(point1, point2, param.M_Radius - 1, hitResultArr, LayerData.ServerLayerMask);
            for (int i = 0; i < length; i++) {
                var hit = hitResultArr[i];
                var hitType = hit.transform.gameObject.GetComponent<HitType>();
                if (hitType == null || hitType.MyEntity == null) continue;
                int gpoId = hitType.MyEntity.GetGPOID();

                // 检查冷却
                float lastHitTime = 0f;
                hitCooldown.TryGetValue(gpoId, out lastHitTime);
                if (Time.realtimeSinceStartup - lastHitTime >= param.M_AttackCheckTime) {
                    var isHit = IsHitGameObj(hit.transform.gameObject);
                    if (isHit) {
                        hitCooldown[gpoId] = Time.realtimeSinceStartup;
                    }
                }
            }
#if UNITY_EDITOR
            DrawWarningRange(centerPoint, param.M_Radius, delta);
#endif
        }

        private bool IsHitGameObj(GameObject gameObj) {
            var hitType = gameObj.GetComponent<HitType>();
            if (hitType != null) {
                if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                    return false;
                }

                if (hitType.MyEntity != null && hitType.MyEntity.GetGPO() == null) {
                    return false;
                }
                var hitGpo = hitType.MyEntity.GetGPO();
                var hitId = hitType.MyEntity.GetGPOID();
                if (hitId == fireGPO.GetGpoID() || fireGPO.GetTeamID() == hitGpo.GetTeamID()) {
                    return false;
                }
   
                var targetGpo = hitType.MyEntity.GetGPO();
                var abilityData = AbilityConfig.GetAbilityModData(AbilityConfig.DragonFullScreenAOE);
                if (abilityData != null) {
                    var abilityName = abilityData.GetTypeID();
                    mySystem.Dispatcher(new SE_Ability.HitGPO {
                        hitGPO = targetGpo,
                        isHead = false,
                        hitPoint = Vector3.zero,
                        SourceAbilityType = param.GetTypeID(),
                    });
                    return true;
                }
            }

            return false;
        }

        private void AddEndForce() {
            List<IGPO> playerGpo = new List<IGPO>();
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetAllTargetInFightRange() {
                CallBack = (targetList) => { playerGpo = targetList; },
            });
            foreach (var roleGpo in playerGpo) {
                if (!roleGpo.IsDead()) {
                    var rolePoint = roleGpo.GetPoint();
                    rolePoint.y = 0;
                    var centerPoint = fireGPO.GetPoint();
                    centerPoint.y = 0;
                    float sqrDistance = (rolePoint - centerPoint).sqrMagnitude;
                    float sqrThreshold = 6.5f * 6.5f;
                    if (sqrDistance < sqrThreshold) {
                        roleGpo.Dispatcher(new SE_Character.AddSausageRoleMoveForce() {
                            CenterPoint = centerPoint,
                        });
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void DrawWarningRange(Vector3 center, float radius, float dt) {
            Color rangeColor = Color.red;
            int segments = 20;
            float height = param.M_Height;
            Vector3 up = Vector3.up * height;
            for (int i = 0; i < segments; i++) {
                float angle1 = 2 * Mathf.PI * i / segments;
                float angle2 = 2 * Mathf.PI * (i + 1) / segments;
                Vector3 p1 = center + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * radius;
                Vector3 p2 = center + new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * radius;
                Vector3 p1Top = p1 + up;
                Vector3 p2Top = p2 + up;
                Debug.DrawLine(p1, p2, rangeColor, dt);
                Debug.DrawLine(p1Top, p2Top, rangeColor, dt);
                Debug.DrawLine(p1, p1Top, rangeColor, dt);
            }
        }
#endif
    }
}