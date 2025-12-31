using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientVSMode : ComponentBase {
        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Mode.Rpc_OnlineChange.ID, OnOnlineChangeCallBack);
            AddProtoCallBack(Proto_Mode.TargetRpc_OnlineChange.ID, OnTargetRpcOnlineChangeCallBack);
        }

        protected override void OnClear() {
            RemoveProtoCallBack(Proto_Mode.Rpc_OnlineChange.ID, OnOnlineChangeCallBack);
            RemoveProtoCallBack(Proto_Mode.TargetRpc_OnlineChange.ID, OnTargetRpcOnlineChangeCallBack);
        }

        private void OnOnlineChangeCallBack(INetwork network, IProto_Doc protoDoc) {
            var data = (Proto_Mode.Rpc_OnlineChange)protoDoc;
            DispatchOnlineChange(data.playerId, data.isOnline, data.reconnectDuration);
        }

        private void OnTargetRpcOnlineChangeCallBack(INetwork network, IProto_Doc cmdData) {
            var data = (Proto_Mode.TargetRpc_OnlineChange)cmdData;
            DispatchOnlineChange(data.playerId, data.isOnline, data.reconnectDuration);
        }

        private void DispatchOnlineChange(long playerId, bool isOnline, float reconnectDuration) {
            MsgRegister.Dispatcher(new CM_Mode.OnlineChange {
                PlayerId = playerId,
                IsOnline = isOnline,
                ReconnectDuration = reconnectDuration,
            });
        }
    }
}