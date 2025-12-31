using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_PlayWarningEffectSystem : C_Ability_Base {
        private Proto_Ability.Rpc_PlayWarningEffect abilityData;
        private IAbilityMData _modMData;
        private Vector3 scale;

        protected override void OnAwake() {
            base.OnAwake();
            abilityData = (Proto_Ability.Rpc_PlayWarningEffect)InData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            _modMData = AbilityConfig.GetAbilityModData(abilityData.abilityModId);
            CreateEntity(_modMData.GetEffectSign());
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);

            iEntity.SetPoint(abilityData.startPoint);
            if (abilityData.startLookAt == Vector3.zero) {
                iEntity.SetRota(Quaternion.identity);
            } else {
                var rot = Quaternion.LookRotation(abilityData.startLookAt, Vector3.up);
                iEntity.SetRota(rot);
            }
            scale = abilityData.startScale * 0.1f;
            iEntity.SetLocalScale(scale);
        }

        private void AddComponents() {
            AddLifeTime();
        }

        private void AddLifeTime() {
            AddComponent<ClientAbilityLifeCycle>( new ClientAbilityLifeCycle.InitData {
                Duration = abilityData.lifeTime * 0.1f - 0.001f * (DateTimeUtils.GetCurUTCTimestamp() - abilityData.playTimestamp),
                EndCallBack = null
            });
        }
    }
}