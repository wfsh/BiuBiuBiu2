using System.IO;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerWarReport : ComponentBase {
        public float CurrentTime => currentTime;
        private float currentTime = 0f;
        public string path = "";

        protected override void OnAwake() {
            MsgRegister.Register<SM_Mode.SetGameState>(OnSetGameStateCallBack);
            MsgRegister.Register<SM_Mode.StartMode>(OnStartModeCallBack, 1);
            MsgRegister.Register<M_Game.GameEngineClose>(OnGameEngineCloseCallBack, 1);
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
#if BUILD_SERVER
            path = $"{GetSaveUrl()}/{WarData.WarId}.txt";
#else
            path = $"{GetSaveUrl()}/TempWarReport.txt";
#endif
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            MsgRegister.Unregister<SM_Mode.SetGameState>(OnSetGameStateCallBack);
            MsgRegister.Unregister<SM_Mode.StartMode>(OnStartModeCallBack);
            MsgRegister.Unregister<M_Game.GameEngineClose>(OnGameEngineCloseCallBack);
            Stop();
        }


        private void OnStartModeCallBack(SM_Mode.StartMode ent) {
            StartSaveing();
        }

        private void OnUpdate(float deltaTime) {
            if (WarReportData.ReportState == WarReportData.WarReportState.None) {
                return;
            }
            currentTime += Time.deltaTime;
            mySystem.Dispatcher(new SE_WarReport.Event_TickTime {
                TickTime = currentTime
            });
        }
        
        private void OnSetGameStateCallBack(SM_Mode.SetGameState msg) {
            switch (msg.GameState) {
                case ModeData.GameStateEnum.SaveReport:
                    StopSaveing();
                    break;
            }
        }

        public void StartSaveing() {
            currentTime = 0f;
            WarReportData.SetWarReportState(WarReportData.WarReportState.Saveing);
            mySystem.Dispatcher(new SE_WarReport.Event_SaveBegin());
        }

        public void StopSaveing() {
            mySystem.Dispatcher(new SE_WarReport.Event_SaveReport {
                Path = path
            });
            WarReportData.SetWarReportState(WarReportData.WarReportState.None);
        }
        
        private void OnGameEngineCloseCallBack(M_Game.GameEngineClose ent) {
            Stop();
        }
        
        private void Stop() {
            if (WarReportData.ReportState != WarReportData.WarReportState.Saveing) {
                return;
            }
            StopSaveing();
        }
        
        public static string GetSaveUrl() {
            string path = null;
#if BUILD_SERVER
            path = "./log/war_report";
#else
            var projectRoot = Application.dataPath;
            path = Path.Combine(Directory.GetParent(projectRoot).FullName, "Build", "WarReport");
#endif
            return path;
        }
    }
}