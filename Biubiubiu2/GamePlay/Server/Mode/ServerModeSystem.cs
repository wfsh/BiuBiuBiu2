using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerModeSystem : SystemBase {
        private bool initModeComponent = false;
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Mode.StartMode>(OnPlayMode, 1);
            AddComponents();
            InitModeComponent();
        }
        
        protected override void OnStart() {
            base.OnStart();
        }
        
        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_Mode.StartMode>(OnPlayMode);
        }

        private void AddComponents() {
            AddComponent<ServerModeTime>();
            // AddComponent<ServerModeInitSDK>();
            AddComponent<ServerModeCreateAI>();
            // AddComponent<ServerServiceStateCheck>();
            // AddComponent<ServerModeReporter>();
            AddComponent<ServerTaskReporter>();
            AddComponent<ServerModeHandlerErrorCheck>();
        }
        
        private void OnPlayMode(SM_Mode.StartMode ent) {
            InitModeComponent();
        }

        private void InitModeComponent() {
            if (initModeComponent || ModeData.PlayMode == ModeData.ModeEnum.None) {
                return;
            }
            initModeComponent = true;
            if (ModeData.PlayMode != ModeData.ModeEnum.SausageGoldDash && 
                ModeData.PlayMode != ModeData.ModeEnum.SausageBeastCamp && 
                ModeData.PlayMode != ModeData.ModeEnum.SausageFastGoldDash) {
                AddComponent<ServerModeMainLoop>();
                AddComponent<ServerSceneStageLoad>();
            }
            switch (ModeData.PlayMode) {
                case ModeData.ModeEnum.ModeExplore:
                    AddComponent<ServerExploreMode>();
                    AddComponent<ServerModeDropItem>();
                    break;
                case ModeData.ModeEnum.ModeBoss:
                    AddComponent<ServerBossBattleMode>();
                    break;
                case ModeData.ModeEnum.Mode1V1:
                    AddComponent<ServerVSMode>();
                    AddComponent<Server1v1ModeWeaponInit>();
                    break;
                case ModeData.ModeEnum.Mode5V5ReLife:
                    AddComponent<ServerVSReLifeMode>();
                    AddComponent<ServerModeDropItem>();
                    break;
                case ModeData.ModeEnum.SausageGoldDash:
                case ModeData.ModeEnum.SausageBeastCamp:
                case ModeData.ModeEnum.SausageFastGoldDash:
                    AddComponent<ServerGoldDashSceneStageLoad>();
                    AddComponent<ServerModeGoldDashDropItem>();
                    break;
            }
        }
    }
}