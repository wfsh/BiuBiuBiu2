using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_Wirebug {
        public const byte ModID = 10;
        public struct Cmd_WirebugState : ICmd {
            public const byte FuncID = 1;
            public const string ID = "Proto_Wirebug.Cmd_WirebugState";
            public string GetID() => ID;

            public int GetChannel() => NetworkData.Channels.Reliable;

            public byte wirebugType;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(wirebugType);
            }

            public void UnSerialize(ByteBuffer buffer) {
                wirebugType = buffer.ReadByte();
            }
        }
        
        public struct TargetRpc_UseWirebugState : ITargetRpc {
            public const byte FuncID = 2;
            public const string ID = "Proto_Wirebug.TargetRpc_UseWirebugState";
            public string GetID() => ID;
            public bool isTrue;

            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isTrue);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isTrue = buffer.ReadBoolean();
            }
        }
        
        public struct TargetRpc_UseWirebug : ITargetRpc {
            public const byte FuncID = 3;
            public const string ID = "Proto_Wirebug.TargetRpc_UseWirebug";
            public string GetID() => ID;
            public List<float> cdTimeList;

            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(cdTimeList.Count);
                for (int i = 0; i < cdTimeList.Count; i++) {
                    buffer.Write(cdTimeList[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                var len = buffer.ReadInt();
                cdTimeList = new List<float>(len);
                for (int i = 0; i < len; i++) {
                    cdTimeList.Add(buffer.ReadFloat());
                }
            }
        }
        
        public struct Rpc_WirebugState : IRpc {
            public const byte FuncID = 4;
            public const string ID = "Proto_Wirebug.Rpc_UseWirebug";
            public string GetID() => ID;

            public int GetChannel() => NetworkData.Channels.Reliable;

            public byte wirebugType;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(wirebugType);
            }

            public void UnSerialize(ByteBuffer buffer) {
                wirebugType = buffer.ReadByte();
            }
        }
        
        public struct TargetRpc_MaxUseWirebugCD : ITargetRpc {
            public const byte FuncID = 5;
            public const string ID = "Proto_Wirebug.TargetRpc_MaxUseWirebugCD";
            public string GetID() => ID;
            public float maxCDTime;

            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(maxCDTime);
            }

            public void UnSerialize(ByteBuffer buffer) {
                maxCDTime = buffer.ReadFloat();
            }
        }
        public struct Cmd_CreateWirebug : ICmd {
            public const byte FuncID = 6;
            public const string ID = "Proto_Wirebug.Cmd_CreateWirebug";
            public string GetID() => ID;
            public Vector3 targetPoint;
            public int Index;

            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(targetPoint);
                buffer.Write(Index);
            }

            public void UnSerialize(ByteBuffer buffer) {
                targetPoint = buffer.ReadVector3();
                Index = buffer.ReadInt();
            }
        }
        
        public struct Rpc_CreateWirebug : IRpc {
            public const byte FuncID = 7;
            public const string ID = "Proto_Wirebug.Rpc_CreateWirebug";
            public string GetID() => ID;

            public int GetChannel() => NetworkData.Channels.Reliable;
            
            public Vector3 targetPoint;
            public int Index;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(targetPoint);
                buffer.Write(Index);
            }

            public void UnSerialize(ByteBuffer buffer) {
                targetPoint = buffer.ReadVector3();
                Index = buffer.ReadInt();
            }
        }

        public static ICmd ReadCmdBuffer(byte id) {
            ICmd doc = null;
            switch (id) {
                case Cmd_WirebugState.FuncID:
                    doc = new Cmd_WirebugState();
                    break;
                case Cmd_CreateWirebug.FuncID:
                    doc = new Cmd_CreateWirebug();
                    break;
                default:
                    Debug.LogError("Proto_Character:ReadCmdBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
        
        public static IProto_Doc ReadRpcBuffer(byte id) {
            IProto_Doc doc = null;
            switch (id) {
                case TargetRpc_UseWirebugState.FuncID:
                    doc = new TargetRpc_UseWirebugState();
                    break;
                case TargetRpc_UseWirebug.FuncID:
                    doc = new TargetRpc_UseWirebug();
                    break;
                case Rpc_WirebugState.FuncID:
                    doc = new Rpc_WirebugState();
                    break;
                case TargetRpc_MaxUseWirebugCD.FuncID:
                    doc = new TargetRpc_MaxUseWirebugCD();
                    break;
                case Rpc_CreateWirebug.FuncID:
                    doc = new Rpc_CreateWirebug();
                    break;
                default:
                    Debug.LogError("Proto_Character:ReadRpcBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
    }
}