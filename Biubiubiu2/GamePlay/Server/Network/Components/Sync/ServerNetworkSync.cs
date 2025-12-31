using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

// 作为网络组件的补充。用于同步 system 到客户端
// 同步该 Synstem 到客户端，主要包括 ITargetRpc 下自定义的数据结构，作用为比如玩家登录游戏，该组件可以自动把 AB 当前的数据同步给登录的玩家（默认为所有玩家）
// 需要在 ClientNetworkSync 做对应事件注册
// 需要根据区域同步，需要添加  SyncArea
// 要同步 AB 的位置，需要添加  SyncTransorm;
namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerNetworkSync : ComponentBase, IServerWorldNetwork {
        public struct InitData : SystemBase.IComponentInitData {
            public Action<ServerNetworkSync> CallBack;
            public int OR_SyncDistance; // 没有特殊需求别填
        }
        private const float LimitTime = 5f; // 网络组件添加删除的时间间隔

        public class LimitData {
            public INetworkCharacter Network;
            public float Time;
        }

        private bool isOnline = true;
        private ITargetRpc sendProto;
        protected List<INetworkCharacter> allNetworks = new List<INetworkCharacter>();
        private List<INetworkCharacter> spawnNetworks = new List<INetworkCharacter>();
        private List<LimitData> limitList = new List<LimitData>();
        private event Action<ServerNetworkSync> onGetSpawnProto;
        protected float syncDistance = -1; // 同步距离 -1 为不限制
        private float syncCheckTime = 0f;
        private bool isSetSpawnProto = false;

        protected override void OnAwake() {
            MsgRegister.Register<M_Network.DisconnectNetwork>(OnDisconnectNetwork);
            MsgRegister.Register<SM_Character.CharacterLogin>(OnCharacterLogin);
            this.mySystem.Register<SE_Network.Event_GetSpawnNetworks>(OnGetSpawnNetworks);
            this.mySystem.Register<SE_Ability.HasConnAbility>(OnHasConnAbility);
            AddNetwork();
            MsgRegister.Dispatcher(new M_Network.GetAllNetwork {
                CallBack = OnSetAllNetwork
            });
            var initData = (InitData)initDataBase;
            SetSpawnProtoFunc(initData.CallBack);
            SetSyncDistance(initData.OR_SyncDistance);
        }

        private void AddNetwork() {
            var connId = ++NetworkData.ConnIndex;
            this.mySystem.SetConnID(connId);
            if (this.mySystem.GetSpawnConnType() == NetworkData.SpawnConnType.None) {
                Debug.LogError("SyncType 不能是 none");
            }
            MsgRegister.Dispatcher(new SM_Network.AddWorldNetworkBehaviour {
                network = this
            });
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            TargetRpcSpawnNetworks(new Proto_Network.TargetRpc_RemoveWorldNetwork {
                syncId = ConnID,
            });
            allNetworks.Clear();
            spawnNetworks.Clear();
            onGetSpawnProto = null;
            this.mySystem.Unregister<SE_Ability.HasConnAbility>(OnHasConnAbility);
            this.mySystem.Unregister<SE_Network.Event_GetSpawnNetworks>(OnGetSpawnNetworks);
            MsgRegister.Unregister<SM_Character.CharacterLogin>(OnCharacterLogin);
            MsgRegister.Unregister<M_Network.DisconnectNetwork>(OnDisconnectNetwork);
            RemoveUpdate(OnUpdate);
        }
        
        private void OnHasConnAbility(ISystemMsg body, SE_Ability.HasConnAbility ent) {
            ent.CallBack.Invoke();
        }

        private void OnUpdate(float delta) {
            if (isSetSpawnProto == false) {
                return;
            }
            UpdateRemoveLimitList();
            CheckSyncDistance();
        }
        
        public void SetSpawnRPC(ITargetRpc protoData) {
            sendProto = protoData;
        }

        public byte[] SerializeProto(IProto_Doc protoData) {
            byte[] bytes = null;
            MsgRegister.Dispatcher(new M_Network.RPCSerialize {
                ProtoDoc = protoData,
                ConnID = 0,
                Channel = 0,
                CallBack = (i, i1, arg3) => {
                    bytes = arg3;
                },
            });
            return bytes;
        }

        private void OnCharacterLogin(SM_Character.CharacterLogin ent) {
            if (ent.INetwork == null) {
                Debug.LogError("networkBase 没有正确赋值");
                return;
            }
            SetNetwork(ent.INetwork);
            syncCheckTime = 0f;
        }

        private void OnSetAllNetwork(List<INetworkCharacter> networks) {
            for (int i = 0; i < networks.Count; i++) {
                SetNetwork(networks[i]);
            }
        }

        public void SetSpawnProtoFunc(Action<ServerNetworkSync> action) {
            isSetSpawnProto = true;
            onGetSpawnProto += action;
        }

        public void SetSyncDistance(float distance) {
            if (distance <= 0) {
                syncDistance = NetworkData.GetNetworkSyncDistance();
            } else {
                syncDistance = distance;
            }
        }

        public void SetIsDebug(bool isDebug) {
            this.IsDebug = isDebug;
        }

        // 根据  syncDistance 从 allNetworks 中筛选出 spawnNetworks
        public void CheckSyncDistance() {
            if (syncCheckTime > 0f) {
                syncCheckTime -= Time.deltaTime;
                return;
            }
            syncCheckTime = 0.1f;
            var tempSpawnNetworkList = GetSpawnNetworksForDistance();
            CheckNetworkOutDistance(tempSpawnNetworkList);
            CheckNetworkInDistance(tempSpawnNetworkList);
        }

        /// <summary>
        ///  获取在距离范围内的玩家列表
        /// </summary>
        /// <returns></returns>
        virtual protected List<INetworkCharacter> GetSpawnNetworksForDistance() {
            var tempSpawnNetworkList = new List<INetworkCharacter>();
            for (int i = 0; i < allNetworks.Count; i++) {
                var network = allNetworks[i];
                if (network.IsIgnoreSyncDistance()) { // 战报无视同步信息
                    tempSpawnNetworkList.Add(network);
                    continue;
                }
                if (network.IsDestroy() || networkBase.HasObservers(network.ConnId()) == false) {
                    continue;
                }
                if (syncDistance <= 0f || WarReportData.IsStartSausageWarReport) {
                    tempSpawnNetworkList.Add(network);
                } else {
                    IGPO targetGPO = null;
                    MsgRegister.Dispatcher(new SM_GPO.GetGPOForCharacterNetwork {
                        ConnId = network.ConnId(),
                        CallBack = gpo => {
                            targetGPO = gpo;
                        }
                    });
                    var distance = 0f;
                    if (targetGPO != null) {
                        distance = Vector3.Distance(targetGPO.GetPoint(), iEntity.GetPoint());
                    } else {
                        distance = Vector3.Distance(network.GetPoint(), iEntity.GetPoint());
                    }
                    if (distance <= syncDistance) {
                        tempSpawnNetworkList.Add(network);
                    }
                }
            }
            return tempSpawnNetworkList;
        }
        // 有玩家超出了同步距离
        private void CheckNetworkOutDistance(List<INetworkCharacter> spawnList) {
            var removeList = new List<INetworkCharacter>();
            for (int i = 0; i < spawnNetworks.Count; i++) {
                var spawn = spawnNetworks[i];
                if (CheckLimitList(spawn)) {
                    continue;
                }
                if (spawnList.Contains(spawn) == false) {
                    spawnNetworks.Remove(spawn);
                    i--;
                    removeList.Add(spawn);
                    limitList.Add(new LimitData {
                        Network = spawn, Time = LimitTime,
                    });
                }
            }

            if (removeList.Count > 0) {
                this.networkBase.TargetRpcList(removeList, new Proto_Network.TargetRpc_RemoveWorldNetwork {
                    syncId = ConnID,
                });
            }
        }

        // 有新的玩家进入了同步距离
        private void CheckNetworkInDistance(List<INetworkCharacter> spawnList) {
            sendProto = null;
            var sendNetwork = new List<INetworkCharacter>();
            for (int i = 0; i < spawnList.Count; i++) {
                var spawn = spawnList[i];
                if (CheckLimitList(spawn)) {
                    continue;
                }
                if (spawnNetworks.Contains(spawn) == false) {
                    spawnNetworks.Add(spawn);
                    sendNetwork.Add(spawn);
                    limitList.Add(new LimitData {
                        Network = spawn, Time = LimitTime,
                    });
                }
            }
            SpawnNetData(sendNetwork);
        }

        // 检查是否在 limitList 列表中
        private bool CheckLimitList(INetworkCharacter network) {
            for (int i = 0; i < limitList.Count; i++) {
                var limit = limitList[i];
                if (limit.Network == network) {
                    return true;
                }
            }
            return false;
        }

        // 根据时间移除 limitList 中的数据
        private void UpdateRemoveLimitList() {
            for (int i = 0; i < limitList.Count; i++) {
                var limit = limitList[i];
                limit.Time -= Time.deltaTime;
                if (limit.Time <= 0) {
                    limitList.RemoveAt(i);
                    i--;
                }
            }
        }

        private void SetNetwork(INetworkCharacter networkBase) {
            if (networkBase == null) {
                Debug.LogError("networkBase 没有正确赋值");
                return;
            }
            for (int i = 0; i < allNetworks.Count; i++) {
                var saveNetwork = allNetworks[i];
                if (saveNetwork == networkBase) {
                    return;
                }
            }
            allNetworks.Add(networkBase);
        }

        private void OnDisconnectNetwork(M_Network.DisconnectNetwork ent) {
            for (int i = 0; i < allNetworks.Count; i++) {
                var saveNetwork = allNetworks[i];
                if (saveNetwork.IsDestroy() || saveNetwork.ConnId() == ent.INetwork.ConnId()) {
                    allNetworks.RemoveAt(i);
                }
            }
        }

        private void SpawnNetData(List<INetworkCharacter> networks) {
            if (networks.Count <= 0) {
                return;
            }
            GetSpawnProto();
            if (sendProto == null) {
                Debug.LogError("需要通过 SetSpawnRPC 设置 ITargetRpc 数据");
                return;
            }
            MsgRegister.Dispatcher(new M_Network.RPCSerialize {
                ProtoDoc = sendProto,
                ConnID = ConnID,
                Channel = sendProto.GetChannel(),
                CallBack = (channel, connId, bytes) => {
                    TargetRpcList(networks, new Proto_Network.TargetRpc_SyncSpawnData {
                        connID = connId, spawnData = bytes, connType = (byte)this.mySystem.GetSpawnConnType(),
                    });
                }
            });
            mySystem.Dispatcher(new SE_Network.Event_InDistanceNetwork {
                NetworkList = networks,
            });
        }

        protected void TargetRpcSpawnNetworks(ITargetRpc proto) {
            this.networkBase.TargetRpcList(spawnNetworks, proto);
        }

        private void GetSpawnProto() {
            onGetSpawnProto?.Invoke(this);
        }

        private void OnGetSpawnNetworks(ISystemMsg body, SE_Network.Event_GetSpawnNetworks ent) {
            ent.CallBack.Invoke(spawnNetworks);
        }

        public void SetIsOnline(bool isOnline) {
            this.isOnline = isOnline;
        }
    }
}