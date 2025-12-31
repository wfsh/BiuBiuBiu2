using System.IO;
using Standart.Hash.xxHash;

namespace Sofunny.BiuBiuBiu2.Util {
    public class xxHashUtil {
        public static uint ComputeHash32(string str, uint seed = 0) {
            return xxHash32.ComputeHash(str, seed);
        }
        
        public static uint ComputeHash32(Stream stream, int bufferSize = 8192, uint seed = 0) {
            return xxHash32.ComputeHash(stream, bufferSize, seed);
        }

        public static ulong ComputeHash64(string str, uint seed = 0) {
            return xxHash64.ComputeHash(str, seed);
        }
        public static ulong ComputeHash64(Stream stream, int bufferSize = 8192, ulong seed = 0) {
            return xxHash64.ComputeHash(stream, bufferSize, seed);
        }
    }
}