using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIManager : ManagerBase {
        private INetwork network;
        private ClientAISystem system;

        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<M_Network.SetNetwork>(SetNetwork);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Network.SetNetwork>(SetNetwork);
            network = null;
            if (system != null) {
                system.Clear();
                system = null;
            }
        }

        private void SetNetwork(M_Network.SetNetwork ent) {
            network = ent.iNetwork;
            if (system == null) {
                system = AddSystem(delegate(ClientAISystem system) {
                    system.SetNetwork(network);
                });
            } else {
                system.SetNetwork(network);
            }
        }
    }
}