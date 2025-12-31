namespace Sofunny.BiuBiuBiu2.Data {
    public class TestToolData {
        public static bool IsSkipAds { get; private set; } = false;
        public static bool IsQuickCombine { get; private set; } = false;

        public static void SetSkipAds(bool isSkipAds) {
            IsSkipAds = isSkipAds;
        }

        public static void SetQuickCombineHighQuality(bool isQuickCombine) {
            IsQuickCombine = isQuickCombine;
        }
    }
}
