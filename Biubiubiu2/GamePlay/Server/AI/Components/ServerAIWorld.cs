using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public partial class ServerAIWorld : ComponentBase {
        private Dictionary<int, S_AI_Base> aiDatas = new Dictionary<int, S_AI_Base>();
        private List<S_AI_Base> aiList = new List<S_AI_Base>();

        protected override void OnAwake() {
            MsgRegister.Register<SM_AI.Event_AddAI>(OnAddAICallBack);
            MsgRegister.Register<SM_AI.Event_AddMasterAI>(OnAddMasterAICallBack);
            MsgRegister.Register<SM_AI.Event_RemoveAI>(OnRemoveAICallBack);
            MsgRegister.Register<SM_Character.CharacterLogin>(OnCharacterLogin);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_AI.Event_AddAI>(OnAddAICallBack);
            MsgRegister.Unregister<SM_AI.Event_AddMasterAI>(OnAddMasterAICallBack);
            MsgRegister.Unregister<SM_AI.Event_RemoveAI>(OnRemoveAICallBack);
            MsgRegister.Unregister<SM_Character.CharacterLogin>(OnCharacterLogin);
            RemoveAllRole();
        }

        private void OnCharacterLogin(SM_Character.CharacterLogin ent) {
            if (ent.INetwork == null) {
                Debug.LogError("networkBase 没有正确赋值");
                return;
            }
            var aiPids = new int[aiList.Count];
            for (int i = 0; i < aiList.Count; i++) {
                aiPids[i] = aiList[i].GpoId;
            }
            TargetRpc(ent.INetwork, new Proto_AI.TargetRpc_InWorldAIList {
                aiPids = aiPids
            });
        }
        
        private void OnAddAICallBack(SM_AI.Event_AddAI ent) {
            var gpoMData = GpoSet.GetGPOMData(ent.AISign, ModeData.MatchId);
            if (gpoMData == null || gpoMData.GetId() == 0) {
                gpoMData = GpoSet.GetGPOMData(ent.AISign);
            }
            if (gpoMData.GetId() == 0) {
                Debug.LogError($"Server 缺少匹配模式 {ModeData.MatchId} 的怪物数据 AISign:" + ent.AISign);
                return;
            }
            var skinSign = gpoMData.GetAssetSign();
            if (string.IsNullOrEmpty(ent.OR_AISkinSign) == false) {
                skinSign = ent.OR_AISkinSign;
            }
            var teamId = ent.OR_TeamId > 0 ? ent.OR_TeamId : GPOData.GPOTeamID_AI;
            var gpoType = ent.OR_GpoType == GPOData.GPOType.NULL ? GPOData.GPOType.AI : ent.OR_GpoType;
            var ai = AddAI(gpoMData, ent.OR_InData, ent.OR_GpoId, teamId, gpoType, skinSign, null);
            ai.SetStartPoint(ent.StartPoint);
            ai.SetStartRota(ent.OR_StartRota);
            ent.OR_CallBack?.Invoke(ai);
        }

        private void OnAddMasterAICallBack(SM_AI.Event_AddMasterAI ent) {
            var gpoId = 0;
            var aiSign = ent.OR_AIData != null ? ent.OR_AIData.Sign : ent.AISign;
            var gpoMData = GpoSet.GetGPOMData(aiSign, ModeData.MatchId);
            if (gpoMData == null || gpoMData.GetId() == 0) {
                gpoMData = GpoSet.GetGPOMData(aiSign);
            }
            if (gpoMData.GetId() == 0) {
                Debug.LogError($"Server 缺少匹配模式 {ModeData.MatchId} 的怪物数据 AISign:" + aiSign);
                return;
            }
            var skinSign = gpoMData.GetAssetSign();
            if (ent.OR_AIData != null) {
                gpoId = ent.OR_AIData.GpoId;
                if (string.IsNullOrEmpty(ent.OR_AIData.SkinSign) == false) {
                    skinSign = ent.OR_AIData.SkinSign;
                } else {
                    ent.OR_AIData.SkinSign = skinSign;
                }
            } else {
                if (string.IsNullOrEmpty(ent.OR_AISkinSign) == false) {
                    skinSign = ent.OR_AISkinSign;
                }
            }
            var ai = AddAI(
                gpoMData, 
                ent.OR_InData, 
                gpoId, 
                ent.MasterGPO.GetTeamID(), 
                GPOData.GPOType.MasterAI, 
                skinSign, 
                ent.MasterGPO,
                ent.OR_AIData);

            ai.SetStartPoint(ent.StartPoint);
            ai.SetStartRota(ent.OR_StartRota);
            ent.OR_CallBack?.Invoke(ai);
        }

        private void OnRemoveAICallBack(SM_AI.Event_RemoveAI ent) {
            RemoveAI(ent.GpoId);
        }

        private S_AI_Base AddAI(
            IGPOM gpoMData, 
            IGPOInData inData, 
            int gpoId,  
            int teamId, 
            GPOData.GPOType gpoType,  
            string skinSign,
            IGPO masteriGpo,
            GPOData.AttributeData aiData = null) {
            var system = AddAIForGpoMTypeId(gpoMData.GetGpoType(), gpoMData.GetId(), delegate(S_AI_Base system) {
                system.SetNetwork(this.networkBase);
                system.SetAIData(gpoMData, inData, gpoId, teamId, gpoType, skinSign, masteriGpo);
                if (aiData != null) {
                    system.SetAttributeData(aiData);
                }
                aiList.Add(system);
                aiDatas.Add(system.GpoId, system);
            });
            return system;
        }
        
        private S_AI_Base GetAI(int gpoId) {
            S_AI_Base aiBase;
            if (aiDatas.TryGetValue(gpoId, out aiBase)) {
                return aiBase;
            }
            return null;
        }

        private void RemoveAI(int gpoId) {
            for (int i = 0; i < aiList.Count; i++) {
                var ai = aiList[i];
                if (ai.GpoId == gpoId) {
                    ai.Clear();
                    aiList.RemoveAt(i);
                    aiDatas.Remove(ai.GpoId);
                }
            }
        }

        private void RemoveAllRole() {
            for (int i = 0; i < aiList.Count; i++) {
                var ai = aiList[i];
                ai.Clear();
            }
            aiList.Clear();
            aiDatas.Clear();
        }
    }
}
