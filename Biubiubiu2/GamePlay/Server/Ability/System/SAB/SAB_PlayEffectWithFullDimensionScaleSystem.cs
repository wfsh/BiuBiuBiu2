using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_PlayEffectWithFullDimensionScaleSystem : S_Ability_Base {
        private AbilityIn_PlayEffectWithFullDimensionScale useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_PlayEffectWithFullDimensionScale)InData;
            iEntity.SetPoint(useInData.In_StartPoint);
            AddComponents();
            FireGPO.Register<SE_GPO.Event_SetIsDead>(OnSetDeadCallBack);
        }

        protected override void AddComponents() {
            base.AddComponents();
            AddLifeTime();
        }

        protected override void OnStart() {
            base.OnStart();
            RPCAbility(new Proto_AbilityAB_Auto.Rpc_PlayEffectWithFullDimensionScale() {
                    configId = ConfigID,
                    rowId = RowId,
                    startPoint = useInData.In_StartPoint,
                    startRota = useInData.In_StartRota,
                    scale = useInData.In_StartScale * 10f,
                    lifeTime = (ushort)Mathf.CeilToInt(useInData.In_LifeTime * 10f),
                    playTimestamp = TimeUtil.GetCurUTCTimestamp(),
                    audioKey = useInData.In_AudioKey,
                }
            );
        }

        protected override void OnClear() {
            base.OnClear();
            FireGPO.Unregister<SE_GPO.Event_SetIsDead>(OnSetDeadCallBack);
        }

        private void OnSetDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead) {
                LifeTimeEnd();
            }
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = useInData.In_LifeTime,
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