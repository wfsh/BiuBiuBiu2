using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class WarData {
        public static int TestSelectMode = 0;
        public static int TestWeaponId1 = 0;
        public static int TestWeaponSkinItemId1 = 0;
        public static int TestWeaponId2 = 0;
        public static int TestWeaponSkinItemId2 = 0;
        public static int TestSuperWeaponId = 0;
        public static int TestSuperWeaponSkinItemId = 0;
        public static int TestMaxHpInput = 0;
        public static bool TestIsLocalSelfHp = false;
        public static bool TestIsAIMove = true;
        public static bool TestIsAIFire = true;
        public static string WarId {
            get;
            private set;
        }
        private static bool useInstantiateAsync = true;
        
        public static bool UseInstantiateAsync {
            get {
                return useInstantiateAsync;
            }
        }
        
        public static void SetUseInstantiateAsync(bool use) {
            useInstantiateAsync = use;
        }

        public static void SetWarId(string warId) {
            WarId = warId;
        }
        
        public static WarLevel GetWarLevel(int killCount) {
            WarLevel result = default;
            foreach (WarLevel data in WarLevelSet.Data) {
                if (killCount >= data.KillsMin && killCount <= data.KillsMax) {
                    result = data;
                    break;
                }
            }
            return result;
        }
    }
}