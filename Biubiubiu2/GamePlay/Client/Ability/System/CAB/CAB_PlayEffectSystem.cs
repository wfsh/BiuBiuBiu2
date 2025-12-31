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
    public class CAB_PlayEffectSystem : C_Ability_Base {
        private Proto_AbilityAB_Auto.Rpc_PlayEffect useInData;
        private bool isUse1P = false;
        private float scale = 1f;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (Proto_AbilityAB_Auto.Rpc_PlayEffect)InData;
            iEntity.SetPoint(useInData.startPoint);
            if (useInData.startRota != Quaternion.identity) {
                iEntity.SetRota(useInData.startRota);
            }
            if (useInData.scale > 0) {
                scale = useInData.scale * 0.1f;
            }
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            MsgRegister.Dispatcher(new CM_GPO.GetLocalGPO {
                CallBack = localGpo => {
                    isUse1P = localGpo != null && localGpo.GetGpoID() == FireGpoId;
                }
            });
            if (string.IsNullOrEmpty(MData.GetEffectSign())) {
                Debug.LogError("PlayEffectSystem: EffectSign is null or empty for " + MData.GetTypeID() + ", RowId: " + MData.GetRowID());
                LifeTimeEnd();
            } else {
                CreateEntityToPool(MData.GetEffectSign());
            }
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
            if (MData is AbilityM_PlayEffect) {
                var playEffectData = (AbilityM_PlayEffect)MData;
                if (string.IsNullOrEmpty(playEffectData.M_AudioSign) == false) {
                    AudioPoolManager.OnPlayAudio(isUse1P ? AssetURL.GetAudio1P(playEffectData.M_AudioSign) : AssetURL.GetAudio3P($"{playEffectData.M_AudioSign}_3P"), iEntity.GetPoint());
                }
            }
            if (scale > 0) {
                iEntity.SetLocalScale(new Vector3(scale, scale, scale));
            }
        }

        private void AddComponents() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                CallBack = LifeTimeEnd,
                LifeTime = useInData.lifeTime * 0.1f,
            });
            AddComponent<ClientWWiseAudio>(new ClientWWiseAudio.InitData {
                WWiseID = useInData.audioKey,
                IsFollow = true,
            });
        }

        private void LifeTimeEnd() {
            this.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = this.AbilityId
            });
        }
    }
}