using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientWarReport : ComponentBase {
        public float CurrentTime => currentTime;
        private float currentTime = 0f;
        public string path = "";
        private bool isStartConnectTask = false;
        private bool isQuitToLobby = false;
        private StageData.LoadEnum stageLoadState = StageData.LoadEnum.None;
        private IGPO lookGPO;

        protected override void OnAwake() {
            mySystem.Register<CE_WarReport.Event_StopPlay>(OnStopPlay);
            MsgRegister.Register<M_Stage.TaskLoadStart>(OnTaskLoadStartCallBack);
            MsgRegister.Register<CM_GPO.AddGPO>(OnAddGPOCallBack);
            
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            MsgRegister.Unregister<M_Stage.TaskLoadStart>(OnTaskLoadStartCallBack);
            MsgRegister.Unregister<CM_GPO.AddGPO>(OnAddGPOCallBack);
            mySystem.Unregister<CE_WarReport.Event_StopPlay>(OnStopPlay);
            Stop();
        }
        private void OnTaskLoadStartCallBack(M_Stage.TaskLoadStart ent) {
            stageLoadState = ent.loadState;
            switch (ent.loadState) {
                case StageData.LoadEnum.Connect:
                    isStartConnectTask = true;
                    StartPlayback();
                    break;
                case StageData.LoadEnum.Room:
                    isQuitToLobby = true;
                    break;
                case StageData.LoadEnum.LoginWar:
                    MsgRegister.Dispatcher(new M_Stage.TaskLoadEnd {
                        loadState = StageData.LoadEnum.LoginWar,
                    });
                    break;
                case StageData.LoadEnum.SendLoginInfo:
                    MsgRegister.Dispatcher(new M_Stage.TaskLoadEnd {
                        loadState = StageData.LoadEnum.SendLoginInfo,
                    });
                    break;
                case StageData.LoadEnum.SetLookRole:
                    EndTaskSetLookRole();
                    break;
            }
        }
        
        private void OnAddGPOCallBack(CM_GPO.AddGPO ent) {
            Debug.Log("添加战报角色 : " + ent.IGpo.GetGpoID());
            if (ent.IGpo.GetGPOType() == GPOData.GPOType.Role) {
                lookGPO = ent.IGpo;
                EndTaskSetLookRole();
            }
        }

        private void EndTaskSetLookRole() {
            if (stageLoadState != StageData.LoadEnum.SetLookRole || lookGPO == null) {
                return;
            }
            Debug.Log("设置战报角色完成 : " + lookGPO.GetGpoID());
            MsgRegister.Dispatcher(new CM_GPO.AddLookGPO {
                LookGPO = lookGPO,
                IsDrive = false,
            });
            MsgRegister.Dispatcher(new M_Stage.TaskLoadEnd {
                loadState = StageData.LoadEnum.SetLookRole,
            });
        }

        private void OnUpdate(float deltaTime) {
            if (WarReportData.ReportState == WarReportData.WarReportState.None) {
                return;
            }
            currentTime += Time.deltaTime;
            mySystem.Dispatcher(new CE_WarReport.Event_TickTime {
                TickTime = currentTime
            });
        }
        

        public void StartPlayback() {
            Debug.Log("开始播放战报 : " + path);
            currentTime = 0f;
            WarReportData.SetWarReportState(WarReportData.WarReportState.Playing);
            mySystem.Dispatcher(new CE_WarReport.Event_PlayBegin {
                Bytes = WarReportData.PlayWarReportBytes
            });
        }
        
        private void OnStopPlay(ISystemMsg body, CE_WarReport.Event_StopPlay ent) {
            StopPlayback();
        }

        public void StopPlayback() {
            Debug.LogError("停止播放战报");
            WarReportData.SetWarReportState(WarReportData.WarReportState.None);
            mySystem.Dispatcher(new CE_WarReport.Event_PlayEnd());
        }
        
        private void Stop() {
            if (WarReportData.ReportState != WarReportData.WarReportState.Playing) {
                return;
            }
            StopPlayback();
        }
    }
}