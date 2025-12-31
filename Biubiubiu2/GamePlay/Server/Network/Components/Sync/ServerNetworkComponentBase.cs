using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerNetworkComponentBase : ComponentBase {
        protected override void OnAwakeBase() {
            base.OnAwakeBase();
            // 针对玩家进入视野范围或登录， 当前 GPO 对象（非玩家） 需要下发数据给玩家
            mySystem.Register<SE_Network.Event_InDistanceNetwork>(OnAIInDistance);
        }

        protected override void OnSetNetworkBase() {
            base.OnSetNetworkBase();
            if (networkBase is INetworkCharacter) { 
                // 针对玩家进入视野范围或登录， 当前玩家需要下发数据给玩家
                networkBase.AddObserverNetwork(OnCharacterInDistance);
            }
        }

        protected override void OnClearBase() {
            base.OnClearBase();
            networkBase?.RemoveObserverNetwork(OnCharacterInDistance);
            mySystem.Unregister<SE_Network.Event_InDistanceNetwork>(OnAIInDistance);
        }
        /// <summary>
        ///  有真实玩家进入视野范围或登录，下发需要同步的信息
        /// </summary>
        /// <param name="network"></param>
        private void OnCharacterInDistance(INetwork network) {
            if (network is INetworkCharacter == false) {
                Debug.LogError("区域剔除数据只能同步给角色对象");
                return;
            }
            Sync((INetworkCharacter)network);
        }
        
        private void OnAIInDistance(ISystemMsg body, SE_Network.Event_InDistanceNetwork ent) {
            if (networkBase is INetworkCharacter) {
                return;
            }
            Sync(ent.NetworkList);
        }

        private void Sync(INetworkCharacter network) {
            var list = new List<INetworkCharacter>();
            list.Add(network);
            Sync(list);
        }

        /// <summary>
        /// 有玩家连接（包括自己），需要下发的数据可以在这边写（几乎等价于 Sync）, 不同的只会在玩家登录时候触发
        /// </summary>
        /// <returns></returns>
        virtual protected void Sync(List<INetworkCharacter> networkList) {
            var sendProto = SyncData();
            if (sendProto != null) {
                TargetRpcList(networkList, sendProto);
            }
            var sendProtoList = SyncList();
            if (sendProtoList != null && sendProtoList.Count > 0) {
                for (int i = 0; i < sendProtoList.Count; i++) {
                    var proto = sendProtoList[i];
                    TargetRpcList(networkList, proto);
                }
            }
        }
        

        /// <summary>
        /// 有玩家连接（包括自己），需要下发的数据可以在这边写（几乎等价于 Sync）, 不同的只会在玩家登录时候触发
        /// 注意：重写前需要先注册 RegisterCharacterLogin
        /// </summary>
        /// <returns></returns>
        virtual protected ITargetRpc SyncData() {
            return null;
        }
        
        /// <summary>
        /// 有玩家连接（包括自己），需要下发的数据可以在这边写（几乎等价于 Sync）, 不同的只会在玩家登录时候触发
        /// 发送一个同步列表
        /// </summary>
        /// <returns></returns>
        virtual protected List<ITargetRpc> SyncList() {
            return null;
        }

        protected override void Rpc(IRpc proto) {
            if (null == networkBase || networkBase.IsDestroy()) {
                return;
            }
            if (networkBase is INetworkCharacter) {
                this.networkBase.Rpc(proto);
            } else {
                RpcToConnId(proto);
            }
        }

        /// <summary>
        /// 通过通用网络组件进行 RPC 数据下发， 发送数据给所有对象的 ConnId 目标
        /// </summary>
        /// <param name="proto"></param>
        private void RpcToConnId(IRpc proto) {
            if (ConnID <= 0) {
                Debug.LogError($"TargetRpcALL  WorldSyncId 不能为 0");
                return;
            }
            var spawnNetworks = GetSpawnNetworks();
            if (spawnNetworks == null) {
                Debug.LogError("RpcToConnId 需要结合 ServerAbilityNetworkSync 一起使用" + this);
                return;
            }
            this.networkBase.TargetRpcList(spawnNetworks, ConnID, proto);
        }
        

        protected override void TargetRpc(INetwork network, ITargetRpc proto) {
            if (null == networkBase || networkBase.IsDestroy()) {
                return;
            }
            if (networkBase is INetworkCharacter) {
                this.networkBase.TargetRpc(network, proto);
            } else {
                TargetRpcToConnId(network, proto);
            }
        }
        
        protected override void TargetRpcList(List<INetworkCharacter> list, ITargetRpc proto) {
            if (null == networkBase || networkBase.IsDestroy()) {
                return;
            }
            if (networkBase is INetworkCharacter) {
                this.networkBase.TargetRpcList(list, proto);
            } else {
                TargetRpcToConnId(list, proto);
            }
        }
        
        /// <summary>
        /// 通过通用网络组件进行 TargetRpc 数据下发， 发送数据给指定对象的 ConnId 目标
        /// </summary>
        /// <param name="proto"></param>
        private void TargetRpcToConnId(List<INetworkCharacter> networks, ITargetRpc proto) {
            if (networkBase is INetworkCharacter) {
                Debug.LogError($"角色的范围同步直接使用 RPC 而不是 TargetRpcALL");
                return;
            }
            if (ConnID < 0) {
                Debug.LogError($"TargetRpcALL  WorldSyncId 不能 小于 0 Name:{GetName()} ConnID:{ConnID} IsClear:{isClear}  Proto:{proto.GetID()}");
                return;
            }
            var spawnNetworks = GetSpawnNetworks();
            if (spawnNetworks == null) {
                Debug.LogError("TargetRpcToConnId 需要结合 ServerAbilityNetworkSync 一起使用" + this);
                return;
            }
            var sendList = new List<INetworkCharacter>();
            for (int i = 0; i < networks.Count; i++) {
                var characterNetwork = networks[i];
                if (spawnNetworks.Contains(characterNetwork) == false) {
                    continue;
                }
                sendList.Add(characterNetwork);
            }
            this.networkBase.TargetRpcList(sendList, ConnID, proto);
        }
        
        private void TargetRpcToConnId(INetwork  network, ITargetRpc proto) {
            if (networkBase is INetworkCharacter) {
                Debug.LogError($"角色的范围同步直接使用 RPC 而不是 TargetRpcALL");
                return;
            }
            if (network is INetworkCharacter == false) {
                Debug.LogError($"数据只能同步给角色对象");
                return;
            }
            var characterNetwork = (INetworkCharacter)network;
            if (ConnID < 0) {
                Debug.LogError($"TargetRpcALL  WorldSyncId 不能 小于 0 Name:{GetName()} ConnID:{ConnID} IsClear:{isClear}  Proto:{proto.GetID()}");
                return;
            }
            var spawnNetworks = GetSpawnNetworks();
            if (spawnNetworks == null) {
                Debug.LogError("TargetRpcToConnId 需要结合 ServerAbilityNetworkSync 一起使用" + this);
                return;
            }
            if (spawnNetworks.Contains(characterNetwork) == false) {
                return;
            }
            this.networkBase.TargetRpc(characterNetwork, ConnID, proto);
        }
        
        private List<INetworkCharacter> GetSpawnNetworks() {
            List<INetworkCharacter> spawnNetworks = null;
            this.mySystem.Dispatcher(new SE_Network.Event_GetSpawnNetworks {
                CallBack = list => {
                    spawnNetworks = list;
                } 
            });
            return spawnNetworks;
        }
    }
}