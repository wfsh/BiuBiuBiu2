using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_Guide {
        public struct GuideInfo {
            public RectTransform ClickBtn; // 点击按钮和父物体
            public int SortId;
            public RectTransform DragStartTransform;
            public RectTransform DragEndTransform;
            public bool DisableOverrideSort;
            public Vector2 SizeData;
            public bool isCircleGuide;
        }

        public enum GuideShowType {
            Click = 1,
            Drag,
            NoneGuide
        }
        
        public struct ExcuteClickEvent : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<ExcuteClickEvent>();
            public int GetID() => _id;
            public sbyte eventId;
        }
        
        public struct FinishClickEvent : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<FinishClickEvent>();
            public int GetID() => _id;
            public int eventId;
        }
        
        public struct OpenUI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<OpenUI>();
            public int GetID() => _id;
            // 下面写你的参数
            public GuideInfo guideInfo;
            public int eventId;
        }
        
        public struct OpenUIEnd : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<OpenUIEnd>();
            public int GetID() => _id;
            public int eventId;
        }
        
        public struct CloseEquipHub : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CloseEquipHub>();
            public int GetID() => _id;
        }

        public struct CloseLobbyHub : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CloseLobbyHub>();
            public int GetID() => _id;
        }
        
        public struct CheckCanCombine : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CheckCanCombine>();
            public int GetID() => _id;
        }
        
        public struct SetForceGuideStatus : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SetForceGuideStatus>();
            public int GetID() => _id;
            public bool isShowForceGuide;
        }
        
        public struct NotifyExcuteGuideStep : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<NotifyExcuteGuideStep>();
            public int GetID() => _id;
            public int eventId;
        }
    }
}
