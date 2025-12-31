using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_PlatformMovementSystem : C_Ability_Base {
        private int elementId = 0;
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            var data = (Proto_Ability.TargetRpc_ScreenElement)InData;
            elementId = data.elementId;
            // GetSceneEntity(elementId);
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
        }

        private void AddComponents() {
            AddComponent<ClientPlatformMovementGPOEnter>();
            AddComponent<ClientNetworkTransform>();
        }
    }
}