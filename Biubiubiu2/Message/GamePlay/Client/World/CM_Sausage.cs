using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_Sausage {
        public struct PlayWWise : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<PlayWWise>();

            public int GetID() => _id;

            // 下面写你的参数
            public string WwiseEventName;
            public AudioData.AudioTypeEnum AudioType;
            public bool IsFollow;
            public GameObject WWiseObject;
            public Vector3 PlayPoint;
            public Action<uint> PlayCallBack; // < playingId >
        }

        public struct StopWWise : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<StopWWise>();

            public int GetID() => _id;

            // 下面写你的参数
            public uint PlayingId;
            public string TriggerKey;
        }

        public struct WWiseSetRTPC : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<WWiseSetRTPC>();

            public int GetID() => _id;

            // 下面写你的参数
            public string WwiseEventName;
            public int RTPCValue;
        }

        public struct Event_AddPlayerGPOMapping : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_AddPlayerGPOMapping>();
            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public IGPO gpo;
        }

        public struct Event_RemovePlayerGPOMapping : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_RemovePlayerGPOMapping>();
            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public IGPO gpo;
        }

        public struct Event_GPOHurtOutOfFightRange : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_GPOHurtOutOfFightRange>();
            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
        }
        
        public struct Event_SetGpoMatType : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_SetGpoMatType>();
            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
            public byte MatType;
            public string TeamId;
            public bool IsForward;
            public GameObject GpoObject;
        }

        public struct Event_RemoveGpoMatType : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<Event_RemoveGpoMatType>();
            public int GetID() => _id;
            // 下面写你的参数
            public int GpoId;
        }

        public struct GetSausageAI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetSausageAI>();
            public int GetID() => _id;
            // 下面写你的参数
            public float Range;
            public Vector3 NowPoint;
            public Action<List<Vector3>, List<int>> CallBack;
        }

        public struct BossFightFailed : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<BossFightFailed>();
            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct LocalPlayerChangeBossFightBGM : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<LocalPlayerChangeBossFightBGM>();
            public int GetID() => _id;
            // 下面写你的参数
            public bool isInBossFight;
        }

        public struct GetCurTimestampMilliseconds : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetCurTimestampMilliseconds>();

            public int GetID() => _id;

            // 下面写你的参数
            public Action<long> CallBack;
        }
        
        public struct TryToStartBossFightBGM : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<TryToStartBossFightBGM>();

            public int GetID() => _id;

            // 下面写你的参数
            public IGPO BossGPO;
            public string BossFightBGMKey;
            public Action<uint> CallBack; // < playingId >
        }

        public struct StopBossFightBGM : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<StopBossFightBGM>();

            public int GetID() => _id;

            // 下面写你的参数
            public IGPO BossGPO;
            public uint FightBGMPlayingId;
        }

        public struct PlayGiantDaDaDeadAudio : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<PlayGiantDaDaDeadAudio>();

            public int GetID() => _id;

            // 下面写你的参数
            public string AudioKey;
        }

        public struct PlayBubble : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<PlayBubble>();
            public int GetID() => _id;
            
            // 下面写你的参数
            public string EffectSign;
            public Vector3 EffectPos;
            public float LifeTime;
        }
    }
}