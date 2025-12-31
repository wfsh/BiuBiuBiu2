using System;

namespace Sofunny.BiuBiuBiu2.Util {
    public class TimeUtil {
        private static long curTimeCache;
        private static long serverTimeOffset;
        // 获取当前时间戳 （毫秒）
        public static long GetMillisecond() {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }
        
        // 获取当前时间戳 （秒）
        public static long GetSecond() {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        public static string ConvertTimestampToDateString(long timestamp) {
            // 将毫秒级时间戳转换为 DateTime
            var dateTime = new DateTime(621355968000000000 + timestamp * 10000, DateTimeKind.Utc);
            var localTime = dateTime.ToLocalTime();
            var formattedDate = localTime.ToString("yyyy年MM月dd日 HH:mm:ss");
            return formattedDate;
        }

        public static long GetCurUTCTimestamp() {
            return curTimeCache;
        }

        public static void SetCurTimeCache(long curTime) {
            curTimeCache = curTime;
        }
        

        public static string ConvertSecondsToDateString(long seconds, string format) {
            var dateTime = DateTimeOffset.FromUnixTimeSeconds(seconds).LocalDateTime;
            return dateTime.ToString(format);
        }

        public static long GetUtcNowSeconds() {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds() - serverTimeOffset;
        }

        public static void SetServerTime(long serverTime) {
            serverTimeOffset = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - serverTime;
        }

        public static long GetRemainingSecondsToMidnight() {
            var localTime = GetUtcNowSeconds();
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var utcNow = epoch.AddSeconds(localTime).ToLocalTime();
            var serverMidnight = new DateTime(
                utcNow.Year,
                utcNow.Month,
                utcNow.Day,
                0, 0, 0,
                DateTimeKind.Utc
            ).AddDays(1); // 明天的 00:00:00

            var remaining = serverMidnight - utcNow;
            return Math.Max(0, (long)remaining.TotalSeconds);
        }
        
        public static string GetTimeDifference(long remainingTime) { 
            remainingTime = Math.Max(0, remainingTime);
            var hours = (remainingTime % 86400) / 3600;
            var minutes = (remainingTime % 3600) / 60;
            var seconds = remainingTime % 60;

            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }
    }
}