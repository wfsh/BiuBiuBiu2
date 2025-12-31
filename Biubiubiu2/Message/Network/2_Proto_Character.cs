using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_Character {
        public const byte ModID = 2;

        public struct Cmd_JumpType : ICmd {
            public const string ID = "Proto_Character.Cmd_JumpType";
            public string GetID() => ID;
            public const byte FuncID = 1;
            public CharacterData.JumpType jumpType;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write((int)jumpType);
            }

            public void UnSerialize(ByteBuffer buffer) {
                jumpType = (CharacterData.JumpType)buffer.ReadInt();
            }

            public int GetChannel() => NetworkData.Channels.Reliable;
        }

        public struct Cmd_FlyType : ICmd {
            public const string ID = "Proto_Character.Cmd_FlyType";
            public string GetID() => ID;
            public const byte FuncID = 2;
            public CharacterData.FlyType flyType;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write((int)flyType);
            }

            public void UnSerialize(ByteBuffer buffer) {
                flyType = (CharacterData.FlyType)buffer.ReadInt();
            }
        }

        public struct Cmd_StandType : ICmd {
            public const string ID = "Proto_Character.Cmd_StandType";
            public string GetID() => ID;
            public const byte FuncID = 3;
            public CharacterData.StandType standType;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write((int)standType);
            }

            public void UnSerialize(ByteBuffer buffer) {
                standType = (CharacterData.StandType)buffer.ReadInt();
            }
        }

        public struct Cmd_MoveDir : ICmd {
            public const string ID = "Proto_Character.Cmd_MoveDir";
            public string GetID() => ID;
            public const byte FuncID = 4;
            public short moveX;
            public short moveY;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(moveX);
                buffer.Write(moveY);
            }

            public void UnSerialize(ByteBuffer buffer) {
                moveX = buffer.ReadShort();
                moveY = buffer.ReadShort();
            }
        }

        public struct Rpc_MoveDir : IRpc {
            public const string ID = "Proto_Character.Rpc_MoveDir";
            public string GetID() => ID;
            public const byte FuncID = 5;
            public short moveX;
            public short moveY;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(moveX);
                buffer.Write(moveY);
            }

            public void UnSerialize(ByteBuffer buffer) {
                moveX = buffer.ReadShort();
                moveY = buffer.ReadShort();
            }
        }

        public struct Cmd_Dodga : ICmd {
            public const string ID = "Proto_Character.Cmd_Dodga";
            public string GetID() => ID;
            public const byte FuncID = 6;
            public bool isDodge;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isDodge);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isDodge = buffer.ReadBoolean();
            }
        }

        public struct Cmd_CameraVRota : ICmd {
            public const string ID = "Proto_Character.Cmd_CameraVRota";
            public string GetID() => ID;
            public const byte FuncID = 7;
            public short vRota;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(vRota);
            }

            public void UnSerialize(ByteBuffer buffer) {
                vRota = buffer.ReadShort();
            }
        }

        public struct Rpc_CameraVRota : IRpc {
            public const string ID = "Proto_Character.Rpc_CameraVRota";
            public string GetID() => ID;
            public const byte FuncID = 8;
            public short vRota;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(vRota);
            }

            public void UnSerialize(ByteBuffer buffer) {
                vRota = buffer.ReadShort();
            }
        }
        public struct Cmd_FallToGrounded : ICmd {
            public const byte FuncID = 13;
            public const string ID = "Proto_Character.Cmd_FallToGrounded";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }

        public struct Rpc_FallToGrounded : IRpc {
            public const byte FuncID = 14;
            public const string ID = "Proto_Character.Rpc_FallToGrounded";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }

        public struct Cmd_StayPlatformMovement : ICmd {
            public const byte FuncID = 15;
            public const string ID = "Proto_Character.Cmd_StayPlatformMovement";
            public string GetID() => ID;
            public int elementId;
            public bool isStay;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(elementId);
                buffer.Write(isStay);
            }

            public void UnSerialize(ByteBuffer buffer) {
                elementId = buffer.ReadInt();
                isStay = buffer.ReadBoolean();
            }
        }

        public struct Cmd_PlatformMovement : ICmd {
            public const byte FuncID = 16;
            public const string ID = "Proto_Character.Cmd_PlatformMovement";
            public string GetID() => ID;
            public int elementId;
            public Vector3 localPoint;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(elementId);
                buffer.Write(localPoint);
            }

            public void UnSerialize(ByteBuffer buffer) {
                elementId = buffer.ReadInt();
                localPoint = buffer.ReadVector3();
            }
        }


        public struct TargetRpc_UseAerocraft : ITargetRpc {
            public const byte FuncID = 23;
            public const string ID = "Proto_Character.TargetRpc_UseAerocraft";
            public string GetID() => ID;
            public string aerocraftSign;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(aerocraftSign);
            }

            public void UnSerialize(ByteBuffer buffer) {
                aerocraftSign = buffer.ReadString();
            }
        }

        public struct TargetRpc_Knockback : ITargetRpc {
            public const byte FuncID = 29;
            public const string ID = "Proto_Character.TargetRpc_Knockback";
            public string GetID() => ID;
            public Vector3 Dir; // 朝向
            public float Speed; // 击飞速度
            public float Duration; // 击飞持续时间
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(Dir);
                buffer.Write(Speed);
                buffer.Write(Duration);
            }

            public void UnSerialize(ByteBuffer buffer) {
                Dir = buffer.ReadVector3();
                Speed = buffer.ReadFloat();
                Duration = buffer.ReadFloat();
            }
        }

        public struct TargetRpc_StrikeFly : ITargetRpc {
            public const byte FuncID = 30;
            public const string ID = "Proto_Character.TargetRpc_StrikeFly";
            public string GetID() => ID;
            public Vector3 Dir; // 朝向
            public float Force; // 击飞力度
            public float Duration; // 击飞持续时间
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(Dir);
                buffer.Write(Force);
                buffer.Write(Duration);
            }

            public void UnSerialize(ByteBuffer buffer) {
                Dir = buffer.ReadVector3();
                Force = buffer.ReadFloat();
                Duration = buffer.ReadFloat();
            }
        }

        public struct TargetRpc_MovePoint : ITargetRpc {
            public const byte FuncID = 31;
            public const string ID = "Proto_Character.TargetRpc_MovePoint";
            public string GetID() => ID;
            public Vector3 Point; // 朝向
            public Quaternion Rotation; // 朝向
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(Point);
                buffer.Write(Rotation);
            }

            public void UnSerialize(ByteBuffer buffer) {
                Point = buffer.ReadVector3();
                Rotation = buffer.ReadQuaternion();
            }
        }

        public struct Cmd_Ping : ICmd {
            public const byte FuncID = 32;
            public const string ID = "Proto_Character.Cmd_Ping";
            public string GetID() => ID;
            public byte index;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(index);
            }

            public void UnSerialize(ByteBuffer buffer) {
                index = buffer.ReadByte();
            }
        }

        public struct TargetRpc_Ping : ITargetRpc {
            public const byte FuncID = 33;
            public const string ID = "Proto_Character.TargetRpc_Ping";
            public string GetID() => ID;
            public byte index;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(index);
            }

            public void UnSerialize(ByteBuffer buffer) {
                index = buffer.ReadByte();
            }
        }

        public struct Cmd_TakeBackAerocraft : ICmd {
            public const byte FuncID = 34;
            public const string ID = "Proto_Character.Cmd_TakeBackAerocraft";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
            }

            public void UnSerialize(ByteBuffer buffer) {
            }
        }

        public struct TargetRpc_UsePackAerocraft : ITargetRpc {
            public const byte FuncID = 35;
            public const string ID = "Proto_Character.TargetRpc_UsePackAerocraft";
            public string GetID() => ID;
            public string aerocraftSign;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(aerocraftSign);
            }

            public void UnSerialize(ByteBuffer buffer) {
                aerocraftSign = buffer.ReadString();
            }
        }

        public struct Rpc_UseAerocraft : IRpc {
            public const byte FuncID = 36;
            public const string ID = "Proto_Character.Rpc_UseAerocraft";
            public string GetID() => ID;
            public string aerocraftSign;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(aerocraftSign);
            }

            public void UnSerialize(ByteBuffer buffer) {
                aerocraftSign = buffer.ReadString();
            }
        }

        public struct TargetRpc_DiscardMonster : ITargetRpc {
            public const byte FuncID = 37;
            public const string ID = "Proto_Character.TargetRpc_DiscardMonster";
            public string GetID() => ID;
            public uint monsterPID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(monsterPID);
            }

            public void UnSerialize(ByteBuffer buffer) {
                monsterPID = buffer.ReadUInt();
            }
        }
        public struct Cmd_Slide : ICmd {
            public const byte FuncID = 39;
            public const string ID = "Proto_Character.Cmd_Slide";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public bool isSlide;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isSlide);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isSlide = buffer.ReadBoolean();
            }
        }

        public struct Rpc_Slide : IRpc {
            public const byte FuncID = 40;
            public const string ID = "Proto_Character.Rpc_Slide";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public bool isSlide;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isSlide);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isSlide = buffer.ReadBoolean();
            }
        }
        
        public struct TargetRpc_Slide : ITargetRpc {
            public const byte FuncID = 41;
            public const string ID = "Proto_Drive.TargetRpc_Slide";
            public string GetID() => ID;
            public bool isSlide;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(isSlide);
            }

            public void UnSerialize(ByteBuffer buffer) {
                isSlide = buffer.ReadBoolean();
            }
        }

        
        public struct Cmd_ToClientPacketLossRate : ICmd {
            public const byte FuncID = 42;
            public const string ID = "Proto_Character.Cmd_ToClientPacketLossRate";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public ushort index;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(index);
            }

            public void UnSerialize(ByteBuffer buffer) {
                index = buffer.ReadUShort();
            }
        }
        
        public struct TargetRpc_ToClientPacketLossRate : ITargetRpc {
            public const byte FuncID = 43;
            public const string ID = "Proto_Character.TargetRpc_ToClientPacketLossRate";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Unreliable;
            public ushort index;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(index);
            }

            public void UnSerialize(ByteBuffer buffer) {
                index = buffer.ReadUShort();
            }
        }
        
        public struct Cmd_ToServerPacketLossRate : ICmd {
            public const byte FuncID = 44;
            public const string ID = "Proto_Character.Cmd_ToServerPacketLossRate";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Unreliable;
            public ushort index;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(index);
            }

            public void UnSerialize(ByteBuffer buffer) {
                index = buffer.ReadUShort();
            }
        }
        
        public struct TargetRpc_ToServerPacketLossRate : ITargetRpc {
            public const byte FuncID = 45;
            public const string ID = "Proto_Character.TargetRpc_ToServerPacketLossRate";
            public string GetID() => ID;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public ushort index;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(index);
            }

            public void UnSerialize(ByteBuffer buffer) {
                index = buffer.ReadUShort();
            }
        }

        public struct Cmd_Throw : ICmd {
            public const byte FuncID = 46;
            public const string ID = "Proto_Character.Cmd_Throw";
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
        public static ICmd ReadCmdBuffer(byte id) {
            ICmd doc = null;
            switch (id) {
                case Cmd_JumpType.FuncID:
                    doc = new Cmd_JumpType();
                    break;
                case Cmd_FlyType.FuncID:
                    doc = new Cmd_FlyType();
                    break;
                case Cmd_StandType.FuncID:
                    doc = new Cmd_StandType();
                    break;
                case Cmd_MoveDir.FuncID:
                    doc = new Cmd_MoveDir();
                    break;
                case Cmd_Dodga.FuncID:
                    doc = new Cmd_Dodga();
                    break;
                case Cmd_CameraVRota.FuncID:
                    doc = new Cmd_CameraVRota();
                    break;
                case Cmd_FallToGrounded.FuncID:
                    doc = new Cmd_FallToGrounded();
                    break;
                case Cmd_StayPlatformMovement.FuncID:
                    doc = new Cmd_StayPlatformMovement();
                    break;
                case Cmd_PlatformMovement.FuncID:
                    doc = new Cmd_PlatformMovement();
                    break;
                case Cmd_Ping.FuncID:
                    doc = new Cmd_Ping();
                    break;
                case Cmd_TakeBackAerocraft.FuncID:
                    doc = new Cmd_TakeBackAerocraft();
                    break;
                case Cmd_Slide.FuncID:
                    doc = new Cmd_Slide();
                    break;
                case Cmd_ToClientPacketLossRate.FuncID:
                    doc = new Cmd_ToClientPacketLossRate();
                    break;
                case Cmd_ToServerPacketLossRate.FuncID:
                    doc = new Cmd_ToServerPacketLossRate();
                    break;
                case Cmd_Throw.FuncID:
                    doc = new Cmd_Throw();
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
                case Rpc_MoveDir.FuncID:
                    doc = new Rpc_MoveDir();
                    break;
                case Rpc_CameraVRota.FuncID:
                    doc = new Rpc_CameraVRota();
                    break;
                case Rpc_FallToGrounded.FuncID:
                    doc = new Rpc_FallToGrounded();
                    break;
                case TargetRpc_UseAerocraft.FuncID:
                    doc = new TargetRpc_UseAerocraft();
                    break;
                case TargetRpc_Knockback.FuncID:
                    doc = new TargetRpc_Knockback();
                    break;
                case TargetRpc_StrikeFly.FuncID:
                    doc = new TargetRpc_StrikeFly();
                    break;
                case TargetRpc_MovePoint.FuncID:
                    doc = new TargetRpc_MovePoint();
                    break;
                case TargetRpc_Ping.FuncID:
                    doc = new TargetRpc_Ping();
                    break;
                case TargetRpc_UsePackAerocraft.FuncID:
                    doc = new TargetRpc_UsePackAerocraft();
                    break;
                case Rpc_UseAerocraft.FuncID:
                    doc = new Rpc_UseAerocraft();
                    break;
                case TargetRpc_DiscardMonster.FuncID:
                    doc = new TargetRpc_DiscardMonster();
                    break;
                case Rpc_Slide.FuncID:
                    doc = new Rpc_Slide();
                    break;
                case TargetRpc_Slide.FuncID:
                    doc = new TargetRpc_Slide();
                    break;
                case TargetRpc_ToClientPacketLossRate.FuncID:
                    doc = new TargetRpc_ToClientPacketLossRate();
                    break;
                case TargetRpc_ToServerPacketLossRate.FuncID:
                    doc = new TargetRpc_ToServerPacketLossRate();
                    break;
                default:
                    Debug.LogError("Proto_Character:ReadRpcBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
    }
}