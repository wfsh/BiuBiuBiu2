using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashSceneStageLoad : ComponentBase {
        private AsyncOperation async;
        
        private struct LoadData {
            public LoadSceneMode LoadMode;
            public string Sign;
            public bool IsActive;
        }

        private LoadData loadData;
        private bool isLoadSceneIng = false;

        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnStart() {
            if (SceneData.HasSceneData(ModeData.SceneId) == false) {
                return;
            }
            AddUpdate(OnUpdate);
            StartMode();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
        }

        public void OnUpdate(float delta) {
            if (isLoadSceneIng == false) {
                return;
            }
            if (async.isDone) {
                async = null;
                Debug.LogFormat("加载服务器场景 完成-->{0}", loadData.Sign);
                if (loadData.IsActive) {
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadData.Sign));  
                }
                MsgRegister.Dispatcher(new M_Game.LoginGameScene());
                loadData.Sign = "";
                isLoadSceneIng = false;
            }
        }
        private void StartMode() {
            var data = SceneData.Get(ModeData.SceneId);
            if (isLoadSceneIng) {
                Debug.LogError("正在加载场景中 :" + data.StageSign);
                return;
            }
            Debug.LogFormat("加载服务器场景-->{0}  loadMode： {1}", data.StageSign, loadData.LoadMode);
            loadData = new LoadData();
            loadData.Sign = data.StageSign;
            loadData.LoadMode = LoadSceneMode.Additive;
            loadData.IsActive = false;
            LoadScene();
        }

        private void LoadScene() {
            var url = string.Concat("Server", loadData.Sign);
            AssetManager.LoadSceneAB(url, () => {
                isLoadSceneIng = true;
                async = SceneManager.LoadSceneAsync(url, loadData.LoadMode);
            });
        }
    }
}