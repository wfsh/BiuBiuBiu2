using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_ShortcutTool {
        public struct CreateWebSocket : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<CreateWebSocket>();
            public int GetID() => _id;

            // 下面写你的参数
            public string OpenMessage;
        }

        public struct CloseWebSocket : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<CloseWebSocket>();
            public int GetID() => _id;

            // 下面写你的参数
        }
        public struct OutMessage : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<OutMessage>();
            public int GetID() => _id;

            // 下面写你的参数
            public string Message;
        }
        public struct InMessage : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<InMessage>();
            public int GetID() => _id;

            // 下面写你的参数
            public string Message;
        }
    }
}