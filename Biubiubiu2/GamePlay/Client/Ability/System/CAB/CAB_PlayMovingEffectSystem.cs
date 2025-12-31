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
    public class CAB_PlayMovingEffectSystem : C_Ability_Base {
        private Proto_AbilityAB_Auto.Rpc_PlayMovingEffect useInData;
        private Vector3 scale;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_AbilityAB_Auto.Rpc_PlayMovingEffect)InData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            if (string.IsNullOrEmpty(MData.GetEffectSign())) {
                Dispatcher(new CE_Ability.RemoveAbility() {
                    AbilityId = GetAbilityId()
                });
            } else {
                CreateEntity(MData.GetEffectSign());
            }
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
            var speed = 0.1f * useInData.moveSpeed;
            iEntity.SetPoint(useInData.startPoint);
            var rot = Quaternion.LookRotation(useInData.startLookAt, Vector3.up);
            iEntity.SetRota(rot);
            scale = useInData.startScale;
            iEntity.SetLocalScale(scale);
            var targetPoint = useInData.startPoint + speed * useInData.moveDir * useInData.lifeTime * 0.1f;
            AddComponent<MoveToPointBySpeed>(new MoveToPointBySpeed.InitData {
                Speed = speed,
                StartPoint = useInData.startPoint,
                TargetPoint = targetPoint,
                MoveEndCallBack = null,
            });
        }

        private void AddComponents() {
            AddLifeTime();
            AddComponent<ClientWWiseAudio>(new ClientWWiseAudio.InitData {
                IsFollow = true,
                WWiseID = useInData.audioKey,
            });
        }

        private void AddLifeTime() {
            AddComponent<ClientAbilityLifeCycle>( new ClientAbilityLifeCycle.InitData {
                Duration = useInData.lifeTime * 0.1f,
                EndCallBack = null
            });
        }
    }
}