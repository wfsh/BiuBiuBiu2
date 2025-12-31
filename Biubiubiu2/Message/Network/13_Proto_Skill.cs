using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_Skill {
        public const byte ModID = 13;
        public struct Cmd_UseSkill : ICmd {
            public const byte FuncID = 1;
            public const string ID = "Proto_Skill.Cmd_UseSkill";
            public string GetID() => ID;
            public int skillId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(skillId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                skillId = buffer.ReadInt();
            }
        }

        public struct TargetRpc_Skill : ITargetRpc {
            public const byte FuncID = 2;
            public const string ID = "Proto_Skill.Cmd_UpdateSkillPoint";
            public string GetID() => ID;
            public int skillId;
            public int skillPoint;
            public float skillCD;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(skillId);
                buffer.Write(skillPoint);
            }

            public void UnSerialize(ByteBuffer buffer) {
                skillId = buffer.ReadInt();
                skillPoint = buffer.ReadInt();
            }
        }

        public struct TargetRpc_UpdateSkillPoint : ITargetRpc {
            public const byte FuncID = 3;
            public const string ID = "Proto_Skill.TargetRpc_UpdateSkillPoint";
            public string GetID() => ID;
            public int skillId;
            public int skillPoint;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(skillId);
                buffer.Write(skillPoint);
            }

            public void UnSerialize(ByteBuffer buffer) {
                skillId = buffer.ReadInt();
                skillPoint = buffer.ReadInt();
            }
        }
        public struct TargetRpc_StartUseSkill : ITargetRpc {
            public const byte FuncID = 4;
            public const string ID = "Proto_Skill.TargetRpc_StartUseSkill";
            public string GetID() => ID;
            public int skillId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(skillId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                skillId = buffer.ReadInt();
            }
        }
        public struct TargetRpc_UseSkillEnd : ITargetRpc {
            public const byte FuncID = 5;
            public const string ID = "Proto_Skill.TargetRpc_UseSkillEnd";
            public string GetID() => ID;
            public int skillId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(skillId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                skillId = buffer.ReadInt();
            }
        }
        
        

        public struct TargetRpc_RemoveAllSkills : ITargetRpc {
            public const byte FuncID = 6;
            public const string ID = "Proto_Skill.TargetRpc_RemoveAllSkills";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) { }
        }

        public struct Rpc_HoldOnSign : IRpc {
            public const byte FuncID = 7;
            public const string ID = "Proto_Skill.Rpc_SkillHoldOnSign";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public string holdOnSign;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(holdOnSign);
            }

            public void UnSerialize(ByteBuffer buffer) {
                holdOnSign = buffer.ReadString();
            }
        }

        public struct TargetRpc_HoldOnSign : ITargetRpc {
            public const byte FuncID = 8;
            public const string ID = "Proto_Skill.TargetRpc_HoldOnSign";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public string holdOnSign;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(holdOnSign);
            }

            public void UnSerialize(ByteBuffer buffer) {
                holdOnSign = buffer.ReadString();
            }
        }

        public struct Cmd_CancelUseSkill : ICmd {
            public const byte FuncID = 9;
            public const string ID = "Proto_Skill.Cmd_CancelUseSkill";
            public string GetID() => ID;
            public int skillId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(skillId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                skillId = buffer.ReadInt();
            }
        }

        public struct TargetRpc_SkillAreaRadius : ITargetRpc {
            public const byte FuncID = 10;
            public const string ID = "Proto_Skill.TargetRpc_SkillAreaRadius";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public float areaRadius;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(areaRadius);
            }

            public void UnSerialize(ByteBuffer buffer) {
                areaRadius = buffer.ReadFloat();
            }
        }

        public struct TargetRpc_SetSkillInProgress : ITargetRpc {
            public const byte FuncID = 11;
            public const string ID = "Proto_Skill.TargetRpc_SetSkillInProgress";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public bool isInProgress;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isInProgress);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isInProgress = buffer.ReadBoolean();
            }
        }

        public struct TargetRpc_UseSkillFailed : ITargetRpc {
            public const byte FuncID = 12;
            public const string ID = "Proto_Skill.TargetRpc_UseSkillFailed";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public byte failedReason;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(failedReason);
            }

            public void UnSerialize(ByteBuffer buffer) {
                failedReason = buffer.ReadByte();
            }
        }

        public static ICmd ReadCmdBuffer(byte id) {
            ICmd doc = null;
            switch (id) {
                case Cmd_UseSkill.FuncID:
                    doc = new Cmd_UseSkill();
                    break;
                case Cmd_CancelUseSkill.FuncID:
                    doc = new Cmd_CancelUseSkill();
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
                case TargetRpc_Skill.FuncID:
                    doc = new TargetRpc_Skill();
                    break;
                case TargetRpc_UpdateSkillPoint.FuncID:
                    doc = new TargetRpc_UpdateSkillPoint();
                    break;
                case TargetRpc_StartUseSkill.FuncID:
                    doc = new TargetRpc_StartUseSkill();
                    break;
                case TargetRpc_UseSkillEnd.FuncID:
                    doc = new TargetRpc_UseSkillEnd();
                    break;
                case TargetRpc_RemoveAllSkills.FuncID:
                    doc = new TargetRpc_RemoveAllSkills();
                    break;
                case Rpc_HoldOnSign.FuncID:
                    doc = new Rpc_HoldOnSign();
                    break;
                case TargetRpc_HoldOnSign.FuncID:
                    doc = new TargetRpc_HoldOnSign();
                    break;
                case TargetRpc_SkillAreaRadius.FuncID:
                    doc = new TargetRpc_SkillAreaRadius();
                    break;
                case TargetRpc_SetSkillInProgress.FuncID:
                    doc = new TargetRpc_SetSkillInProgress();
                    break;
                case TargetRpc_UseSkillFailed.FuncID:
                    doc = new TargetRpc_UseSkillFailed();
                    break;
                default:
                    Debug.LogError("Proto_Character:ReadRpcBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
    }
}