using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    /// <summary>
    /// 常驻类型 AB 编写范例。支持重连，单独类型同步
    /// </summary>
    public class CAB_TestFire : C_Ability_Base {
        private Proto_Ability.TargetRpc_PlayTestFire abilityData;
        private IAbilityMData _modMData;

        protected override void OnAwake() {
            base.OnAwake();
            abilityData = (Proto_Ability.TargetRpc_PlayTestFire)InData;
            iEntity.SetPoint(abilityData.startPoint);
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
        }

        private void AddComponents() {
            AddComponent<ClientAbilityNetworkBehaviour>(); // NetworkBehaviour
            AddComponent<ClientAbilityTestFire>();
        }
    }
}