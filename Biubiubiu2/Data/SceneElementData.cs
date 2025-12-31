using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class SceneElementData {
        public enum ElementType {
            None = 1,
            MonsterCreateArea = 2,
            StartPoint1 = 3, // 起点坐标 1
            Updraft = 4, // 上升气流
            PlatformMovement = 6, // 移动模块
            SlipRope = 8, // 场景滑索
            NaturalResource = 9, // 场景资源
            StartPoint2 = 10, // 起点坐标 2
        }
        
        public class Data {
            public ElementType EType;
            public ushort Ability;
        }

        public static List<Data> datas = new List<Data>() {
            new Data() {
                EType = ElementType.Updraft, Ability = AbilityConfig.SceneUpdraft,
            },
            new Data() {
                EType = ElementType.PlatformMovement, Ability = AbilityConfig.PlatformMovement,
            },
            new Data() {
                EType = ElementType.SlipRope, Ability = AbilityConfig.SlipRope,
            },
        };

        public static Data Get(ElementType sign) {
            for (int i = 0; i < datas.Count; i++) {
                var data = datas[i];
                if (data.EType == sign) {
                    return data;
                }
            }
            Debug.LogError(" SceneElementData 没有找到对应 ElementType :" + sign);
            return null;
        }
    }
}