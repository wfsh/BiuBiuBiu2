using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_AI_GiantDaDa {
        public struct Event_SetCanUseAbility : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetCanUseAbility>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool CanUseAbility;
        }

        public struct Event_SwitchStage : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SwitchStage>();

            public int GetID() => _id;
            // 下面写你的参数
            public int NextStage;
        }

        public struct Event_GetCurAbilityUseStatus : GamePlayEvent.ISystemEvent {
            public enum AbilityUseStatus {
                Undefined,
                Idle,
                Using,
                Recovery,
            }

            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetCurAbilityUseStatus>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<AbilityUseStatus> CallBack;
        }
        public struct Event_GiantDaDaRefillCandidate : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GiantDaDaRefillCandidate>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_GiantDaDaPlayStageAnim : GamePlayEvent.ISystemEvent {
            public enum StageEnum {
                Undefined,
                Born,
                Switch,
                Leave,
            }

            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GiantDaDaPlayStageAnim>();

            public int GetID() => _id;
            // 下面写你的参数
            public StageEnum Stage;
        }

        public struct Event_GiantDaDaPlayIdleAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GiantDaDaPlayIdleAnim>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_GiantDaDaPlayAirBillowAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GiantDaDaPlayAirBillowAnim>();

            public int GetID() => _id;
            // 下面写你的参数
            public int Stage;
        }

        public struct Event_GiantDaDaPlayRollingStoneAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GiantDaDaPlayRollingStoneAnim>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_GiantDaDaPlayElectricArcAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GiantDaDaPlayElectricArcAnim>();

            public int GetID() => _id;
            // 下面写你的参数
            public int Stage;
        }

        public struct Event_GiantDaDaPlayDropBugAnim : GamePlayEvent.ISystemEvent {
            public enum AnimStage {
                Undefined,
                Start,
                Loop,
                End
            }

            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GiantDaDaPlayDropBugAnim>();

            public int GetID() => _id;
            // 下面写你的参数
            public AnimStage Stage;
        }

        public struct Event_GiantDaDaPlaySurgeIncomingAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GiantDaDaPlaySurgeIncomingAnim>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Event_GiantDaDaPlayLightningWithBugAnim : GamePlayEvent.ISystemEvent {
            public enum AnimStage {
                Undefined,
                LightningCast,
                BugCast,
                BugLoop,
                BugPost,
                LightningPost
            }

            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GiantDaDaPlayLightningWithBugAnim>();

            public int GetID() => _id;
            // 下面写你的参数
            public AnimStage Stage;
        }

        public struct Event_GiantDaDaOnlyUseSelectedAbility : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GiantDaDaOnlyUseSelectedAbility>();

            public int GetID() => _id;
            // 下面写你的参数
            public int abId;
        }

        public struct Event_AbilityRecoveryTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AbilityRecoveryTime>();

            public int GetID() => _id;
            // 下面写你的参数
            public float AbilityRecoveryTime;
        }
    }
}