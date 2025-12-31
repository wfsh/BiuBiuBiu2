using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIRemove : ClientGPORemove {
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<CE_Network.IsOnline>(OnIsOnlineCannBack);
        }
        
        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<CE_Network.IsOnline>(OnIsOnlineCannBack);
        }
        
        protected override void OnRemoveGPO() {
            base.OnRemoveGPO();
            RemoveMonster();
        }

        private void OnIsOnlineCannBack(ISystemMsg body, CE_Network.IsOnline ent) {
            if (ent.IsTrue == false) {
                RemoveMonster();
            }
        }

        private void RemoveMonster() {
            Dispatcher(new CE_AI.RemoveAI {
                GpoId = GpoID
            });
        }
    }
}