using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Util;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityGoldJokerFloatingGun : ComponentBase {
        private SAB_GoldJokerFloatingGunSystem abSystem;
        private AbilityM_GoldJokerFloatingGunSpawner useMData;
        private AbilityIn_GoldJokerFloatingGun useInData;
        private AbilityM_GoldJokerFloatingGun useFloatingMData;
        private ServerGPO fireGPO;
        private Vector3 followDir;
        private Vector3 followPoint;
        private List<IGPO> gpoList;
        private IGPO lockGPO;
        private Vector3 initDir;
        private IAbilitySystem ray;

        private Vector3 offset;
        private float warningTimer;
        private float attackTimer;
        private Vector3 fightRangeCenter;
        private float fightRangeRadius;
        private bool blockDamage;
        private bool isLoadWarningEffect = false;
        private bool syncBoolCallBackCache = false;
        private Action<bool> getIsBeRatBuffNoNeedFindByBossCallBack;
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_GoldJokerFloatingGunSystem)mySystem;
            useInData = (AbilityIn_GoldJokerFloatingGun)abSystem.InData;
            useFloatingMData = (AbilityM_GoldJokerFloatingGun)abSystem.MData;
            useMData = useInData.In_Param;
            fireGPO = abSystem.FireGPO;
            initDir = iEntity.GetRota() * Vector3.forward;
            warningTimer = useMData.M_WarningTime;
            offset = fireGPO.GetPoint() - iEntity.GetPoint();
            isLoadWarningEffect = false;
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = (gpos => gpoList = gpos)
            });
            getIsBeRatBuffNoNeedFindByBossCallBack = isBeRatBuffNoNeedFindByBossCallBack => {
                syncBoolCallBackCache = isBeRatBuffNoNeedFindByBossCallBack;
            };
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
            
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData() {
                CallBack = SetFightRangeDataCallBack
            });
            FindLockGPO();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireGPO = null;
        }

        private void OnUpdate(float deltaTime) {
            if (fireGPO.IsDead() || fireGPO.IsClear()) {
                return;
            }

            FindLockGPO();
            if (lockGPO == null) {
                followPoint = Vector3.zero;
                followDir = initDir;
            } else {
                var targetPoint = GetTargetPoint(lockGPO);
                if (followPoint == Vector3.zero) {
                    followPoint = targetPoint;
                }

                var speed = useMData.M_FollowSpeed * deltaTime;
                if (Vector3.SqrMagnitude(targetPoint - followPoint) > speed * speed) {
                    followPoint += (targetPoint - followPoint).normalized * speed;
                } else {
                    followPoint = targetPoint;
                }

                followDir = followPoint - iEntity.GetPoint();
            }

            iEntity.SetPoint(fireGPO.GetPoint() - offset);
            iEntity.SetRota(Quaternion.LookRotation(followDir));
            if (warningTimer > 0) {
                warningTimer -= deltaTime;
                if (lockGPO == null) {
                    ClearRay();
                } else if (ray != null) {
                    ray.Dispatcher(new SE_Entity.SyncPointAndRota {
                        Point = iEntity.GetPoint(),
                        Rota = iEntity.GetRota(),
                    });
                } else if(ray == null) {
                    PlayWarning(iEntity.GetPoint(), followDir);
                }

                return;
            }
            
            ClearRay();
            if (attackTimer > 0) {
                attackTimer -= deltaTime;
            } else {
                attackTimer = useMData.M_AttackInterval;
                BulletFire(iEntity.GetPoint(), iEntity.GetPoint() + followDir * useMData.M_MaxDistance);
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

        private void ClearRay() {
            if (ray != null && !ray.IsClear()) {
                MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                    AbilityId = ray.GetAbilityId()
                });

                ray = null;
            }
        }

        private void FindLockGPO() {
            lockGPO = null;
            float minDis = int.MaxValue;
            float minAngle = float.MaxValue;
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
                
                var isWeak = false;
                gpo.Dispatcher(new SE_Character.GetSausageRoleIsWeak() {
                    Callback = v => isWeak = v
                });
                
                if (isWeak) {
                    return;
                }
                
                if (IsBlockDamage(gpo)) {
                    return;
                }

                var targetDir = GetTargetPoint(gpo) - fireGPO.GetPoint();
                targetDir.y = 0;
                float angle = Vector3.Angle(targetDir, initDir);
                if (angle > useFloatingMData.M_Angle / 2) {
                    continue;
                }
                
                float dis = targetDir.sqrMagnitude;
                if (dis > useMData.M_MaxDistance * useMData.M_MaxDistance) {
                    continue;
                }
                
                // 先找最小角度，再在最小角度范围内找最近距离
                if (angle < minAngle || (Mathf.Approximately(angle, minAngle) && dis < minDis)) {
                    minAngle = angle;
                    minDis = dis;
                    lockGPO = gpo;
                }
            }
        }

        private void PlayWarning(Vector3 startPoint, Vector3 lookAt) {
            if (!isLoadWarningEffect) {
                isLoadWarningEffect = true;
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = fireGPO,
                    MData = AbilityM_PlayRay.CreateForID((byte)useMData.M_RayEffectId),
                    InData = new AbilityIn_PlayRay() {
                        In_StartPoint = startPoint,
                        In_Dir = lookAt,
                        In_MaxDistance = useMData.M_MaxDistance,
                        In_LifeTime = warningTimer - 0.2f,
                        In_RayATK = 0,
                    },
                    OR_CallBack = ab => {
                        ray = ab;
                        isLoadWarningEffect = false;
                    }
                });
            
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = fireGPO,
                    MData = AbilityM_PlayWWiseAudio.Create(),
                    InData = new AbilityIn_PlayWWiseAudio() {
                        In_WWiseId = WwiseAudioSet.Id_GoldDashBossjorkerGunAim,
                        In_StartPoint = startPoint,
                        In_LifeTime = warningTimer
                    }
                });
            }
        }

        private void BulletFire(Vector3 startPoint, Vector3 followPoint) {
            if (lockGPO == null) {
                return;
            }
            
            var firePoint = startPoint;
            var endPoint = GetRandomRange(followPoint);
            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                FireGPO = fireGPO,
                AbilityMData = new AbilityData.PlayAbility_BulletWithStartPos() {
                    ConfigId = useMData.M_BulletEffectId,
                    In_StartPoint = firePoint,
                    In_TargetPoint = endPoint,
                    In_Speed = useMData.M_MoveSpeed,
                    In_MoveDistance = useMData.M_MaxDistance,
                }
            });

            if (useMData.M_FireEffectId > 0) {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = fireGPO,
                    MData = AbilityM_PlayEffect.CreateForID((byte)useMData.M_FireEffectId),
                    InData = new AbilityIn_PlayEffect() {
                        In_StartPoint = firePoint,
                        In_StartRota = Quaternion.FromToRotation(Vector3.forward, followPoint - firePoint),
                        In_LifeTime = 0.4f
                    }
                });
            }
        }

        private Vector3 GetRandomRange(Vector3 endPoint) {
            return endPoint + Random.insideUnitSphere * useMData.M_FireRange;
        }

        private Vector3 GetTargetPoint(IGPO gpo) {
            var targetBody = gpo.GetBodyTran(GPOData.PartEnum.Body);
            var point = gpo.GetPoint();
            if (targetBody == null) {
                point.y += 0.5f;
            } else {
                point = targetBody.position;
            }

            return point;
        }
    }
}
