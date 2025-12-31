using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Util;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityGoldJokerDollBombSpawner : ComponentBase {
        private enum State {
            Warn,
            Down,
            Spawn,
            Over
        }
        
        private AbilityM_GoldJokerDollBombSpawner useMData;
        private SAB_GoldJokerDollBombSpawnerSystem abSystem;
        private ServerGPO fireGPO;
        private float timer;
        private State state;
        private bool isCreateRobot = false;
        private List<Vector3> points;
        private Vector3 fightRangeCenter;
        private float fightRangeRadius;
        private bool blockDamage;
        private float waitCreateRobotTime = 0.05f;
        private int createRobotIndex = 0;

        protected override void OnAwake() {
            abSystem = (SAB_GoldJokerDollBombSpawnerSystem)mySystem;
            useMData = (AbilityM_GoldJokerDollBombSpawner)abSystem.MData;
            fireGPO = abSystem.FireGPO;
        }

        protected override void OnStart() {
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData() {
                CallBack = SetFightRangeDataCallBack
            });

            fireGPO.Dispatcher(new SE_AI.Event_PlayBossAnim() {
                Id = AnimConfig_GoldDash_BOSSAceJoker.Anim_BOSSAceJoker_DollBomb
            });
            var playerPositions = GetPlayerPositions();
            if (playerPositions.Count > 0) {
                isCreateRobot = true;
                points = GenerateDiscretePoints(playerPositions, fightRangeCenter, fightRangeRadius, useMData.M_RobotCount, useMData.M_SpawnInterval);
                for (int i = 0; i < points.Count; i++) {
#if UNITY_EDITOR
                    Debug.DrawRay(points[i] + Vector3.up * 5, Vector3.down * 10, Color.red);
#endif
                    if (Physics.Raycast(points[i] + Vector3.up * 5, Vector3.down, out var hit, 10, LayerData.DefaultLayerMask)) {
                        points[i] = hit.point;
                    }
                }
            }
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossjorkerClownDollStart,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = 1.1f,
                }
            });
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            fireGPO = null;
        }

        private void OnUpdate(float delta) {
            if (!isCreateRobot) {
                return;
            }
            timer -= delta;
            
            if (timer <= 0) {
                switch (state) {
                    case State.Warn:
                        timer = useMData.M_WarningTime;
                        state = State.Spawn;
                        foreach (var point in points) {
                            PlayWarning(point);
                        }
                        break;
                    case State.Spawn:
                        waitCreateRobotTime-= delta;
                        if (waitCreateRobotTime <= 0) {
                            if (createRobotIndex == points.Count) {
                                state = State.Over;
                                break;
                            }
                            waitCreateRobotTime = 0.02f;
                            var point = points[createRobotIndex];
                            SpawnRobot(point);
                            createRobotIndex++;
                        }
                        break;
                }   
            }
        }
        
        private void SetFightRangeDataCallBack(Vector3 center, float radius, float endTime, bool blockDamage) {
            fightRangeCenter = center - Vector3.up * 3.5f;
            fightRangeRadius = radius;
            this.blockDamage = blockDamage;
        }

        private bool IsBlockDamage(IGPO gpo) {
            if (!blockDamage) {
                return false;
            }

            Vector3 gpoPoint = gpo.GetPoint();
            gpoPoint.y = fightRangeCenter.y;
            float sqrtDisToOri = (gpoPoint - fightRangeCenter).sqrMagnitude;
            if (sqrtDisToOri >= fightRangeRadius * fightRangeRadius) {
                return true;
            }

            return false;
        }

        private List<Vector3> GetPlayerPositions() {
            List<Vector3> playerPositions = new List<Vector3>();
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = gpos => {
                    foreach (var gpo in gpos) {
                        if (gpo.GetTeamID() == fireGPO.GetTeamID()) {
                            continue;
                        }

                        if (gpo.IsDead() || gpo.IsClear()) {
                            continue;
                        }

                        if (IsBlockDamage(gpo)) {
                            continue;
                        }
                        
                        var distanceCenter = (gpo.GetPoint() - fightRangeCenter).sqrMagnitude;
                        if (distanceCenter > fightRangeRadius * fightRangeRadius) {
                            continue;
                        }

                        var point = gpo.GetPoint();
                        point.y = fightRangeCenter.y;
                        playerPositions.Add(point);
                    }
                }
            });

            return playerPositions;
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
                // 随机选一个基础点为圆心
                var baseIdx = rand.Next(basePoints.Count);
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

        private void PlayWarning(Vector3 point) {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                FireGPO = fireGPO,
                AbilityMData = new AbilityData.PlayAbility_PlayWarningEffect() {
                    ConfigId = AbilityConfig.PlayCircleWarningEffect,
                    In_StartPoint = point,
                    In_LifeTime = useMData.M_WarningTime,
                    In_StartScale = new Vector3(useMData.M_AttackRange, 1f, useMData.M_AttackRange),
                }
            });
            
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffect.CreateForID((byte)useMData.M_DownStartEffectId),
                InData = new AbilityIn_PlayEffect() {
                    In_StartPoint = point,
                    In_StartRota = Quaternion.identity,
                    In_LifeTime = useMData.M_WarningTime
                },
            });
            
            // 下落特效
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffect.CreateForID((byte)useMData.M_DownEffectId),
                InData = new AbilityIn_PlayEffect {
                    In_StartPoint = point + Vector3.up * useMData.M_HeightAboveGround,
                    In_StartRota = iEntity.GetRota()
                }
            });
            
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayMovingEffect.CreateForID((byte)useMData.M_DownMoveEffectId),
                InData = new AbilityIn_PlayMovingEffect {
                    In_StartPoint = point + Vector3.up * useMData.M_HeightAboveGround,
                    In_StartLookAt = fireGPO.GetForward(),
                    In_LifeTime = useMData.M_WarningTime,
                    In_MoveDir = Vector3.down,
                    In_MoveSpeed = useMData.M_HeightAboveGround / useMData.M_WarningTime,
                    In_StartScale = Vector3.one,
                }
            });
        }
        
        private void SpawnRobot(Vector3 point) {
            //需要修改的小丑技能逻辑
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_GoldJokerDollBomb.CreateForID((byte)useMData.M_RobotEffectId),
                InData = new AbilityIn_GoldJokerDollBomb() {
                    In_StartPoint = point,
                    In_StartRot = Quaternion.identity,
                    In_Param = useMData
                }
            });
        }
    }
}
