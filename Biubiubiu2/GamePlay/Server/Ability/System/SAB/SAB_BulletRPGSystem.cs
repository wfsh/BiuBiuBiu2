using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_BulletRPGSystem : S_Ability_Base {
        private AbilityData.PlayAbility_RPG _abilityData;

        protected override void OnAwake() {
            base.OnAwake();
            _abilityData = (AbilityData.PlayAbility_RPG)MData;
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
            AddComponent<ServerAbilityStrikeFlyGPO>();
        }

        protected override void OnStart() {
            base.OnStart();
            var forwart = iEntity.GetRota() * Vector3.forward;
            var maxEndPoint = iEntity.GetPoint() + forwart * 200;
            RPCAbility(new Proto_Ability.Rpc_BulletFire {
                    configId = ConfigID,
                    rowId = RowId,
                    speed = (ushort)Mathf.CeilToInt(_abilityData.M_Speed * 10f),
                    targetPoint = maxEndPoint,
                }
            );
        }

        protected override void OnClear() {
            base.OnClear();
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>( new TimeReduce.InitData {
                LifeTime = _abilityData.M_LifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void AddMove() {
            AddComponent<MoveLineConstantSpeed>(new MoveLineConstantSpeed.InitData {
                Speed = _abilityData.M_Speed,
            });
        }

        private void AddSelect() {
            AddComponent<RPGSelectGPO>(new RPGSelectGPO.InitData {
                IgnoreGpoID = FireGPO.GetGpoID(),
                HitCallBack = (o, hit) => {
                    LifeTimeEnd();
                }
            });
        }

        private void AddHit() {
            AddComponent<ServerAbilityHurtGPO>( new ServerAbilityHurtGPO.InitData {
                Power = _abilityData.M_Power,
                WeaponItemId = ItemSet.Id_Rpg,
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}