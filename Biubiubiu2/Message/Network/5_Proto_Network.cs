using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_Network {
        public const byte ModID = 5;

        public struct TargetRpc_SyncSpawnData : ITargetRpc {
            public const string ID = "Proto_Network.TargetRpc_SyncSpawnData";
            public string GetID() => ID;
            public const byte FuncID = 1;
            public int connID;
            public byte connType;
            public byte[] spawnData;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(connID);
                buffer.Write(connType);
                buffer.Write(spawnData.Length);
                for (int i = 0; i < spawnData.Length; i++) {
                    buffer.Write(spawnData[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                connID = buffer.ReadInt();
                connType = buffer.ReadByte();
                var len = buffer.ReadInt();
                spawnData = new byte[len];
                for (int i = 0; i < len; i++) {
                    spawnData[i] = buffer.ReadByte();
                }
            }
        }

        public struct Rpc_TransformPointRota : IRpc {
            public const string ID = "Proto_Network.Rpc_TransformPointRota";
            public string GetID() => ID;
            public const byte FuncID = 2;
            public Vector3 point;
            public Quaternion rota;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(point);
                buffer.Write(rota);
            }

            public void UnSerialize(ByteBuffer buffer) {
                point = buffer.ReadVector3();
                rota = buffer.ReadQuaternion();
            }
        }

        public struct Rpc_TransformPoint : IRpc {
            public const string ID = "Proto_Network.Rpc_TransformPoint";
            public string GetID() => ID;
            public const byte FuncID = 3;
            public Vector3 point;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(point);
            }

            public void UnSerialize(ByteBuffer buffer) {
                point = buffer.ReadVector3();
            }
        }

        public struct Rpc_TransformRota : IRpc {
            public const string ID = "Proto_Network.Rpc_TransformRota";
            public string GetID() => ID;
            public const byte FuncID = 4;
            public Quaternion rota;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(rota);
            }

            public void UnSerialize(ByteBuffer buffer) {
                rota = buffer.ReadQuaternion();
            }
        }

        public struct TargetRpc_RemoveWorldNetwork : ITargetRpc {
            public const string ID = "Proto_Network.TargetRpc_RemoveWorldNetwork";
            public string GetID() => ID;
            public const byte FuncID = 5;
            public int syncId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(syncId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                syncId = buffer.ReadInt();
            }
        }

        public struct Rpc_SyncTransformPoint : IRpc {
            public const string ID = "Proto_Network.Rpc_SyncTransformPoint";
            public string GetID() => ID;
            public const byte FuncID = 6;
            public Vector3 point;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(point);
            }

            public void UnSerialize(ByteBuffer buffer) {
                point = buffer.ReadVector3();
            }
        }
        
        public struct TargetRpc_TransformPointRota : ITargetRpc {
            public const string ID = "Proto_Network.TargetRpc_TransformPointRota";
            public string GetID() => ID;
            public const byte FuncID = 7;
            public Vector3 point;
            public Quaternion rota;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(point);
                buffer.Write(rota);
            }

            public void UnSerialize(ByteBuffer buffer) {
                point = buffer.ReadVector3();
                rota = buffer.ReadQuaternion();
            }
        }
        
        public struct Rpc_TransformScale : IRpc {
            public const string ID = "Proto_Network.Rpc_TransformScale";
            public string GetID() => ID;
            public const byte FuncID = 8;
            public Vector3 scale;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(scale);
            }

            public void UnSerialize(ByteBuffer buffer) {
                scale = buffer.ReadVector3();
            }
        }

        public struct Rpc_SyncTransformRota : IRpc {
            public const string ID = "Proto_Network.Rpc_SyncTransformRota";
            public string GetID() => ID;
            public const byte FuncID = 9;
            public Quaternion rota;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(rota);
            }

            public void UnSerialize(ByteBuffer buffer) {
                rota = buffer.ReadQuaternion();
            }
        }

        public struct Rpc_SyncTransformScale : IRpc {
            public const string ID = "Proto_Network.Rpc_SyncTransformScale";
            public string GetID() => ID;
            public const byte FuncID = 10;
            public Vector3 scale;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(scale);
            }

            public void UnSerialize(ByteBuffer buffer) {
                scale = buffer.ReadVector3();
            }
        }
        
        public struct Rpc_TransformFull : ITargetRpc {
            public const string ID = "Proto_Network.Rpc_TransformFull";
            public string GetID() => ID;
            public const byte FuncID = 11;
            public Vector3 point;
            public Vector3 scale;
            public Quaternion rota;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(point);
                buffer.Write(scale);
                buffer.Write(rota);
            }

            public void UnSerialize(ByteBuffer buffer) {
                point = buffer.ReadVector3();
                scale = buffer.ReadVector3();
                rota = buffer.ReadQuaternion();
            }
        }

        public static IProto_Doc ReadRpcBuffer(byte id) {
            IProto_Doc doc = null;
            switch (id) {
                case TargetRpc_SyncSpawnData.FuncID:
                    doc = new TargetRpc_SyncSpawnData();
                    break;
                case Rpc_TransformPointRota.FuncID:
                    doc = new Rpc_TransformPointRota();
                    break;
                case Rpc_TransformPoint.FuncID:
                    doc = new Rpc_TransformPoint();
                    break;
                case Rpc_TransformRota.FuncID:
                    doc = new Rpc_TransformRota();
                    break;
                case TargetRpc_RemoveWorldNetwork.FuncID:
                    doc = new TargetRpc_RemoveWorldNetwork();
                    break;
                case Rpc_SyncTransformPoint.FuncID:
                    doc = new Rpc_SyncTransformPoint();
                    break;
                case Rpc_SyncTransformRota.FuncID:
                    doc = new Rpc_SyncTransformRota();
                    break;
                case Rpc_SyncTransformScale.FuncID:
                    doc = new Rpc_SyncTransformScale();
                    break;
                case TargetRpc_TransformPointRota.FuncID:
                    doc = new TargetRpc_TransformPointRota();
                    break;
                case Rpc_TransformScale.FuncID:
                    doc = new Rpc_TransformScale();
                    break;
                case Rpc_TransformFull.FuncID:
                    doc = new Rpc_TransformFull();
                    break;
                default:
                    Debug.LogError("Proto_Network:ReadRpcBuffer 没有注册对应 ID:" + id);
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