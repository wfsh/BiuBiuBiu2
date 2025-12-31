using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerModeInitSDK : ComponentBase {
        protected override void OnAwake() {
            // MsgRegister.Register<SM_Mode.CheckCharacterLogin>(OnCheckCharacterLogin);
            MsgRegister.Register<SM_Mode.StartMode>(OnStartModeCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            // MsgRegister.Unregister<SM_Mode.CheckCharacterLogin>(OnCheckCharacterLogin);
            MsgRegister.Unregister<SM_Mode.StartMode>(OnStartModeCallBack);
        }
        
        private void OnStartModeCallBack(SM_Mode.StartMode ent) {
            InitPerfanalyzer();
        }

        private void OnCheckCharacterLogin(SM_Mode.CheckCharacterLogin ent) {
        }

        void InitPerfanalyzer() {
            Debug.Log("InitPerfanalyzer");
            if (!PerfAnalyzerAgent.IsInit) {
                var matchData = GameMatchSet.GetGameMatchById(ModeData.MatchId);
                SentrySDKAgent.Instance.SetScene(matchData.Desc);
                PerfAnalyzerAgent.Init("Yz8DJFOa5zlEvoZxeteFc4ll", PerfAnalyzerToken.UploadTypeEnum.Default,matchData.Desc, false);
                PerfAnalyzerAgent.EnabledOnlyConsoleSaveReport(true);
                PerfAnalyzerAgent.SetGameVersion(GameData.ShowGameVersion);
                PerfAnalyzerAgent.SetBuildPackageName(GameData.GetBuildPackageName());
                PerfAnalyzerAgent.SetPlayerId(WarData.WarId);
                PerfAnalyzerAgent.SetOnStartFeatureCallBack(() => {
                    PerfAnalyzerAgent.SetLog("Network 版本号:" + GameData.NetworkVersion);
                    PerfAnalyzerAgent.SetReportSign(WarData.WarId);
                    PerfAnalyzerAgent.SetCustomLabel("NetworkTransport", NetworkData.Config.IsKCP ? "KCP" : "WEB");
                });
            }
        }
    }
}