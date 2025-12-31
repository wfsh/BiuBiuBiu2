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
using Sofunny.BiuBiuBiu2.Util;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityGoldJokerRocketBomb : ComponentBase {
        private SAB_GoldJokerRocketBombSystem abSystem;
        private ServerGPO fireGPO;
        private AbilityM_GoldJokerRocketBombSpawner useMData;
        private Vector3 startPoint;
        private Vector3 startDir;
        private Quaternion startRot;
        private float timer;
        private float downTimer;
        private bool isPlayDownEffect;
        private bool isCheckDamage;
        private List<IGPO> gpoList;
        private Vector3 fightRangeCenter;
        private float fightRangeRadius;
        private bool blockDamage;
        private Vector3 attackOffset;
        private int dataIndex = 0;
        private bool syncBoolCallBackCache = false;
        private Action<bool> getIsBeRatBuffNoNeedFindByBossCallBack;
        
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_GoldJokerRocketBombSystem)mySystem;
            useMData = abSystem.useInData.In_Param;
            dataIndex = abSystem.useInData.In_Index;
            fireGPO = abSystem.FireGPO;
            startPoint = fireGPO.GetPoint();
            startDir = fireGPO.GetForward();
            startRot = fireGPO.GetRota();
            var offset = useMData.M_AttackOffset[dataIndex];
            attackOffset = new Vector3(0, offset.y, offset.z);
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = (gpos => gpoList = gpos)
            });
            getIsBeRatBuffNoNeedFindByBossCallBack = isBeRatBuffNoNeedFindByBossCallBack => {
                syncBoolCallBackCache = isBeRatBuffNoNeedFindByBossCallBack;
            };
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
            downTimer = useMData.M_DelayDownTime[dataIndex];
            timer = useMData.M_WarningTime[dataIndex];
            var createPoint = startPoint;
            createPoint.y += attackOffset.y;
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayHollowCircleWarningEffect.CreateForID(useMData.M_BossType == 1 ? AbilityM_PlayHollowCircleWarningEffect.ID_AceJoker : AbilityM_PlayHollowCircleWarningEffect.ID_GoldJoker),
                InData = new AbilityIn_PlayHollowCircleWarningEffect() {
                    In_StartPoint = createPoint,
                    In_LifeTime = useMData.M_WarningTime[dataIndex],
                    In_MaxDistance = useMData.M_MaxDistance[dataIndex],
                    In_AttackOffset = attackOffset.z
                }
            });
            
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData() {
                CallBack = SetFightRangeDataCallBack
            });
            
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffect.CreateForID((byte)useMData.M_StartEffectId[dataIndex]),
                InData = new AbilityIn_PlayEffect() {
                    In_StartPoint = startPoint,
                    In_StartRota = startRot,
                    In_LifeTime = useMData.M_WarningTime[dataIndex]
                }
            });
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            fireGPO = null;
        }

        private void OnUpdate(float delta) {
#if UNITY_EDITOR
            DrawLine();
#endif
            if (downTimer > 0) {
                downTimer -= delta;
            } else if (!isPlayDownEffect) {
                isPlayDownEffect = true;
                PlayDownEffect();
            }

            if (timer > 0) {
                timer -= delta;
            } else if (!isCheckDamage) {
                isCheckDamage = true;
                PlayEffect();
                CheckDamage();
                PlayWalk();
            }
        }
        
        private void SetFightRangeDataCallBack(Vector3 center, float radius, float endTime, bool blockDamage) {
            fightRangeCenter = center;
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

        private void PlayDownEffect() {
            var halfAngle = useMData.M_AttackAngle[dataIndex] / 2;
            var step = useMData.M_AttackAngle[dataIndex] / (useMData.M_DownEffectCount[dataIndex] + 1);
            for (var i = -halfAngle + step; i < halfAngle; i += step) {
                var offset = Quaternion.Euler(0, i, 0) * new Vector3(0, 0, (useMData.M_MaxDistance[dataIndex] + attackOffset.z) / 2);
                var curPos = startPoint + startRot * offset + Vector3.up * (attackOffset.y + useMData.M_DownEffectHeight[dataIndex]);
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = fireGPO,
                    MData = AbilityM_PlayMovingEffect.CreateForID((byte)useMData.M_DownEffectId[dataIndex]),
                    InData = new AbilityIn_PlayMovingEffect {  
                        In_StartPoint = curPos,
                        In_StartLookAt = Vector3.down,
                        In_StartScale = Vector3.one,
                        In_LifeTime = (useMData.M_DownEffectHeight[dataIndex] + 0.3f) / useMData.M_DownEffectSpeed[dataIndex],
                        In_MoveDir = Vector3.down,
                        In_MoveSpeed = useMData.M_DownEffectSpeed[dataIndex],
                    },
                });
            }
        }

        private void PlayEffect() {
            var halfAngle = useMData.M_AttackAngle[dataIndex] / 2;
            var step = useMData.M_AttackAngle[dataIndex] / (useMData.M_DownEffectCount[dataIndex] + 1);
            for (var i = -halfAngle + step; i < halfAngle; i += step) {
                var offset = Quaternion.Euler(0, i, 0) * new Vector3(0, 0, (useMData.M_MaxDistance[dataIndex] + attackOffset.z) / 2);
                var curPos = startPoint + startRot * offset + Vector3.up * attackOffset.y;
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = fireGPO,
                    MData = AbilityM_PlayEffect.CreateForID((byte)useMData.M_BombEffectId[dataIndex]),
                    InData = new AbilityIn_PlayEffect() {
                        In_StartPoint = curPos,
                        In_StartRota = startRot,
                        In_LifeTime = useMData.M_LifeTime[dataIndex] - useMData.M_WarningTime[dataIndex]
                    }
                });
            }
            
        }
        
        private void PlayWalk() {
            fireGPO.Dispatcher(new SE_AI.Event_PlayBossAnim() {
                Id = AnimConfig_GoldDash_BOSSAceJoker.Anim_BOSSAceJoker_Walk
            });
        }

        private void CheckDamage() {
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.GetTeamID() == fireGPO.GetTeamID()) {
                    continue;
                }
                
                gpo.Dispatcher(new SE_Character.GetIsBeRatBuffNoNeedFindByBoss() {
                    CallBack = getIsBeRatBuffNoNeedFindByBossCallBack
                });
                bool isBeRatBuffNoNeedFindByBoss = syncBoolCallBackCache;
                if (gpo.IsDead() || gpo.IsClear() || isBeRatBuffNoNeedFindByBoss) {
                    continue;
                }
                
                if (IsBlockDamage(gpo)) {
                    return;
                }
                var targetDir = gpo.GetPoint() - startPoint;
                targetDir.y = 0;
                if (Vector3.Angle(targetDir, startDir) > useMData.M_AttackAngle[dataIndex] / 2) {
                    continue;
                }
                var dis = Vector3.Distance(startPoint, gpo.GetPoint());
                if (dis > useMData.M_MaxDistance[dataIndex] || dis < attackOffset.z) {
                    continue;
                }
                if (Mathf.Abs(startPoint.y + attackOffset.y - gpo.GetPoint().y) > useMData.M_AttackHeight[dataIndex]) {
                    continue;
                }
                mySystem.Dispatcher(new SE_Ability.HitGPO {
                    hitGPO = gpo,
                    isHead = false,
                    hitPoint = gpo.GetPoint(),
                    HurtRatio = 1f,
                });
            }
        }


        private void DrawLine() {
            var origin = startPoint + Vector3.up * attackOffset.y;
            var start = startPoint + Quaternion.LookRotation(startDir) * attackOffset;
            var end1 = origin + Quaternion.Euler(0, -useMData.M_AttackAngle[dataIndex] / 2, 0) * startDir * useMData.M_MaxDistance[dataIndex];
            var end2 = origin + Quaternion.Euler(0, useMData.M_AttackAngle[dataIndex] / 2, 0) * startDir * useMData.M_MaxDistance[dataIndex];
            DrowCircle(origin, startDir, useMData.M_MaxDistance[dataIndex], useMData.M_AttackAngle[dataIndex]);
            if (attackOffset.z > 0) {
                var start1 = end1 - Quaternion.Euler(0, -useMData.M_AttackAngle[dataIndex] / 2, 0) * startDir * attackOffset.z;
                var start2 = end2 - Quaternion.Euler(0, useMData.M_AttackAngle[dataIndex] / 2, 0) * startDir * attackOffset.z;
                Debug.DrawLine(start1, end1, Color.red);
                Debug.DrawLine(start2, end2, Color.red);
                DrowCircle(origin, startDir, attackOffset.z, useMData.M_AttackAngle[dataIndex]);
            } else {
                Debug.DrawLine(start, end1, Color.red);
                Debug.DrawLine(start, end2, Color.red);
            }
        }

        private void DrowCircle(Vector3 start, Vector3 dir, float dis, float angle) {
            angle = Mathf.Min(angle, 360);
            int delta = 5;
            dir *= dis;
            var prevEnd1 = start + dir;
            var prevEnd2 = start + dir;
            for (int i = 0; i < angle / 2; i += delta) {
                var end1 = start + Quaternion.Euler(0, i, 0) * dir;
                var end2 = start + Quaternion.Euler(0, -i, 0) * dir;
                Debug.DrawLine(prevEnd1, end1, Color.red);
                Debug.DrawLine(prevEnd2, end2, Color.red);
                prevEnd1 = end1;
                prevEnd2 = end2;
            }
        }
    }
}
