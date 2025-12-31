using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_BulletWithStartPointSystem : S_Ability_Base {
        private AbilityData.PlayAbility_BulletWithStartPos _abilityData;
        private float lifeTime = 0.0f;

        protected override void OnAwake() {
            base.OnAwake();
            _abilityData = (AbilityData.PlayAbility_BulletWithStartPos)MData;
            lifeTime = _abilityData.In_MoveDistance / _abilityData.In_Speed;
            iEntity.SetPoint(_abilityData.In_StartPoint);
            iEntity.SetRota(Quaternion.LookRotation((_abilityData.In_TargetPoint - _abilityData.In_StartPoint).normalized));
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddMove();
            AddSelect();
            AddHit();
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = lifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void AddMove() {
            AddComponent<MoveLineConstantSpeed>(new MoveLineConstantSpeed.InitData {
                Speed = _abilityData.In_Speed,
                MaxDistance = _abilityData.In_MoveDistance
            });
        }

        private void AddSelect() {
            AddComponent<BulletSelectGPO>(new BulletSelectGPO.InitData {
                IgnoreTeamId = FireGPO.GetTeamID(),
                IgnoreGpoID = FireGPO.GetGpoID(),
                HitCallBack = (o, hit) => {
                    LifeTimeEnd();
                }
            });
            AddComponent<MoveTargetRaycastHit>(new MoveTargetRaycastHit.InitData {
                IgnoreTeamId = FireGPO.GetTeamID(),
                IsIgnoreCollierTrigge = false,
                LayerMask = LayerData.ServerLayerMask | LayerData.DefaultLayerMask,
                MaxDistance = _abilityData.In_MoveDistance,
                HitCallBack = OnRpcAbility
            });
        }

        private void OnRpcAbility(bool isHit, Vector3 hitPoint, RaycastHit hitRay) {
            if (isHit == false) {
                RPCAbility(new Proto_Ability.Rpc_BulletFireWithStartPoint {
                    speed = (ushort)Mathf.CeilToInt(_abilityData.In_Speed * 10f),
                    startPoint = _abilityData.In_StartPoint,
                    targetPoint = hitPoint,
                    abilityModId = _abilityData.ConfigId,
                });
            } else {
                RPCAbility(new Proto_Ability.Rpc_BulletFireDecalWithStartPoint {
                    speed = (ushort)Mathf.CeilToInt(_abilityData.In_Speed * 10f),
                    startPoint = _abilityData.In_StartPoint,
                    targetPoint = hitPoint,
                    targetNormal = hitRay.normal,
                    abilityModId = _abilityData.ConfigId,
                });
            }
        }

        private void AddHit() {
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = _abilityData.M_Power,
                WeaponItemId = _abilityData.In_WeaponItemId,
                Atk = _abilityData.M_ATK + _abilityData.In_RandomAtk,
                AttnMaps = _abilityData.M_BulletAttnMap,
                MaxDistance = _abilityData.In_MoveDistance,
                HitSplatter = _abilityData.M_HitSplatter
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}