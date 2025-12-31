using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientCharacterDropItemHub : ComponentBase {
        private int maxIndex = 0;
        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnSetEntityObj(IEntity entity) {
            base.OnSetEntityObj(entity);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Item.TargetRpc_DropItemData.ID, OnRpcDropItemDataCallBack);
            AddProtoCallBack(Proto_Item.TargetRpc_GetMaxDropItemNum.ID, OnGetMaxDropItemNumCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_Item.TargetRpc_DropItemData.ID, OnRpcDropItemDataCallBack);
            RemoveProtoCallBack(Proto_Item.TargetRpc_GetMaxDropItemNum.ID, OnGetMaxDropItemNumCallBack);
        }

        private void OnGetMaxDropItemNumCallBack(INetwork network, IProto_Doc proto) {
            var data = (Proto_Item.TargetRpc_GetMaxDropItemNum)proto;
            SetMaxDropItemNum(data.maxItemNum);
        }

        private void OnRpcDropItemDataCallBack(INetwork network, IProto_Doc proto) {
            var data = (Proto_Item.TargetRpc_DropItemData)proto;
            MsgRegister.Dispatcher(new CM_Item.DropItemData {
                ItemId = data.itemId, Point = data.point
            });
            SetMaxDropItemNum(data.maxItemNum);
        }

        private void SetMaxDropItemNum(int value) {
            maxIndex = value;
            MsgRegister.Dispatcher(new CM_Item.MaxGetDropItemNum {
                MaxDropItemNum = maxIndex,
            });
        }
    }
}