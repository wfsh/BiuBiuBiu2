using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sofunny.BiuBiuBiu2.Stage {
    public class StageManager {
        private AsyncOperation async;
        
        private struct LoadData {
            public LoadSceneMode LoadMode;
            public string Sign;
            public bool IsActive;
            public Action LoadEndCallBack;
        }

        private LoadData nowLoadData;
        private List<LoadData> loadList;

        public void Init() {
            loadList = new List<LoadData>(5);
            MsgRegister.Register<M_Stage.LoadScene>(LoadStageCallBack);
            UpdateRegister.AddUpdate(OnUpdate);
        }

        public void Clear() {
            loadList.Clear();
            loadList = null;
            MsgRegister.Unregister<M_Stage.LoadScene>(LoadStageCallBack);
            UpdateRegister.RemoveUpdate(OnUpdate);
        }

        public void OnUpdate(float delta) {
            if (async == null) {
                return;
            }
            if (async.isDone) {
                async = null;
                if (nowLoadData.IsActive) {
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(nowLoadData.Sign));  
                }
                nowLoadData.LoadEndCallBack?.Invoke();
                nowLoadData.Sign = "";
            }

            if (async == null && loadList.Count > 0) {
                var loadData = loadList[0];
                loadList.RemoveAt(0);
                LoadScene(loadData);
            }
        }

        private void LoadStageCallBack(M_Stage.LoadScene ent) {
            var loadData = new LoadData();
            loadData.Sign = ent.Sign;
            loadData.LoadMode = ent.LoadMode;
            loadData.IsActive = ent.IsActive;
            loadData.LoadEndCallBack = ent.LoadEndCallBack;
            if (async == null) {
                LoadScene(loadData);
            } else {
                loadList.Add(loadData);
            }
        }

        private void LoadScene(LoadData loadData) {
            PerfAnalyzerAgent.LoginScene(loadData.Sign);
            Debug.LogFormat("加载场景-->{0}  loadMode： {1}", loadData.Sign, loadData.LoadMode);
            nowLoadData = loadData;
            AssetManager.LoadSceneAB(loadData.Sign, () => {
                async = SceneManager.LoadSceneAsync(loadData.Sign, loadData.LoadMode);
            });
        }
    }
}