using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_PlungerEndAttack : S_Ability_Base {
        private AbilityData.PlayAbility_PlungerEndAttack inData;
        protected override void OnAwake() {
            base.OnAwake();
            inData = (AbilityData.PlayAbility_PlungerEndAttack)MData;
            iEntity.SetRota(inData.In_StartRota);
            iEntity.SetPoint(inData.In_StartPoint);
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(inData.M_EffectSign);
            PlayHitEffect();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddComponent<AbilityPlungerEndAttack>();
            AddComponent<ServerAbilityStrikeFlyGPO>(new ServerAbilityStrikeFlyGPO.InitData {
                Duration = 2f,
                Force = 10f
            });
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = inData.M_Power,
                WeaponItemId = 0,
            });
        }
        
        private void PlayHitEffect() {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = Gpo,
                MData = AbilityM_PlayEffect.CreateForID(AbilityM_PlayEffect.ID_PlungerEndAttack),
                InData = new AbilityIn_PlayEffect {
                    In_StartPoint = inData.In_StartPoint,
                    In_StartRota = inData.In_StartRota
                }
            });
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = inData.M_LifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}