using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_PlayMovingEffectSystem : S_Ability_Base {
        private AbilityM_PlayMovingEffect useMData;
        private AbilityIn_PlayMovingEffect useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_PlayMovingEffect)MData;
            useInData = (AbilityIn_PlayMovingEffect)InData;
            // 设置 AB 初始位置
            iEntity.SetPoint(useInData.In_StartPoint);
            AddComponents();
        }

        protected override void OnClear() {
            useMData = null;
            useInData = null;
        }

        protected override void OnStart() {
            RPCAbility(new Proto_AbilityAB_Auto.Rpc_PlayMovingEffect() {
                configId = ConfigID,
                rowId = RowId,
                startPoint = useInData.In_StartPoint,
                startLookAt = useInData.In_StartLookAt,
                startScale = useInData.In_StartScale,
                lifeTime = (ushort)Mathf.CeilToInt((useInData.In_LifeTime * 10f)),
                moveDir = useInData.In_MoveDir,
                moveSpeed = (ushort)Mathf.CeilToInt(useInData.In_MoveSpeed * 10f),
                audioKey = useInData.In_AudioKey,
            });
        }

        override protected void AddComponents() {
            base.AddComponents();
             AddComponent<TimeReduce>(new TimeReduce.InitData {
                 LifeTime = useInData.In_LifeTime,
                 CallBack = () => {
                    MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                        AbilityId = AbilityId
                    });
                 }
             });
        }
    }
}
