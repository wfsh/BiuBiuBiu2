using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_GrenadeSystem : S_Ability_Base {
        private AbilityIn_Grenade useInData;
        private AbilityM_Grenade useMData;
        private int hurtValue = 0;
        private float attackRange = 0f;
        private float baseAttackRange = 0f;
        private string fireGPOName = "";

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_Grenade)InData;
            useMData = (AbilityM_Grenade)MData;
            AddComponents();
        }
        
        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddMove();
            AddSelect();
        }

        protected override void OnStart() {
            base.OnStart();
            CountHurtValue();
            CreateEntity(useMData.M_EffectSign);
            RPCAbility(new Proto_Ability.Rpc_ThreadGrenade() {
                    configId = ConfigID,
                    rowId = RowId,
                    points = useInData.In_Points,
                }
            );
        }

        private void CountHurtValue() {
            fireGPOName = FireGPO.GetName();
            FireGPO.Dispatcher(new SE_GPO.Event_GetATK {
                CallBack = value => {
                    hurtValue = Mathf.FloorToInt(useMData.M_Power * value * 0.01f);
                }
            });
            FireGPO.Dispatcher(new SE_GPO.Event_GetAttackRange {
                CallBack = range => {
                    attackRange = range;
                }
            });
        }

        protected override void OnClear() {
            base.OnClear();
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>( new TimeReduce.InitData {
                LifeTime = useMData.M_LifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void AddMove() {
            AddComponent<MoveGrenade>(new MoveGrenade.InitData {
                Speed = useMData.M_Speed,
                Points = useInData.In_Points,
            });
        }

        private void AddSelect() {
            AddComponent<GrenadeHit>(new GrenadeHit.InitData {
                IsHitBomb = useMData.M_IsHitBomb,
                FireGpoId = FireGPO.GetGpoID(),
                CallBack = LifeTimeEnd
            });
            AddComponent<MovePointRaycastHit>(new MovePointRaycastHit.InitData() {
                IgnoreGpoID = FireGPO.GetGpoID(),
                LayerMask = ~(LayerData.ClientLayerMask),
                HitCallBack = (o, hit) => {
                    LifeTimeEnd();
                }
            });
        }

        private void LifeTimeEnd() {
            PlayAE();
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }

        private void PlayAE() {
            if (hurtValue <= 0f) {
                Debug.LogError($" SAB_GrenadeSystem hurtValue <= 0  {fireGPOName}");
                return;
            }
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = FireGPO,
                MData = AbilityM_Explosive.CreateForID(AbilityM_Explosive.ID_BulletRPGExplosive),
                InData = new AbilityIn_Explosive {
                    In_StartPoint = iEntity.GetPoint(),
                    In_Hurt = hurtValue,
                    In_WeaponId = useInData.In_WeaponItemId,
                    In_Range = attackRange
                }
            });
        }
    }
}