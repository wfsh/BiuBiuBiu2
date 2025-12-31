using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIJokerUAVAttack : ComponentBase {
        private IGPO lockGpo;
        private Transform fireBox;
        private float chargingTimer;
        private float fireIntervalTimer;
        private float fireDurationTimer;
        private float forceAttackTimer;
        private bool isStartCharge;
        private bool isStartAutoFire;
        private bool isStartRayFire;
        private float findTargetTimer;
        private Vector3 targetPoint;
        private Vector3 followPoint;
        private Vector3 lastFollowPoint;
        private S_AI_Base aiBase;
        private IAbilitySystem ray;
        private GPOM_JokerUav useMData;
        private IGPO fireGpo;
        private float waitPlayFireSoundTimer = 0.2F;
        
        protected override void OnAwake() {
            base.OnAwake();
            aiBase = (S_AI_Base)mySystem;
            useMData = (GPOM_JokerUav)aiBase.MData;
            fireGpo = aiBase.GetGPO();
            mySystem.Register<SE_AI.Event_SetFireCycle>(OnEnabledAutoFireCallBack);
            mySystem.Register<SE_GPO.Event_GetATK>(OnGetATKCallBack);
            mySystem.Register<SE_AI.Event_TriggerAlertStatus>(EnterAlertStatus);
            mySystem.Register<SE_AI.Event_TriggerFightStatus>(EnterFightStatus);
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnGpoDeadCallBack);
            mySystem.Register<SE_Behaviour.Event_GetForceAttacking>(GetForceAttacking);
        }

        protected override void OnStart() {
            base.OnStart();
            InitConfig();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            fireBox = ((AIEntity)iEntity).AttackTran;
        }

        protected override void OnClear() {
            mySystem.Unregister<SE_AI.Event_SetFireCycle>(OnEnabledAutoFireCallBack);
            mySystem.Unregister<SE_GPO.Event_GetATK>(OnGetATKCallBack);
            mySystem.Unregister<SE_AI.Event_TriggerAlertStatus>(EnterAlertStatus);
            mySystem.Unregister<SE_AI.Event_TriggerFightStatus>(EnterFightStatus);
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnGpoDeadCallBack);
            mySystem.Unregister<SE_Behaviour.Event_GetForceAttacking>(GetForceAttacking);
            fireBox = null;
            RemoveUpdate(OnUpdate);
            base.OnClear();
        }

        private void InitConfig() {
            fireDurationTimer = useMData.AttackIntervalTime;
            Dispatcher(new SE_AI.Event_SetShieldParam() {
                ConfigId = AbilityM_GoldJokerFollowEffect.ID_GoldJokerFollowUAVEffect,
                UpHpConfigId = AbilityM_PlayEffect.ID_BossAceJokerAddHpBuff,
                UpHp = Mathf.CeilToInt(useMData.UpHp),
                UpHpInterval = useMData.UpHpInterval,
                Distance = useMData.ShieldDistance
            });
        }
        
        private void OnEnabledAutoFireCallBack(ISystemMsg body, SE_AI.Event_SetFireCycle ent) {
            isStartAutoFire = ent.isEnabled;
            if (!isStartAutoFire) {
                chargingTimer = useMData.ChargingTime;
            }
        }
        
        private void OnGetATKCallBack(ISystemMsg body, SE_GPO.Event_GetATK ent) {
            ent.CallBack.Invoke(useMData.Atk);
        }
        
        private void EnterAlertStatus(ISystemMsg body, SE_AI.Event_TriggerAlertStatus ent) {
            if (!ent.isEnabled) {
                isStartAutoFire = false;
            }
        }
        
        private void EnterFightStatus(ISystemMsg body, SE_AI.Event_TriggerFightStatus ent) {
            if (!ent.isEnabled) {
                isStartAutoFire = false;
            }
        }
        
        private void GetForceAttacking(ISystemMsg body, SE_Behaviour.Event_GetForceAttacking ent) {
            ent.CallBack(forceAttackTimer > 0);
        }
        
        private void OnGpoDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            MsgRegister.Dispatcher(new SM_Sausage.RemoveHighLevelMonster() {
                GpoId = fireGpo.GetGpoID()
            });
        }
        
        private void OnUpdate(float delta) {
            if (forceAttackTimer > 0) {
                forceAttackTimer -= delta;
                iGPO.Dispatcher(new SE_Behaviour.Event_StopMove());
            }

            if (!isStartAutoFire) {
                followPoint = Vector3.zero;
                return;
            }

            if (iGPO.IsClear() || ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                return;
            }

            iGPO.Dispatcher(new SE_Behaviour.Event_GetMaxHateTargetData() {
                CallBack = FindLockGPO
            });

            if (lockGpo == null) {
                return;
            }
            
            targetPoint = GetTargetPoint();
            if (followPoint == Vector3.zero) {
                followPoint = targetPoint;
            }

            var speed = useMData.FollowSpeed * delta;
            if (Vector3.SqrMagnitude(targetPoint - followPoint) > speed * speed) {
                followPoint += (targetPoint - followPoint).normalized * speed;
            } else {
                followPoint = targetPoint;
            }
            
            iEntity.LookAT(new Vector3(followPoint.x, iEntity.GetPoint().y, followPoint.z));
            if (chargingTimer > 0) {
                if (!isStartCharge) {
                    isStartCharge = true;
                    Charging();
                }

                chargingTimer -= delta;
                return;
            }
            
            var dir = (targetPoint - iGPO.GetPoint()).normalized;
            var distance = Vector3.Distance(iGPO.GetPoint(), targetPoint);
            bool isHitWall = Physics.Raycast(iGPO.GetPoint(), dir, out var htiObj, distance, LayerData.DefaultLayerMask);
            if (isHitWall) {
                followPoint = htiObj.point;
            }
            
            isStartCharge = false;
            if (useMData.IsRay) {
                RayAttack(delta);
            } else {
                BulletAttack(delta);
            }
            
       
            if (waitPlayFireSoundTimer > 0) {
                waitPlayFireSoundTimer -= delta;
                if (waitPlayFireSoundTimer <= 0) {
                    waitPlayFireSoundTimer = 0.2f;
                    iGPO.Dispatcher(new SE_AI.Event_SetJokerDroneAttackState() {
                        isAttack = true
                    });
                    iGPO.Dispatcher(new SE_AI.Event_PlayJokerDroneAttackAnim() {
                        isRayAttack = useMData.IsRay
                    });
                }
            }
        }

        private void BulletAttack(float delta) {
            if (fireDurationTimer > 0) {
                fireDurationTimer -= delta;

                if (fireIntervalTimer > 0) {
                    fireIntervalTimer -= delta;
                    return;
                }

                fireIntervalTimer = useMData.AttackIntervalTime;
                BulletFire();
            } else {
                fireDurationTimer = useMData.Duration;
                chargingTimer = useMData.ChargingTime;
            }
        }

        private void RayAttack(float delta) {
            if (fireDurationTimer > 0) {
                fireDurationTimer -= delta;
                if (!isStartRayFire) {
                    RayFire();
                    isStartRayFire = true;
                    forceAttackTimer = useMData.Duration;
                }
            } else {
                fireDurationTimer = useMData.Duration;
                chargingTimer = useMData.ChargingTime;
                isStartRayFire = false;
            }
            
            if (Vector3.SqrMagnitude(lastFollowPoint - followPoint) > 0.025f) {
                ray?.Dispatcher(new SE_Entity.SyncRota {
                    Rota = Quaternion.LookRotation(followPoint - fireBox.position)
                });
                lastFollowPoint = followPoint;
            }
        }

        private void Charging() {
            if (useMData.ChargeEffectId > 0) {
                var firePoint = fireBox.position;
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = iGPO,
                    MData = AbilityM_PlayEffect.CreateForID((byte)useMData.ChargeEffectId),
                    InData = new AbilityIn_PlayEffect() {
                        In_StartPoint = firePoint,
                        In_StartRota = iGPO.GetRota(),
                        In_LifeTime = useMData.ChargingTime
                    }   
                });
            }
        }

        private void BulletFire() {
            var firePoint = fireBox.position;
            var endPoint = GetRandomRange(firePoint, followPoint);
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO,
                MData = AbilityM_Bullet.CreateForID(AbilityM_Bullet.ID_JokerUAVSpadesBullet),
                InData = new AbilityIn_Bullet() {
                    In_StartPoint = firePoint,
                    In_TargetPoint = endPoint,
                    In_Speed = useMData.BulletMoveSpeed,
                    In_MoveDistance = useMData.MaxAttackDistance,
                }
            });
            
            if (useMData.FireEffectId > 0) {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = iGPO,
                    MData = AbilityM_PlayEffect.CreateForID((byte)useMData.FireEffectId),
                    InData = new AbilityIn_PlayEffect() {
                        In_StartPoint = firePoint,
                        In_StartRota = iGPO.GetRota(),
                        In_LifeTime = 0.4f
                    }
                });
                
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = iGPO,
                    MData = AbilityM_PlayWWiseAudio.Create(),
                    InData = new AbilityIn_PlayWWiseAudio() {
                        In_WWiseId = WwiseAudioSet.Id_GoldDashJorkerDroneSpadesFire,
                        In_StartPoint = firePoint,
                        In_LifeTime = 0.5f,
                    }
                });
            }
        }

        private void RayFire() {
            var firePoint = fireBox.position;
            lastFollowPoint = followPoint;
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO,
                MData = AbilityM_PlayRay.CreateForID((byte)useMData.AttackEffectId),
                InData = new AbilityIn_PlayRay() {
                    In_StartPoint = fireBox.position,
                    In_Dir = (followPoint - firePoint).normalized,
                    In_MaxDistance = Mathf.CeilToInt(useMData.MaxAttackDistance),
                    In_RayATK = useMData.Atk,
                    In_RayATKInterval = useMData.AttackIntervalTime,
                    In_LifeTime = useMData.Duration,
                    In_IsFollowFireGPO = false,
                },
                OR_CallBack = ab => ray = ab
            });
            
            if (useMData.FireEffectId > 0) {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = iGPO,
                    MData = AbilityM_PlayEffect.CreateForID((byte)useMData.FireEffectId),
                    InData = new AbilityIn_PlayEffect() {
                        In_StartPoint = firePoint,
                        In_StartRota = iGPO.GetRota(),
                    }   
                });
            }
        }

        private Vector3 GetRandomRange(Vector3 startPoint, Vector3 endPoint) {
            var scale = Vector3.SqrMagnitude(startPoint - endPoint) / (useMData.MaxAttackDistance * useMData.MaxAttackDistance);
            return endPoint + scale * Random.insideUnitSphere * useMData.FireRange;
        }

        private void FindLockGPO(IGPO gpo, float value) {
            lockGpo = gpo;
            if (lockGpo != null && lockGpo.IsClear()) {
                lockGpo = null;
            }
        }

        private Vector3 GetTargetPoint() {
            var targetBody = lockGpo.GetBodyTran(GPOData.PartEnum.Body);
            var point = lockGpo.GetPoint();
            if (targetBody == null) {
                point.y += 0.5f;
            } else {
                point = targetBody.position;
            }

            point.y += 0.5f;
            return point;
        }
    }
}