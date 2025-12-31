using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_GPOSpawnerWave {
        public const byte ModID = 16;
        public struct Rpc_IntoNextWave : IRpc {
            public const byte FuncID = 1;
            public const string ID = "Proto_AI.Rpc_IntoNextWave";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public byte currentWaveIndex;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(currentWaveIndex);
            }

            public void UnSerialize(ByteBuffer buffer) {
                currentWaveIndex = buffer.ReadByte();
            }
        }
        public struct TargetRpc_SyncData : ITargetRpc {
            public const byte FuncID = 2;
            public const string ID = "Proto_AI.TargetRpc_SyncData";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public byte currentWaveIndex;
            public byte nextWaveIndex;
            public int[] gpoIds;
            public int[] gpoMIds;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(currentWaveIndex);
                buffer.Write(nextWaveIndex);
                var len = gpoIds.Length;
                buffer.Write(len);
                for (int i = 0; i < len; i++) {
                    buffer.Write(gpoIds[i]);
                }
                var lenM = gpoMIds.Length;
                buffer.Write(lenM);
                for (int i = 0; i < lenM; i++) {
                    buffer.Write(gpoMIds[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                currentWaveIndex = buffer.ReadByte();
                nextWaveIndex = buffer.ReadByte();
                var len = buffer.ReadInt();
                gpoIds = new int[len];
                for (int i = 0; i < len; i++) {
                    gpoIds[i] = buffer.ReadInt();
                }
                var lenM = buffer.ReadInt();
                gpoMIds = new int[lenM];
                for (int i = 0; i < lenM; i++) {
                    gpoMIds[i] = buffer.ReadInt();
                }
            }
        }
        
        public struct Rpc_SpawnerGpo : IRpc {
            public const byte FuncID = 3;
            public const string ID = "Proto_AI.Rpc_SpawnerGpo";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public int gpoId;
            public int gpoMId;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gpoId);
                buffer.Write(gpoMId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                gpoId = buffer.ReadInt();
                gpoMId = buffer.ReadInt();
            }
        }
        
        public struct Rpc_DelayWaveSpawnTime : IRpc {
            public const byte FuncID = 4;
            public const string ID = "Proto_AI.Rpc_DelaySpawnTime";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public ushort time;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(time);
            }

            public void UnSerialize(ByteBuffer buffer) {
                time = buffer.ReadUShort();
            }
        }
        
        public struct Rpc_DeadGpo : IRpc {
            public const byte FuncID = 5;
            public const string ID = "Proto_AI.Rpc_DeadGpo";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public int gpoId;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gpoId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                gpoId = buffer.ReadInt();
            }
        }
        public struct Rpc_WaveEnd : IRpc {
            public const byte FuncID = 6;
            public const string ID = "Proto_AI.Rpc_WaveEnd";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public int waveIndex;
            public int maxWaveIndex;
            public bool isWin;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(waveIndex);
                buffer.Write(maxWaveIndex);
                buffer.Write(isWin);
            }

            public void UnSerialize(ByteBuffer buffer) {
                waveIndex = buffer.ReadInt();
                maxWaveIndex = buffer.ReadInt();
                isWin = buffer.ReadBoolean();
            }
        }
        public struct Rpc_IntoWave : IRpc {
            public const byte FuncID = 7;
            public const string ID = "Proto_AI.Rpc_IntoWave";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public byte currentWaveIndex;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(currentWaveIndex);
            }

            public void UnSerialize(ByteBuffer buffer) {
                currentWaveIndex = buffer.ReadByte();
            }
        }
        
        public static IProto_Doc ReadRpcBuffer(byte id) {
            IProto_Doc doc = null;
            switch (id) {
                case Rpc_IntoNextWave.FuncID:
                    doc = new Rpc_IntoNextWave();
                    break;
                case TargetRpc_SyncData.FuncID:
                    doc = new TargetRpc_SyncData();
                    break;
                case Rpc_SpawnerGpo.FuncID:
                    doc = new Rpc_SpawnerGpo();
                    break;
                case Rpc_DelayWaveSpawnTime.FuncID:
                    doc = new Rpc_DelayWaveSpawnTime();
                    break;
                case Rpc_DeadGpo.FuncID:
                    doc = new Rpc_DeadGpo();
                    break;
                case Rpc_WaveEnd.FuncID:
                    doc = new Rpc_WaveEnd();
                    break;
                case Rpc_IntoWave.FuncID:
                    doc = new Rpc_IntoWave();
                    break;
                default:
                    Debug.LogError("Proto_Ability:ReadRpcBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }

        public static ICmd ReadCmdBuffer(byte id) {
            ICmd doc = null;
            switch (id) {
                default:
                    Debug.LogError("Proto_Network:ReadCmdBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
    }
}