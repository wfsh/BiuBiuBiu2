using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAbilityNetworkBehaviour : ClientWorldNetworkBehaviour {
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<CE_Ability.HasConnAbility>(OnHasConnAbilityCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<CE_Ability.HasConnAbility>(OnHasConnAbilityCallBack);
        }
        
        private void OnHasConnAbilityCallBack(ISystemMsg body, CE_Ability.HasConnAbility ent) {
            ent.CallBack?.Invoke();
        }

        override protected void OnSetIsOnline(bool isOnline) {
            if (isOnline == false) {
                var abilityId = ((C_Ability_Base)mySystem).AbilityId;
                Dispatcher(new CE_Ability.RemoveAbility {
                    AbilityId = abilityId
                });
            }
        }
    }
}