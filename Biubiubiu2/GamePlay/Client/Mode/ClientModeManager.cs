using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientModeManager : ManagerBase {
        private ClientModeSystem modeSystem;

        protected override void OnAwake() {
            base.OnAwake();
            ModeData.SetIsIntoMode(true);
            MsgRegister.Register<M_Network.SetNetwork>(SetNetwork);
            MsgRegister.Register<M_Stage.TaskLoadStart>(OnTaskLoadStartCallBack);
            MsgRegister.Register<CM_Game.QuitGame>(OnQuitGameCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            if (modeSystem != null) {
                modeSystem.Clear();
                modeSystem = null;
            }
            MsgRegister.Unregister<M_Stage.TaskLoadStart>(OnTaskLoadStartCallBack);
            MsgRegister.Unregister<M_Network.SetNetwork>(SetNetwork);
            MsgRegister.Unregister<CM_Game.QuitGame>(OnQuitGameCallBack);
        }

        private void SetNetwork(M_Network.SetNetwork ent) {
            if (modeSystem != null) {
                modeSystem.SetNetwork(ent.iNetwork);
            } else {
                modeSystem = AddSystem(delegate(ClientModeSystem monsterSystem) {
                    monsterSystem.SetNetwork(ent.iNetwork);
                });
            }
        }

        private void OnTaskLoadStartCallBack(M_Stage.TaskLoadStart ent) {
            if (ent.loadState != StageData.LoadEnum.AddClientGameScene) {
                return;
            }
            var sceneData = SceneData.Get(ModeData.SceneId);
            MsgRegister.Dispatcher(new M_Stage.LoadScene {
                IsActive = true,
                LoadMode = LoadSceneMode.Additive,
                Sign = sceneData.StageSign,
                LoadEndCallBack = () => {
                    MsgRegister.Dispatcher(new M_Game.LoginGameScene());
                    MsgRegister.Dispatcher(new M_Stage.TaskLoadEnd {
                        loadState = StageData.LoadEnum.AddClientGameScene,
                    });
                }
            });
        }

        private void OnQuitGameCallBack(CM_Game.QuitGame ent) {
            Debug.Log("OnQuitGameCallBack");
            MsgRegister.Unregister<CM_Game.QuitGame>(OnQuitGameCallBack);
            ModeData.ResetModeData();
            GamePlayInputSystem.Dispose();
            MsgRegister.Dispatcher(new M_Stage.StartLoadStage {
                stageType = StageData.StageType.Room,
            });
        }
    }
}