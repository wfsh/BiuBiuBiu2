using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_PlayHollowCircleWarningEffectSystem : S_Ability_Base {
        private AbilityM_PlayHollowCircleWarningEffect useMData;
        private AbilityIn_PlayHollowCircleWarningEffect useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_PlayHollowCircleWarningEffect)MData;
            useInData = (AbilityIn_PlayHollowCircleWarningEffect)InData;
            iEntity.SetPoint(useInData.In_StartPoint);
            iEntity.SetLocalScale(Vector3.one * useInData.In_MaxDistance);
            AddComponents();
        }

        protected override void OnClear() {
            useMData = null;
            useInData = null;
        }

        protected override void OnStart() {
            RPCAbility(new Proto_AbilityAB_Auto.Rpc_PlayHollowCircleWarningEffect() {
                    configId = ConfigID,
                    rowId = RowId,
                    startPoint = useInData.In_StartPoint,
                    lifeTime = useInData.In_LifeTime,
                    maxDistance = (byte)useInData.In_MaxDistance,
                    attackOffset = (ushort)(useInData.In_AttackOffset * 100),
                }
            );
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<ServerAbilityLifeCycle_GpoDead>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useInData.In_LifeTime,
                EndTimeCallBack = null,
            });
        }

    }
}
