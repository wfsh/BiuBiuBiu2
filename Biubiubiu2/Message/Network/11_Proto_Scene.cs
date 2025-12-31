using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_Scene {
        public const byte ModID = 11;
        public struct TargetRpc_AddSceneElement : ITargetRpc {
            public const string ID = "Proto_Scene.TargetRpc_AddSceneElement";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 1;
            public ushort element;
            public int createID;
            public Vector3 point;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(element);
                buffer.Write(createID);
                buffer.Write(point);
            }

            public void UnSerialize(ByteBuffer buffer) {
                element = buffer.ReadUShort();
                createID = buffer.ReadInt();
                point = buffer.ReadVector3();
            }
        }
        public struct Rpc_AddSceneElement : IRpc {
            public const string ID = "Proto_Scene.Rpc_AddSceneElement";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 2;
            public ushort element;
            public int createID;
            public Vector3 point;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(element);
                buffer.Write(createID);
                buffer.Write(point);
            }

            public void UnSerialize(ByteBuffer buffer) {
                element = buffer.ReadUShort();
                createID = buffer.ReadInt();
                point = buffer.ReadVector3();
            }
        }

        public struct Rpc_RemoveSceneElement : IRpc {
            public const string ID = "Proto_Scene.Rpc_RemoveSceneElement";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 3;
            public int createID;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(createID);
            }

            public void UnSerialize(ByteBuffer buffer) {
                createID = buffer.ReadInt();
            }
        }

        public struct Cmd_GatheringElement : ICmd {
            public const string ID = "Proto_Scene.Cmd_GatheringElement";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 4;
            public int createId;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(createId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                createId = buffer.ReadInt();
            }
        }

        public struct TargetRpc_GatheringElement : ITargetRpc {
            public const string ID = "Proto_Scene.TargetRpc_GatheringElement";
            public string GetID() => ID;
            public const byte FuncID = 5;
            public bool state;
            // 剩余采集次数
            public int count;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(state);
                buffer.Write(count);
            }

            public void UnSerialize(ByteBuffer buffer) {
                state = buffer.ReadBoolean();
                count = buffer.ReadInt();
            }
        }

        public static IProto_Doc ReadRpcBuffer(byte id) {
            IProto_Doc doc = null;
            switch (id) {
                case TargetRpc_AddSceneElement.FuncID:
                    doc = new TargetRpc_AddSceneElement();
                    break;
                case Rpc_AddSceneElement.FuncID:
                    doc = new Rpc_AddSceneElement();
                    break;
                case Rpc_RemoveSceneElement.FuncID:
                    doc = new Rpc_RemoveSceneElement();
                    break;
                case TargetRpc_GatheringElement.FuncID:
                    doc = new TargetRpc_GatheringElement();
                    break;
                default:
                    Debug.LogError("Proto_NaturalResource:ReadRpcBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
        
        public static ICmd ReadCmdBuffer(byte id) {
            ICmd doc = null;
            switch (id) {
                case Cmd_GatheringElement.FuncID:
                    doc = new Cmd_GatheringElement();
                    break;
                default:
                    Debug.LogError("Proto_NaturalResource:ReadCmdBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
    }
}