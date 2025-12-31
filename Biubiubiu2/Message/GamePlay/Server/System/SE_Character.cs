using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_Character {
        // 添加角色宠物
        public struct Event_AddMasterAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AddMasterAI>();
            public int GetID() => _id;

            // 下面写你的参数
            public string MonsterSign;
            public string MonsterSkinSign;
            public int HP;
            public int ATK;
            public float AttackRange;
        }

        // 删除角色宠物
        public struct Event_RemoveMasterAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_RemoveMasterAI>();
            public int GetID() => _id;

            // 下面写你的参数
            public string MonsterSign;
        }

        // 掉落所有跟随的怪物
        public struct Event_DropAllFollowAI : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DropAllFollowAI>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsDrop; // 丢弃的宠物是否掉落在地上
        }
        // 采集状态
        public struct Event_GatheringState : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GatheringState>();
            public int GetID() => _id;

            // 下面写你的参数
            public bool State;
            public int Count;
        }

        public struct GetPlayerId : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetPlayerId>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<long> CallBack;
        }

        public struct GetKnockDownStatus : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetKnockDownStatus>();

            public int GetID() => _id;

            // 下面写你的参数
            public Action<bool> CallBack; // < IsKnockedDown >
        }
        
        public struct GetIsBeRatBuffNoNeedFindByBoss : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetIsBeRatBuffNoNeedFindByBoss>();

            public int GetID() => _id;

            // 下面写你的参数
            public Action<bool> CallBack; // < IsKnockedDown >
        }
        
        public struct SetSausageLockGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetSausageLockGPO>();

            public int GetID() => _id;

            // 下面写你的参数
            public IGPO LockGpo;
        }

        public struct AddSausageRoleMoveForce : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<AddSausageRoleMoveForce>();

            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 CenterPoint;
        }
        
        // 手持物品
        public struct Event_SetHoldOnSign : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetHoldOnSign>();
            public int GetID() => _id;

            // 下面写你的参数
            public string HoldOnSign;
        }
        
        public struct GetSausageRoleIsWeak : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetSausageRoleIsWeak>();

            public int GetID() => _id;

            // 下面写你的参数
            public Action<bool> Callback;
        }

        public struct PlayBubble : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<PlayBubble>();
            public int GetID() => _id;

            public long PlayerId;
            public string EffectSign;
            public Vector3 EffectPos;
            public float LifeTime;
        }

        public struct StandTypeChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<StandTypeChange>();
            public int GetID() => _id;
            
            // 下面写你的参数
            public CharacterData.StandType StandType;
        }

        public struct PlayAudio : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<PlayAudio>();
            public int GetID() => _id;

            public long PlayerId;
            public ushort WwiseId;
        }
    }
}