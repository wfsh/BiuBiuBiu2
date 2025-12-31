using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.GamePlay {
    public class GoldDashGameEngine {
        private GameWorldManager gameWorldManager;
        private MsgRegister msgRegister;
        private UpdateRegister updateRegister;
        private bool isInit = false;
        public void Init() {
            isInit = true;
            InitUpdateRegister();
            InitDeviceLevel();
            ModeData.Init();
            PrefabPoolManager.Init();
            AudioPoolManager.Init();
            WeaponData.Init();
            msgRegister = MsgRegister.Init();
            MsgRegister.Register<M_Game.GameEngineClose>(OnGameEngineCloseCallBack);
        }
        void InitDeviceLevel() {
            var level = PerfAnalyzerAgent.GetDeviceLevel();
            QualityData.Init(level);
        }
        
        public void InitNetConfig(string ip, ushort port, bool isServer, bool isClient) {
            NetworkData.InitConfig(ip, port, isServer, isClient);
        }
        
        public void Clear() {
            if (isInit == false) {
                return;
            }
            isInit = false;
            MsgRegister.Unregister<M_Game.GameEngineClose>(OnGameEngineCloseCallBack);
            AudioPoolManager.Dispose();
            PrefabPoolManager.Dispose();
            gameWorldManager.Clear();
            gameWorldManager = null;
            if (msgRegister != null) {
                msgRegister.Dispose();
                msgRegister = null;
            }
            if (updateRegister != null) {
                updateRegister.Clear();
                updateRegister = null;
            }
        }


        public void InitGameWorldManager() {
            gameWorldManager = new GameWorldManager();
            gameWorldManager.Init();
        }

        void InitUpdateRegister() {
            updateRegister = new UpdateRegister();
            updateRegister.Init();
        }

        public void Update() {
            if (updateRegister != null) {
                updateRegister.OnUpdate(Time.deltaTime);
            }
            gameWorldManager?.Update(Time.deltaTime);
            AudioPoolManager.Update(Time.deltaTime);
            PrefabPoolManager.Update(Time.deltaTime);
        }

        public void FixedUpdate() {
            PerfAnalyzerAgent.BeginSample("GameEngine:FixedUpdate");
            if (updateRegister != null) {
                updateRegister.OnFixedUpdate(Time.fixedDeltaTime);
            }
            PerfAnalyzerAgent.EndSample("GameEngine:FixedUpdate");
        }

        public void LateUpdate() {
            PerfAnalyzerAgent.BeginSample("GameEngine:LateUpdate");
            if (updateRegister != null) {
                updateRegister.OnLateUpdate(Time.deltaTime);
            }
            PerfAnalyzerAgent.EndSample("GameEngine:LateUpdate");
        }

        private void OnGameEngineCloseCallBack(M_Game.GameEngineClose ent) {
            MsgRegister.Unregister<M_Game.GameEngineClose>(OnGameEngineCloseCallBack);
            Clear();
        }
    }
}