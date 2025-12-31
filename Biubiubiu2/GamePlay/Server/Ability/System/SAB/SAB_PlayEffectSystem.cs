using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_PlayEffectSystem : S_Ability_Base {
        private AbilityIn_PlayEffect useInData;
        private AbilityM_PlayEffect useMData;
        
        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_PlayEffect)InData;
            useMData = (AbilityM_PlayEffect)MData;
            iEntity.SetPoint(useInData.In_StartPoint);
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
        }

        protected override void OnStart() {
            base.OnStart();
            RPCAbility(new Proto_AbilityAB_Auto.Rpc_PlayEffect() {
                    configId = ConfigID,
                    rowId = RowId,
                    startPoint = useInData.In_StartPoint,
                    startRota = useInData.In_StartRota,
                    lifeTime = (ushort)Mathf.CeilToInt(GetLifeTime() * 10f),
                    audioKey = useInData.In_AudioKey,
                }
            );
        }

        protected override void OnClear() {
            base.OnClear();
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>( new TimeReduce.InitData {
                LifeTime = GetLifeTime(),
                CallBack = LifeTimeEnd
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }

        private float GetLifeTime() {
            var lifeTime = useMData.M_LifeTime;
            if (useInData.In_LifeTime > 0f) {
                lifeTime = useInData.In_LifeTime;
            }
            return lifeTime;
        }
    }
}