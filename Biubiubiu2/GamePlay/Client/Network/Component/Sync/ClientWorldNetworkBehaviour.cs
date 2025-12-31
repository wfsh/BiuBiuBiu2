using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientWorldNetworkBehaviour : ComponentBase, IClientWorldNetwork {
        private bool isOnline = true;
        protected override void OnAwake() {
            mySystem.Register<CE_Network.GetIsOnline>(OnGetIsOnlineCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            if (ConnID <= 0) {
                return;
            }
            MsgRegister.Dispatcher(new CM_Network.AddWorldNetworkBehaviour {
                IWorldNetwork = this,
            });
        }

        protected override void OnClear() {
            MsgRegister.Dispatcher(new CM_Network.RemoveWorldNetworkBehaviour {
                ConnID = ConnID
            });
            mySystem.Unregister<CE_Network.GetIsOnline>(OnGetIsOnlineCallBack);
            RemoveProtoCallBack(Proto_GPO.TargetRpc_SyncGPOID.ID, TargetRpcSyncGPOIDSpawnDataCallBack);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_GPO.TargetRpc_SyncGPOID.ID, TargetRpcSyncGPOIDSpawnDataCallBack);
        }

        private void TargetRpcSyncGPOIDSpawnDataCallBack(INetwork network, IProto_Doc data) {
            var rpcData = (Proto_GPO.TargetRpc_SyncGPOID)data;
            AddIGPOComponent(rpcData.gpoID, rpcData.teamID, rpcData.gpoType);
        }

        private void AddIGPOComponent(int gpoId, int teamId, GPOData.GPOType gpoType) {
            if (iGPO != null) {
                if (iGPO.GetGpoID() != gpoId) {
                    Debug.LogError("GPO ID 不一致  " + iGPO.GetGpoID() + "  " + gpoId);
                }
                return;
            }
            mySystem.AddComponent<ClientGPO>(new ClientGPO.InitData {
                    GpoId = gpoId,
                    TeamId = teamId,
                    GpoType = gpoType,
                    IsLocalGPO = false,
                });
        }
        
        private void OnGetIsOnlineCallBack(ISystemMsg body, CE_Network.GetIsOnline ent) {
            ent.CallBack?.Invoke(isOnline);
        }

        public void SetIsOnline(bool isOnline) {
            this.isOnline = isOnline;
            OnSetIsOnline(isOnline);
            mySystem.Dispatcher(new CE_Network.IsOnline {
                IsTrue = isOnline,
            });
        }

        virtual protected void OnSetIsOnline(bool isOnline) {
        }

    }
}