using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAICatch : ComponentBase {
        protected override void OnClear() {
            RemoveProtoCallBack(Proto_AI.Rpc_CatchAI.ID, OnMonsterCatch);
            base.OnClear();
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_AI.Rpc_CatchAI.ID, OnMonsterCatch);
        }

        private void OnMonsterCatch(INetwork network, IProto_Doc docData) {
            Dispatcher(new CE_AI.RemoveAI {
                GpoId = GpoID
            });
        }
    }
}