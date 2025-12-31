using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_BulletSystem : S_Ability_Base {
        private static int rpcBulletCount = 0;
        public static int IgnoreRpcBulletCount = 0;
        private static float startCheckTime = 0f;
        private AbilityM_Bullet useMData;
        private AbilityIn_Bullet useInData;
        private float lifeTime = 0.0f;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_Bullet)MData;
            useInData = (AbilityIn_Bullet)InData;
            lifeTime = useInData.In_MoveDistance / useInData.In_Speed;
            iEntity.SetPoint(useInData.In_StartPoint);
            iEntity.SetRota(Quaternion.LookRotation((useInData.In_TargetPoint - useInData.In_StartPoint).normalized));
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddMove();
            AddSelect();
            AddHit();
            AddComponentByConfigId();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = lifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void AddMove() {
            AddComponent<MoveLineConstantSpeed>(new MoveLineConstantSpeed.InitData {
                MaxDistance = useInData.In_MoveDistance,
                Speed = useInData.In_Speed,
            });
        }

        private void AddSelectByConfigId() {
            switch (useMData.K_ID) {
                case AbilityM_Bullet.ID_BulletRPG:
                case AbilityM_Bullet.ID_BulletParticlecannon:
                    AddComponent<RPGSelectGPO>(new RPGSelectGPO.InitData {
                        AbilityMData = useMData,
                        IgnoreTeamId = FireGPO.GetTeamID(),
                        IgnoreGpoID = FireGPO.GetGpoID(),
                        HitCallBack = (o, hit) => {
                            LifeTimeEnd();
                        }
                    });
                    break;
                case AbilityM_Bullet.ID_BulletFlashGun:
                    AddComponent<FlashGunBulletSelectGPO>(new FlashGunBulletSelectGPO.InitData {
                        AbilityMData = useInData,
                        IgnoreTeamId = FireGPO.GetTeamID(),
                        IgnoreGpoID = FireGPO.GetGpoID(),
                        HitCallBack = (o, hit) => {
                            LifeTimeEnd();
                        }
                    });
                    break;
                case AbilityM_Bullet.ID_BulletMachineGun:
                    AddComponent<MachineGunBulletSelectGPO>(new MovePointRaycastHit.InitData {
                        IgnoreTeamId = FireGPO.GetTeamID(),
                        IgnoreGpoID = FireGPO.GetGpoID(),
                        HitCallBack = (o, hit) => {
                            LifeTimeEnd();
                        }
                    });
                    break;
                default:
                    AddComponent<BulletSelectGPO>(new MovePointRaycastHit.InitData {
                        IgnoreTeamId = FireGPO.GetTeamID(),
                        IgnoreGpoID = FireGPO.GetGpoID(),
                        HitCallBack = (o, hit) => {
                            LifeTimeEnd();
                        }
                    });
                    break;
            }
        }

        private void AddSelect() {
            AddSelectByConfigId();
            CheckFireLimit();
            if (rpcBulletCount >= 10) {
                IgnoreRpcBulletCount++;
                return;
            }
            if (startCheckTime <= 0f) {
                startCheckTime = Time.realtimeSinceStartup;
            }
            AddComponent<MoveTargetRaycastHit>(new MoveTargetRaycastHit.InitData {
                IgnoreTeamId = FireGPO.GetTeamID(),
                IgnoreGpoID = FireGPO.GetGpoID(),
                LayerMask = LayerData.ServerLayerMask | LayerData.DefaultLayerMask,
                IsIgnoreCollierTrigge = false,
                MaxDistance = useInData.In_MoveDistance,
                HitCallBack = OnRpcAbility
            });
        }

        private void CheckFireLimit() {
            if (startCheckTime > 0f && Time.realtimeSinceStartup - startCheckTime > 0.05f) {
                startCheckTime = 0f;
                rpcBulletCount = 0;
            }
        }

        private void OnRpcAbility(bool isHit, Vector3 hitPoint, RaycastHit hitRay) {
            rpcBulletCount++;
            if (isHit == false) {
                RPCAbility(new Proto_Ability.Rpc_BulletFire {
                    configId = ConfigID,
                    rowId = RowId,
                    speed = (ushort)Mathf.CeilToInt(useInData.In_Speed * 10f),
                    targetPoint = hitPoint,
                });
            } else {
                RPCAbility(new Proto_Ability.Rpc_BulletFireDecal {
                    configId = ConfigID,
                    rowId = RowId,
                    speed = (ushort)Mathf.CeilToInt(useInData.In_Speed * 10f),
                    targetPoint = hitPoint,
                    targetNormal = hitRay.normal,
                });
            }
        }

        private void AddHit() {
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                HitSplatter = useMData.M_HitSplatter,
                DamageType = DamageType.Normal,
                Power = useMData.M_Power,
                WeaponItemId = useInData.In_WeaponItemId,
                AttnMaps = useInData.In_BulletAttnMap,
                MaxDistance = useInData.In_MoveDistance,
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }

        private void AddComponentByConfigId() {
            if (ConfigID == AbilityM_Bullet.ID_BulletSplit) {
                AddComponent<BulletSplitTimeEnd>(new BulletSplitTimeEnd.InitData {
                    inData = useInData,
                    FireGPO = FireGPO,
                });
            }
        }
    }
}