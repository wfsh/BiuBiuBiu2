using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPOSpawnerWaveMainLoop : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public string configSign;
        }
        private string configSign = "";
        private AIGpoSpawnerConfig config = null;

        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            configSign = initData.configSign;
        }

        protected override void OnStart() {
            base.OnStart();
            LoadConfig();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_GPOSpawnerWave.Rpc_IntoNextWave.ID, OnRpcIntoNextWave);
            RemoveProtoCallBack(Proto_GPOSpawnerWave.TargetRpc_SyncData.ID, OnTargetRpcSyncData);
            RemoveProtoCallBack(Proto_GPOSpawnerWave.Rpc_SpawnerGpo.ID, OnRpcSpawnerGpo);
            RemoveProtoCallBack(Proto_GPOSpawnerWave.Rpc_DelayWaveSpawnTime.ID, OnRpcDelayWaveSpawnTime);
            RemoveProtoCallBack(Proto_GPOSpawnerWave.Rpc_DeadGpo.ID, OnRpcDeadGpo);
            RemoveProtoCallBack(Proto_GPOSpawnerWave.Rpc_WaveEnd.ID, OnRpcWaveEnd);
            RemoveProtoCallBack(Proto_GPOSpawnerWave.Rpc_IntoWave.ID, OnRpcIntoWave);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_GPOSpawnerWave.Rpc_IntoNextWave.ID, OnRpcIntoNextWave);
            AddProtoCallBack(Proto_GPOSpawnerWave.TargetRpc_SyncData.ID, OnTargetRpcSyncData);
            AddProtoCallBack(Proto_GPOSpawnerWave.Rpc_SpawnerGpo.ID, OnRpcSpawnerGpo);
            AddProtoCallBack(Proto_GPOSpawnerWave.Rpc_DelayWaveSpawnTime.ID, OnRpcDelayWaveSpawnTime);
            AddProtoCallBack(Proto_GPOSpawnerWave.Rpc_DeadGpo.ID, OnRpcDeadGpo);
            AddProtoCallBack(Proto_GPOSpawnerWave.Rpc_WaveEnd.ID, OnRpcWaveEnd);
            AddProtoCallBack(Proto_GPOSpawnerWave.Rpc_IntoWave.ID, OnRpcIntoWave);
        }
        private void LoadConfig() {
            AssetManager.LoadAISO($"GpoSpawner/{configSign}", so => {
                if (isClear || config != null) {
                    return;
                }
                config = (AIGpoSpawnerConfig)so;
            });
        }
        
        private void OnRpcIntoWave(INetwork network, IProto_Doc proto) {
            var msg = (Proto_GPOSpawnerWave.Rpc_IntoWave)proto;
            MsgRegister.Dispatcher(new CM_UI.ShowToast {
                Message = $"进入第 {msg.currentWaveIndex} 回合"
            });
        }
        
        private void OnRpcIntoNextWave(INetwork network, IProto_Doc proto) {
            var msg = (Proto_GPOSpawnerWave.Rpc_IntoNextWave)proto;
            // MsgRegister.Dispatcher(new CM_UI.ShowToast {
            //     Message = $"准备进入第 {msg.currentWaveIndex} 回合"
            // });
        }
        
        private void OnTargetRpcSyncData(INetwork network, IProto_Doc proto) {
        }
        
        private void OnRpcSpawnerGpo(INetwork network, IProto_Doc proto) {
            var msg = (Proto_GPOSpawnerWave.Rpc_SpawnerGpo)proto;
            var gpoMData = GpoSet.GetGpoById(msg.gpoMId);
            // Debug.Log("客户端收到生成 GPO 消息 GpoId:" + msg.gpoId + " Name:" + gpoMData.Name);
        }
        
        private void OnRpcDelayWaveSpawnTime(INetwork network, IProto_Doc proto) {
            var msg = (Proto_GPOSpawnerWave.Rpc_DelayWaveSpawnTime)proto;
            MsgRegister.Dispatcher(new CM_UI.ShowToast {
                Message = $"等待进入下一回合 {msg.time} 秒"
            });
        }
        
        private void OnRpcDeadGpo(INetwork network, IProto_Doc proto) {
            var msg = (Proto_GPOSpawnerWave.Rpc_DeadGpo)proto;
            // Debug.Log("客户端收到 GPO 死亡消息 GpoId:" + msg.gpoId);
        }

        private void OnRpcWaveEnd(INetwork network, IProto_Doc proto) {
        }
    }
}