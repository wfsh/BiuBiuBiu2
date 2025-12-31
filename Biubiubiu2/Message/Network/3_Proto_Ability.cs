using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_Ability {
        public const byte ModID = 3;

        public struct TargetRpc_RemoveAbility : ITargetRpc {
            public const string ID = "Proto_Ability.TargetRpc_RemoveAbility";
            public string GetID() => ID;
            public const byte FuncID = 1;
            public int abilityId;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityId = buffer.ReadInt();
            }

            public int GetChannel() => NetworkData.Channels.Reliable;
        }

        public struct TargetRpc_PlayAbility : ITargetRpc {
            public const string ID = "Proto_Ability.TargetRpc_PlayAbility";
            public string GetID() => ID;
            public const byte FuncID = 2;
            public ushort fireGpoId;
            public int abilityId;
            public byte[] protoDoc;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityId);
                buffer.Write(fireGpoId);
                buffer.Write((ushort)protoDoc.Length);
                for (int i = 0; i < protoDoc.Length; i++) {
                    buffer.Write(protoDoc[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityId = buffer.ReadInt();
                fireGpoId = buffer.ReadUShort();
                var len = buffer.ReadUShort();
                protoDoc = new byte[len];
                for (int i = 0; i < len; i++) {
                    protoDoc[i] = buffer.ReadByte();
                }
            }

            public int GetChannel() => NetworkData.Channels.Reliable;
        }

        /// <summary>
        /// 子弹移动， - 附带开火播放
        /// </summary>
        public struct Rpc_BulletFire : IAbilityABCreateRpc {
            public const string ID = "Proto_Ability.Rpc_BulletFire";
            public string GetID() => ID;
            public ushort GetConfigID() => configId;
            public byte GetRowID() => rowId;
            public const byte FuncID = 3;
            public ushort configId;
            public byte rowId;
            public Vector3 targetPoint;
            public ushort speed;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(targetPoint);
                buffer.Write(speed);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                targetPoint = buffer.ReadVector3();
                speed = buffer.ReadUShort();
            }
        }

        public struct Rpc_BulletFireDecal : IAbilityABCreateRpc {
            public const string ID = "Proto_Ability.Rpc_BulletFireDecal";
            public string GetID() => ID;
            public ushort GetConfigID() => configId;
            public byte GetRowID() => rowId;
            public const byte FuncID = 4;
            public ushort configId;
            public byte rowId;
            public Vector3 targetPoint;
            public Vector3 targetNormal;
            public ushort speed;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(targetPoint);
                buffer.Write(targetNormal);
                buffer.Write(speed);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                targetPoint = buffer.ReadVector3();
                targetNormal = buffer.ReadVector3();
                speed = buffer.ReadUShort();
            }
        }

        public struct Rpc_ThreadGrenade : IAbilityABCreateRpc {
            public const string ID = "Proto_Ability.Rpc_ThreadGrenade";
            public string GetID() => ID;
            public byte GetRowID() => rowId;
            public ushort GetConfigID() => configId;
            public const byte FuncID = 5;
            public ushort configId;
            public byte rowId;
            public Vector3[] points;

            public int GetChannel() => NetworkData.Channels.Reliable;
            
            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                var len = points.Length;
                buffer.Write(len);
                for (int i = 0; i < len; i++) {
                    buffer.Write(points[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                var len = buffer.ReadInt();
                points = new Vector3[len];
                for (int i = 0; i < len; i++) {
                    points[i] = buffer.ReadVector3();
                }
            }
        }

        public struct TargetRpc_ScreenElement : ITargetRpc {
            public const string ID = "Proto_Ability.TargetRpc_ScreenElement";
            public string GetID() => ID;
            public const byte FuncID = 6;
            public int abilityId;
            public string playAbility;
            public int elementId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityId);
                buffer.Write(elementId);
                buffer.Write(playAbility);
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityId = buffer.ReadInt();
                elementId = buffer.ReadInt();
                playAbility = buffer.ReadString();
            }
        }

        public struct Rpc_ThreadMonsterBall : IRpc {
            public const string ID = "Proto_Ability.Rpc_ThreadMonsterBall";
            public string GetID() => ID;
            public const byte FuncID = 8;
            public string EffectSign;
            public float LifeTime;
            public float Speed;
            public uint MonsterPID;
            public Vector3[] points;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(EffectSign);
                buffer.Write(LifeTime);
                buffer.Write(Speed);
                buffer.Write(MonsterPID);
                var len = points.Length;
                buffer.Write(len);
                for (int i = 0; i < len; i++) {
                    buffer.Write(points[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                EffectSign = buffer.ReadString();
                LifeTime = buffer.ReadFloat();
                Speed = buffer.ReadFloat();
                MonsterPID = buffer.ReadUInt();
                var len = buffer.ReadInt();
                points = new Vector3[len];
                for (int i = 0; i < len; i++) {
                    points[i] = buffer.ReadVector3();
                }
            }
        }

        /// <summary>
        /// 子弹移动， - 附带开火播放
        /// </summary>
        public struct Rpc_PenetratorGrenade : IRpc {
            public const string ID = "Proto_Ability.Rpc_PenetratorGrenade";
            public string GetID() => ID;
            public const byte FuncID = 9;
            public string effectSign;
            public Vector3 startPoint;
            public Quaternion startRota;
            public float lifeTime;
            public float speed;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(startPoint);
                buffer.Write(startRota);
                buffer.Write(effectSign);
                buffer.Write(lifeTime);
                buffer.Write(speed);
            }

            public void UnSerialize(ByteBuffer buffer) {
                startPoint = buffer.ReadVector3();
                startRota = buffer.ReadQuaternion();
                effectSign = buffer.ReadString();
                lifeTime = buffer.ReadFloat();
                speed = buffer.ReadFloat();
            }
        }

        public struct Rpc_PenetratorGrenadeHit : IRpc {
            public const string ID = "Proto_Ability.Rpc_PenetratorGrenadeHit";
            public string GetID() => ID;
            public const byte FuncID = 10;
            public int abilityId;
            public Vector3 hitPoint;
            public Quaternion hitRota;
            public int hitGpoID;
            public int hitGpoType;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityId);
                buffer.Write(hitPoint);
                buffer.Write(hitRota);
                buffer.Write(hitGpoID);
                buffer.Write(hitGpoType);
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityId = buffer.ReadInt();
                hitPoint = buffer.ReadVector3();
                hitRota = buffer.ReadQuaternion();
                hitGpoID = buffer.ReadInt();
                hitGpoType = buffer.ReadInt();
            }
        }

      

        public struct Rpc_AbilityEffect : IRpc {
            public const string ID = "Proto_Ability.Rpc_AbilityEffect";
            public string GetID() => ID;
            public const byte FuncID = 12;
            public int abilityEffect;
            public float value;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityEffect);
                buffer.Write(value);
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityEffect = buffer.ReadInt();
                value = buffer.ReadFloat();
            }
        }

        public struct TargetRpc_AbilityEffect : ITargetRpc {
            public const string ID = "Proto_Ability.TargetRpc_AbilityEffect";
            public string GetID() => ID;
            public const byte FuncID = 13;
            public int abilityEffect;
            public float value;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityEffect);
                buffer.Write(value);
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityEffect = buffer.ReadInt();
                value = buffer.ReadFloat();
            }
        }

        public struct TargetRpc_PlayTestFire : ITargetRpc {
            public const string ID = "Proto_Ability.TargetRpc_PlayTestFire";
            public string GetID() => ID;
            public const byte FuncID = 14;
            public Vector3 startPoint;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(startPoint);
            }

            public void UnSerialize(ByteBuffer buffer) {
                startPoint = buffer.ReadVector3();
            }
        }

        /// <summary>
        /// 播放特效
        /// </summary>
        public struct TargetRpc_PlayGPOEffect : IAbilityAECreateRpc {
            public const string ID = "Proto_Ability.TargetRpc_PlayGPOEffect";
            public const byte FuncID = 15;
            public string GetID() => ID;
            public byte GetRowID() => rowId;
            public ushort GetConfigID() => configId;
            public ushort configId;
            public byte rowId;
            public int gpoId;

            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(gpoId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                gpoId = buffer.ReadInt();
            }
        }

        /// <summary>
        /// 播放特效
        /// </summary>
        public struct Rpc_PlayDynamicScalingEffect : IAbilityABCreateRpc {
            public const string ID = "Proto_Ability.Rpc_PlayDynamicScalingEffect";
            public const byte FuncID = 16;
            public string GetID() => ID;
            public ushort GetConfigID() => configId;
            public byte GetRowID() => rowId;
            public ushort configId;
            public byte rowId;
            public Vector3 startPoint;
            public Quaternion startRota;
            public ushort lifeTime; // unit: 0.1s
            public Vector3 startScale; // unit: 0.1
            public Vector3 scaleChangeSpeed; // unit: 0.1
            public long playTimestamp; // unit: 1
            public ushort audioKey;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(startPoint);
                buffer.Write(startRota);
                buffer.Write(lifeTime);
                buffer.Write(startScale);
                buffer.Write(scaleChangeSpeed);
                buffer.Write(playTimestamp);
                buffer.Write(audioKey);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                startPoint = buffer.ReadVector3();
                startRota = buffer.ReadQuaternion();
                lifeTime = buffer.ReadUShort();
                startScale = buffer.ReadVector3();
                scaleChangeSpeed = buffer.ReadVector3();
                playTimestamp = buffer.ReadLong();
                audioKey = buffer.ReadUShort();
            }
        }


        /// <summary>
        /// 播放特效
        /// </summary>
        public struct Rpc_PlayRotatingEffect : IRpc {
            public const string ID = "Proto_Ability.Rpc_PlayRotatingEffect";
            public string GetID() => ID;
            public const byte FuncID = 18;
            public ushort abilityModId;
            public ushort startDeg; // unit: 0.1
            public ushort startRadius; // unit: 0.1
            public Vector3 rotateAroundPoint;
            public ushort lifeTime; // unit: 0.1s
            public short moveAngularSpeed; // unit: 0.1deg/s
            public bool endWhenFireGPODead;
            public long playTimestamp; // unit: 1
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityModId);
                buffer.Write(startDeg);
                buffer.Write(startRadius);
                buffer.Write(rotateAroundPoint);
                buffer.Write(lifeTime);
                buffer.Write(moveAngularSpeed);
                buffer.Write(endWhenFireGPODead);
                buffer.Write(playTimestamp);
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityModId = buffer.ReadUShort();
                startDeg = buffer.ReadUShort();
                startRadius = buffer.ReadUShort();
                rotateAroundPoint = buffer.ReadVector3();
                lifeTime = buffer.ReadUShort();
                moveAngularSpeed = buffer.ReadShort();
                endWhenFireGPODead = buffer.ReadBoolean();
                playTimestamp = buffer.ReadLong();
            }
        }

        /// <summary>
        /// 巨像达达播放键盘特效
        /// </summary>
        public struct Rpc_GiantDaDaPlayDropBugKeyboardEffect : IRpc {
            public const string ID = "Proto_Ability.Rpc_GiantDaDaPlayDropBugKeyboardEffect";
            public string GetID() => ID;
            public const byte FuncID = 20;
            public Vector3 startPoint;
            public Quaternion startRota;
            public ushort castTime; // 起手时间 unit: 0.1s
            public ushort loopTime; // 循环时间 unit: 0.1s
            public ushort postTime; // 后摇时间 unit: 0.1s
            public bool endWhenFireGPODead;
            public long playTimestamp;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(startPoint);
                buffer.Write(startRota);
                buffer.Write(castTime);
                buffer.Write(loopTime);
                buffer.Write(postTime);
                buffer.Write(endWhenFireGPODead);
                buffer.Write(playTimestamp);
            }

            public void UnSerialize(ByteBuffer buffer) {
                startPoint = buffer.ReadVector3();
                startRota = buffer.ReadQuaternion();
                castTime = buffer.ReadUShort();
                loopTime = buffer.ReadUShort();
                postTime = buffer.ReadUShort();
                endWhenFireGPODead = buffer.ReadBoolean();
                playTimestamp = buffer.ReadLong();
            }
        }

        /// <summary>
        /// 播放特效
        /// </summary>
        public struct Rpc_PlayRotatingRayEffect : IRpc {
            public const string ID = "Proto_Ability.Rpc_PlayRotatingRayEffect";
            public string GetID() => ID;
            public const byte FuncID = 22;
            public ushort abilityModId;
            public Vector3 startPoint;
            public ushort startLength; // unit: 0.1
            public ushort startDeg; // unit: 0.1deg
            public ushort lifeTime; // unit: 0.1s
            public short moveAngularSpeed; // unit: 0.1deg/s
            public bool endWhenFireGPODead;
            public long playTimestamp; // unit: 1
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityModId);
                buffer.Write(startPoint);
                buffer.Write(startLength);
                buffer.Write(startDeg);
                buffer.Write(lifeTime);
                buffer.Write(moveAngularSpeed);
                buffer.Write(endWhenFireGPODead);
                buffer.Write(playTimestamp);
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityModId = buffer.ReadUShort();
                startPoint = buffer.ReadVector3();
                startLength = buffer.ReadUShort();
                startDeg = buffer.ReadUShort();
                lifeTime = buffer.ReadUShort();
                moveAngularSpeed = buffer.ReadShort();
                endWhenFireGPODead = buffer.ReadBoolean();
                playTimestamp = buffer.ReadLong();
            }
        }

        /// <summary>
        /// 播放特效
        /// </summary>
        public struct Rpc_PlayDynamicScalingRippleEffect : IRpc {
            public const string ID = "Proto_Ability.Rpc_PlayDynamicScalingRingEffect";
            public string GetID() => ID;
            public const byte FuncID = 23;
            public ushort abilityModId;
            public Vector3 startPoint;
            public Quaternion startRota;
            public ushort lifeTime; // unit: 0.1s
            public float scaleChangeSpeed; // unit: 1
            public long playTimestamp; // unit: 1
            public ushort audioKey;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(startPoint);
                buffer.Write(startRota);
                buffer.Write(abilityModId);
                buffer.Write(lifeTime);
                buffer.Write(scaleChangeSpeed);
                buffer.Write(playTimestamp);
                buffer.Write(audioKey);
            }

            public void UnSerialize(ByteBuffer buffer) {
                startPoint = buffer.ReadVector3();
                startRota = buffer.ReadQuaternion();
                abilityModId = buffer.ReadUShort();
                lifeTime = buffer.ReadUShort();
                scaleChangeSpeed = buffer.ReadFloat();
                playTimestamp = buffer.ReadLong();
                audioKey = buffer.ReadUShort();
            }
        }

        /// <summary>
        /// 创建战斗区域
        /// </summary>
        public struct TargetRpc_GoldDashFightBossRange : IAbilityABCreateRpc {
            public const string ID = "Proto_Ability.TargetRpc_GoldDashFightBossRange";
            public string GetID() => ID;
            public ushort GetConfigID() => configId;
            public byte GetRowID() => rowId;
            public const byte FuncID = 24;
            public ushort configId;
            public byte rowId;
            public Vector3 startPoint;
            public Vector3 startScale; // unit: 1
            public float removeTimeAfterDead;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(startPoint);
                buffer.Write(startScale);
                buffer.Write(removeTimeAfterDead);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                startPoint = buffer.ReadVector3();
                startScale = buffer.ReadVector3();
                removeTimeAfterDead = buffer.ReadFloat();
            }
        }

        public struct Rpc_BulletFireWithStartPoint : IRpc {
            public const string ID = "Proto_Ability.Rpc_BulletFireWithStartPoint";
            public string GetID() => ID;
            public const byte FuncID = 26;
            public Vector3 startPoint;
            public Vector3 targetPoint;
            public ushort abilityModId;
            public ushort speed;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(startPoint);
                buffer.Write(targetPoint);
                buffer.Write(abilityModId);
                buffer.Write(speed);
            }

            public void UnSerialize(ByteBuffer buffer) {
                startPoint = buffer.ReadVector3();
                targetPoint = buffer.ReadVector3();
                abilityModId = buffer.ReadUShort();
                speed = buffer.ReadUShort();
            }
        }

        public struct Rpc_BulletFireDecalWithStartPoint : IRpc {
            public const string ID = "Proto_Ability.Rpc_BulletFireDecalWithStartPoint";
            public string GetID() => ID;
            public const byte FuncID = 27;
            public Vector3 startPoint;
            public Vector3 targetPoint;
            public Vector3 targetNormal;
            public ushort abilityModId;
            public ushort speed;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(startPoint);
                buffer.Write(targetPoint);
                buffer.Write(targetNormal);
                buffer.Write(abilityModId);
                buffer.Write(speed);
            }

            public void UnSerialize(ByteBuffer buffer) {
                startPoint = buffer.ReadVector3();
                targetPoint = buffer.ReadVector3();
                targetNormal = buffer.ReadVector3();
                abilityModId = buffer.ReadUShort();
                speed = buffer.ReadUShort();
            }
        }

        public struct Rpc_SplitCone : IRpc {
            public const string ID = "Proto_Ability.Rpc_SplitCone";
            public string GetID() => ID;
            public const byte FuncID = 28;
            public Vector3 startPoint;
            public Quaternion startRota;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(startPoint);
                buffer.Write(startRota);
            }

            public void UnSerialize(ByteBuffer buffer) {
                startPoint = buffer.ReadVector3();
                startRota = buffer.ReadQuaternion();
            }
        }

        /// <summary>
        /// 子弹移动， - 附带开火播放
        /// </summary>
        public struct Rpc_EffectFollowServerTransform : IRpc {
            public const string ID = "Proto_Ability.Rpc_TrackingMissle";
            public string GetID() => ID;
            public const byte FuncID = 29;
            public ushort abilityModId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityModId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityModId = buffer.ReadUShort();
            }
        }

        /// <summary>
        /// 闪电特效
        /// </summary>
        public struct Rpc_RangeFlash : IRpc {
            public const string ID = "Proto_Ability.Rpc_RangeFlash";
            public string GetID() => ID;
            public const byte FuncID = 30;
            public ushort abilityModId;
            public ushort lifeTime;
            public int[] GPOId;
            public int GetChannel() => NetworkData.Channels.Unreliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityModId);
                buffer.Write(lifeTime);
                var len = GPOId.Length;
                buffer.Write(len);
                for (int i = 0; i < len; i++) {
                    buffer.Write(GPOId[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityModId = buffer.ReadUShort();
                lifeTime = buffer.ReadUShort();
                var len = buffer.ReadInt();
                GPOId = new int[len];
                for (int i = 0; i < len; i++) {
                    GPOId[i] = buffer.ReadInt();
                }
            }
        }
        
        public struct Rpc_PlayFillScaleEffect : IRpc {
            public const string ID = "Proto_Ability.Rpc_PlayFillScaleEffect";
            public string GetID() => ID;
            public const byte FuncID = 35;
            public ushort abilityModId;
            public Vector3 startPoint;
            public Vector3 startLookAt;
            public Vector3 endScale; // unit: 0.1
            public ushort fillTime; // unit: 0.1s
            public bool isCircleFill;
            public ushort lifeTime; // unit: 0.1s
            public long playTimestamp; // unit: 1
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityModId);
                buffer.Write(startPoint);
                buffer.Write(startLookAt);
                buffer.Write(endScale);
                buffer.Write(fillTime);
                buffer.Write(isCircleFill);
                buffer.Write(lifeTime);
                buffer.Write(playTimestamp);
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityModId = buffer.ReadUShort();
                startPoint = buffer.ReadVector3();
                startLookAt = buffer.ReadVector3();
                endScale = buffer.ReadVector3();
                fillTime = buffer.ReadUShort();
                isCircleFill = buffer.ReadBoolean();
                lifeTime = buffer.ReadUShort();
                playTimestamp = buffer.ReadLong();
            }
        }
         
        /// <summary>
        /// 播放预警特效
        /// </summary>
        public struct Rpc_PlayWarningEffect : IRpc {
            public const string ID = "Proto_Ability.Rpc_PlayWarningEffect";
            public string GetID() => ID;
            public const byte FuncID = 36;
            public ushort abilityModId;
            public Vector3 startPoint;
            public Vector3 startLookAt;
            public Vector3 startScale; // unit: 0.1
            public ushort fillTime; // unit: 0.1s
            public bool isCircleFill;
            public ushort lifeTime; // unit: 0.1s
            public long playTimestamp; // unit: 1
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityModId);
                buffer.Write(startPoint);
                buffer.Write(startLookAt);
                buffer.Write(startScale);
                buffer.Write(fillTime);
                buffer.Write(isCircleFill);
                buffer.Write(lifeTime);
                buffer.Write(playTimestamp);
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityModId = buffer.ReadUShort();
                startPoint = buffer.ReadVector3();
                startLookAt = buffer.ReadVector3();
                startScale = buffer.ReadVector3();
                fillTime = buffer.ReadUShort();
                isCircleFill = buffer.ReadBoolean();
                lifeTime = buffer.ReadUShort();
                playTimestamp = buffer.ReadLong();
            }
        }

        /// <summary>
        /// 变更喷火枪的火焰大小
        /// </summary>
        public struct Rpc_ChangeFireGunFireRange : IRpc {
            public const string ID = "Proto_Ability.Rpc_ChangeFireGunFireRange";
            public string GetID() => ID;
            public const byte FuncID = 31;
            public float range;
            public float initRange;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(range);
                buffer.Write(initRange);
            }

            public void UnSerialize(ByteBuffer buffer) {
                range = buffer.ReadFloat();
                initRange = buffer.ReadFloat();
            }
        }

        public struct TargetRpc_Missile : ITargetRpc {
            public const string ID = "Proto_Ability.TargetRpc_Missile";
            public string GetID() => ID;
            public const byte FuncID = 32;
            public Vector3[] points;
            public bool isMoveEnd;
            public float areaRadius;
            public ushort abilityModId;
            public int skinItemId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(points.Length);
                for (int i = 0; i < points.Length; i++) {
                    buffer.Write(points[i]);
                }
                buffer.Write(isMoveEnd);
                buffer.Write(areaRadius);
                buffer.Write(abilityModId);
                buffer.Write(skinItemId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                points = new Vector3[buffer.ReadInt()];
                for (int i = 0; i < points.Length; i++) {
                    points[i] = buffer.ReadVector3();
                }
                isMoveEnd = buffer.ReadBoolean();
                areaRadius = buffer.ReadFloat();
                abilityModId = buffer.ReadUShort();
                skinItemId = buffer.ReadInt();
            }
        }

        public struct Rpc_MissileBomb : IRpc {
            public const string ID = "Proto_Ability.Rpc_MissileBomb";
            public string GetID() => ID;
            public const byte FuncID = 33;
            public ushort abilityModId;
            public Vector3[] points;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityModId);
                var len = points.Length;
                buffer.Write(len);
                for (int i = 0; i < len; i++) {
                    buffer.Write(points[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityModId = buffer.ReadUShort();
                var len = buffer.ReadInt();
                points = new Vector3[len];
                for (int i = 0; i < len; i++) {
                    points[i] = buffer.ReadVector3();
                }
            }
        }
        
        
        /// <summary>
        /// 播放喷火枪的火焰
        /// </summary>
        public struct Rpc_PlayFireGunFire : IRpc {
            public const string ID = "Proto_Ability.Rpc_PlayFireGunFire";
            public string GetID() => ID;
            public const byte FuncID = 34;
            
            public ushort abilityModId;
            
            public float range;
            public float initRange;

            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(abilityModId);
                buffer.Write(range);
                buffer.Write(initRange);
            }

            public void UnSerialize(ByteBuffer buffer) {
                abilityModId = buffer.ReadUShort();
                range = buffer.ReadFloat();
                initRange = buffer.ReadFloat();
            }
        }
        
        public struct Rpc_SyncTransformEffect : IAbilityABCreateRpc {
            public const string ID = "Proto_Ability.Rpc_SyncTransformEffect";
            public string GetID() => ID;
            public ushort GetConfigID() => configId;
            public byte GetRowID() => rowId;
            public const byte FuncID = 37;
            public ushort configId;
            public byte rowId;
            public Vector3 startPoint;
            public Quaternion startRota;
            public Vector3 startScale;
            public ushort lifeTime;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(startPoint);
                buffer.Write(startRota);
                buffer.Write(startScale);
                buffer.Write(lifeTime);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                startPoint = buffer.ReadVector3();
                startRota = buffer.ReadQuaternion();
                startScale = buffer.ReadVector3();
                lifeTime = buffer.ReadUShort();
            }
        }
        
        public struct Rpc_GoldJokerDollBombState : IAbilityABCreateRpc {
            public const string ID = "Proto_Ability.Rpc_GoldJokerDollBombState";
            public string GetID() => ID;
            public ushort GetConfigID() => configId;
            public byte GetRowID() => rowId;
            public const byte FuncID = 48;
            public ushort configId;
            public byte rowId;
            public byte state;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(configId);
                buffer.Write(rowId);
                buffer.Write(state);
            }

            public void UnSerialize(ByteBuffer buffer) {
                configId = buffer.ReadUShort();
                rowId = buffer.ReadByte();
                state = buffer.ReadByte();
            }
        }

        public struct TargetRpc_InWorldAbilityList : ITargetRpc {
            public const string ID = "Proto_Ability.TargetRpc_InWorldAbilityList";
            public string GetID() => ID;
            public const byte FuncID = 49;
            public int GetChannel() => NetworkData.Channels.Reliable;
            public List<int> abList;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                var len = abList.Count;
                buffer.Write(len);
                for (int i = 0; i < len; i++) {
                    buffer.Write(abList[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                var len = buffer.ReadInt();
                abList = new List<int>();
                for (int i = 0; i < len; i++) {
                    abList.Add(buffer.ReadInt());
                }
            }
        }
        
        public static IProto_Doc ReadRpcBuffer(byte funcId) {
            IProto_Doc doc = null;
            switch (funcId) {
                case TargetRpc_RemoveAbility.FuncID:
                    doc = new TargetRpc_RemoveAbility();
                    break;
                case TargetRpc_PlayAbility.FuncID:
                    doc = new TargetRpc_PlayAbility();
                    break;
                case Rpc_BulletFire.FuncID:
                    doc = new Rpc_BulletFire();
                    break;
                case Rpc_BulletFireDecal.FuncID:
                    doc = new Rpc_BulletFireDecal();
                    break;
                case Rpc_ThreadGrenade.FuncID:
                    doc = new Rpc_ThreadGrenade();
                    break;
                case TargetRpc_ScreenElement.FuncID:
                    doc = new TargetRpc_ScreenElement();
                    break;
                case Rpc_ThreadMonsterBall.FuncID:
                    doc = new Rpc_ThreadMonsterBall();
                    break;
                case Rpc_PenetratorGrenade.FuncID:
                    doc = new Rpc_PenetratorGrenade();
                    break;
                case Rpc_PenetratorGrenadeHit.FuncID:
                    doc = new Rpc_PenetratorGrenadeHit();
                    break;
                case TargetRpc_AbilityEffect.FuncID:
                    doc = new TargetRpc_AbilityEffect();
                    break;
                case Rpc_AbilityEffect.FuncID:
                    doc = new Rpc_AbilityEffect();
                    break;
                case TargetRpc_PlayTestFire.FuncID:
                    doc = new TargetRpc_PlayTestFire();
                    break;
                case TargetRpc_PlayGPOEffect.FuncID:
                    doc = new TargetRpc_PlayGPOEffect();
                    break;
                case Rpc_PlayDynamicScalingEffect.FuncID:
                    doc = new Rpc_PlayDynamicScalingEffect();
                    break;
                case Rpc_PlayRotatingEffect.FuncID:
                    doc = new Rpc_PlayRotatingEffect();
                    break;
                case Rpc_GiantDaDaPlayDropBugKeyboardEffect.FuncID:
                    doc = new Rpc_GiantDaDaPlayDropBugKeyboardEffect();
                    break;
                case Rpc_PlayRotatingRayEffect.FuncID:
                    doc = new Rpc_PlayRotatingRayEffect();
                    break;
                case Rpc_PlayDynamicScalingRippleEffect.FuncID:
                    doc = new Rpc_PlayDynamicScalingRippleEffect();
                    break;
                case TargetRpc_GoldDashFightBossRange.FuncID:
                    doc = new TargetRpc_GoldDashFightBossRange();
                    break;
                case Rpc_RangeFlash.FuncID:
                    doc = new Rpc_RangeFlash();
                    break;
                case Rpc_ChangeFireGunFireRange.FuncID:
                    doc = new Rpc_ChangeFireGunFireRange();
                    break;
                case TargetRpc_Missile.FuncID:
                    doc = new TargetRpc_Missile();
                    break;
                case Rpc_MissileBomb.FuncID:
                    doc = new Rpc_MissileBomb();
                    break;
                case Rpc_BulletFireWithStartPoint.FuncID:
                    doc = new Rpc_BulletFireWithStartPoint();
                    break;
                case Rpc_BulletFireDecalWithStartPoint.FuncID:
                    doc = new Rpc_BulletFireDecalWithStartPoint();
                    break;
                case Rpc_SplitCone.FuncID:
                    doc = new Rpc_SplitCone();
                    break;
                case Rpc_EffectFollowServerTransform.FuncID:
                    doc = new Rpc_EffectFollowServerTransform();
                    break;
                case Rpc_PlayFireGunFire.FuncID:
                    doc = new Rpc_PlayFireGunFire();
                    break;
                case Rpc_PlayWarningEffect.FuncID:
                    doc = new Rpc_PlayWarningEffect();
                    break;
                case Rpc_PlayFillScaleEffect.FuncID:
                    doc = new Rpc_PlayFillScaleEffect();
                    break;
                case Rpc_SyncTransformEffect.FuncID:
                    doc = new Rpc_SyncTransformEffect();
                    break;
                case Rpc_GoldJokerDollBombState.FuncID:
                    doc = new Rpc_GoldJokerDollBombState();
                    break;
                case TargetRpc_InWorldAbilityList.FuncID:
                    doc = new TargetRpc_InWorldAbilityList();
                    break;
                default:
                    Debug.LogError("Proto_Ability:ReadRpcBuffer 没有注册对应 ID:" + funcId);
                    return null;
            }
            return doc;
        }
        
        public static ICmd ReadCmdBuffer(byte funcId) {
            ICmd doc = null;
            switch (funcId) {
                default:
                    Debug.LogError("Proto_Ability:ReadCmdBuffer 没有注册对应 ID:" + funcId);
                    return null;
            }
            return doc;
        }
    }
}