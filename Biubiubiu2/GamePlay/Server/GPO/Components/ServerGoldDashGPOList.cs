using System;
using System.Collections.Generic;
using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashGPOList : ServerGPOList {
        private Dictionary<int, IGPO> characterGPOData = new Dictionary<int, IGPO>();
        private Dictionary<int, IGPO> aiCharacterGPOData = new Dictionary<int, IGPO>();
        private long playerIdCache;
        private int aiIdCache;
        
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Sausage.GetSausageAI>(OnGetSausageAICallBack);
            MsgRegister.Register<SM_GPO.GetCharacterGPOByPlayerId>(OnGetCharacterGPOCallBack);
            MsgRegister.Register<SM_GPO.GetAICharacterGPOByAiId>(OnGetAICharacterGPOCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_Sausage.GetSausageAI>(OnGetSausageAICallBack);
            MsgRegister.Unregister<SM_GPO.GetCharacterGPOByPlayerId>(OnGetCharacterGPOCallBack);
            MsgRegister.Unregister<SM_GPO.GetAICharacterGPOByAiId>(OnGetAICharacterGPOCallBack);
            characterGPOData.Clear();
            aiCharacterGPOData.Clear();
        }

        protected override void OnAddGPO(IGPO gpo) {
            base.OnAddGPO(gpo);
            switch (gpo.GetGPOType()) {
                case GPOData.GPOType.Role:
                    characterGPOData.Add(gpo.GetGpoID(), gpo);
                    break;
                case GPOData.GPOType.RoleAI:
                    aiCharacterGPOData.Add(gpo.GetGpoID(), gpo);
                    break;
            }
        }

        protected override void OnRemoveGPO(int gpoId) {
            base.OnRemoveGPO(gpoId);
            characterGPOData.Remove(gpoId);
            aiCharacterGPOData.Remove(gpoId);
        }

        private void OnGetSausageAICallBack(SM_Sausage.GetSausageAI ent) {
            var pointList = new List<Vector3>();
            var idList = new List<int>();
            var gpoList = GetGPOListForGpoType(GPOData.GPOType.AI);
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (Vector3.Distance(gpo.GetPoint(), ent.NowPoint) < ent.Range) {
                    pointList.Add(gpo.GetPoint());
                    idList.Add(gpo.GetGpoID());
                }
            }
            ent.CallBack.Invoke(pointList, idList);
        }

        private void OnGetCharacterGPOCallBack(SM_GPO.GetCharacterGPOByPlayerId ent) {
            IGPO resultGPO = null;
            foreach (var item in characterGPOData) {
                IGPO gpo = item.Value;
                gpo.Dispatcher(new SE_Character.GetPlayerId() {
                    CallBack = SetCharacterGPOCache
                });
                if (playerIdCache == ent.PlayerId) {
                    resultGPO = gpo;
                }
            }
            ent.CallBack.Invoke(resultGPO);
            playerIdCache = 0;
        }

        private void SetCharacterGPOCache(long result) {
            playerIdCache = result;
        }

        private void OnGetAICharacterGPOCallBack(SM_GPO.GetAICharacterGPOByAiId ent) {
            IGPO resultGPO = null;
            foreach (var item in aiCharacterGPOData) {
                IGPO gpo = item.Value;
                gpo.Dispatcher(new SE_AICharacter.GetAiId() {
                    CallBack = id => aiIdCache = id
                });
                if (aiIdCache == ent.AiId) {
                    resultGPO = gpo;
                }
            }

            ent.CallBack.Invoke(resultGPO);
            aiIdCache = 0;
        }
    }
}