using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public interface IGPOAttribute {
    }
    // 服务器使用的数据接口， 客户端的 In 正常式网络协议
    public interface IGPOInData {
    }
    public class GPOData {
        public static int GPOTeamID_AI = 9999;
        public static int GPOIndex = 0;
        
        public enum GPOType {
            NULL,
            Role,
            RoleAI,
            AI,
            MasterAI
        }
        public enum FollowPointType {
            UAV
        }
        public enum PartEnum {
            Head = 0,
            Body = 1,
            LeftLeg = 2,
            RightLeg = 3,
            Tail = 4,
            Controller = 5, // 最外层的控制节点
            CameraLook = 7,
            Object = 8,
            Driver = 9,
            CameraTarget = 10,
            MeleePack = 11,
            GunPack1 = 12,
            GunPack2 = 13,
            RightHand = 14,
            LeftHand = 15,
            RootBody = 16,
            None = 17,
            AttactPoint1 = 18,
            FootRotaCheck = 19,
            HeadLife = 20,
        }
        
        public class AttributeData : IGPOAttribute {
            public int GpoId;
            public string Sign;
            public string SkinSign;
            public int Level;
            public int maxHp;
            public int nowHp;
            public int ATK;
            public float AttackRange;
            public float Speed; // 角色初始移动速度
        }
        
        public class HearAttributeData : AttributeData {
            public int HeroId;
            public float JumpHeight = 1.5f; // 跳跃到 1.5 米的高度
            public float AirJumpHeight = 1.0f; // 二段跳额外提升的高度
        }
        
        public enum LayerEnum {
            World,
            Ignore,
        }
        
        public static bool CanCheckHitPart(PartEnum part) {
            return part == PartEnum.Head || part == PartEnum.Body;
        }
    }
}