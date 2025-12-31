using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_AI {
        public const byte ModID = 6;

        public struct TargetRpc_AddAI : ITargetRpc {
            public const byte FuncID = 1;
            public const string ID = "Proto_AI.TargetRpc_AddAI";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public int gpoId;
            public int teamId;
            public ushort gpoMId;
            public string aiSkinSign;
            public Vector3 startPoint;
            public Quaternion startRota;
            public byte[] protoDoc;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gpoId);
                buffer.Write(teamId);
                buffer.Write(gpoMId);
                buffer.Write(startPoint);
                buffer.Write(startRota);
                buffer.Write(aiSkinSign);
                buffer.Write((ushort)protoDoc.Length);
                for (int i = 0; i < protoDoc.Length; i++) {
                    buffer.Write(protoDoc[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                gpoId = buffer.ReadInt();
                teamId = buffer.ReadInt();
                gpoMId = buffer.ReadUShort();
                startPoint = buffer.ReadVector3();
                startRota = buffer.ReadQuaternion();
                aiSkinSign = buffer.ReadString();
                var len = buffer.ReadUShort();
                protoDoc = new byte[len];
                for (int i = 0; i < len; i++) {
                    protoDoc[i] = buffer.ReadByte();
                }
            }
        }

        public struct TargetRpc_AIMaster : ITargetRpc {
            public const byte FuncID = 3;
            public const string ID = "Proto_AI.TargetRpc_AIMaster";
            public string GetID() => ID;
            public int masterGpoID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(masterGpoID);
            }

            public void UnSerialize(ByteBuffer buffer) {
                masterGpoID = buffer.ReadInt();
            }
        }

        public struct Rpc_CatchAI : IRpc {
            public const byte FuncID = 4;
            public const string ID = "Proto_AI.Rpc_CatchAI";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }

        public struct Rpc_TakeBack : IRpc {
            public const byte FuncID = 6;
            public const string ID = "Proto_AI.Rpc_TakeBack";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }

        public struct Rpc_SkillType : IRpc {
            public const byte FuncID = 7;
            public const string ID = "Proto_AI.Rpc_SkillType";
            public string GetID() => ID;
            public bool isSkillType;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isSkillType);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isSkillType = buffer.ReadBoolean();
            }
        }

        public struct Rpc_Anim : IRpc {
            public const byte FuncID = 10;
            public const string ID = "Proto_AI.Rpc_Anim";
            public string GetID() => ID;
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

        public struct TargetRpc_RemoveAI : ITargetRpc {
            public const byte FuncID = 11;
            public const string ID = "Proto_AI.TargetRpc_RemoveAI";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }

        public struct TargetRpc_Anim : ITargetRpc {
            public const byte FuncID = 12;
            public const string ID = "Proto_AI.TargetRpc_Anim";
            public string GetID() => ID;
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

        public struct TargetRpc_SkillType : ITargetRpc {
            public const byte FuncID = 13;
            public const string ID = "Proto_AI.TargetRpc_SkillType";
            public string GetID() => ID;
            public bool isSkillType;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isSkillType);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isSkillType = buffer.ReadBoolean();
            }
        }

        public struct Cmd_TakeOutAI : ICmd {
            public const byte FuncID = 14;
            public const string ID = "Proto_AI.Cmd_TakeOutAI";
            public string GetID() => ID;
            public uint gpoId;
            public Vector3 point;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gpoId);
                buffer.Write(point);
            }

            public void UnSerialize(ByteBuffer buffer) {
                gpoId = buffer.ReadUInt();
                point = buffer.ReadVector3();
            }
        }

        public struct TargetRpc_CatchAI : ITargetRpc {
            public const byte FuncID = 15;
            public const string ID = "Proto_AI.TargetRpc_CatchAI";
            public string GetID() => ID;
            public int aiGpoId;
            public string aiSign;
            public int aiLevel;
            public int maxHp;
            public int nowHp;
            public int atk;
            public int def;
            public int nowSkillPoint;
            public bool isSkillType;
            public int nextExp;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(aiGpoId);
                buffer.Write(aiSign);
                buffer.Write(aiLevel);
                buffer.Write(maxHp);
                buffer.Write(nowHp);
                buffer.Write(nowSkillPoint);
                buffer.Write(isSkillType);
                buffer.Write(atk);
                buffer.Write(def);
                buffer.Write(nextExp);
            }

            public void UnSerialize(ByteBuffer buffer) {
                aiGpoId = buffer.ReadInt();
                aiSign = buffer.ReadString();
                aiLevel = buffer.ReadInt();
                maxHp = buffer.ReadInt();
                nowHp = buffer.ReadInt();
                nowSkillPoint = buffer.ReadInt();
                isSkillType = buffer.ReadBoolean();
                atk = buffer.ReadInt();
                def = buffer.ReadInt();
                nextExp = buffer.ReadInt();
            }
        }

        public struct TargetRpc_FollowAI : ITargetRpc {
            public const byte FuncID = 16;
            public const string ID = "Proto_AI.TargetRpc_FollowAI";
            public string GetID() => ID;
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

        public struct TargetRpc_MasterAIHPChange : ITargetRpc {
            public const byte FuncID = 17;
            public const string ID = "Proto_AI.TargetRpc_MasterAIHPChange";
            public string GetID() => ID;
            public int gpoId;
            public int nowHp;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gpoId);
                buffer.Write(nowHp);
            }

            public void UnSerialize(ByteBuffer buffer) {
                gpoId = buffer.ReadInt();
                nowHp = buffer.ReadInt();
            }
        }

        public struct Cmd_UseAISkill : ICmd {
            public const byte FuncID = 18;
            public const string ID = "Proto_AI.Cmd_UseAISkill";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }

        public struct TargetRpc_FollowAISkillPoint : ITargetRpc {
            public const byte FuncID = 19;
            public const string ID = "Proto_AI.TargetRpc_FollowAISkillPoint";
            public string GetID() => ID;
            public uint aiPID;
            public int skillPoint;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(aiPID);
                buffer.Write(skillPoint);
            }

            public void UnSerialize(ByteBuffer buffer) {
                aiPID = buffer.ReadUInt();
                skillPoint = buffer.ReadInt();
            }
        }

        public struct TargetRpc_ChangeLevel : ITargetRpc {
            public const byte FuncID = 20;
            public const string ID = "Proto_AI.TargetRpc_ChangeLevel";
            public string GetID() => ID;
            public uint aiPid;
            public int level;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(aiPid);
                buffer.Write(level);
            }

            public void UnSerialize(ByteBuffer buffer) {
                aiPid = buffer.ReadUInt();
                level = buffer.ReadInt();
            }
        }

        public struct TargetRpc_UpNextExp : ITargetRpc {
            public const byte FuncID = 21;
            public const string ID = "Proto_AI.TargetRpc_UpNextExp";
            public string GetID() => ID;
            public uint aiPid;
            public int nextExp;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(aiPid);
                buffer.Write(nextExp);
            }

            public void UnSerialize(ByteBuffer buffer) {
                aiPid = buffer.ReadUInt();
                nextExp = buffer.ReadInt();
            }
        }

        public struct Rpc_SyncTankUpperBodyRota : IRpc {
            public const byte FuncID = 22;
            public const string ID = "Proto_AI.Rpc_SyncTankUpperBodyRota";
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

        public struct Rpc_TankFire : IRpc {
            public const byte FuncID = 23;
            public const string ID = "Proto_AI.Rpc_TankFire";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }

        public struct Rpc_SyncHelicopterRootPosY : IRpc {
            public const byte FuncID = 24;
            public const string ID = "Proto_AI.Rpc_SyncHelicopterRootPosY";
            public string GetID() => ID;
            public float py;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(py);
            }

            public void UnSerialize(ByteBuffer buffer) {
                py = buffer.ReadFloat();
            }
        }

        public struct Rpc_SyncHelicopterRootRotaX : IRpc {
            public const byte FuncID = 25;
            public const string ID = "Proto_AI.Rpc_SyncHelicopterRooRotaX";
            public string GetID() => ID;
            public float rx;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(rx);
            }

            public void UnSerialize(ByteBuffer buffer) {
                rx = buffer.ReadFloat();
            }
        }

        public struct Rpc_Effect : IRpc {
            public const byte FuncID = 26;
            public const string ID = "Proto_AI.Rpc_Effect";
            public string GetID() => ID;
            public string effectSign;
            public GPOData.PartEnum offsetParentPart;
            public Vector3 offset;
            public float lifeTime;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(effectSign);
                buffer.Write((short)offsetParentPart);
                buffer.Write(offset);
                buffer.Write(lifeTime);
            }

            public void UnSerialize(ByteBuffer buffer) {
                effectSign = buffer.ReadString();
                offsetParentPart = (GPOData.PartEnum)buffer.ReadShort();
                offset = buffer.ReadVector3();
                lifeTime = buffer.ReadFloat();
            }
        }

        public struct Rpc_GoldDashRemoveFightRange : IRpc {
            public const byte FuncID = 27;
            public const string ID = "Proto_AI.Rpc_GiantDaDaRemoveFightRange";
            public string GetID() => ID;
            public int BelongGPOId;

            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(BelongGPOId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                BelongGPOId = buffer.ReadInt();
            }
        }

        public struct Rpc_GiantDaDaSwitchStage : IRpc {
            public const byte FuncID = 28;
            public const string ID = "Proto_AI.Rpc_GiantDaDaSwitchStage";
            public string GetID() => ID;
            public byte curStage;

            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(curStage);
            }

            public void UnSerialize(ByteBuffer buffer) {
                curStage = buffer.ReadByte();
            }
        }

        public struct TargetRpc_GiantDaDaSwitchStage : ITargetRpc {
            public const byte FuncID = 29;
            public const string ID = "Proto_AI.TargetRpc_GiantDaDaSwitchStage";
            public string GetID() => ID;
            public byte curStage;

            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(curStage);
            }

            public void UnSerialize(ByteBuffer buffer) {
                curStage = buffer.ReadByte();
            }
        }

        public struct TargetRpc_InWorldAIList : ITargetRpc {
            public const byte FuncID = 30;
            public const string ID = "Proto_AI.TargetRpc_InWorldAIList";
            public string GetID() => ID;
            public int[] aiPids;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(aiPids.Length);
                for (int i = 0; i < aiPids.Length; i++) {
                    var aiPid = aiPids[i];
                    buffer.Write(aiPid);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                var length = buffer.ReadInt();
                aiPids = new int[length];
                for (int i = 0; i < length; i++) {
                    aiPids[i] = buffer.ReadInt();
                }
            }
        }
        public struct Rpc_SyncMachineGunUpperBodyRota : IRpc {
            public const byte FuncID = 31;
            public const string ID = "Proto_AI.Rpc_SyncMachineGunUpperBodyRota";
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
        
        public struct Rpc_JokerUAVState : IRpc {
            public const byte FuncID = 32;
            public const string ID = "Proto_AI.Rpc_JokerUAVState";
            public string GetID() => ID;
            public ushort fillValue; // 100
            public byte state;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(fillValue);
                buffer.Write(state);
            }

            public void UnSerialize(ByteBuffer buffer) {
                fillValue = buffer.ReadUShort();
                state = buffer.ReadByte();
            }
        }
        
        public struct TargetRpc_GPOHurtOutOfFightRange : ITargetRpc {
            public const byte FuncID = 33;
            public const string ID = "Proto_AI.TargetRpc_GPOHurtOutOfFightRange";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }
        
        public struct TargetRpc_GPOChangeFightRangeStage : ITargetRpc {
            public const byte FuncID = 34;
            public const string ID = "Proto_AI.TargetRpc_GPOChangeFightRangeStage";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public bool isInFightRange;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isInFightRange);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isInFightRange = buffer.ReadBoolean();
            }
        }
        
        public struct TargetRpc_PlayBubble : ITargetRpc {
            public const byte FuncID = 35;
            public const string ID = "Proto_AI.TargetRpc_PlayBubble";

            public string effectSign;
            public float lifeTime;
            public Vector3 effectPos;

            public string GetID() => ID;

            public int GetChannel() => NetworkData.Channels.Reliable;
            
            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(effectSign);
                buffer.Write(lifeTime);
                buffer.Write(effectPos);
            }

            public void UnSerialize(ByteBuffer buffer) {
                effectSign = buffer.ReadString();
                lifeTime = buffer.ReadFloat();
                effectPos = buffer.ReadVector3();
            }
        }
        
        public struct TargetRpc_AddAIDefault : ITargetRpc {
            public const byte FuncID = 36;
            public const string ID = "Proto_AI.TargetRpc_AddAIDefault";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public int maxHp;
            public int nowHp;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(maxHp);
                buffer.Write(nowHp);
            }

            public void UnSerialize(ByteBuffer buffer) {
                maxHp = buffer.ReadInt();
                nowHp = buffer.ReadInt();
            }
        }       
        
        public struct TargetRpc_AddAIHero : ITargetRpc {
            public const byte FuncID = 37;
            public const string ID = "Proto_AI.TargetRpc_AddAIHero";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public int heroId;
            public int maxHp;
            public int nowHp;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(maxHp);
                buffer.Write(nowHp);
                buffer.Write(heroId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                maxHp = buffer.ReadInt();
                nowHp = buffer.ReadInt();
                heroId = buffer.ReadInt();
            }
        }

        public struct TargetRpc_PlayAudio : ITargetRpc {
            public const byte FuncID = 38;
            public const string ID = "Proto_AI.TargetRpc_PlayAudio";
            public string GetID() => ID;
            public ushort WwiseId;

            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(WwiseId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                WwiseId = buffer.ReadUShort();
            }
        }

        public static ICmd ReadCmdBuffer(byte id) {
            ICmd doc = null;
            switch (id) {
                case Cmd_TakeOutAI.FuncID:
                    doc = new Cmd_TakeOutAI();
                    break;
                case Cmd_UseAISkill.FuncID:
                    doc = new Cmd_UseAISkill();
                    break;
                default:
                    Debug.LogError("Proto_Network:ReadCmdBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }

        public static IProto_Doc ReadRpcBuffer(byte id) {
            IProto_Doc doc = null;
            switch (id) {
                case TargetRpc_AddAI.FuncID:
                    doc = new TargetRpc_AddAI();
                    break;
                case Rpc_CatchAI.FuncID:
                    doc = new Rpc_CatchAI();
                    break;
                case TargetRpc_AIMaster.FuncID:
                    doc = new TargetRpc_AIMaster();
                    break;
                case Rpc_TakeBack.FuncID:
                    doc = new Rpc_TakeBack();
                    break;
                case Rpc_SkillType.FuncID:
                    doc = new Rpc_SkillType();
                    break;
                case Rpc_Anim.FuncID:
                    doc = new Rpc_Anim();
                    break;
                case TargetRpc_RemoveAI.FuncID:
                    doc = new TargetRpc_RemoveAI();
                    break;
                case TargetRpc_Anim.FuncID:
                    doc = new TargetRpc_Anim();
                    break;
                case TargetRpc_SkillType.FuncID:
                    doc = new TargetRpc_SkillType();
                    break;
                case TargetRpc_CatchAI.FuncID:
                    doc = new TargetRpc_CatchAI();
                    break;
                case TargetRpc_FollowAI.FuncID:
                    doc = new TargetRpc_FollowAI();
                    break;
                case TargetRpc_MasterAIHPChange.FuncID:
                    doc = new TargetRpc_MasterAIHPChange();
                    break;
                case TargetRpc_FollowAISkillPoint.FuncID:
                    doc = new TargetRpc_FollowAISkillPoint();
                    break;
                case TargetRpc_ChangeLevel.FuncID:
                    doc = new TargetRpc_ChangeLevel();
                    break;
                case TargetRpc_UpNextExp.FuncID:
                    doc = new TargetRpc_UpNextExp();
                    break;
                case Rpc_SyncTankUpperBodyRota.FuncID:
                    doc = new Rpc_SyncTankUpperBodyRota();
                    break;
                case Rpc_TankFire.FuncID:
                    doc = new Rpc_TankFire();
                    break;
                case Rpc_SyncHelicopterRootPosY.FuncID:
                    doc = new Rpc_SyncHelicopterRootPosY();
                    break;
                case Rpc_SyncHelicopterRootRotaX.FuncID:
                    doc = new Rpc_SyncHelicopterRootRotaX();
                    break;
                case Rpc_Effect.FuncID:
                    doc = new Rpc_Effect();
                    break;
                case Rpc_GoldDashRemoveFightRange.FuncID:
                    doc = new Rpc_GoldDashRemoveFightRange();
                    break;
                case Rpc_GiantDaDaSwitchStage.FuncID:
                    doc = new Rpc_GiantDaDaSwitchStage();
                    break;
                case TargetRpc_GiantDaDaSwitchStage.FuncID:
                    doc = new TargetRpc_GiantDaDaSwitchStage();
                    break;
                case TargetRpc_InWorldAIList.FuncID:
                    doc = new TargetRpc_InWorldAIList();
                    break;
                case Rpc_SyncMachineGunUpperBodyRota.FuncID:
                    doc = new Rpc_SyncMachineGunUpperBodyRota();
                    break;
                case Rpc_JokerUAVState.FuncID:
                    doc = new Rpc_JokerUAVState();
                    break;
                case TargetRpc_GPOHurtOutOfFightRange.FuncID:
                    doc = new TargetRpc_GPOHurtOutOfFightRange();
                    break;
                case TargetRpc_GPOChangeFightRangeStage.FuncID:
                    doc = new TargetRpc_GPOChangeFightRangeStage();
                    break;
                case TargetRpc_PlayBubble.FuncID:
                    doc = new TargetRpc_PlayBubble();
                    break;
                case TargetRpc_AddAIDefault.FuncID:
                    doc = new TargetRpc_AddAIDefault();
                    break;
                case TargetRpc_AddAIHero.FuncID:
                    doc = new TargetRpc_AddAIHero();
                    break;
                case TargetRpc_PlayAudio.FuncID:
                    doc = new TargetRpc_PlayAudio();
                    break;
                default:
                    Debug.LogError("Proto_Ability:ReadRpcBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
    }
}