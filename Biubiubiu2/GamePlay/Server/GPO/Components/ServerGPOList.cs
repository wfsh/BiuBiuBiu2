using System;
using System.Collections.Generic;
using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOList : ComponentBase {
        private List<IGPO> gpoList = new List<IGPO>();
        private Dictionary<int, IGPO> gpoData = new Dictionary<int, IGPO>();
        private Dictionary<int, List<IGPO>> gpoTypeDataList = new Dictionary<int, List<IGPO>>();
        private Dictionary<int, List<IGPO>> gpoTeamDataList = new Dictionary<int, List<IGPO>>();
        private int perf_GPOCount = PerfAnalyzerKey.StringToHash("GPO 数量");
        private int perf_MonsterCount = PerfAnalyzerKey.StringToHash("野生宠物数量");
        private int perf_MasterMonsterCount = PerfAnalyzerKey.StringToHash("宠物数量");
        private float perfDeltaTime = 0.0f;

        protected override void OnAwake() {
            MsgRegister.Register<SM_GPO.RemoveGPO>(OnRemoveGPOCallBack);
            MsgRegister.Register<SM_GPO.AddGPO>(OnAddGPOCallBack);
            MsgRegister.Register<SM_GPO.GetGPOList>(OnGetGPOListCallBack);
            MsgRegister.Register<SM_GPO.GetGPOListForTeamId>(OnGetGPOListForTeamIdCallBack);
            MsgRegister.Register<SM_GPO.GetGPOListForGpoType>(OnGetGPOListForGpoTypeCallBack);
            MsgRegister.Register<SM_GPO.GetGPOListForRoleAndAI>(OnGetGPOListForRoleAndAICallBack);
            MsgRegister.Register<SM_GPO.GetGPO>(OnGetGPOCallBack);
            MsgRegister.Register<SM_GPO.GetGPOForCharacterNetwork>(GetGPOForCharacterNetwork);
        }
        
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            gpoData.Clear();
            gpoList.Clear();
            gpoTypeDataList.Clear();
            gpoTeamDataList.Clear();
            RemoveUpdate(OnUpdate);
            MsgRegister.Unregister<SM_GPO.RemoveGPO>(OnRemoveGPOCallBack);
            MsgRegister.Unregister<SM_GPO.AddGPO>(OnAddGPOCallBack);
            MsgRegister.Unregister<SM_GPO.GetGPOList>(OnGetGPOListCallBack);
            MsgRegister.Unregister<SM_GPO.GetGPOListForTeamId>(OnGetGPOListForTeamIdCallBack);
            MsgRegister.Unregister<SM_GPO.GetGPOListForGpoType>(OnGetGPOListForGpoTypeCallBack);
            MsgRegister.Unregister<SM_GPO.GetGPOListForRoleAndAI>(OnGetGPOListForRoleAndAICallBack);
            MsgRegister.Unregister<SM_GPO.GetGPO>(OnGetGPOCallBack);
            MsgRegister.Unregister<SM_GPO.GetGPOForCharacterNetwork>(GetGPOForCharacterNetwork);
        }

        private void OnUpdate(float deltaTime) {
            SavePerfanalyzer();
        }

        private void SavePerfanalyzer() {
            if (perfDeltaTime >= 0f) {
                perfDeltaTime -= Time.deltaTime;
                return;
            }
            perfDeltaTime = 0.3f;
            var monsterCount = 0;
            var masterMonsterCount = 0;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                switch (gpo.GetGPOType()) {
                    case GPOData.GPOType.AI:
                        monsterCount++;
                        break;
                    case GPOData.GPOType.MasterAI:
                        masterMonsterCount++;
                        break;
                }
            }
            PerfAnalyzerAgent.SetCustomRecorder(perf_GPOCount, gpoList.Count);
            PerfAnalyzerAgent.SetCustomRecorder(perf_MonsterCount, monsterCount);
            PerfAnalyzerAgent.SetCustomRecorder(perf_MasterMonsterCount, masterMonsterCount);
        }

        private void OnAddGPOCallBack(SM_GPO.AddGPO ent) {
            if (gpoData.ContainsKey(ent.IGpo.GetGpoID())) {
                Debug.LogError("GpoID 为唯一 ID，不能重复");
                return;
            }
            gpoData.Add(ent.IGpo.GetGpoID(), ent.IGpo);
            gpoList.Add(ent.IGpo);
            var gpoTypeList = GetGPOListForGpoType(ent.IGpo.GetGPOType());
            gpoTypeList.Add(ent.IGpo);
            var gpoTeamList = GetGPOListForTeamId(ent.IGpo.GetTeamID());
            gpoTeamList.Add(ent.IGpo);
            MsgRegister.Dispatcher(new SM_GPO.AddGPOEnd {
                IGpo = ent.IGpo,
            });
            OnAddGPO(ent.IGpo);
        }

        virtual protected void OnAddGPO(IGPO gpo) {
        }
        
        protected List<IGPO> GetGPOListForGpoType(GPOData.GPOType gpoType) {
            var gpoTypeInt = (int)gpoType;
            if (gpoTypeDataList.TryGetValue(gpoTypeInt, out var gpoList)) {
                return gpoList;
            }
            gpoList = new List<IGPO>();
            gpoTypeDataList.Add(gpoTypeInt, gpoList);
            return gpoList;
        }
        
        protected List<IGPO> GetGPOListForTeamId(int teamId) {
            if (gpoTeamDataList.TryGetValue(teamId, out var gpoList)) {
                return gpoList;
            }
            gpoList = new List<IGPO>();
            gpoTeamDataList.Add(teamId, gpoList);
            return gpoList;
        }
        
        private void OnRemoveGPOCallBack(SM_GPO.RemoveGPO ent) {
            RemoveGPO(ent.GpoId);
        }

        private void RemoveGPO(int gpoId) {
            IGPO removeGpo = null;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.GetGpoID() == gpoId) {
                    gpoList.RemoveAt(i);
                    removeGpo = gpo;
                    break;
                }
            }
            gpoData.Remove(gpoId);
            if (removeGpo != null) {
                RemoveGPOForGpoType(removeGpo.GetGPOType(), gpoId);
                RemoveGPOForTeamId(removeGpo.GetTeamID(), gpoId);
                MsgRegister.Dispatcher(new SM_GPO.RemoveGPOEnd {
                    IGpo = removeGpo,
                });
                OnRemoveGPO(gpoId);
            }
        }

        virtual protected void OnRemoveGPO(int gpoId) {
        }
        
        private void RemoveGPOForGpoType(GPOData.GPOType gpoType, int gpoId) {
            var gpoTypeInt = (int)gpoType;
            if (gpoTypeDataList.TryGetValue(gpoTypeInt, out var gpoTypeList)) {
                for (int i = 0; i < gpoTypeList.Count; i++) {
                    var gpo = gpoTypeList[i];
                    if (gpo.GetGpoID() != gpoId) {
                        continue;
                    }
                    gpoTypeList.RemoveAt(i);
                    return;
                }
            }
        }
        
        private void RemoveGPOForTeamId(int teamId, int gpoId) {
            if (gpoTeamDataList.TryGetValue(teamId, out var gpoList)) {
                for (int i = 0; i < gpoList.Count; i++) {
                    var gpo = gpoList[i];
                    if (gpo.GetGpoID() != gpoId) {
                        continue;
                    }
                    gpoList.RemoveAt(i);
                    return;
                }
            }
        }

        private void OnGetGPOListCallBack(SM_GPO.GetGPOList ent) {
            ent.CallBack.Invoke(gpoList);
        }
        
        private void OnGetGPOListForTeamIdCallBack(SM_GPO.GetGPOListForTeamId ent) {
            var gpoList = GetGPOListForTeamId(ent.TeamId);
            ent.CallBack.Invoke(gpoList);
        }

        private void OnGetGPOListForGpoTypeCallBack(SM_GPO.GetGPOListForGpoType ent) {
            var gpoList = GetGPOListForGpoType(ent.GpoType);
            ent.CallBack.Invoke(gpoList);
        }
        
        private void OnGetGPOListForRoleAndAICallBack(SM_GPO.GetGPOListForRoleAndAI ent) {
            var gpoTypeList = GetGPOListForGpoType(GPOData.GPOType.Role);
            var aiTypeList = GetGPOListForGpoType(GPOData.GPOType.RoleAI);
            var list = new List<IGPO>(gpoTypeList.Count + aiTypeList.Count);
            list.AddRange(gpoTypeList);
            list.AddRange(aiTypeList);
            ent.CallBack.Invoke(list);
        }
        
        private void GetGPOForCharacterNetwork(SM_GPO.GetGPOForCharacterNetwork ent) {
            var gpoList = GetGPOListForGpoType(GPOData.GPOType.Role);
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.GetNetwork().ConnId() == ent.ConnId) {
                    ent.CallBack(gpo);
                    return;
                }
            }
            ent.CallBack.Invoke(null);
        }

        private void OnGetGPOCallBack(SM_GPO.GetGPO ent) {
            if (gpoData.TryGetValue(ent.GpoId, out var gpo)) {
                ent.CallBack.Invoke(gpo);
            } else {
                ent.CallBack.Invoke(null);
            }
        }
    }
}