using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class AIData {
        
        public enum AIQuality {
            Undefined = 0,
            Normal = 1,
            Senior = 2,
            Elite = 3,
            Boss = 4
        }

        public enum MoveType {
            Walk,
            Run,
            BackMove,
            Stand,
        }
    }
}