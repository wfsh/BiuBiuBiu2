using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_UpdraftSystem : S_Ability_Base {
        private AbilityData.PlayAbility_Updraft inData;
        protected override void OnAwake() {
            base.OnAwake();
            inData = (AbilityData.PlayAbility_Updraft)MData;
            AddComponents();
        }

        override protected void AddComponents() {
            base.AddComponents();
            if (inData.M_LifeTime > 0) {
                AddLifeTime();
            }
            AddSelect();
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>( new TimeReduce.InitData {
                LifeTime = inData.M_LifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void AddSelect() {
            AddComponent<UpdraftGetRangeGPO>( new UpdraftGetRangeGPO.InitData {
                CheckPoint = inData.In_StartPoint,
                RangeXZ = inData.M_RangeXZ,
                RangeY = inData.M_RangeY,
                Power = inData.M_Power
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}