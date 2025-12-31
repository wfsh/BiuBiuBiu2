using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public partial class ClientAIWorld : ComponentBase {
        private Dictionary<int, C_AI_Base> aiData = new Dictionary<int, C_AI_Base>();
        private List<IAI> aiList = new List<IAI>();
        private List<C_AI_Base> removeAIList = new List<C_AI_Base>();
        private ClientAISystem system;

        protected override void OnAwake() {
            MsgRegister.Register<CM_Network.SpawnWorldNetwork>(OnSpawnWorldNetworkCallBack);
            MsgRegister.Register<CM_AI.Event_GetAIList>(OnGetAIListCallBack);
        }

        protected override void OnStart() {
        }

        protected override void OnClear() {
            removeAIList.Clear();
            ClearAllAI();
            RemoveProtoCallBack(Proto_AI.TargetRpc_InWorldAIList.ID, OnTargetRpcInWorldAIList);
            MsgRegister.Unregister<CM_Network.SpawnWorldNetwork>(OnSpawnWorldNetworkCallBack);
            MsgRegister.Unregister<CM_AI.Event_GetAIList>(OnGetAIListCallBack);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_AI.TargetRpc_InWorldAIList.ID, OnTargetRpcInWorldAIList);
        }

        private void OnTargetRpcInWorldAIList(INetwork network, IProto_Doc protoDoc) {
            var rpcData = (Proto_AI.TargetRpc_InWorldAIList)protoDoc;
            removeAIList.Clear();
            for (int i = 0; i < aiList.Count; i++) {
                var ai = (C_AI_Base)aiList[i];
                var hasAI = false;
                foreach (var pid in rpcData.aiPids) {
                    if (pid == ai.GetGPOId()) {
                        hasAI = true;
                        break;
                    }
                }
                if (hasAI == false) {
                    removeAIList.Add(ai);
                }
            }
            for (int i = 0; i < removeAIList.Count; i++) {
                var ai = removeAIList[i];
                ai.Dispatcher(new CE_GPO.Event_StartRemoveGPO());
            }
        }

        private void OnSpawnWorldNetworkCallBack(CM_Network.SpawnWorldNetwork ent) {
            if (ent.ConnType != NetworkData.SpawnConnType.AI) {
                return;
            }
            var rpcData = (Proto_AI.TargetRpc_AddAI)ent.ProtoDoc;
            if (rpcData.gpoId <= 0) {
                Debug.LogError("ClientAIWorld 下发的 AI 对象 GPO ID 不能 <= 0 MID:" + rpcData.gpoMId + " SkinSign:" + rpcData.aiSkinSign);
                return;
            }
            var system = GetAI(rpcData.gpoId);
            if (system != null) {
                system.SetConnID(ent.ConnID);
                system.SetNetwork(networkBase);
            } else {
                var gpoMData = GpoSet.GetGPOMData(rpcData.gpoMId, ModeData.MatchId);
                if (gpoMData == null || gpoMData.GetId() == 0) {
                    gpoMData = GpoSet.GetGPOMData(rpcData.gpoMId);
                }
                if (gpoMData.GetId() == 0) {
                    Debug.LogError($"Client 缺少匹配模式 {ModeData.MatchId} 的怪物数据 gpoMId:" + rpcData.gpoMId);
                    return;
                }
                var inData = GetProtoInData(rpcData.protoDoc);
                system = AddSystem(gpoMData.GetGpoType(), gpoMData.GetId(), delegate(C_AI_Base aiBase) {
                    aiBase.Register<CE_AI.RemoveAI>(OnRemoveAICallBack);
                    aiBase.SetAIData(gpoMData, inData, rpcData.gpoId, rpcData.teamId, rpcData.aiSkinSign, rpcData.startPoint, rpcData.startRota);
                    aiBase.SetConnID(ent.ConnID);
                    aiBase.SetNetwork(networkBase);
                });
                aiData.Add(rpcData.gpoId, system);
                aiList.Add(system);
            }
        }

        private IProto_Doc GetProtoInData(byte[] data) {
            if (data.Length == 0) {
                return null;
            }
            IProto_Doc protoDoc = null;
            MsgRegister.Dispatcher(new M_Network.RPCUnSerialize {
                Datas = data,
                ConnID = 0,
                CallBack = (cid, proto) => {
                    protoDoc = proto;
                },
            });
            return protoDoc;
        }

        private void ClearAllAI() {
            for (int i = 0; i < aiList.Count; i++) {
                var ai = (C_AI_Base)aiList[i];
                ai.Clear();
            }
            aiList.Clear();
            aiData.Clear();
        }

        private void OnGetAIListCallBack(CM_AI.Event_GetAIList ent) {
            ent.CallBack(aiList);
        }        
        private void OnRemoveAICallBack(ISystemMsg body, CE_AI.RemoveAI ent) {
            C_AI_Base system;
            if (aiData.TryGetValue(ent.GpoId, out system)) {
                system.Clear();
                aiList.Remove(system);
                aiData.Remove(ent.GpoId);
            }
        }

        private C_AI_Base GetAI(int gpoId) {
            C_AI_Base system;
            if (aiData.TryGetValue(gpoId, out system)) {
                return system;
            }
            return null;
        }
    }
}