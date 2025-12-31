using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class AIBehaviourData {
        public enum FightStateEnum {
            None,
            Idle, // 待机
            Alert, // 发现敌人
            Warning,  // 警戒
            Fight // 战斗
        }
    }
}