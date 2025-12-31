using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerWarReportSave : ComponentBase {
        private List<WarReportData.FrameData> saveList = new List<WarReportData.FrameData>();
        private ByteBuffer byteBuffer = ByteBuffer.Allocate();
        private float currentTime = 0f;
        private bool isSaveEnd = false;
        protected override void OnAwake() {
            Register<SE_WarReport.Event_TickTime>(OnTickTime);
            Register<SE_WarReport.Event_SaveBegin>(OnSaveBegin);
            Register<SE_WarReport.Event_SaveReport>(OnSaveReport);
            MsgRegister.Register<M_WarReport.SetRpcReportData>(OnRpcSetReportData);
            MsgRegister.Register<M_WarReport.SetTargetRpcReportData>(OnTargetRpcReportData);
            MsgRegister.Register<M_WarReport.SetCharacterSyncData>(OnSetCharacterSyncDataNetwork);
            MsgRegister.Register<M_WarReport.SetCharacterPoint>(OnSetCharacterPointNetwork);
            MsgRegister.Register<M_WarReport.SetCharacterRota>(OnSetCharacterRotaNetwork);
            MsgRegister.Register<M_Character.SetNetwork>(OnSetCharacterNetwork);
            MsgRegister.Register<M_Character.DestoryNetwork>(OnDestoryCharacterNetwork);
        }
        
        protected override void OnStart() {
        }

        protected override void OnClear() {
            Unregister<SE_WarReport.Event_TickTime>(OnTickTime);
            Unregister<SE_WarReport.Event_SaveBegin>(OnSaveBegin);
            Unregister<SE_WarReport.Event_SaveReport>(OnSaveReport);
            MsgRegister.Unregister<M_WarReport.SetRpcReportData>(OnRpcSetReportData);
            MsgRegister.Unregister<M_WarReport.SetTargetRpcReportData>(OnTargetRpcReportData);
            MsgRegister.Unregister<M_WarReport.SetCharacterSyncData>(OnSetCharacterSyncDataNetwork);
            MsgRegister.Unregister<M_WarReport.SetCharacterPoint>(OnSetCharacterPointNetwork);
            MsgRegister.Unregister<M_WarReport.SetCharacterRota>(OnSetCharacterRotaNetwork);
            MsgRegister.Unregister<M_Character.SetNetwork>(OnSetCharacterNetwork);
            MsgRegister.Unregister<M_Character.DestoryNetwork>(OnDestoryCharacterNetwork);
            saveList.Clear();
            byteBuffer.Dispose();
            byteBuffer = null;
        }
        
        private void OnSetCharacterNetwork(M_Character.SetNetwork ent) {
            if (isSaveEnd == true) {
                return;
            }
            if (WarReportData.ReportState != WarReportData.WarReportState.Saveing) {
                Debug.LogError("[WarReport] 服務器端没有开启保存状态，无法记录 Network 创建数据。请检查是否调用了 SetWarReportState(WarReportState.Saveing) 方法。");
                return;
            }
            Debug.Log("[WarReport] 创建角色 Network: " + ent.iNetwork.GetNetworkId() + " ConnId: " + ent.iNetwork.ConnId());
            saveList.Add(new WarReportData.CreateNetworkData {
                ReportType = WarReportData.ReportFrameType.CreateNetwork,
                Time = currentTime,
                NetworkId = ent.iNetwork.GetNetworkId(),
                CreateConnId = ent.iNetwork.ConnId(),
                IsCharacter = true,
            });
        }
        
        private void OnDestoryCharacterNetwork(M_Character.DestoryNetwork ent) {
            if (isSaveEnd == true) {
                return;
            }
            if (WarReportData.ReportState != WarReportData.WarReportState.Saveing) {
                Debug.LogError("[WarReport] 服務器端没有开启保存状态，无法记录 Network 销毁数据。请检查是否调用了 SetWarReportState(WarReportState.Saveing) 方法。");
                return;
            }
            saveList.Add(new WarReportData.FrameData() {
                Time = currentTime,
                ReportType = WarReportData.ReportFrameType.DestroyNetwork,
                NetworkId = ent.NetworkId,
            });
        }
        
        private void OnTickTime(ISystemMsg body, SE_WarReport.Event_TickTime ent) {
            if (WarReportData.ReportState != WarReportData.WarReportState.Saveing) return;
            currentTime = ent.TickTime;
        }
        
        private void OnSaveBegin(ISystemMsg body, SE_WarReport.Event_SaveBegin ent) {
            isSaveEnd = false;
            saveList.Clear();
            saveList.Add(new WarReportData.ModeData {
                ReportType = WarReportData.ReportFrameType.Mode,
                Time = currentTime,
                NetworkId = 0,
                ModeId = ModeData.ModeId,
                MatchId = ModeData.MatchId,
                SceneId = ModeData.SceneId,
            });
            saveList.Add(new WarReportData.CreateNetworkData {
                ReportType = WarReportData.ReportFrameType.CreateNetwork,
                NetworkId = networkBase.GetNetworkId(),
                Time = currentTime,
                CreateConnId = networkBase.ConnId(),
                IsCharacter = false,
            });
            CreateCharacterNetwork(1, -99);
        }
        
        public void CreateCharacterNetwork(byte networkId, int createConnId) {
            var go = new GameObject("[ReportCharacterNetwork]");
            var network = go.AddComponent<WarReportCharacterNetwork>();
            go.AddComponent<WarReportCharacterSync>();
            network.Init(createConnId, networkId);
            MsgRegister.Dispatcher(new M_Stage.SetGamePlayWorldLayer {
                layer = StageData.GameWorldLayerType.Base, transform = go.transform
            });
        }
        
        private void OnSaveReport(ISystemMsg body, SE_WarReport.Event_SaveReport ent) {
            if (WarReportData.ReportState != WarReportData.WarReportState.Saveing) return;
            Debug.LogError("[WarReport] 保存报告数据到文件: " + ent.Path + " 当前时间: " + currentTime + " 保存数据数量: " + saveList.Count);
            isSaveEnd = true;
            var data = SaveListToByteBuffer();
            SaveReportData(ent.Path, data);
            saveList.Clear();
        }
        
        public void OnRpcSetReportData(M_WarReport.SetRpcReportData ent) {
            if (isSaveEnd == true) {
                return;
            }
            if (WarReportData.ReportState != WarReportData.WarReportState.Saveing) {
                Debug.LogError("[WarReport] 服務器端没有开启保存状态，无法记录 RPC 数据。请检查是否调用了 SetWarReportState(WarReportState.Saveing) 方法。");
                return;
            }
            saveList.Add(new WarReportData.RpcFrameData {
                ReportType = WarReportData.ReportFrameType.RpcData, 
                NetworkId = ent.NetworkId,
                TargetNetworkId = ent.TargetNetworkId,
                Time = currentTime,
                Bytes = ent.Bytes,
                ConnId = ent.ConnID,
                Channel = ent.Channel
            });
        }
        
        public void OnTargetRpcReportData(M_WarReport.SetTargetRpcReportData ent) {
            if (isSaveEnd == true) {
                return;
            }
            if (WarReportData.ReportState != WarReportData.WarReportState.Saveing) {
                Debug.LogError("[WarReport] 服務器端没有开启保存状态，无法记录 Target RPC 数据。请检查是否调用了 SetWarReportState(WarReportState.Saveing) 方法。");
                return;
            }
            saveList.Add(new WarReportData.RpcFrameData {
                ReportType = WarReportData.ReportFrameType.TargetRpcData, 
                NetworkId = ent.NetworkId,
                TargetNetworkId = ent.TargetNetworkId,
                Time = currentTime,
                Bytes = ent.Bytes,
                ConnId = ent.ConnID,
                Channel = ent.Channel
            });
        }
        
        private void OnSetCharacterSyncDataNetwork(M_WarReport.SetCharacterSyncData ent) {
            if (isSaveEnd == true) {
                return;
            }
            if (WarReportData.ReportState != WarReportData.WarReportState.Saveing) {
                Debug.LogError("[WarReport] 服務器端没有开启保存状态，无法记录角色同步数据。请检查是否调用了 SetWarReportState(WarReportState.Saveing) 方法。");
                return;
            }
            saveList.Add(new WarReportData.CharacterSyncData {
                ReportType = WarReportData.ReportFrameType.SyncCharacterData, 
                NetworkId = ent.NetworkId,
                SyncType = ent.SyncType,
                Value = ent.Value,
                Time = currentTime,
            });
        }
        
        private void OnSetCharacterPointNetwork(M_WarReport.SetCharacterPoint ent) {
            if (isSaveEnd == true) {
                return;
            }
            if (WarReportData.ReportState != WarReportData.WarReportState.Saveing) {
                Debug.LogError("[WarReport] 服務器端没有开启保存状态，无法记录角色位置和旋转数据。请检查是否调用了 SetWarReportState(WarReportState.Saveing) 方法。");
                return;
            }
            saveList.Add(new WarReportData.CharacterPointData {
                ReportType = WarReportData.ReportFrameType.CharacterPoint,
                NetworkId = ent.NetworkId,
                Position = ent.Point,
                Time = currentTime,
            });
        }
        
        private void OnSetCharacterRotaNetwork(M_WarReport.SetCharacterRota ent) {
            if (isSaveEnd == true) {
                return;
            }
            if (WarReportData.ReportState != WarReportData.WarReportState.Saveing) {
                Debug.LogError("[WarReport] 服務器端没有开启保存状态，无法记录角色位置和旋转数据。请检查是否调用了 SetWarReportState(WarReportState.Saveing) 方法。");
                return;
            }
            saveList.Add(new WarReportData.CharacterRotaData {
                ReportType = WarReportData.ReportFrameType.CharacterRota,
                NetworkId = ent.NetworkId,
                Rotation = ent.Rota,
                Time = currentTime,
            });
        }
        
        private byte[] SaveListToByteBuffer() {
            byteBuffer.Write(saveList.Count);
            foreach (var frameData in saveList) {
                byteBuffer.Write((byte)frameData.ReportType);
                byteBuffer.Write(frameData.Time);
                byteBuffer.Write(frameData.NetworkId);
                switch (frameData.ReportType) {
                    case WarReportData.ReportFrameType.Mode:
                        var modeData = (WarReportData.ModeData)frameData;
                        byteBuffer.Write(modeData.ModeId);
                        byteBuffer.Write(modeData.MatchId);
                        byteBuffer.Write(modeData.SceneId);
                        break;
                    case WarReportData.ReportFrameType.CreateNetwork:
                        var createNetworkData = (WarReportData.CreateNetworkData)frameData;
                        byteBuffer.Write(createNetworkData.CreateConnId);
                        byteBuffer.Write(createNetworkData.IsCharacter);
                        break;
                    case WarReportData.ReportFrameType.RpcData:
                    case WarReportData.ReportFrameType.TargetRpcData:
                        var rpcFrameData = (WarReportData.RpcFrameData)frameData;
                        byteBuffer.Write(rpcFrameData.TargetNetworkId);
                        byteBuffer.Write(rpcFrameData.ConnId);
                        byteBuffer.Write(rpcFrameData.Channel);
                        byteBuffer.Write(rpcFrameData.Bytes.Length);
                        foreach (var b in rpcFrameData.Bytes) {
                            byteBuffer.Write(b);
                        }
                        break;
                    case WarReportData.ReportFrameType.SyncCharacterData:
                        var syncData = (WarReportData.CharacterSyncData)frameData;
                        byteBuffer.Write((byte)syncData.SyncType);
                        byteBuffer.Write(syncData.Value);
                        break;
                    case WarReportData.ReportFrameType.CharacterPoint:
                        var pointData = (WarReportData.CharacterPointData)frameData;
                        byteBuffer.Write(pointData.Position);
                        break;
                    case WarReportData.ReportFrameType.CharacterRota:
                        var rotaData = (WarReportData.CharacterRotaData)frameData;
                        byteBuffer.Write(rotaData.Rotation);
                        break;
                    case WarReportData.ReportFrameType.DestroyNetwork:
                        break;
                    default:
                        Debug.LogError("未知的报告帧类型：" + frameData.ReportType);
                        break;
                }
            }
            var copyData = GZipBytes(byteBuffer.RawBuffer, byteBuffer.WriteIndex);
            Debug.LogFormat("[WarReport] GZip 压缩，源数据大小:{0} => {1}", byteBuffer.WriteIndex, copyData.Length);
            return copyData;
        }
        /// <summary>
        /// 将缓存报告同时写入本地 (防止杀端，崩溃后还未上传的数据丢失)
        /// </summary>
        private void SaveReportData(string full, byte[] data) {
            try {
                if (File.Exists(full)) {
                    File.Delete(full);
                }
                // 缺少目录创建目录
                var dir = Path.GetDirectoryName(full);
                if (!Directory.Exists(dir)) {
                    Debug.Log("[WarReport] 缺少目录，创建目录: " + dir);
                    Directory.CreateDirectory(dir);
                }
                File.WriteAllBytes(full, data);
                Debug.Log("[WarReport] 保存报告数据成功  数据长度: " + data.Length);
            } catch (Exception e) {
                Debug.LogError("本地 WarReport 数据写入失败，比如磁盘满了等情况。URL:" + full + " [E.Message] " + e.Message);
            }
        }
        
        public byte[] GZipBytes(byte[] data, int size) {
            try {
                var ms = new MemoryStream();
                var zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(data, 0, size);
                zip.Flush();
                zip.Close();
                var buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
                ms.Close();
                return buffer;
            } catch (Exception e) {
                throw new Exception(e.Message);
            }
        }
    }
}