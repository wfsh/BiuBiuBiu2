using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CM_FunnyDBLog {
        public struct OnInviteToFriend : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CM_FunnyDBLog.OnInviteToFriend>();

            public int GetID() => _id;
            public long timeStamp;
            public int modeId;
        }
    }
}
