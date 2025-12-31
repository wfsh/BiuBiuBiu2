using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Grpc;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGrpc : ComponentBase {
        public string WarServerAddr = "";
        public string Area = "";
        public string Host = "";
        public ushort Port = 0;
        public ushort MaxConnections = 0;
        public byte ServerType = 0;
        private GrpcClientImpl grpcClient;
        private bool isSendStartMode = false;
        public List<TeamInfo> teamInfos = null;

        protected override void OnAwake() {
            MsgRegister.Register<M_Network.ServerStart>(OnServerStartSuccessCallBack);
            MsgRegister.Register<SM_Mode.SetGameState>(OnSetGameStateCallBack);
            MsgRegister.Register<SM_Mode.Event_WarEndTeamPlayerList>(OnWarEndTeamPlayerListCallBack);
            MsgRegister.Register<SM_GPO.ReportTask>(OnReportTask);
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
            if (NetworkData.IsStartClient) {
                return;
            }
            Debug.Log("ServerGrpc OnStart");
            if (Application.isEditor) {
#if BUILD_SERVER
                NetworkData.InitConfig("", "", 7778);
#endif
                NetworkData.Config.IsKCP = true;
            } else {
                LoadConfig();
            }
            MsgRegister.Dispatcher(new M_Network.NetworkConfigInit());
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            MsgRegister.Unregister<SM_Mode.SetGameState>(OnSetGameStateCallBack);
            MsgRegister.Unregister<M_Network.ServerStart>(OnServerStartSuccessCallBack);
            MsgRegister.Unregister<SM_Mode.Event_WarEndTeamPlayerList>(OnWarEndTeamPlayerListCallBack);
            MsgRegister.Unregister<SM_GPO.ReportTask>(OnReportTask);
            Debug.Log("OnClear - StopGrpc");
            StopGrpc();
        }

        private void OnUpdate(float deltaTime) {
            if (ModeData.PlayMode != ModeData.ModeEnum.None) {
                SendStartMode();
            }
        }

        private void OnServerStartSuccessCallBack(M_Network.ServerStart ent) {
            if (Application.isEditor || NetworkData.IsStartClient) {
                InitEditorTest();
            } else {
                Debug.Log("异步启动 GRPC");
                CreateGrpc();
            }
        }

        private void OnSetGameStateCallBack(SM_Mode.SetGameState ent) {
            if (ent.GameState == ModeData.GameStateEnum.ModeOver) {
                Debug.Log("模式结束 - StopGrpc");
                StopGrpc();
            }
        }

        private void InitEditorTest() {
            if (ModeData.PlayMode == ModeData.ModeEnum.None) {
                Debug.Log("添加测试用模式");
                // WarData.SetWarId("TestWarId");
                // ModeData.SetSceneId(SceneData.Id_OldCity);
                // ModeData.SetPlayMode(ModeData.Id_Mode5V5);
                // ModeData.SetWeaponSource(false);
            }
            teamInfos = new List<TeamInfo>();
        }

        private void SendStartMode() {
            if (isSendStartMode || teamInfos == null) {
                return;
            }
            isSendStartMode = true;
            MsgRegister.Dispatcher(new SM_Mode.StartMode {
                TeamInfos = teamInfos
            });
        }

        public void LoadConfig() {
            var configFile = Application.dataPath + "/config.json";
            if (!File.Exists(configFile)) {
                Debug.LogError("config.json not found");
                return;
            }
            var conf = (Hashtable)MiniJSON.jsonDecode(File.ReadAllText(configFile));
            MaxConnections = Convert.ToUInt16(conf["MaxConnections"].ToString());
            Host = conf["Host"].ToString();
            Port = Convert.ToUInt16(conf["Port"].ToString());
            WarServerAddr = conf["WarServerAddr"].ToString();
            Area = conf["Area"].ToString();
            ServerType = Convert.ToByte(conf["ServerType"].ToString());
            NetworkData.InitConfig("", Host, Port, (Hashtable)conf["NetConfig"]);
            File.WriteAllText("./flag_isstarton", "1");
        }

        private string CreateWarID() {
            var warID = Guid.NewGuid();
            return warID.ToString("N"); // 去掉连接符号
        }

        private void CreateGrpc() {
            WarData.SetWarId(CreateWarID());
            Debug.Log("WarId:" + WarData.WarId);
            SentrySDKAgent.Instance.SetWarId(WarData.WarId);
            var netConfig = MiniJSON.jsonEncode(NetworkData.Config.NetConfig);
            // 异步创建 gRPC 客户端
            grpcClient = new GrpcClientImpl(WarServerAddr, WarData.WarId, Host, Port, Area, netConfig, CreateGrpcCallBack);
            // 注册报告日志
            MsgRegister.Register<SM_FunnyDB.ReportLog>(ReportFunnyDBLog);
        }

        private void CreateGrpcCallBack(bool isTrue) {
            if (isTrue == false) {
                return;
            }
            RegGrpcMessages();
            Debug.Log(" GRPC OnGrpcConnect" + isTrue);
        }

        private void RegGrpcMessages() {
            grpcClient.RegisterCallback(COMMON_MSG_TYPE.InitRoom, OnInitRoomCallBack);
        }

        private GrpcCallBack OnInitRoomCallBack(string str, ByteString data) {
            Debug.Log(" GRPC OnInitRoomCallBack" + str);
            var roomData = InitRoomReq.Parser.ParseFrom(data);
            var matchData = GameMatchSet.GetGameMatchById(roomData.MatchId);
            ModeData.SetMatchId(matchData.Id);
            ModeData.SetSceneId(matchData.MapId);
            ModeData.SetPlayMode(matchData.GameMode);
            ModeData.SetWeaponSource(roomData.RandWeapon);
            teamInfos = roomData.Teams.ToList();
            File.WriteAllText("./flag_isstarton", TimeUtil.GetSecond().ToString());
            return new GrpcCallBack() {
                data = new InitRoomResp {
                    Result = "ok"
                }.ToByteString(),
                errMsg = ""
            };
        }

        private void StopGrpc() {
            if (grpcClient != null) {
                Debug.Log("grpcClient.Dispose");
                grpcClient.Dispose();
                grpcClient = null;
                MsgRegister.Unregister<SM_FunnyDB.ReportLog>(ReportFunnyDBLog);
            }
        }

        private void OnReportTask(SM_GPO.ReportTask ent) {
            if (Application.isEditor) {
                return;
            }
            var task = new WarEvent();
            task.Uuid = (ent.PlayerId + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()).ToString();
            task.SysJsonTypeId = ent.SysJsonTypeId;
            task.JsonData = ent.TaskJson;
            task.WarId = WarData.WarId;
            grpcClient.PostEventData(MESSAGE_TYPE.WarEvent, ent.PlayerId, task.ToByteString());
        }

        private void OnWarEndTeamPlayerListCallBack(SM_Mode.Event_WarEndTeamPlayerList ent) {
            if (Application.isEditor) {
                return;
            }
            var warInfo = "GRPC:\n";
            var reportOpponent = ModeData.PlayMode == ModeData.ModeEnum.Mode1V1;
            for (int i = 0; i < ent.WinTeamList.Count; i++) {
                var warResultData = ent.WinTeamList[i];
                warInfo += "WinTeamPlayerId:" + warResultData.playerId + "\n";
                // 1v1 需要上报对手 id
                var opponentId = 0l;
                if (reportOpponent && ent.LoseTeamList.Count == 1) {
                    opponentId = ent.LoseTeamList[0].playerId;
                }
                var result = new ReportWarResult() {
                    IsWin = true,
                    WarId = WarData.WarId,
                    GroupId = warResultData.GroupId,
                    PlayerId = warResultData.playerId,
                    DeathTimes = warResultData.deathCount,
                    KillTimes = warResultData.killCount,
                    IsMvp = warResultData.isMvp,
                    OpponentId = opponentId,
                    MatchId = ModeData.MatchId,
                    OriginMatchId = warResultData.originMatchId,
                    Damage = warResultData.damage,
                    WeaponSwitchTimes = warResultData.switchWeaponTimes,
                    SlideTimes = warResultData.slideTimes,
                    JumpTimes = warResultData.jumpTimes,
                    AirJumpTimes = warResultData.airJumpTimes,
                    SuperWeaponUseTimes = warResultData.superWeaponUseTimes,
                    StartWithParty = warResultData.isTeamUp
                };
                result.DropItems.AddRange(warResultData.items);
                result.CarryItems.AddRange(warResultData.carryItems);
                grpcClient.PostReportData(MESSAGE_TYPE.WarResult, warResultData.playerId, result.ToByteString());
            }
            for (int i = 0; i < ent.LoseTeamList.Count; i++) {
                var warResultData = ent.LoseTeamList[i];
                warInfo += "LoseTeamPlayerId:" + warResultData.playerId + "\n";
                // 1v1 需要上报对手 id
                var opponentId = 0l;
                if (reportOpponent && ent.WinTeamList.Count == 1) {
                    opponentId = ent.WinTeamList[0].playerId;
                }
                var result = new ReportWarResult() {
                    IsWin = false,
                    WarId = WarData.WarId,
                    GroupId = warResultData.GroupId,
                    PlayerId = warResultData.playerId,
                    DeathTimes = warResultData.deathCount,
                    KillTimes = warResultData.killCount,
                    IsMvp = warResultData.isMvp,
                    OpponentId = opponentId,
                    MatchId = ModeData.MatchId,
                    OriginMatchId = warResultData.originMatchId,
                    Damage = warResultData.damage,
                    WeaponSwitchTimes = warResultData.switchWeaponTimes,
                    SlideTimes = warResultData.slideTimes,
                    JumpTimes = warResultData.jumpTimes,
                    AirJumpTimes = warResultData.airJumpTimes,
                    SuperWeaponUseTimes = warResultData.superWeaponUseTimes,
                    StartWithParty = warResultData.isTeamUp
                };
                result.DropItems.AddRange(warResultData.items);
                result.CarryItems.AddRange(warResultData.carryItems);
                grpcClient.PostReportData(MESSAGE_TYPE.WarResult, warResultData.playerId, result.ToByteString());
            }
            Debug.Log(warInfo);
        }

        #region FunnyDB

        private void ReportFunnyDBLog(SM_FunnyDB.ReportLog ent) {
            grpcClient.PostEventLog(ent.EventName, ent.Value);
        }

        #endregion
    }
}