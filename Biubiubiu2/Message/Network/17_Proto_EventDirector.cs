using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_EventDirector {
        public const byte ModID = 17;
        public struct Rpc_WaitAction : IRpc {
            public const byte FuncID = 1;
            public const string ID = "Proto_EventDirector.Rpc_WaitAction";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public int eventId;
            public ushort eventDataId;
            public float waitTime;
            public ushort actionType;
            public List<int> gpoIds;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(eventId);
                buffer.Write(eventDataId);
                buffer.Write(waitTime);
                buffer.Write(actionType);
                var len = gpoIds.Count;
                buffer.Write(len);
                for (int i = 0; i < len; i++) {
                    buffer.Write(gpoIds[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                eventId = buffer.ReadInt();
                eventDataId = buffer.ReadUShort();
                waitTime = buffer.ReadFloat();
                actionType = buffer.ReadUShort();
                var len = buffer.ReadInt();
                gpoIds = new List<int>(len);
                for (int i = 0; i < len; i++) {
                    gpoIds.Add(buffer.ReadInt());
                }
            }
        }
        public struct Rpc_EnterAction : IRpc {
            public const byte FuncID = 2;
            public const string ID = "Proto_EventDirector.Rpc_EnterAction";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public int eventId;
            public ushort eventDataId;
            public ushort actionType;
            public List<int> gpoIds;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(eventId);
                buffer.Write(eventDataId);
                buffer.Write(actionType);
                var len = gpoIds.Count;
                buffer.Write(len);
                for (int i = 0; i < len; i++) {
                    buffer.Write(gpoIds[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                eventId = buffer.ReadInt();
                eventDataId = buffer.ReadUShort();
                actionType = buffer.ReadUShort();
                var len = buffer.ReadInt();
                gpoIds = new List<int>(len);
                for (int i = 0; i < len; i++) {
                    gpoIds.Add(buffer.ReadInt());
                }
            }
        }
        public struct Rpc_QuitAction : IRpc {
            public const byte FuncID = 3;
            public const string ID = "Proto_EventDirector.Rpc_QuitAction";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public int eventId;
            public ushort eventDataId;
            public ushort actionType;
            public List<int> gpoIds;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(eventId);
                buffer.Write(eventDataId);
                buffer.Write(actionType);
                var len = gpoIds.Count;
                buffer.Write(len);
                for (int i = 0; i < len; i++) {
                    buffer.Write(gpoIds[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                eventId = buffer.ReadInt();
                eventDataId = buffer.ReadUShort();
                actionType = buffer.ReadUShort();
                var len = buffer.ReadInt();
                gpoIds = new List<int>(len);
                for (int i = 0; i < len; i++) {
                    gpoIds.Add(buffer.ReadInt());
                }
            }
        }
        
        public static IProto_Doc ReadRpcBuffer(byte id) {
            IProto_Doc doc = null;
            switch (id) {
                case Rpc_WaitAction.FuncID:
                    doc = new Rpc_WaitAction();
                    break;
                case Rpc_EnterAction.FuncID:
                    doc = new Rpc_EnterAction();
                    break;
                case Rpc_QuitAction.FuncID:
                    doc = new Rpc_QuitAction();
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