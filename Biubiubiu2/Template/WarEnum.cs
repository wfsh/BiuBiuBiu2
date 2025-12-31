using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Template {
    ///  根据业务需求可以替换为模板数据
    public enum DamageType
    {
        /// 不限伤害类型
        DamageTypeNull = 0,
        /// 普通伤害类型
        Normal = 1,
        /// 爆炸伤害类型
        Explosive = 2,
        /// 灼烧伤害类型
        Burn = 3,
        /// 闪电伤害类型
        Flash = 4,
    }
}