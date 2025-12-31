using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterOtherDrive : CharacterGPODrive {
        private INetworkCharacter characterNetwork;
        
        protected override void OnAwake() {
            base.OnAwake();
        }
        
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            characterNetwork = networkBase as INetworkCharacter;
        }

        private void OnUpdate(float deltaTime) {
            if (networkBase == null || networkBase.IsDestroy()) {
                return;
            }
            if (driveGPO != null && characterNetwork != null) {
                driveGPO.Dispatcher(new CE_AI.Event_DriverPointRota {
                    Point = characterNetwork.GetPoint(),
                    Rota = characterNetwork.GetRota()
                });
            }
        }
    }
}