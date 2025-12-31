using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_PlayBloodSplatterSystem : S_Ability_Base {
        private AbilityIn_PlayBloodSplatter useInData;
        private AbilityM_PlayBloodSplatter useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_PlayBloodSplatter)InData;
            useMData = (AbilityM_PlayBloodSplatter)MData;
            iEntity.SetPoint(useInData.In_HitPoint);
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
        }

        protected override void OnStart() {
            base.OnStart();
            RPCAbility(new Proto_AbilityAB_Auto.Rpc_PlayBloodSplatter() {
                    configId = ConfigID,
                    rowId = RowId,
                    bloodValue = useInData.In_BloodValue,
                    hitGpoId = useInData.In_HitGpoId,
                    hitItemId = useInData.In_HitItemId,
                    diffPos = useInData.In_DiffPos
                }
            );
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>( new TimeReduce.InitData {
                LifeTime = useMData.M_LifeTime,
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