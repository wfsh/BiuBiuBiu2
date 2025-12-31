using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_UpHPForFireRoleSystem : S_Ability_Base {
        private AbilityIn_UpHPForFireRole useInData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_UpHPForFireRole)InData;
        }

        protected override void OnStart() {
            base.OnStart();
            FireGPO.Dispatcher(new SE_GPO.Event_UpHP {
                UpHp = useInData.In_UpHPValue,
            });
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}