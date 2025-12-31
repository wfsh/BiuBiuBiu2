using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientWarReportPlay : ComponentBase {
        private List<WarReportData.FrameData> list = new List<WarReportData.FrameData>();
        private Dictionary<byte, INetwork> networkData = new Dictionary<byte, INetwork>();
        private Dictionary<byte, ICharacterSync> networkSyncData = new Dictionary<byte, ICharacterSync>();
        private Dictionary<byte, INetworkCharacter> characterNetworkData = new Dictionary<byte, INetworkCharacter>();
        private int currentIndex = 0;
        private float currentTime = 0f;
        private int maxIndex = 0;
        private float maxPlayTime = 0f;
        private ByteBuffer byteBuffer;

        protected override void OnAwake() {
            Register<CE_WarReport.Event_TickTime>(OnTickTime);
            Register<CE_WarReport.Event_PlayBegin>(OnPlayBegin);
            Register<CE_WarReport.Event_PlayEnd>(OnPlayEnd);
            MsgRegister.Register<CM_WarReport.StopPlay>(OnStopPlay);
            MsgRegister.Register<CM_WarReport.PlaySpeedScale>(OnPlaySpeedScale);
            MsgRegister.Register<CM_WarReport.GetPlayBeginState>(OnGetPlayBeginStateScale);
        }
        
        protected override void OnStart() {
        }

        protected override void OnClear() {
            MsgRegister.Unregister<CM_WarReport.StopPlay>(OnStopPlay);
            MsgRegister.Unregister<CM_WarReport.PlaySpeedScale>(OnPlaySpeedScale);
            MsgRegister.Unregister<CM_WarReport.GetPlayBeginState>(OnGetPlayBeginStateScale);
            Unregister<CE_WarReport.Event_TickTime>(OnTickTime);
            Unregister<CE_WarReport.Event_PlayBegin>(OnPlayBegin);
            Unregister<CE_WarReport.Event_PlayEnd>(OnPlayEnd);
            DestoryAllNetwork();
            byteBuffer = null;
            PlayStateReset();
        }
        
        private void OnStopPlay(CM_WarReport.StopPlay ent) {
            Debug.Log("停止播放战报:" + ent.IsStop + " 当前状态：" + WarReportData.ReportState);
            if (ent.IsStop) {
                if (WarReportData.ReportState == WarReportData.WarReportState.Playing) {
                    WarReportData.SetWarReportState(WarReportData.WarReportState.Stoped);
                }
            } else {
                if (WarReportData.ReportState == WarReportData.WarReportState.Stoped) {
                    WarReportData.SetWarReportState(WarReportData.WarReportState.Playing);
                }
            }
        }
        
        private void OnPlaySpeedScale(CM_WarReport.PlaySpeedScale ent) {
            if (WarReportData.ReportState != WarReportData.WarReportState.Playing) return;
            Time.timeScale = ent.SpeedScale;
        }
        
        private void OnGetPlayBeginStateScale(CM_WarReport.GetPlayBeginState ent) {
            if (WarReportData.ReportState != WarReportData.WarReportState.Playing) return;
            ent.Callback(currentTime, maxPlayTime);
        }
        
        private void OnPlayBegin(ISystemMsg body, CE_WarReport.Event_PlayBegin ent) {
            var bytes = ent.Bytes; 
            if (bytes == null || bytes.Length == 0) {
                Debug.LogError("播放失败，战报数据为空");
                mySystem.Dispatcher(new CE_WarReport.Event_StopPlay());
                return;
            }
            byteBuffer = ByteBuffer.Allocate();
            PlayStateReset();
            RenderPlayData(bytes);
            MsgRegister.Dispatcher(new CM_WarReport.PlayBegin {
                MaxPlayTime = maxPlayTime,
            });
            Debug.Log("开始播放战报数据，数据长度：" + bytes.Length + " 列表长度：" + list.Count);
        }

        private void RenderPlayData(byte[] bytes) {
            list = new List<WarReportData.FrameData>();
            byteBuffer.OnWrap(bytes);
            maxIndex = byteBuffer.ReadInt();
            for (int i = 0; i < maxIndex; i++) {
                var reportType = (WarReportData.ReportFrameType)byteBuffer.ReadByte();
                var time = byteBuffer.ReadFloat();
                var networkId = byteBuffer.ReadByte();
                WarReportData.FrameData frameData = null;
                switch (reportType) {
                    case WarReportData.ReportFrameType.Mode:
                        var modeData = new WarReportData.ModeData();
                        modeData.ModeId = byteBuffer.ReadInt();
                        modeData.MatchId = byteBuffer.ReadInt();
                        modeData.SceneId = byteBuffer.ReadInt();
                        frameData = modeData;
                        break;
                    case WarReportData.ReportFrameType.RpcData:
                    case WarReportData.ReportFrameType.TargetRpcData:
                        var rpcData = new WarReportData.RpcFrameData();
                        rpcData.TargetNetworkId = byteBuffer.ReadByte();
                        rpcData.ConnId = byteBuffer.ReadInt();
                        rpcData.Channel = byteBuffer.ReadByte();
                        var length = byteBuffer.ReadInt();
                        rpcData.Bytes = new byte[length];
                        for (int j = 0; j < length; j++) {
                            rpcData.Bytes[j] = byteBuffer.ReadByte();
                        }
                        frameData = rpcData;
                        break;
                    case WarReportData.ReportFrameType.CreateNetwork:
                        var createData = new WarReportData.CreateNetworkData();
                        createData.CreateConnId = byteBuffer.ReadInt();
                        createData.IsCharacter = byteBuffer.ReadBoolean();
                        frameData = createData;
                        break;
                    case WarReportData.ReportFrameType.DestroyNetwork:
                        frameData = new WarReportData.FrameData();
                        break;
                    case WarReportData.ReportFrameType.SyncCharacterData:
                        var syncData = new WarReportData.CharacterSyncData();
                        syncData.SyncType = (WarReportData.CharacterSyncType)byteBuffer.ReadByte();
                        syncData.Value = byteBuffer.ReadString();
                        frameData = syncData;
                        break;
                    case WarReportData.ReportFrameType.CharacterPoint:
                        var pointData = new WarReportData.CharacterPointData();
                        pointData.Position = byteBuffer.ReadVector3();
                        frameData = pointData;
                        break;
                    case WarReportData.ReportFrameType.CharacterRota:
                        var rotaData = new WarReportData.CharacterRotaData();
                        rotaData.Rotation = byteBuffer.ReadQuaternion();
                        frameData = rotaData;
                        break;
                    default:
                        Debug.LogError("RenderPlayData 未知的报告帧类型：" + reportType);
                        break;
                }
                frameData.Time = time;
                frameData.NetworkId = networkId;
                frameData.ReportType = reportType;
                maxPlayTime = Mathf.Max(maxPlayTime, time);
                list.Add(frameData);
            }
            byteBuffer.OnWrap(new byte[0]);
            byteBuffer.Dispose();
        }

        private void OnPlayEnd(ISystemMsg body, CE_WarReport.Event_PlayEnd ent) {
            PlayStateReset();
        }
        
        private void PlayStateReset() {
            Time.timeScale = 1;
            currentIndex = 0;
            currentTime = 0f;
            maxIndex = 0;
            list.Clear();
        }

        private void OnTickTime(ISystemMsg body, CE_WarReport.Event_TickTime ent) {
            if (WarReportData.ReportState != WarReportData.WarReportState.Playing) return;
            currentTime = ent.TickTime;
            MsgRegister.Dispatcher(new CM_WarReport.PlayTick {
                PlayTime = currentTime,
            });
            if (currentIndex >= maxIndex) {
                // 如果已经播放完所有数据，则不再处理
                Debug.Log("已播放完所有数据，当前索引：" + currentIndex + " 最大索引：" + maxIndex);
                mySystem.Dispatcher(new CE_WarReport.Event_StopPlay());
                return;
            } else {
                while (currentIndex < list.Count) {
                    var currentFrameData = list[currentIndex];
                    if (currentFrameData.Time > currentTime) {
                        // 如果当前帧数据的时间大于当前时间，则跳出循环
                        break;
                    }
                    currentIndex++;
                    switch (currentFrameData.ReportType) {
                        case WarReportData.ReportFrameType.Mode:
                            var modeData = (WarReportData.ModeData)currentFrameData;
                            SetModeData(modeData);
                            break;
                        case WarReportData.ReportFrameType.CreateNetwork:
                            var createNetworkData = (WarReportData.CreateNetworkData)currentFrameData;
                            CreateNetwork(createNetworkData);
                            break;
                        case WarReportData.ReportFrameType.RpcData:
                        case WarReportData.ReportFrameType.TargetRpcData:
                            var rpcData = (WarReportData.RpcFrameData)currentFrameData;
                            RpcData(rpcData);
                            break;
                        case WarReportData.ReportFrameType.SyncCharacterData:
                            var syncData = (WarReportData.CharacterSyncData)currentFrameData;
                            SyncCharacterData(syncData);
                            break;
                        case WarReportData.ReportFrameType.CharacterPoint:
                            var pointData = (WarReportData.CharacterPointData)currentFrameData;
                            CharacterPoint(pointData);
                            break;
                        case WarReportData.ReportFrameType.CharacterRota:
                            var rotaData = (WarReportData.CharacterRotaData)currentFrameData;
                            CharacterRota(rotaData);
                            break;
                        case WarReportData.ReportFrameType.DestroyNetwork:
                            // 处理销毁网络连接的逻辑
                            DestroyNetwork(currentFrameData.NetworkId);
                            break;
                        default:
                            Debug.LogError("OnTickTime 未知的报告帧类型：" + currentFrameData.ReportType);
                            break;
                    }
                }
            }
        }
        
        private void SetModeData(WarReportData.ModeData modeData) {
            // 处理模式数据的逻辑
            Debug.LogError("设置模式数据，ModeId: " + modeData.ModeId + ", MatchId: " + modeData.MatchId + ", SceneId: " + modeData.SceneId);
            ModeData.SetPlayMode(modeData.ModeId);
            ModeData.SetMatchId(modeData.MatchId);
            ModeData.SetSceneId(modeData.SceneId);
            MsgRegister.Dispatcher(new M_Network.ClientConnect());
            MsgRegister.Dispatcher(new M_Stage.TaskLoadEnd {
                loadState = StageData.LoadEnum.Connect,
            });
        }
        
        private void CreateNetwork(WarReportData.CreateNetworkData createData) {
            // 处理创建网络连接的逻辑
            Debug.Log("创建网络连接，ConnId: " + createData.CreateConnId + " NetworkID:" +createData.NetworkId + ", IsCharacter: " + createData.IsCharacter);
            INetwork network;
            if (createData.IsCharacter) {
                network = CreateCharacterNetwork(createData.NetworkId, createData.CreateConnId);
            } else {
                network = CreateWorldNetwork(createData.NetworkId, createData.CreateConnId);
            }
            networkData.Add(createData.NetworkId, network);
        }
        
        public INetwork CreateCharacterNetwork(byte networkId, int createConnId) {
            var go = new GameObject($"[ReportCharacterNetwork NetworkId:{networkId}  ConnId:{createConnId}]]");
            var network = go.AddComponent<WarReportCharacterNetwork>();
            var sync = go.AddComponent<WarReportCharacterSync>();
            network.Init(createConnId, networkId);
            networkSyncData.Add(networkId, sync);
            characterNetworkData.Add(networkId, network);
            return network;
        }
        
        public INetwork CreateWorldNetwork(byte networkId, int createConnId) {
            var go = new GameObject($"[ReportWorldNetwork NetworkId:{networkId}  ConnId:{createConnId}]");
            var network = go.AddComponent<WarReportWorldNetwork>();
            network.Init(createConnId, networkId);
            return network;
        }
        
        private void RpcData (WarReportData.RpcFrameData rpcData) {
            if (rpcData.Bytes == null || rpcData.Bytes.Length == 0) {
                Debug.LogError("WarReport Play RpcData Bytes is null or empty, ConnId: " + rpcData.ConnId);
                return;
            }
            // 处理RPC数据的逻辑
            // Debug.Log("处理RPC数据，TargetNetworkId: " + rpcData.TargetNetworkId + ", ConnId: " + rpcData.ConnId + ", Channel: " + rpcData.Channel);
            var network = GetNetwork(rpcData.NetworkId);
            var reportNetwork = (WarReportNetworkBase)network;
            if (rpcData.ReportType == WarReportData.ReportFrameType.RpcData) {
                reportNetwork.RPC(rpcData.Channel, rpcData.ConnId, rpcData.Bytes);
            } else if (rpcData.ReportType == WarReportData.ReportFrameType.TargetRpcData) {
                reportNetwork.TargetRPC(rpcData.Channel, rpcData.ConnId, rpcData.Bytes, GetNetwork(rpcData.TargetNetworkId));
            } else {
                Debug.LogError("WarReport Play RpcData 未知的报告帧类型：" + rpcData.ReportType);
            }
        }
        
        private void SyncCharacterData(WarReportData.CharacterSyncData syncData) {
            // 处理角色同步数据的逻辑
            // Debug.Log("处理角色同步数据，NetworkId: " + syncData.NetworkId + ", SyncType: " + syncData.SyncType + ", Value: " + syncData.Value);
            var networkSync = GetCharacterSync(syncData.NetworkId);
            if (networkSync != null) {
                switch (syncData.SyncType) {
                    case WarReportData.CharacterSyncType.PlayerId:
                        networkSync.SyncPlayerId(long.Parse(syncData.Value));
                        break;
                    case WarReportData.CharacterSyncType.NickName:
                        networkSync.SyncNickName(syncData.Value);
                        break;
                    case WarReportData.CharacterSyncType.GpoId:
                        networkSync.SyncGpoId(int.Parse(syncData.Value));
                        break;
                    case WarReportData.CharacterSyncType.TeamId:
                        networkSync.SyncTeamId(int.Parse(syncData.Value));
                        break;
                    case WarReportData.CharacterSyncType.JumpType:
                        if (Enum.TryParse(syncData.Value, out CharacterData.JumpType jumpType)) {
                            networkSync.JumpType(jumpType);
                        } else {
                            Debug.LogError("WarReport Play SyncCharacterData 无效的 JumpType 值：" + syncData.Value);
                        }
                        break;
                    case WarReportData.CharacterSyncType.FlyType:
                        if (Enum.TryParse(syncData.Value, out CharacterData.FlyType flyType)) {
                            networkSync.FlyType(flyType);
                        } else {
                            Debug.LogError("WarReport Play SyncCharacterData 无效的 FlyType 值：" + syncData.Value);
                        }
                        break;
                    case WarReportData.CharacterSyncType.StandType:
                        if (Enum.TryParse(syncData.Value, out CharacterData.StandType standType)) {
                            networkSync.StandType(standType);
                        } else {
                            Debug.LogError("WarReport Play SyncCharacterData 无效的 StandType 值：" + syncData.Value);
                        }
                        break;
                    case WarReportData.CharacterSyncType.IsDodge:
                        networkSync.IsDodge(bool.Parse(syncData.Value));
                        break;
                    case WarReportData.CharacterSyncType.UseHoldOn:
                        networkSync.UseHoldOn(syncData.Value);
                        break;
                    case WarReportData.CharacterSyncType.Level:
                        networkSync.SetLevel(int.Parse(syncData.Value));
                        break;
                    case WarReportData.CharacterSyncType.Hp:
                        networkSync.SetHP(int.Parse(syncData.Value));
                        break;
                    case WarReportData.CharacterSyncType.MaxHp:
                        networkSync.SetMaxHp(int.Parse(syncData.Value));
                        break;
                    case WarReportData.CharacterSyncType.Atk:
                        networkSync.SetATK(int.Parse(syncData.Value));
                        break;
                    case WarReportData.CharacterSyncType.Def:
                        break;
                    default:
                        Debug.LogError("WarReport Play SyncCharacterData 未知的同步类型：" + syncData.SyncType);
                        break;
                }
            } else {
                Debug.LogError("WarReport Play SyncCharacterData 获取网络连接失败 NetworkId:" + syncData.NetworkId);
            }
        }
        
        private void CharacterPoint (WarReportData.CharacterPointData pointData) {
            // 处理角色位置同步的逻辑
            // Debug.Log("处理角色位置同步，NetworkId: " + pointData.NetworkId + ", Position: " + pointData.Position);
            var network = GetCharacterNetwork(pointData.NetworkId);
            if (network != null) {
                network.SetPoint(pointData.Position);
            } else {
                Debug.LogError("WarReport Play CharacterPoint 获取网络连接失败 NetworkId:" + pointData.NetworkId);
            }
        }
        
        private void CharacterRota (WarReportData.CharacterRotaData rotaData) {
            // 处理角色旋转同步的逻辑
            // Debug.Log("处理角色旋转同步，NetworkId: " + rotaData.NetworkId + ", Rotation: " + rotaData.Rotation);
            var network = GetCharacterNetwork(rotaData.NetworkId);
            if (network != null) {
                network.SetRota(rotaData.Rotation);
            } else {
                Debug.LogError("WarReport Play CharacterRota 获取网络连接失败 NetworkId:" + rotaData.NetworkId);
            }
        }
        
        private void DestroyNetwork (byte networkId) {
            // 处理销毁网络连接的逻辑
            Debug.Log("销毁网络连接，NetworkId: " + networkId);
            if (networkData.TryGetValue(networkId, out var network)) {
                var reportNetwork = (WarReportNetworkBase)network;
                GameObject.Destroy(reportNetwork.gameObject);
                networkData.Remove(networkId);
                networkSyncData.Remove(networkId);
                characterNetworkData.Remove(networkId);
            } else {
                Debug.LogError("WarReport Play DestroyNetwork 获取网络连接失败 NetworkId:" + networkId);
            }
        }
        
        private void DestoryAllNetwork() {
            // 销毁所有网络连接
            Debug.Log("销毁所有网络连接");
            foreach (var network in networkData.Values) {
                var reportNetwork = (WarReportNetworkBase)network;
                GameObject.Destroy(reportNetwork.gameObject);
            }
            networkData.Clear();
            networkSyncData.Clear();
            characterNetworkData.Clear();
        }
        
        private INetwork GetNetwork(byte networkId) {
            if (networkId == 0) {
                return null;
            }
            if (networkData.TryGetValue(networkId, out var network)) {
                return network;
            }
            Debug.LogError("WarReport 获取网络连接失败 NetworkId:" + networkId);
            return null;
        }
        
        
        private ICharacterSync GetCharacterSync(byte networkId) {
            if (networkSyncData.TryGetValue(networkId, out var sync)) {
                return sync;
            }
            Debug.LogError("WarReport 获取角色同步数据失败 NetworkId:" + networkId);
            return null;
        }

        private INetworkCharacter GetCharacterNetwork(int networkId) {
            if (characterNetworkData.TryGetValue((byte)networkId, out var network)) {
                return network;
            }
            Debug.LogError("WarReport 获取角色网络连接失败 NetworkId:" + networkId);
            return null;
        }
    }
}