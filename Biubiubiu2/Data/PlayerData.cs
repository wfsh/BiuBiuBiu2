using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 玩家数据
/// </summary>
/// 
namespace Sofunny.BiuBiuBiu2.Data {
    public class PlayerData {
        public static long PlayerId = 0;
        public static string UID = "";
        public static string NickName = "";
        public static string AvatarUrl = "";
        public static long GroupId = 0;
        public static string OpenId = "";

        public class GameFunctionData {
            public int Id;
            public string Description;
        }

        // 已解锁的功能列表
        private static Dictionary<int, GameFunctionData> unlockedFunctions = new Dictionary<int, GameFunctionData>();

        // 增加 10 进制 PlayerId 转 36 进制的字符串
        public static void InitUID() {
            UID = PlayerIDToUID(PlayerId);
        }

        public static void TestPlayerData() {
            PlayerId = Random.Range(0, 9999999);
            NickName = "Player_" + PlayerData.PlayerId;
        }

        public static string PlayerIDToUID(long playerId) {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            var value = playerId;
            if (value == 0) return "0";
            bool isNegative = value < 0;
            if (isNegative) value = -value;
            var result = string.Empty;
            while (value > 0) {
                result = chars[(int)(value % 36)] + result;
                value /= 36;
            }
            return isNegative ? "-" + result : result;
        }

        public static long UIDToPlayerId(string uid) {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            var input = uid;
            if (string.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));
            bool isNegative = input[0] == '-';
            if (isNegative) input = input.Substring(1);
            long result = 0;
            foreach (char c in input) {
                int value = chars.IndexOf(c);
                if (value < 0) throw new ArgumentException("Invalid character in Base36 string.");
                result = result * 36 + value;
            }
            return isNegative ? -result : result;
        }

        public static void SetGameFunctionList(List<GameFunctionData> functionList) {
            unlockedFunctions.Clear();
            GameFunctionData data;
            for (int i = 0; i < functionList.Count; i++) {
                data = functionList[i];
                unlockedFunctions.TryAdd(data.Id, data);
            }
        }

        public static void AddGameFunction(GameFunctionData data) {
            unlockedFunctions.TryAdd(data.Id, data);
        }

        public static bool IsFunctionUnlocked(int gameFunctionId) {
            return unlockedFunctions.ContainsKey(gameFunctionId);
        }
    }
}
