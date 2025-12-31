using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPOGodMode : ComponentBase {
        private bool isGodMode = false;
        protected override void OnAwake() {
            base.OnAwake();
        }
        
        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_GPO.TargetRpc_IsGodMode.ID, OnTargetRpcIsGodModeCallBack);
            RemoveProtoCallBack(Proto_GPO.Rpc_IsGodMode.ID, OnRpcIIsGodModeCallBack);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_GPO.TargetRpc_IsGodMode.ID, OnTargetRpcIsGodModeCallBack);
            AddProtoCallBack(Proto_GPO.Rpc_IsGodMode.ID, OnRpcIIsGodModeCallBack);
        }
        
        private void OnRpcIIsGodModeCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_GPO.Rpc_IsGodMode)cmdData;
            SetBecomeTarget(data.isTrue);
        }
        
        private void OnTargetRpcIsGodModeCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_GPO.TargetRpc_IsGodMode)cmdData;
            SetBecomeTarget(data.isTrue);
        }
        private void SetBecomeTarget(bool isTrue) {
            isGodMode = isTrue;
            mySystem.Dispatcher(new CE_GPO.Event_IsGodMode {
                IsTrue = isTrue,
            });
        }
    }
}