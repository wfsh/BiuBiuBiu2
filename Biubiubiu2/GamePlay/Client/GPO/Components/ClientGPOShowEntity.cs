using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPOShowEntity : ComponentBase {
        private bool isShowEntity = true;
        private bool isOnline = true;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<CE_Network.IsOnline>(OnIsOnlineCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            isOnline = true;
            mySystem.Dispatcher(new CE_Network.GetIsOnline {
                CallBack = b => {
                    isOnline = b;
                }
            });
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_GPO.Rpc_IsShowEntity.ID, OnRpcIsShowEnityCallBack);
            RemoveProtoCallBack(Proto_GPO.TargetRpc_IsShowEntity.ID, OnTargetRpcIsShowEntityCallBack);
            mySystem.Unregister<CE_Network.IsOnline>(OnIsOnlineCallBack);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            CheckShowEntity();
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_GPO.Rpc_IsShowEntity.ID, OnRpcIsShowEnityCallBack);
            AddProtoCallBack(Proto_GPO.TargetRpc_IsShowEntity.ID, OnTargetRpcIsShowEntityCallBack);
        }

        private void OnIsOnlineCallBack(ISystemMsg body, CE_Network.IsOnline ent) {
            isOnline = ent.IsTrue;
            CheckShowEntity();
        }

        private void OnRpcIsShowEnityCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_GPO.Rpc_IsShowEntity)cmdData;
            isShowEntity = data.isTrue;
            CheckShowEntity();
        }

        private void OnTargetRpcIsShowEntityCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_GPO.TargetRpc_IsShowEntity)cmdData;
            isShowEntity = data.isShowEntity;
            CheckShowEntity();
        }

        private void CheckShowEntity() {
            if (isOnline && isShowEntity) {
                ShowEntity(true);
            } else {
                ShowEntity(false);
            }
        }

        private void ShowEntity(bool isShow) {
            if (iEntity is EntityBase) {
                var entity = (EntityBase)iEntity;
                entity.SetActive(isShow);
            }
        }
    }
}