using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_UpHPForFireRole : AbilityMData {
        
        /// <summary>
        /// 判断是否可以使用该能力
        /// </summary>
        /// <param name="nowHp">当前血量</param>
        /// <param name="maxHp">最大血量</param>
        /// <returns></returns>
        public static bool CheckCanUse(int nowHp, int maxHp) {
            if (nowHp < maxHp) {
                return true;
            }
            return false;
        }
    }

    public class AbilityIn_UpHPForFireRole : IAbilityInData {
        public int In_UpHPValue;
    }
}