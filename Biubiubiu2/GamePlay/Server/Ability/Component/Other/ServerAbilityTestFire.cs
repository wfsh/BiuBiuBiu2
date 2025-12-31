using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityTestFire : ServerNetworkComponentBase {
        private S_Ability_Base abSystem;
        private float checkDeltaTime = 0.0f;
        private float lifeTime = 20f;
        private int abilityId = 0;

        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnStart() {
            base.OnStart();
            abSystem = (S_Ability_Base)mySystem;
            abilityId = abSystem.AbilityId;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            abSystem = null;
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            if (checkDeltaTime > 0) {
                checkDeltaTime -= Time.deltaTime;
                return;
            }
            checkDeltaTime = 1.0f;
            lifeTime--;
            Rpc(new Proto_AbilityAB_Auto.Rpc_PlayEffect {
                configId = AbilityM_PlayEffect.ConfigId,
                rowId = AbilityM_PlayEffect.ID_Bomb,
                startPoint = iEntity.GetPoint(),
                startRota = iEntity.GetRota(),
                lifeTime = 1,
                scale = 1,
            });
            if (lifeTime <= 0) {
                LifeTimeEnd();
            }
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = abilityId
            });
        }
    }
}