using System.Collections;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_Login {
        public const byte ModID = 1;

        public struct Cmd_Login : ICmd {
            public const string ID = "Proto_Login.Cmd_Login";
            public string GetID() => ID;
            public const byte FuncID = 1;
            public long playerId;
            public long version;
            public int modeId;
            public string warId;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(playerId);
                buffer.Write(version);
                buffer.Write(modeId);
                buffer.Write(warId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                playerId = buffer.ReadLong();
                version = buffer.ReadLong();
                modeId = buffer.ReadInt();
                warId = buffer.ReadString();
            }

            public int GetChannel() => NetworkData.Channels.Reliable;
        }

        public struct TargetRpc_Login_State : ITargetRpc {
           public const string ID = "Proto_Login.TargetRpc_Login_State";
            public string GetID() => ID;
            public const byte FuncID = 2;
            public int state;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(state);
            }

            public void UnSerialize(ByteBuffer buffer) {
                state = buffer.ReadInt();
            }

            public int GetChannel() => NetworkData.Channels.Reliable;
        }

        public struct TargetRpc_RoleInfo : ITargetRpc {
           public const string ID = "Proto_Login.TargetRpc_RoleInfo";
            public string GetID() => ID;
            public const byte FuncID = 3;
            public int gpoId;
            public int teamId;
            public Vector3 point;
            public Quaternion rota;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gpoId);
                buffer.Write(teamId);
                buffer.Write(point);
                buffer.Write(rota);
            }

            public void UnSerialize(ByteBuffer buffer) {
                gpoId = buffer.ReadInt();
                teamId = buffer.ReadInt();
                point = buffer.ReadVector3();
                rota = buffer.ReadQuaternion();
            }

            public int GetChannel() => NetworkData.Channels.Reliable;
        }

        public struct Cmd_Login_Stage : ICmd {
           public const string ID = "Proto_Login.Cmd_Login_Stage";
            public string GetID() => ID;
            public const byte FuncID = 4;
            public long playerId;
            public string name;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(playerId);
                buffer.Write(name);
            }

            public void UnSerialize(ByteBuffer buffer) {
                playerId = buffer.ReadLong();
                name = buffer.ReadString();
            }

            public int GetChannel() => NetworkData.Channels.Reliable;
        }
        
        public struct TargetRpc_CharacterInfo : ITargetRpc {
            public const string ID = "Proto_Login.TargetRpc_CharacterInfo";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 5;
            public long playerId;
            public float speed;
            public float jumpHeight;
            public float airJumpHeight;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(playerId);
                buffer.Write(speed);
                buffer.Write(jumpHeight);
                buffer.Write(airJumpHeight);
            }

            public void UnSerialize(ByteBuffer buffer) {
                playerId = buffer.ReadLong();
                speed = buffer.ReadFloat();
                jumpHeight = buffer.ReadFloat();
                airJumpHeight = buffer.ReadFloat();
            }
        }

        public static ICmd ReadCmdBuffer(byte id) {
            ICmd doc = null;
            switch (id) {
                case Cmd_Login.FuncID:
                    doc = new Cmd_Login();
                    break;
                case Cmd_Login_Stage.FuncID:
                    doc = new Cmd_Login_Stage();
                    break;
                default:
                    Debug.LogError("Proto_Login:ReadCmdBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }

        public static IProto_Doc ReadRpcBuffer(byte id) {
            IProto_Doc doc = null;
            switch (id) {
                case TargetRpc_Login_State.FuncID:
                    doc = new TargetRpc_Login_State();
                    break;
                case TargetRpc_RoleInfo.FuncID:
                    doc = new TargetRpc_RoleInfo();
                    break;
                case TargetRpc_CharacterInfo.FuncID:
                    doc = new TargetRpc_CharacterInfo();
                    break;
                default:
                    Debug.LogError("Proto_Login:ReadRpcBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
    }
}