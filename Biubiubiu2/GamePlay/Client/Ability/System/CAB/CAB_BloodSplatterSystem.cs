using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_BloodSplatterSystem : C_Ability_Base {
        private Proto_AbilityAB_Auto.Rpc_PlayBloodSplatter useInData;
        private AbilityM_PlayBloodSplatter useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_AbilityAB_Auto.Rpc_PlayBloodSplatter)InData;
            useMData = (AbilityM_PlayBloodSplatter)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntityToPool(useMData.M_EffectSign);
        }


        private void AddComponents() {
            AddLifeTime();
            AddComponent<ClientBloodSplatter>(new ClientBloodSplatter.InitData {
                BloodValue = useInData.bloodValue,
                DiffPos = useInData.diffPos,
                HitGpoId = useInData.hitGpoId,
                HitItemId = useInData.hitItemId,
            });
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = useMData.M_LifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void LifeTimeEnd() {
            this.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = this.AbilityId
            });
        }
    }
}