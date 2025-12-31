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
    public class CAB_PlayFillScaleEffectSystem : C_Ability_Base {
        public Proto_Ability.Rpc_PlayFillScaleEffect useInData;
        private IAbilityMData useMData;
        private Vector3 scale;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_Ability.Rpc_PlayFillScaleEffect)InData;
            useMData = AbilityConfig.GetAbilityModData(useInData.abilityModId);
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            if (string.IsNullOrEmpty(useMData.GetEffectSign())) {
                Dispatcher(new CE_Ability.RemoveAbility() {
                    AbilityId = GetAbilityId()
                });
            } else {
                CreateEntity(useMData.GetEffectSign());
            }
        }

        protected override void OnLoadEntityEnd(IEntity iEntity) {
            base.OnLoadEntityEnd(iEntity);
            iEntity.SetPoint(useInData.startPoint);
            if (useInData.startLookAt != Vector3.zero) {
                var rot = Quaternion.LookRotation(useInData.startLookAt, Vector3.up);
                iEntity.SetRota(rot);
            }
            if (useInData.isCircleFill) {
                iEntity.SetLocalScale(Vector3.zero);
            } else {
                iEntity.SetLocalScale(new Vector3(useInData.endScale.x, useInData.endScale.y, 0) * 0.1f);
            }
        }

        private void AddComponents() {
            AddLifeTime();
            AddComponent<ClientAbilityFillScale>();
        }

        private void AddLifeTime() {
            AddComponent<ClientAbilityLifeCycle>( new ClientAbilityLifeCycle.InitData {
                Duration = useInData.lifeTime * 0.1f - 0.001f * (DateTimeUtils.GetCurUTCTimestamp() - useInData.playTimestamp),
                EndCallBack = null
            });
        }
    }
}