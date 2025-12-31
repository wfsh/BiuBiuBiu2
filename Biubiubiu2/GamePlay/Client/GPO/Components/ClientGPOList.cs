using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPOList : ComponentBase {
        protected List<IGPO> gpoList = new List<IGPO>();
        private Dictionary<int, IGPO> gpoData = new Dictionary<int, IGPO>();
        private IGPO localGPO;
        private IGPO lookGPO;
        private int perf_RoleCount = PerfAnalyzerKey.StringToHash("角色数量");
        private int perf_MonsterCount = PerfAnalyzerKey.StringToHash("野生宠物数量");
        private int perf_MasterMonsterCount = PerfAnalyzerKey.StringToHash("宠物数量");
        private float perfDeltaTime = 0.0f;

        protected override void OnAwake() {
            MsgRegister.Register<CM_GPO.AddGPO>(OnAddGPOCallBack);
            MsgRegister.Register<CM_GPO.RemoveGPO>(OnRemoveGPOCallBack);
            MsgRegister.Register<CM_GPO.GetGPOList>(OnGetGPOListCallBack);
            MsgRegister.Register<CM_GPO.GetGPO>(OnGetGPOCallBack);
            MsgRegister.Register<CM_GPO.GetLocalGPO>(OnGetLocalGPOCallBack);
            MsgRegister.Register<CM_GPO.GetLookGPO>(OnGetLookGPOCallBack);
            MsgRegister.Register<CM_GPO.AddLocalGPO>(OnAddLocalGPOCallBack);
            MsgRegister.Register<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            gpoData.Clear();
            gpoList.Clear();
            localGPO = null;
            lookGPO = null;
            RemoveUpdate(OnUpdate);
            MsgRegister.Unregister<CM_GPO.AddGPO>(OnAddGPOCallBack);
            MsgRegister.Unregister<CM_GPO.RemoveGPO>(OnRemoveGPOCallBack);
            MsgRegister.Unregister<CM_GPO.GetGPOList>(OnGetGPOListCallBack);
            MsgRegister.Unregister<CM_GPO.GetGPO>(OnGetGPOCallBack);
            MsgRegister.Unregister<CM_GPO.GetLocalGPO>(OnGetLocalGPOCallBack);
            MsgRegister.Unregister<CM_GPO.GetLookGPO>(OnGetLookGPOCallBack);
            MsgRegister.Unregister<CM_GPO.AddLocalGPO>(OnAddLocalGPOCallBack);
            MsgRegister.Unregister<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
        }

        private void OnUpdate(float deltaTime) {
            SavePerfanalyzer();
        }

        private void SavePerfanalyzer() {
            if (NetworkData.IsStartServer) {
                return;
            }
            if (perfDeltaTime >= 0f) {
                perfDeltaTime -= Time.deltaTime;
                return;
            }
            perfDeltaTime = 0.3f;
            var roleCount = 0;
            var monsterCount = 0;
            var masterMonsterCount = 0;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                switch (gpo.GetGPOType()) {
                    case GPOData.GPOType.Role:
                        roleCount++;
                        break;
                    case GPOData.GPOType.AI:
                        monsterCount++;
                        break;
                    case GPOData.GPOType.MasterAI:
                        masterMonsterCount++;
                        break;
                }
            }
            PerfAnalyzerAgent.SetCustomRecorder(perf_RoleCount, roleCount);
            PerfAnalyzerAgent.SetCustomRecorder(perf_MonsterCount, monsterCount);
            PerfAnalyzerAgent.SetCustomRecorder(perf_MasterMonsterCount, masterMonsterCount);
        }

        private void OnGetLocalGPOCallBack(CM_GPO.GetLocalGPO ent) {
            ent.CallBack.Invoke(localGPO);
        }

        private void OnAddLocalGPOCallBack(CM_GPO.AddLocalGPO ent) {
            localGPO = ent.LocalGPO;
        }

        private void OnGetLookGPOCallBack(CM_GPO.GetLookGPO ent) {
            ent.CallBack.Invoke(lookGPO);
        }

        private void OnAddLookGPOCallBack(CM_GPO.AddLookGPO ent) {
            lookGPO = ent.LookGPO;
        }

        private void OnAddGPOCallBack(CM_GPO.AddGPO ent) {
            IGPO saveGPO = null;
            if (gpoData.TryGetValue(ent.IGpo.GetGpoID(), out saveGPO)) {
                Debug.LogError("GpoID 为唯一 ID，不能重复:" + ent.IGpo.GetGpoID() + " " +
                               ent.IGpo.GetGPOType() + " SaveGPO: " + saveGPO.GetGpoID() + " " +
                               saveGPO.GetGPOType() + " 是否相同：" + (ent.IGpo == saveGPO) + " 名字：" + ent.IGpo.GetName());
                return;
            }
            gpoData.Add(ent.IGpo.GetGpoID(), ent.IGpo);
            gpoList.Add(ent.IGpo);
        }

        private void OnRemoveGPOCallBack(CM_GPO.RemoveGPO ent) {
            RemoveGPO(ent.GpoId);
        }

        private void RemoveGPO(int gpoId) {
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.GetGpoID() == gpoId) {
                    gpoList.RemoveAt(i);
                    break;
                }
            }
            gpoData.Remove(gpoId);
        }

        private void OnGetGPOListCallBack(CM_GPO.GetGPOList ent) {
            ent.CallBack.Invoke(gpoList);
        }

        private void OnGetGPOCallBack(CM_GPO.GetGPO ent) {
            IGPO gpo;
            if (gpoData.TryGetValue(ent.GpoId, out gpo)) {
                ent.CallBack.Invoke(gpo);
            } else {
                ent.CallBack.Invoke(null);
            }
        }
    }
}