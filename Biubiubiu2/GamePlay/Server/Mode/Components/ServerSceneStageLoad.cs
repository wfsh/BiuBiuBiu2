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
    public class ServerSceneStageLoad : ComponentBase {
        private StageData.LoadEnum loadState = StageData.LoadEnum.None;
        private int modeId;
        private bool isLoadScene = false;
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Mode.StartMode>(OnStartModeCallBack);
            MsgRegister.Register<M_Stage.TaskLoadStart>(OnTaskLoadStartCallBack);
        }

        protected override void OnClear() {
            MsgRegister.Unregister<SM_Mode.StartMode>(OnStartModeCallBack);
            MsgRegister.Unregister<M_Stage.TaskLoadStart>(OnTaskLoadStartCallBack);
        }

        private void OnTaskLoadStartCallBack(M_Stage.TaskLoadStart ent) {
            if (ent.loadState != StageData.LoadEnum.AddServerGameScene) {
                return;
            }
            loadState = ent.loadState;
            Debug.Log("等待进行场景加载");
            LoadScene();
        }

        private void OnStartModeCallBack(SM_Mode.StartMode ent) {
            modeId = ModeData.ModeId;
            LoadScene();
        }

        private void LoadScene() {
            if (modeId == default || 
                ModeData.IsSausageMode() ||
                loadState != StageData.LoadEnum.AddServerGameScene || 
                isLoadScene) {
                return;
            }
            isLoadScene = true;
            var sceneData = SceneData.Get(ModeData.SceneId);
            Debug.Log("LoadScene ModeId:" + modeId + " -- " + sceneData.StageSign);
            MsgRegister.Dispatcher(new M_Stage.LoadScene {
                IsActive = true,
                LoadMode = LoadSceneMode.Additive,
                Sign = StageData.GetServerStage(sceneData.StageSign),
                LoadEndCallBack = () => {
                    MsgRegister.Dispatcher(new M_Game.LoginGameScene());
                    MsgRegister.Dispatcher(new M_Stage.TaskLoadEnd {
                        loadState = StageData.LoadEnum.AddServerGameScene,
                    });
                }
            });
        }
    }
}