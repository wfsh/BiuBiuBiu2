using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_MissileBombSystem : S_Ability_Base {
        private AbilityData.PlayAbility_MissileBomb inData;
        private int hurtValue = 0;
        private string fireGPOName = "";
        private Action<IAbilitySystem> playAECallback;

        protected override void OnAwake() {
            base.OnAwake();
            inData = (AbilityData.PlayAbility_MissileBomb)MData;
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddMove();
            AddSelect();
            SetAtk(inData.In_ATK);
        }

        protected override void OnStart() {
            fireGPOName = FireGPO.GetName();
            RPCAbility(new Proto_Ability.Rpc_MissileBomb {
                    abilityModId = inData.ConfigId,
                    points = inData.In_Points,
                }
            );
        }

        public void SetAtk(int atk) {
            hurtValue = Mathf.FloorToInt(inData.M_Power * atk * 0.01f);
        }

        public void SetPlayExplosive(Action<IAbilitySystem> callback) {
            playAECallback = callback;
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>( new TimeReduce.InitData {
                LifeTime = inData.M_LifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void AddMove() {
            AddComponent<MoveGrenade>(new MoveGrenade.InitData {
                Speed = inData.M_Speed,
                Points = inData.In_Points,
            });
        }

        private void AddSelect() {
            AddComponent<MovePointRaycastHit>(new MovePointRaycastHit.InitData {
                LayerMask = ~(LayerData.ClientLayerMask),
                IgnoreGpoID = FireGPO.GetGpoID(),
                HitCallBack = (o, hit) => {
                    PlayAE(hit.point);
                    MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                        AbilityId = AbilityId
                    });
                }
            });
        }

        private void LifeTimeEnd() {
            PlayAE(iEntity.GetPoint());
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }

        private void PlayAE(Vector3 startPos) {
            if (hurtValue <= 0f) {
                Debug.LogError($" SAB_GrenadeSystem hurtValue <= 0  {fireGPOName}");
                return;
            }
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = FireGPO,
                MData = AbilityM_Explosive.CreateForID(AbilityM_Explosive.ID_MissileExplosive),
                InData = new AbilityIn_Explosive {
                    In_StartPoint = startPos,
                    In_Hurt = hurtValue,
                    In_WeaponId = inData.In_WeaponItemId,
                    In_Range = inData.In_AttackRangeRange
                }
            });
        }
    }
}