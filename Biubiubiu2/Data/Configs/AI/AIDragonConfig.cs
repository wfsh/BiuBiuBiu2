using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    [CreateAssetMenu(fileName = "AIDragonConfig", menuName = "AIEditor/AIDragonConfig")]
    public class AIDragonConfig : ScriptableObject {
        public float AwakeTime = 1;
        public float DestroyTime = 1;
        public float MoveSpeed = 3;
        public float LockTime = 1;
        public float AngleSpeed = 10;
        public float MinFollowDistance = 3;
        public float MinFollowTime = 0.5f;
        public List<AIDragonSkillData> Skills;
        public List<AIDragonSkillGroup> SkillGroups;
    }

    public enum AIDragonSkillType {
        Tread, // 踩踏连击
        FireBall, // 喷射火球
        Fire, // 旋转喷火（激光）
        DragonCar, // 飞行冲撞  极光
        Tornado, // 地火
        DelayBlast, // 延迟爆炸（球体）
        AOE, // 全屏AOE（特有表演）
        FlyFlame, // 飞行喷火
    }

    [Serializable]
    public class AIDragonSkillGroup {
        public List<AIDragonSkillType> SkillTypes;
        public float MaxDistance;
    }

    [Serializable]
    public class AIDragonSkillData {
        public AIDragonSkillType SkillType;
        public float AnimTime;
        public float NextTime;
        public float SkillCD;
    }
}
