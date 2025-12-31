using System;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Sofunny.BiuBiuBiu2.Main {
    public class LoadBaseAsset : MonoBehaviour {
        private string[] LoadUIList = {
            "LoadStage",
            "Lobby",
        };

        private Action OnComponent;
        private int loadIndex = 0;
        private float targetSizeX = 0f;
        private bool isGameServerLoginFinish = false;
        private string loadSign = "";
        private string loadGameServer = "";
        
        [SerializeField]
        private RectTransform ImgLoading;
        
        [SerializeField]
        private Text TxtLoading;

        public void Init(Action OnComponent) {
            Debug.Log("LoadBaseAsset init");
            MsgRegister.Register<M_Login.LoginGameServerState>(OnLoginGameServerStateCallBack);
            MsgRegister.Register<M_Game.LoginGameServer>(OnLoginFinishCallBack);
            this.OnComponent = OnComponent;
            loadIndex = 0;
            var size = ImgLoading.sizeDelta;
            size.x = 0f;
            ImgLoading.sizeDelta = size;
            StartLoad();
        }

        public void Clear() {
            this.OnComponent = null;
            MsgRegister.Unregister<M_Login.LoginGameServerState>(OnLoginGameServerStateCallBack);
            MsgRegister.Unregister<M_Game.LoginGameServer>(OnLoginFinishCallBack);
        }

        public void OnUpdate(float deltaTime) {
            var size = ImgLoading.sizeDelta;
            if (size.x < targetSizeX) {
                size.x += Time.deltaTime * 300f;
            }
            ImgLoading.sizeDelta = size;
        }

        private void OnLoginGameServerStateCallBack(M_Login.LoginGameServerState ent) {
            var gameServerState = ent.LoginState;
            switch (gameServerState) {
                case 1:
                    loadGameServer = "微信账号登录";
                    break;
                case 2:
                    loadGameServer = "获取游戏信息";
                    break;
                case 3:
                    loadGameServer = "登录账号";
                    break;
                case 4:
                    loadGameServer = "登录大厅服务器";
                    break;
            }
            Debug.Log("State : " + loadGameServer);
            RenderTxt();
        }

        private void OnLoginFinishCallBack(M_Game.LoginGameServer ent) {
            isGameServerLoginFinish = true;
            LoginEnd();
        }

        private void LoginEnd() {
            if (loadIndex >= LoadUIList.Length && isGameServerLoginFinish) {
                OnComponent?.Invoke();
            }
        }

        private void StartLoad() {
            if (loadIndex >= LoadUIList.Length) {
                SetDeltaX(targetSizeX);
                LoginEnd();
                return;
            }
            loadSign = LoadUIList[loadIndex];
            SetDeltaX(1396f * ((float)loadIndex / LoadUIList.Length));
            targetSizeX = 1396f * ((float)(loadIndex + 1) / LoadUIList.Length);
            RenderTxt();
            AssetManager.LoadUI(loadSign, o => {
                loadIndex++;
                StartLoad();
            });
        }

        private void RenderTxt() {
            TxtLoading.text = $"Asset：[{loadSign}] - login：[{loadGameServer}]";
        }
        
        private void SetDeltaX (float x) {
            var size = ImgLoading.sizeDelta;
            size.x = x;
            ImgLoading.sizeDelta = size;
        }
    }
}