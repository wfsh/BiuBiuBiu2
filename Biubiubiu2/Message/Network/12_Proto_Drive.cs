using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_Drive {
        public const byte ModID = 12;

        public struct Rpc_DriveGPOID : IRpc {
            public const string ID = "Proto_Drive.Rpc_DriveGPOID";
            public string GetID() => ID;
            public const byte FuncID = 1;
            public int gpoId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gpoId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                gpoId = buffer.ReadInt();
            }
        }

        public struct TargetRpc_DriveGPOID : ITargetRpc {
            public const string ID = "Proto_Drive.TargetRpc_DriveGPOID";
            public string GetID() => ID;
            public const byte FuncID = 2;
            public int gpoId;
            public Vector3 drivePos;
            public Quaternion driveRota;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gpoId);
                buffer.Write(drivePos);
                buffer.Write(driveRota);
            }

            public void UnSerialize(ByteBuffer buffer) {
                gpoId = buffer.ReadInt();
                drivePos = buffer.ReadVector3();
                driveRota = buffer.ReadQuaternion();
            }
        }

        public struct Cmd_DriveAnim : ICmd {
            public const string ID = "Proto_Drive.Cmd_DriveAnim";
            public string GetID() => ID;
            public const byte FuncID = 3;
            public ushort animId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(animId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                animId = buffer.ReadUShort();
            }
        }

        public struct Rpc_DriveAnim : IRpc {
            public const string ID = "Proto_Drive.Rpc_DriveAnim";
            public string GetID() => ID;
            public const byte FuncID = 4;
            public ushort animId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(animId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                animId = buffer.ReadUShort();
            }
        }

        public struct Cmd_SyncTankUpperBodyRota : ICmd {
            public const byte FuncID = 5;
            public const string ID = "Proto_Monster.Cmd_SyncTankUpperBodyRota";
            public string GetID() => ID;
            public Vector3 eulerAngles;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(eulerAngles);
            }

            public void UnSerialize(ByteBuffer buffer) {
                eulerAngles = buffer.ReadVector3();
            }
        }

        public struct Cmd_DeviceFire : ICmd {
            public const byte FuncID = 6;
            public const string ID = "Proto_Monster.Cmd_DeviceFire";
            public string GetID() => ID;
            public Vector3[] points;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                var len = points.Length;
                buffer.Write(len);
                for (int i = 0; i < len; i++) {
                    buffer.Write(points[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                var len = buffer.ReadInt();
                points = new Vector3[len];
                for (int i = 0; i < len; i++) {
                    points[i] = buffer.ReadVector3();
                }
            }
        }

        public struct TargetRpc_DeviceFireState : ITargetRpc {
            public const byte FuncID = 6;
            public const string ID = "Proto_Monster.TargetRpc_DeviceFireState";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public bool isSuccess;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isSuccess);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isSuccess = buffer.ReadBoolean();
            }
        }
        public static ICmd ReadCmdBuffer(byte id) {
            ICmd doc = null;
            switch (id) {
                case Cmd_DriveAnim.FuncID:
                    doc = new Cmd_DriveAnim();
                    break;
                case Cmd_SyncTankUpperBodyRota.FuncID:
                    doc = new Cmd_SyncTankUpperBodyRota();
                    break;
                case Cmd_DeviceFire.FuncID:
                    doc = new Cmd_DeviceFire();
                    break;
                default:
                    Debug.LogError("Proto_Drive:ReadCmdBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }

        public static IProto_Doc ReadRpcBuffer(byte id) {
            IProto_Doc doc = null;
            switch (id) {
                case Rpc_DriveGPOID.FuncID:
                    doc = new Rpc_DriveGPOID();
                    break;
                case TargetRpc_DriveGPOID.FuncID:
                    doc = new TargetRpc_DriveGPOID();
                    break;
                case Rpc_DriveAnim.FuncID:
                    doc = new Rpc_DriveAnim();
                    break;
                case TargetRpc_DeviceFireState.FuncID:
                    doc = new TargetRpc_DeviceFireState();
                    break;
                default:
                    Debug.LogError("Proto_Character:ReadRpcBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
    }
}