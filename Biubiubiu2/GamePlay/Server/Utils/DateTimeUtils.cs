using System;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class DateTimeUtils {
        private static long curTimeCache;

        public static long GetCurUTCTimestamp() {
            MsgRegister.Dispatcher(new SM_Sausage.GetCurTimestampMilliseconds() {
                CallBack = SetCurTimeCache,
            });
            return curTimeCache;
        }

        private static void SetCurTimeCache(long curTime) {
            curTimeCache = curTime;
        }
    }
}