using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_GPO {
        public const byte ModID = 7;

        public struct TargetRpc_SyncGPOID : ITargetRpc {
            public const string ID = "Proto_GPO.TargetRpc_SyncGPOID";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 1;
            public int gpoID;
            public int teamID;
            public GPOData.GPOType gpoType;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gpoID);
                buffer.Write(teamID);
                buffer.Write((int)gpoType);
            }

            public void UnSerialize(ByteBuffer buffer) {
                gpoID = buffer.ReadInt();
                teamID = buffer.ReadInt();
                gpoType = (GPOData.GPOType)buffer.ReadInt();
            }
        }

        public struct Rpc_ChangeHP : IRpc {
            public const string ID = "Proto_GPO.Rpc_ChangeHP";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 3;
            public int nowHP;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(nowHP);
            }

            public void UnSerialize(ByteBuffer buffer) {
                nowHP = buffer.ReadInt();
            }
        }

        public struct TargetRpc_ChangeATK : ITargetRpc {
            public const string ID = "Proto_GPO.TargetRpc_ChangeATK";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 4;
            public int nowATK;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(nowATK);
            }

            public void UnSerialize(ByteBuffer buffer) {
                nowATK = buffer.ReadInt();
            }
        }

        public struct Rpc_ChangeMaxHP : IRpc {
            public const string ID = "Proto_GPO.Rpc_ChangeMaxHP";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 5;
            public int nowMaxHP;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(nowMaxHP);
            }

            public void UnSerialize(ByteBuffer buffer) {
                nowMaxHP = buffer.ReadInt();
            }
        }

        public struct Rpc_ChangeLevel : IRpc {
            public const string ID = "Proto_GPO.Rpc_ChangeLevel";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 6;
            public int level;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(level);
            }

            public void UnSerialize(ByteBuffer buffer) {
                level = buffer.ReadInt();
            }
        }

        public struct Rpc_IsGodMode : IRpc {
            public const string ID = "Proto_GPO.Rpc_IsGodMode";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 7;
            public bool isTrue;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isTrue);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isTrue = buffer.ReadBoolean();
            }
        }

        public struct TargetRpc_IsGodMode : ITargetRpc {
            public const string ID = "Proto_GPO.TargetRpc_IsGodMode";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 8;
            public bool isTrue;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isTrue);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isTrue = buffer.ReadBoolean();
            }
        }

        public struct Rpc_IsShowEntity : IRpc {
            public const string ID = "Proto_GPO.Rpc_IsShowEntity";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 9;
            public bool isTrue;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isTrue);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isTrue = buffer.ReadBoolean();
            }
        }

        public struct Rpc_Dead : IRpc {
            public const byte FuncID = 10;
            public const string ID = "Proto_GPO.Rpc_Dead";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }

        public struct Rpc_ReLife : IRpc {
            public const byte FuncID = 11;
            public const string ID = "Proto_GPO.Rpc_ReLife";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public Vector3 relifePos;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(relifePos);
            }

            public void UnSerialize(ByteBuffer buffer) {
                relifePos = buffer.ReadVector3();
            }
        }

        public struct Rpc_RemoveGPO : IRpc {
            public const byte FuncID = 12;
            public const string ID = "Proto_GPO.Rpc_RemoveGPO";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }

        public struct TargetRpc_IsShowEntity : ITargetRpc {
            public const string ID = "Proto_GPO.TargetRpc_IsShowEntity";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 13;
            public bool isShowEntity;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isShowEntity);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isShowEntity = buffer.ReadBoolean();
            }
        }

        public struct Rpc_AfterDownHP : IRpc {
            public const string ID = "Proto_GPO.Rpc_AfterDownHP";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 14;
            public int attackerGPOId;
            public float DownHp;
            public bool isHead;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(attackerGPOId);
                buffer.Write(DownHp);
                buffer.Write(isHead);
            }

            public void UnSerialize(ByteBuffer buffer) {
                attackerGPOId = buffer.ReadInt();
                DownHp = buffer.ReadFloat();
                isHead = buffer.ReadBoolean();
            }
        }

        public struct Rpc_GpoMat : IRpc {
            public const string ID = "Proto_GPO.Rpc_GpoMat";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 15;
            public byte matType;
            public string teamId;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(matType);
                buffer.Write(teamId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                matType = buffer.ReadByte();
                teamId = buffer.ReadString();
            }
        }

        public struct TargetRpc_GpoMat : ITargetRpc {
            public const string ID = "Proto_GPO.TargetRpc_GpoMat";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 16;
            public byte matType;
            public string teamId;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(matType);
                buffer.Write(teamId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                matType = buffer.ReadByte();
                teamId = buffer.ReadString();
            }
        }
        
        public struct TargetRpc_Speed : ITargetRpc {
            public const string ID = "Proto_GPO.TargetRpc_Speed";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public const byte FuncID = 17;
            public float speed;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(speed);
            }

            public void UnSerialize(ByteBuffer buffer) {
                speed = buffer.ReadFloat();
            }
        }

        public static IProto_Doc ReadRpcBuffer(byte id) {
            IProto_Doc doc = null;
            switch (id) {
                case TargetRpc_SyncGPOID.FuncID:
                    doc = new TargetRpc_SyncGPOID();
                    break;
                case Rpc_ChangeHP.FuncID:
                    doc = new Rpc_ChangeHP();
                    break;
                case TargetRpc_ChangeATK.FuncID:
                    doc = new TargetRpc_ChangeATK();
                    break;
                case Rpc_ChangeMaxHP.FuncID:
                    doc = new Rpc_ChangeMaxHP();
                    break;
                case Rpc_ChangeLevel.FuncID:
                    doc = new Rpc_ChangeLevel();
                    break;
                case Rpc_IsGodMode.FuncID:
                    doc = new Rpc_IsGodMode();
                    break;
                case TargetRpc_IsGodMode.FuncID:
                    doc = new TargetRpc_IsGodMode();
                    break;
                case Rpc_IsShowEntity.FuncID:
                    doc = new Rpc_IsShowEntity();
                    break;
                case TargetRpc_IsShowEntity.FuncID:
                    doc = new TargetRpc_IsShowEntity();
                    break;
                case Rpc_AfterDownHP.FuncID:
                    doc = new Rpc_AfterDownHP();
                    break;
                case Rpc_Dead.FuncID:
                    doc = new Rpc_Dead();
                    break;
                case Rpc_ReLife.FuncID:
                    doc = new Rpc_ReLife();
                    break;
                case Rpc_RemoveGPO.FuncID:
                    doc = new Rpc_RemoveGPO();
                    break;
                case Rpc_GpoMat.FuncID:
                    doc = new Rpc_GpoMat();
                    break;
                case TargetRpc_GpoMat.FuncID:
                    doc = new TargetRpc_GpoMat();
                    break;
                case TargetRpc_Speed.FuncID:
                    doc = new TargetRpc_Speed();
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
                    Debug.LogError("Proto_GPO:ReadCmdBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
    }
}