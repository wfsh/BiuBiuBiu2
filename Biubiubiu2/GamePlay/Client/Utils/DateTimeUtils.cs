using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class DateTimeUtils {
        private static long curTimeCache;

        public static long GetCurUTCTimestamp() {
            MsgRegister.Dispatcher(new CM_Sausage.GetCurTimestampMilliseconds() {
                CallBack = SetCurTimeCache,
            });
            return curTimeCache;
        }

        private static void SetCurTimeCache(long curTime) {
            curTimeCache = curTime;
        }
    }
}